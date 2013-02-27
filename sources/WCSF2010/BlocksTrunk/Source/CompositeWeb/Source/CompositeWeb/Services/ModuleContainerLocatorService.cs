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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IModuleContainerLocatorService"/>.
	/// </summary>
	public class ModuleContainerLocatorService : IModuleContainerLocatorService
	{
		private const string RootVirtualPath = "~/";

		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields",
			Justification = "Violation fired in error.")] private CompositionContainer _rootContainer;

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleContainerLocatorService"/>.
		/// </summary>
		/// <param name="rootContainer">The container of the service.</param>
		[InjectionConstructor]
		public ModuleContainerLocatorService(CompositionContainer rootContainer)
		{
			_rootContainer = rootContainer;
		}

		#region IModuleContainerLocatorService Members

		/// <summary>
		/// Gets the <see cref="CompositionContainer"/> for the module the requested url belongs to.
		/// </summary>
		/// <param name="relativeRequestUrl">The requested url.</param>
		/// <returns>A <see cref="CompositionContainer"/> instance.</returns>
		public CompositionContainer GetContainer(string relativeRequestUrl)
		{
			IModuleEnumerator moduleEnumerator = _rootContainer.Services.Get<IModuleEnumerator>(true);
			IModuleInfo[] modules = moduleEnumerator.EnumerateModules();
			if (modules == null)
				return _rootContainer;

			List<IModuleInfo> matches = new List<IModuleInfo>();
			foreach (IModuleInfo moduleInfo in modules)
			{
				if (!String.IsNullOrEmpty(moduleInfo.VirtualPath) &&
				    relativeRequestUrl.IndexOf(moduleInfo.VirtualPath, StringComparison.InvariantCultureIgnoreCase) != -1)
				{
					matches.Add(moduleInfo);
				}
			}
			if (matches.Count == 0)
				return _rootContainer;

			return _rootContainer.Containers[GetMatch(matches, relativeRequestUrl).Name];
		}

		#endregion

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification="Violation fired in error."
			)]
		private static IModuleInfo GetMatch(List<IModuleInfo> matches, string relativeRequestUrl)
		{
			bool isRoot = false;
			if (VirtualPathUtility.GetDirectory(relativeRequestUrl).Equals("~/", StringComparison.InvariantCultureIgnoreCase))
			{
				isRoot = true;
			}

			IModuleInfo longestMatch = matches[0];
			foreach (IModuleInfo match in matches)
			{
				if (isRoot && match.VirtualPath.Equals("~/", StringComparison.InvariantCultureIgnoreCase))
					return match;

				if (match.VirtualPath.Length > longestMatch.VirtualPath.Length)
				{
					longestMatch = match;
				}
			}
			return longestMatch;
		}
	}
}