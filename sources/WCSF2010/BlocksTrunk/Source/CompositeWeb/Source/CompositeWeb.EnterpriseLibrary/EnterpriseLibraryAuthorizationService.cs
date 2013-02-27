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
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services
{
	/// <summary>
	/// Implements the <see cref="IAuthorizationService"/> service using Enterprise Library.
	/// </summary>
	public class EnterpriseLibraryAuthorizationService : IAuthorizationService
	{
		private IAuthorizationProvider _authorizationProvider;

		/// <summary>
		/// Initializes a new instance of <see cref="EnterpriseLibraryAuthorizationService"/>.
		/// </summary>
		public EnterpriseLibraryAuthorizationService()
		{
			_authorizationProvider = AuthorizationFactory.GetAuthorizationProvider();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnterpriseLibraryAuthorizationService"/>.
		/// </summary>
		/// <param name="moduleName">The module name to get the authorization rules for.</param>
		public EnterpriseLibraryAuthorizationService(string moduleName)
		{
			_authorizationProvider = AuthorizationFactory.GetAuthorizationProvider(moduleName);
		}

		#region IAuthorizationService Members

		/// <summary>
		/// Determines whether the current user is authorized or not given the specified context.
		/// </summary>
		/// <param name="context">The context or rule to be authorized.</param>
		/// <returns><see langword="true"/> if the current user is authorized; otherwise <see langword="false"/>.</returns>
		public bool IsAuthorized(string context)
		{
			try
			{
				return _authorizationProvider.Authorize(Thread.CurrentPrincipal, context);
			}
			catch (InvalidOperationException)
			{
				return true;
			}
		}

		/// <summary>
		/// Determines whether the specified role is authorized or not given the specified context
		/// </summary>
		/// <param name="role">The role to check authorization for.</param>
		/// <param name="context">The context or rule to be authorized.</param>
		/// <returns><see langword="true"/> if the current user is authorized; otherwise <see langword="false"/>.</returns>
		public bool IsAuthorized(string role, string context)
		{
			IPrincipal principal = new GenericPrincipal(new GenericIdentity("User"), new string[] {role});
			return _authorizationProvider.Authorize(principal, context);
		}

		#endregion
	}
}