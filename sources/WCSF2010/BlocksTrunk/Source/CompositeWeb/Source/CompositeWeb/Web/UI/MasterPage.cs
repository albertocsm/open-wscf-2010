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
	/// Base class for master pages that support the CWAB
	/// dependency injection. Use of this class is optional,
	/// it saves you from having to call BuildItemWithCurrentContext
	/// yourself.
	/// </summary>
	public class MasterPage : System.Web.UI.MasterPage
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
		/// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
		/// to fire the DI engine.
		///</summary>
		///
		///<param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			WebClientApplication.BuildItemWithCurrentContext(this);
		}
	}
}