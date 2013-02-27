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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Microsoft.Practices.CompositeWeb.EnterpriseLibrary
{
	/// <summary>
	/// Implamnets a <see cref="IHttpModule"/> for logging exceptions using Enterprise Library.
	/// </summary>
	public class ExceptionLogger : IHttpModule
	{
		#region IHttpModule Members

		/// <summary>
		/// Disposes the object.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Intializes the module.
		/// </summary>
		/// <param name="context">The application hosting the module.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		public void Init(HttpApplication context)
		{
			Guard.ArgumentNotNull(context, "context");
			if (!Debugger.IsAttached)
			{
				context.Error += new EventHandler(OnUnhandledException);
			}
		}

		#endregion

		/// <summary>
		/// Handles the <see cref="HttpApplication.Error"/> event, logging the expection using the <see cref="ExceptionPolicy"/>.
		/// </summary>
		/// <param name="sender">The object firing the event.</param>
		/// <param name="e">The event data.</param>
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected virtual void OnUnhandledException(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication) sender;
			Exception ex = application.Server.GetLastError().GetBaseException();
			ExceptionPolicy.HandleException(ex, "GlobalExceptionLogger");
		}
	}
}