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
namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines a service to check for authorization.
	/// </summary>
	public interface IAuthorizationService
	{
		/// <summary>
		/// Determines whether the current user is authorized or not given the specified context.
		/// </summary>
		/// <param name="context">The context or rule to be authorized.</param>
		/// <returns><see langword="true"/> if the current user is authorized; otherwise <see langword="false"/>.</returns>
		bool IsAuthorized(string context);

		/// <summary>
		/// Determines whether the specified role is authorized or not given the specified context
		/// </summary>
		/// <param name="role">The role to check authorization for.</param>
		/// <param name="context">The context or rule to be authorized.</param>
		/// <returns><see langword="true"/> if the current user is authorized; otherwise <see langword="false"/>.</returns>
		bool IsAuthorized(string role, string context);
	}
}