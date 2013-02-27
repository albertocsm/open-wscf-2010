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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb.Interfaces;
using System.Security.Principal;
using System.Web;
using System.Collections;
using System.Web.Profile;
using System.Web.SessionState;

namespace OrderManagement.Customers.Tests.Mocks
{
    public class MockHttpContext : IHttpContext
    {
        IPrincipal _user;

        public IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        #region IHttpContext Members

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
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public System.Web.Caching.Cache Cache
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
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
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
            get { throw new Exception("The method or operation is not implemented."); }
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
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IHttpSessionState Session
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool SkipAuthorization
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public DateTime Timestamp
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public TraceContext Trace
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }



        public HttpContext Context
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
