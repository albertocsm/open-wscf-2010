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
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Web
{
	/// <summary>
	/// Implements the <see cref="IHttpContext"/> by wrapping the <see cref="System.Web.HttpContext"/>.
	/// </summary>
	public class HttpContext : IHttpContext
	{
		private System.Web.HttpContext _context;

		/// <summary>
		/// Initializes a new instance of <see cref="HttpContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="System.Web.HttpContext"/> to wrap.</param>
		public HttpContext(System.Web.HttpContext context)
		{
			_context = context;
		}

		#region IHttpContext Members

		/// <summary>
		/// Gets the wrapped <see cref="System.Web.HttpContext"/>.
		/// </summary>
		public System.Web.HttpContext Context
		{
			get { return _context; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public Exception[] AllErrors
		{
			get { return _context.AllErrors; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public HttpApplicationState Application
		{
			get { return _context.Application; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public HttpApplication ApplicationInstance
		{
			get { return _context.ApplicationInstance; }
			set { _context.ApplicationInstance = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public Cache Cache
		{
			get { return _context.Cache; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IHttpHandler CurrentHandler
		{
			get { return _context.CurrentHandler; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public Exception Error
		{
			get { return _context.Error; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IHttpHandler Handler
		{
			get { return _context.Handler; }
			set { _context.Handler = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public bool IsCustomErrorEnabled
		{
			get { return _context.IsCustomErrorEnabled; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public bool IsDebuggingEnabled
		{
			get { return _context.IsDebuggingEnabled; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IDictionary Items
		{
			get { return _context.Items; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IHttpHandler PreviousHandler
		{
			get { return _context.PreviousHandler; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public ProfileBase Profile
		{
			get { return _context.Profile; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public HttpRequest Request
		{
			get { return _context.Request; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public HttpResponse Response
		{
			get { return _context.Response; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IHttpServerUtility Server
		{
			get { return new HttpServerUtility(_context.Server); }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IHttpSessionState Session
		{
			get { return new HttpSessionState(_context.Session); }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public bool SkipAuthorization
		{
			get { return _context.SkipAuthorization; }
			set { _context.SkipAuthorization = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public DateTime Timestamp
		{
			get { return _context.Timestamp; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public TraceContext Trace
		{
			get { return _context.Trace; }
		}


		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public IPrincipal User
		{
			get { return _context.User; }
			set { _context.User = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public void AddError(Exception errorInfo)
		{
			_context.AddError(errorInfo);
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public void ClearError()
		{
			_context.ClearError();
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public object GetSection(string sectionName)
		{
			return _context.GetSection(sectionName);
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public void RewritePath(string path, bool rebaseClientPath)
		{
			_context.RewritePath(path, rebaseClientPath);
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public void RewritePath(string filePath, string pathInfo, string queryString)
		{
			_context.RewritePath(filePath, pathInfo, queryString);
		}

		/// <summary>
		/// See <see cref="System.Web.HttpContext"/> for a description.
		/// </summary>
		public void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
		{
			_context.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
		}

		#endregion
	}
}