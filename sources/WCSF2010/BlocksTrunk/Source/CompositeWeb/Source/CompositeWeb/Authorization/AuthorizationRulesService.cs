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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Authorization
{
	/// <summary>
	/// Defines a catalog of authorization rules associated with a url.
	/// </summary>
	/// <remarks>Several authorization rules can be asoociated witha single url.</remarks>
	public class AuthorizationRulesService : IAuthorizationRulesService
	{
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")] private
			Dictionary<string, List<string>> rulesIndex = new Dictionary<string, List<string>>();

		/// <summary>
		/// Dictionary that contains the rules, using the URL as a key.
		/// </summary>
		/// <remarks>
		/// This is only protected for testability in unit tests.
		/// </remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected Dictionary<string, List<string>> RulesIndex
		{
			get { return rulesIndex; }
		}

		#region IAuthorizationRulesService Members

		/// <summary>
		/// Registers an authorization rule for the specified url.
		/// </summary>
		/// <param name="urlPath">The url path to associate the authorization rule to.</param>
		/// <param name="rule">The name of the authorization rule.</param>
		public void RegisterAuthorizationRule(string urlPath, string rule)
		{
			Guard.ArgumentNotNullOrEmptyString(urlPath, "urlPath");
			Guard.ArgumentNotNullOrEmptyString(rule, "rule");

			if (rulesIndex.ContainsKey(FixKey(urlPath)) == false)
			{
				rulesIndex.Add(FixKey(urlPath), new List<string>());
				rulesIndex[FixKey(urlPath)].Add(rule);
			}
			else
			{
				if (rulesIndex[FixKey(urlPath)].Contains(rule) == false)
				{
					rulesIndex[FixKey(urlPath)].Add(rule);
				}
			}
		}

		/// <summary>
		/// Finds the authorization rules associated with the specified url.
		/// </summary>
		/// <param name="urlPath">The url to search the authorization rules for.</param>
		/// <returns>An <see cref="string"/> array of authoriztion rules.</returns>
		public string[] GetAuthorizationRules(string urlPath)
		{
			Guard.ArgumentNotNullOrEmptyString(urlPath, "urlPath");

			if (rulesIndex.ContainsKey(FixKey(urlPath)))
			{
				return rulesIndex[FixKey(urlPath)].ToArray();
			}
			else
			{
				return new string[0];
			}
		}

		#endregion

		/// <summary>
		/// Fixes a key for use in the dictionary.
		/// </summary>
		/// <param name="key">Key to fix (URL)</param>
		/// <returns>The fixed (all upper-case) key.</returns>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification="Validation done by Guard class.")]
		protected string FixKey(string key)
		{
			Guard.ArgumentNotNullOrEmptyString(key, "key");
			return key.ToUpper(CultureInfo.CurrentCulture);
		}
	}
}