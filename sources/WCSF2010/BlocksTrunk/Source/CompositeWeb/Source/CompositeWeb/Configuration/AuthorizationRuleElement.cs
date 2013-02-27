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
using System.Configuration;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// Associates an authorizarion rule with a URL.
	/// </summary>
	public class AuthorizationRuleElement : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets the url.
		/// </summary>
		[ConfigurationProperty("Url", IsKey = true, IsRequired = true)]
		public string AbsolutePath
		{
			get { return (string) base["Url"]; }
			set { base["Url"] = value; }
		}

		/// <summary>
		/// Gets or sets the authorization rule name.
		/// </summary>
		[ConfigurationProperty("Rule", IsKey = false, IsRequired = true)]
		public string RuleName
		{
			get { return (string) base["Rule"]; }
			set { base["Rule"] = value; }
		}
	}
}