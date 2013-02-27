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
using System.Web;

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Interface to wrap the <see cref="VirtualPathUtility"/> class.
	/// </summary>
	/// <remarks>The goal of this interface is to improve testability.</remarks>
	public interface IVirtualPathUtilityService
	{
		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string AppendTrailingSlash(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string Combine(string basePath, string relativePath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string GetDirectory(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string GetExtension(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string GetFileName(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		bool IsAbsolute(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		bool IsAppRelative(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string MakeRelative(string fromPath, string toPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string RemoveTrailingSlash(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string ToAbsolute(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string ToAbsolute(string virtualPath, string applicationPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string ToAppRelative(string virtualPath);

		/// <summary>
		/// See <see cref="VirtualPathUtility"/> for a description.
		/// </summary>
		string ToAppRelative(string virtualPath, string applicationPath);
	}
}