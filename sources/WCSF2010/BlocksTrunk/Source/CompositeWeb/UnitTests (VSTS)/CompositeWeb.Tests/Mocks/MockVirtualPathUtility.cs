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
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Tests.Mocks
{
	public class MockVirtualPathUtility : IVirtualPathUtilityService
	{
		public string ApplicationPath = "http://MyApplication";
		public string ToAppRelativeResult = null;

		#region IVirtualPathUtilityService Members

		public string AppendTrailingSlash(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string Combine(string basePath, string relativePath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string GetDirectory(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string GetExtension(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string GetFileName(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool IsAbsolute(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool IsAppRelative(string virtualPath)
		{
			return false;
		}

		public string MakeRelative(string fromPath, string toPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string RemoveTrailingSlash(string virtualPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string ToAbsolute(string virtualPath)
		{
			return ToAbsolute(virtualPath, ApplicationPath);
		}

		public string ToAbsolute(string virtualPath, string applicationPath)
		{
			if (virtualPath.StartsWith("~"))
			{
				return virtualPath.Replace("~", ApplicationPath);
			}

			return virtualPath;
		}

		public string ToAppRelative(string virtualPath)
		{
			return ToAppRelativeResult;
		}

		public string ToAppRelative(string virtualPath, string applicationPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}