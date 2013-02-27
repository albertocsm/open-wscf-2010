//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Used by the <see cref="ModuleLoaderService"/> to get the load sequence
	/// for the modules to load according to their dependencies.
	/// </summary>
	public class ModuleDependencySolver
	{
		private ListDictionary<string, string> _dependencyMatrix = new ListDictionary<string, string>();
		private List<string> _knownModules = new List<string>();

		/// <summary>
		/// Gets the number of modules added to the solver.
		/// </summary>
		public int ModuleCount
		{
			get { return _dependencyMatrix.Count; }
		}

		/// <summary>
		/// Adds a module dependency between the modules specified by dependingModule and
		/// dependentModule.
		/// </summary>
		/// <param name="dependingModule">The name of the module with the dependency.</param>
		/// <param name="dependentModule">The name of the module dependingModule
		/// depends on.</param>
		public void AddDependency(string dependingModule, string dependentModule)
		{
			Guard.ArgumentNotNullOrEmptyString(dependingModule, "dependingModule");
			Guard.ArgumentNotNullOrEmptyString(dependentModule, "dependentModule");

			if (!_knownModules.Contains(dependingModule))
			{
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, Resources.DependencyForUnknownModule, dependingModule));
			}

			AddToDependencyMatrix(dependentModule);
			_dependencyMatrix.Add(dependentModule, dependingModule);
		}

		/// <summary>
		/// Adds a module to the solver.
		/// </summary>
		/// <param name="name">The name that uniquely identifies the module.</param>
		public void AddModule(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");

			AddToDependencyMatrix(name);
			AddToKnownModules(name);
		}

		/// <summary>
		/// Calculates an ordered vector according to the defined dependencies.
		/// Non-dependant modules appears at the beginning of the resulting array.
		/// </summary>
		/// <returns>The resulting ordered list of modules.</returns>
		/// <exception cref="ModuleDependencySolverException">This exception is thrown
		/// when a cycle is found in the defined depedency graph.</exception>
		public string[] Solve()
		{
			List<string> skip = new List<string>();

			while (skip.Count < _dependencyMatrix.Count)
			{
				List<string> leaves = FindLeaves(skip);
				if (leaves.Count == 0 && skip.Count < _dependencyMatrix.Count)
				{
					throw new ModuleDependencySolverException(
						String.Format(CultureInfo.CurrentCulture, Resources.CyclicDependencyFound));
				}

				skip.AddRange(leaves);
			}

			skip.Reverse();

			if (skip.Count > _knownModules.Count)
			{
				throw new ModuleDependencySolverException(
					String.Format(CultureInfo.CurrentCulture, Resources.DependencyOnMissingModule, FindMissingModules(skip)));
			}

			return skip.ToArray();
		}

		private void AddToDependencyMatrix(string module)
		{
			if (!_dependencyMatrix.ContainsKey(module))
			{
				_dependencyMatrix.Add(module);
			}
		}

		private void AddToKnownModules(string module)
		{
			if (!_knownModules.Contains(module))
			{
				_knownModules.Add(module);
			}
		}

		private List<string> FindLeaves(List<string> skip)
		{
			List<string> result = new List<string>();

			foreach (string precedent in _dependencyMatrix.Keys)
			{
				if (skip.Contains(precedent))
				{
					continue;
				}

				int count = 0;

				foreach (string dependent in _dependencyMatrix[precedent])
				{
					if (skip.Contains(dependent))
					{
						continue;
					}

					count++;
				}

				if (count == 0)
				{
					result.Add(precedent);
				}
			}

			return result;
		}

		private string FindMissingModules(List<string> skip)
		{
			string missingModules = "";

			foreach (string module in skip)
			{
				if (!_knownModules.Contains(module))
				{
					missingModules += ", ";
					missingModules += module;
				}
			}

			return missingModules.Substring(2);
		}
	}
}