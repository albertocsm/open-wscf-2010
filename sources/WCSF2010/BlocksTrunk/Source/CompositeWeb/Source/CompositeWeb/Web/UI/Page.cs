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

namespace Microsoft.Practices.CompositeWeb.Web.UI
{
	/// <summary>
	/// Base class for pages that use the CWAB dependency injection
	/// system. Use of this is optional, but saves you from having
	/// to call BuildItemWithCurrentContext yourself.
	/// </summary>
	public class Page : System.Web.UI.Page
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Page.PreInit"></see> event at the beginning of page initialization.
		/// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
		/// to fire the DI engine.
		///</summary>
		///
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			WebClientApplication.BuildItemWithCurrentContext(this);
		}
	}
}