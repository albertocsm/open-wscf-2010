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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;
using HttpContext=Microsoft.Practices.CompositeWeb.Web.HttpContext;

namespace Microsoft.Practices.CompositeWeb.Authorization
{
	/// <summary>
	/// Implements a <see cref="IHttpModule"/> used to handle web authorization.
	/// </summary>
	public class WebClientAuthorizationModule : IHttpModule
	{
		#region IHttpModule Members

		/// <summary>
		/// Initializes the HTTP module.
		/// </summary>
		/// <param name="context">The <see cref="HttpApplication"/> hosting this HTTP module.</param>
		/// <remarks>If the httpApplication defines a Root Container, then this HTTP module will handle
		/// the <see cref="HttpApplication.AuthorizeRequest"/> event.</remarks>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		public void Init(HttpApplication context)
		{
			Guard.ArgumentNotNull(context, "httpApplication");
			CompositionContainer rootContainer = ((WebClientApplication) context).RootContainer;
			if (rootContainer != null)
			{
				context.AuthorizeRequest += delegate
				                            	{
				                            		IHttpContext httpContext = new HttpContext(context.Context);
				                            		HandleAuthorization(rootContainer, httpContext);
				                            	};
			}
		}

		/// <summary>
		/// Disposes the module.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Implements the Dispose pattern.
		/// </summary>
		~WebClientAuthorizationModule()
		{
			Dispose(false);
		}

		/// <summary>
		/// Handles the request authorization, using the <see cref="IAuthorizationRulesService"/> and 
		/// the <see cref="IAuthorizationService"/> services.
		/// </summary>
		/// <param name="rootContainer">The application root <see cref="CompositionContainer"/>.</param>
		/// <param name="context">The <see cref="IHttpContext"/> for the processing request.</param>
		/// <exception cref="HttpException">The user is not authorized to access the requested resource.</exception>
		protected virtual void HandleAuthorization(CompositionContainer rootContainer, IHttpContext context)
		{
			if (context.SkipAuthorization) return;
			IAuthorizationRulesService authorizationRulesService = rootContainer.Services.Get<IAuthorizationRulesService>();
			IVirtualPathUtilityService virtualPathUtility = rootContainer.Services.Get<IVirtualPathUtilityService>();
			if (authorizationRulesService == null) return;
			string[] rules =
				authorizationRulesService.GetAuthorizationRules(virtualPathUtility.ToAppRelative(context.Request.Path));
			if (rules == null || rules.Length == 0) return;

			IAuthorizationService authorizationService = rootContainer.Services.Get<IAuthorizationService>(true);
			foreach (string rule in rules)
			{
				if (!authorizationService.IsAuthorized(rule))
				{
					throw new HttpException(403, Resources.UserDoesntHaveAccessToTheRequestedResource);
				}
			}
		}

		/// <summary>
		/// Implements the Dispose pattern.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> when called from Dispose().</param>
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}