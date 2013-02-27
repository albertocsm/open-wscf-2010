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
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Web
{
	/// <summary>
	/// Implements the <see cref="IHttpServerUtility"/> interface by wrapping <see cref="System.Web.HttpServerUtility"/>.
	/// </summary>
	public class HttpServerUtility : IHttpServerUtility
	{
		private System.Web.HttpServerUtility _httpServerUtility;

		/// <summary>
		/// Initializes a new instance of <see cref="HttpServerUtility"/>.
		/// </summary>
		/// <param name="httpServerUtility">The <see cref="System.Web.HttpServerUtility"/> to wrap.</param>
		public HttpServerUtility(System.Web.HttpServerUtility httpServerUtility)
		{
			_httpServerUtility = httpServerUtility;
		}

		#region IHttpServerUtility Members

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string MachineName
		{
			get { return _httpServerUtility.MachineName; }
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public int ScriptTimeout
		{
			get { return _httpServerUtility.ScriptTimeout; }
			set { _httpServerUtility.ScriptTimeout = value; }
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void ClearError()
		{
			_httpServerUtility.ClearError();
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public object CreateObject(string progID)
		{
			return _httpServerUtility.CreateObject(progID);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public object CreateObject(Type type)
		{
			return _httpServerUtility.CreateObject(type);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public object CreateObjectFromClsid(string clsid)
		{
			return _httpServerUtility.CreateObject(clsid);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Execute(string path)
		{
			_httpServerUtility.Execute(path);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Execute(string path, bool preserveForm)
		{
			_httpServerUtility.Execute(path, preserveForm);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Execute(string path, TextWriter writer)
		{
			_httpServerUtility.Execute(path, writer);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
		{
			_httpServerUtility.Execute(handler, writer, preserveForm);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Execute(string path, TextWriter writer, bool preserveForm)
		{
			_httpServerUtility.Execute(path, writer, preserveForm);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public Exception GetLastError()
		{
			return _httpServerUtility.GetLastError();
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string HtmlDecode(string s)
		{
			return _httpServerUtility.HtmlDecode(s);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void HtmlDecode(string s, TextWriter output)
		{
			_httpServerUtility.HtmlDecode(s, output);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string HtmlEncode(string s)
		{
			return _httpServerUtility.HtmlEncode(s);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void HtmlEncode(string s, TextWriter output)
		{
			_httpServerUtility.HtmlEncode(s, output);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string MapPath(string path)
		{
			return _httpServerUtility.MapPath(path);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Transfer(string path)
		{
			_httpServerUtility.Transfer(path);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Transfer(IHttpHandler handler, bool preserveForm)
		{
			_httpServerUtility.Transfer(handler, preserveForm);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void Transfer(string path, bool preserveForm)
		{
			_httpServerUtility.Transfer(path, preserveForm);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string UrlDecode(string s)
		{
			return _httpServerUtility.UrlDecode(s);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void UrlDecode(string s, TextWriter output)
		{
			_httpServerUtility.UrlDecode(s, output);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string UrlEncode(string s)
		{
			return _httpServerUtility.UrlEncode(s);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public void UrlEncode(string s, TextWriter output)
		{
			_httpServerUtility.UrlEncode(s, output);
		}

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		public string UrlPathEncode(string s)
		{
			return _httpServerUtility.UrlPathEncode(s);
		}

		#endregion
	}
}