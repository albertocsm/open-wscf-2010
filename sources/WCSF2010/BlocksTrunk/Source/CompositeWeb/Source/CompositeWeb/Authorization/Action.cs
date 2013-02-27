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
namespace Microsoft.Practices.CompositeWeb.Authorization
{
	/// <summary>
	/// Defines an action that can be performed on the application and its associated authorization rule.
	/// </summary>
	public class Action
	{
		private string _friendlyName;
		private string _ruleName;

		/// <summary>
		/// Initializes a new instance of <see cref="Action"/>.
		/// </summary>
		/// <param name="friendlyName">The name for the action.</param>
		/// <param name="ruleName">The name of the authorization rule.</param>
		public Action(string friendlyName, string ruleName)
		{
			_friendlyName = friendlyName;
			_ruleName = ruleName;
		}

		/// <summary>
		/// Gets or sets the action name.
		/// </summary>
		public string FriendlyName
		{
			get { return _friendlyName; }
			set { _friendlyName = value; }
		}

		/// <summary>
		/// Gets or sets the action authorization rule.
		/// </summary>
		public string RuleName
		{
			get { return _ruleName; }
			set { _ruleName = value; }
		}
	}
}