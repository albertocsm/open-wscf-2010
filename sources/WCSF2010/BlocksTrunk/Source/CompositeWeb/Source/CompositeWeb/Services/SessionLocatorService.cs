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
using System.Web;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Interfaces;
using HttpSessionState=Microsoft.Practices.CompositeWeb.Web.HttpSessionState;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="ISessionStateLocatorService"/> service.
	/// </summary>
	public class SessionStateLocatorService : ISessionStateLocatorService
	{
		#region ISessionStateLocatorService Members

		/// <summary>
		/// Gets the session state as a <see cref="System.Web.SessionState.IHttpSessionState"/> implementation.
		/// </summary>
		public IHttpSessionState GetSessionState()
		{
			//Uses HttpContext.Current.Session without using HttpContextLocatorService. 
			//This service was created to avoid using the ASP.NET session bag directly in the block code, to increase the testing surface.
			return new HttpSessionState(HttpContext.Current.Session);
		}

		#endregion
	}
}