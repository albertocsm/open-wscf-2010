//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Summary description for ADObject.
	/// </summary>
	public struct DsObjectPickerItem
	{
		private string className;
		private string name;
		private string upn;
        private string sid;

		/// <summary>
		/// Gets or sets the name of the class.
		/// </summary>
		/// <value>The name of the class.</value>
		public string ClassName
		{
			get	{ return className; }
			set	{className = value;	}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get	{ return name; }
			set	{ name = value; }
		}

		/// <summary>
		/// Gets or sets the UPN.
		/// </summary>
		/// <value>The UPN.</value>
		public string Upn
		{
			get	{ return upn; }
			set	{ upn = value; }
		}

        /// <summary>
        /// See <see cref="T:System.Security.Principal.SecurityIdentifier.Value"></see>.
        /// </summary>
        /// <value>The sid.</value>
        public string Sid
        {
            get { return sid; }
            set { sid = value; }
        }
	}
}
