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
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IVirtualPathUtilityService"/> wrapping the <see cref="System.Web.VirtualPathUtility"/>.
	/// </summary>
	/// <remarks>The goal of this class is to improve testability.</remarks>
	public class VirtualPathUtilityService : IVirtualPathUtilityService
	{
		#region IVirtualPathUtilityService Members

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string AppendTrailingSlash(string virtualPath)
		{
			return VirtualPathUtility.AppendTrailingSlash(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string Combine(string basePath, string relativePath)
		{
			return VirtualPathUtility.Combine(basePath, relativePath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string GetDirectory(string virtualPath)
		{
			return VirtualPathUtility.GetDirectory(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string GetExtension(string virtualPath)
		{
			return VirtualPathUtility.GetExtension(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string GetFileName(string virtualPath)
		{
			return VirtualPathUtility.GetFileName(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public bool IsAbsolute(string virtualPath)
		{
			return VirtualPathUtility.IsAbsolute(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public bool IsAppRelative(string virtualPath)
		{
			return VirtualPathUtility.IsAppRelative(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string MakeRelative(string fromPath, string toPath)
		{
			return VirtualPathUtility.MakeRelative(fromPath, toPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string RemoveTrailingSlash(string virtualPath)
		{
			return VirtualPathUtility.RemoveTrailingSlash(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string ToAbsolute(string virtualPath)
		{
			return VirtualPathUtility.ToAbsolute(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string ToAbsolute(string virtualPath, string applicationPath)
		{
			return VirtualPathUtility.ToAbsolute(virtualPath, applicationPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string ToAppRelative(string virtualPath)
		{
			return VirtualPathUtility.ToAppRelative(virtualPath);
		}

		/// <summary>
		/// See <see cref="System.Web.VirtualPathUtility"/> for more information.
		/// </summary>
		public string ToAppRelative(string virtualPath, string applicationPath)
		{
			return VirtualPathUtility.ToAppRelative(virtualPath);
		}

		#endregion
	}
}