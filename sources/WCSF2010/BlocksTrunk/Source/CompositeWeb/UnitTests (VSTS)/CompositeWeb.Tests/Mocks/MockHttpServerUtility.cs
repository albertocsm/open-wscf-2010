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
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks
{
	// As this Mock has some logic, we needed to UnitTest it!
	[TestClass]
	public class MockHttpServerUtilityFixture
	{
		[TestMethod]
		public void TestMethodName()
		{
			MockHttpServerUtility util = new MockHttpServerUtility();
			Assert.AreEqual("/test1.aspx", util.MapPath("http://myApp/test1.aspx"));
			Assert.AreEqual("/folder/test1.aspx", util.MapPath("http://myApp/folder/test1.aspx"));
		}
	}

	public class MockHttpServerUtility : IHttpServerUtility
	{
		#region IHttpServerUtility Members

		public void ClearError()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object CreateObject(Type type)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object CreateObject(string progID)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object CreateObjectFromClsid(string clsid)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Execute(string path, bool preserveForm)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Execute(string path, TextWriter writer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Execute(string path, TextWriter writer, bool preserveForm)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Execute(string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public Exception GetLastError()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void HtmlDecode(string s, TextWriter output)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string HtmlDecode(string s)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void HtmlEncode(string s, TextWriter output)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string HtmlEncode(string s)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string MachineName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string MapPath(string path)
		{
			Regex rg = new Regex("^(http://){0,1}(?<appname>[\\w\\.\\-]+)(?<path>/(.)*)$");
			MatchCollection matches = rg.Matches(path);
			return matches[0].Groups["path"].Value;
		}

		public int ScriptTimeout
		{
			get { throw new Exception("The method or operation is not implemented."); }
			set { throw new Exception("The method or operation is not implemented."); }
		}

		public void Transfer(string path, bool preserveForm)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Transfer(IHttpHandler handler, bool preserveForm)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Transfer(string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void UrlDecode(string s, TextWriter output)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string UrlDecode(string s)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void UrlEncode(string s, TextWriter output)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string UrlEncode(string s)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string UrlPathEncode(string s)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}