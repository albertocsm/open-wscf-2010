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
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Interface used to wrap the <see cref="System.Web.HttpContext"/> class.
	/// </summary>
	/// <remarks>The goal of this interface is to improve testability.</remarks>
	public interface IHttpContext
	{
		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Exception[] AllErrors { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		HttpApplicationState Application { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		HttpApplication ApplicationInstance { get; set; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		Cache Cache { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IHttpHandler CurrentHandler { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		Exception Error { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IHttpHandler Handler { get; set; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		bool IsCustomErrorEnabled { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		bool IsDebuggingEnabled { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IDictionary Items { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IHttpHandler PreviousHandler { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		ProfileBase Profile { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		HttpRequest Request { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		HttpResponse Response { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IHttpServerUtility Server { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IHttpSessionState Session { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		bool SkipAuthorization { get; set; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		DateTime Timestamp { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		TraceContext Trace { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		IPrincipal User { get; set; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		HttpContext Context { get; }

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		void AddError(Exception errorInfo);

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		void ClearError();

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		object GetSection(string sectionName);

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "2#")]
		void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath);

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "2#")]
		void RewritePath(string filePath, string pathInfo, string queryString);

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		void RewritePath(string path, bool rebaseClientPath);
	}
}