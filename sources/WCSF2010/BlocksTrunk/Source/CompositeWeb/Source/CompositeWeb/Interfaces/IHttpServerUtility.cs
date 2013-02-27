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
using System.IO;
using System.Web;

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Interface used to wrap the <see cref="System.Web.HttpServerUtility"/> class.
	/// </summary>
	/// <remarks>The goal of this interface is to improve testability.</remarks>
	public interface IHttpServerUtility
	{
		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		string MachineName { get; }

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		int ScriptTimeout { get; set; }

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void ClearError();

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		object CreateObject(Type type);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		object CreateObject(string progID);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		object CreateObjectFromClsid(string clsid);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Execute(string path, bool preserveForm);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Execute(string path, TextWriter writer);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Execute(string path, TextWriter writer, bool preserveForm);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Execute(string path);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		Exception GetLastError();

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void HtmlDecode(string s, TextWriter output);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		string HtmlDecode(string s);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void HtmlEncode(string s, TextWriter output);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		string HtmlEncode(string s);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		string MapPath(string path);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Transfer(string path, bool preserveForm);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Transfer(IHttpHandler handler, bool preserveForm);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void Transfer(string path);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		void UrlDecode(string s, TextWriter output);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		/// 
		[SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
		string UrlDecode(string s);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
		void UrlEncode(string s, TextWriter output);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
		string UrlEncode(string s);

		/// <summary>
		/// See <see cref="HttpServerUtility"/> for a description.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
		string UrlPathEncode(string s);
	}
}