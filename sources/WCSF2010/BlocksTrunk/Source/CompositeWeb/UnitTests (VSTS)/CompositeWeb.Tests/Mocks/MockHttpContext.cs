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

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks
{
	public class MockHttpContext : IHttpContext
	{
		private HttpApplication _application = null;
		private HttpContext _context;
		private IHttpHandler _handler;
		private HttpRequest _request = null;
		private IHttpServerUtility _server = null;
		private bool _skipAuthorization = false;
		private IPrincipal _user = null;

		#region IHttpContext Members

		public HttpContext Context
		{
			get { return _context; }
			set { _context = value; }
		}

		public void AddError(Exception errorInfo)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public Exception[] AllErrors
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public HttpApplicationState Application
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public HttpApplication ApplicationInstance
		{
			get { return _application; }
			set { _application = value; }
		}

		public Cache Cache
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void ClearError()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public IHttpHandler CurrentHandler
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public Exception Error
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object GetSection(string sectionName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public IHttpHandler Handler
		{
			get { return _handler; }
			set { _handler = value; }
		}

		public bool IsCustomErrorEnabled
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsDebuggingEnabled
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IDictionary Items
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IHttpHandler PreviousHandler
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ProfileBase Profile
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public HttpRequest Request
		{
			get { return _request; }
			set { _request = value; }
		}

		public HttpResponse Response
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RewritePath(string filePath, string pathInfo, string queryString)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RewritePath(string path, bool rebaseClientPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public IHttpServerUtility Server
		{
			get { return _server; }
			set { _server = value; }
		}

		public IHttpSessionState Session
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool SkipAuthorization
		{
			get { return _skipAuthorization; }
			set { _skipAuthorization = value; }
		}

		public DateTime Timestamp
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public TraceContext Trace
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IPrincipal User
		{
			get { return _user; }
			set { _user = value; }
		}

		#endregion
	}
}