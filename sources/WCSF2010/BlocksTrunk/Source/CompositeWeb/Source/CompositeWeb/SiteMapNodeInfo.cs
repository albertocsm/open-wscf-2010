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
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Holds information for a <see cref="System.Web.SiteMapNode"/> published by a module.
	/// </summary>
	public class SiteMapNodeInfo
	{
		private NameValueCollection _attributes;
		private string _description;
		private NameValueCollection _explicitResourcesKey;
		private string _implicitResourceKey;
		private string _key;
		private IList _roles;
		private string _title;
		private string _url;

		/// <summary>
		/// Initializes a new instance of <see cref="SiteMapNodeInfo"/> using the specified key to identify the page the node represents.
		/// </summary>
		/// <param name="key">The lookup key.</param>
		public SiteMapNodeInfo(string key)
			: this(key, null, null, null, null, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SiteMapNodeInfo"/> using the specified url and key to identify the page the node represents.
		/// </summary>
		/// <param name="key">The lookup key.</param>
		/// <param name="url">The url of the page that the node represents.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		public SiteMapNodeInfo(string key, string url)
			: this(key, url, null, null, null, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SiteMapNodeInfo"/> using the specified title, url and the key to identify the page the node represents.
		/// </summary>
		/// <param name="key">The lookup key.</param>
		/// <param name="url">The url of the page that the node represents.</param>
		/// <param name="title">The title for the node.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		public SiteMapNodeInfo(string key, string url, string title)
			: this(key, url, title, null, null, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SiteMapNodeInfo"/> using the specified description, title, url and the key to identify the page the node represents.
		/// </summary>
		/// <param name="key">The lookup key.</param>
		/// <param name="url">The url of the page that the node represents.</param>
		/// <param name="title">The title for the node.</param>
		/// <param name="description">A description of the page the node represents.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		public SiteMapNodeInfo(string key, string url, string title, string description)
			: this(key, url, title, description, null, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SiteMapNodeInfo"/> using the specified roles, additional attributes,
		/// explicit and implicit resource keys for localization, description, title, url and the key to identify the page the node represents.
		/// </summary>
		/// <param name="key">The lookup key.</param>
		/// <param name="url">The url of the page that the node represents.</param>
		/// <param name="title">The title for the node.</param>
		/// <param name="description">A description of the page the node represents.</param>
		/// <param name="roles">An <see cref="IList"/> of roles allowed to view the page the node represents.</param>
		/// <param name="attributes">A <see cref="NameValueCollection"/> of additional values to initialize the node.</param>
		/// <param name="explicitResourceKeys">A <see cref="NameValueCollection"/> of explicit resource keys used for localization.</param>
		/// <param name="implicitResourceKey">An implicit resource key used for localization.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		public SiteMapNodeInfo(string key, string url, string title, string description, IList roles,
		                       NameValueCollection attributes, NameValueCollection explicitResourceKeys,
		                       string implicitResourceKey)
		{
			Guard.ArgumentNotNull(key, "key");

			_key = key;
			_url = url;
			_title = title;
			_description = description;
			_roles = roles;
			_attributes = attributes;
			_explicitResourcesKey = explicitResourceKeys;
			_implicitResourceKey = implicitResourceKey;
		}

		/// <summary>
		/// Gets or sets the implicit resource key used for localization.
		/// </summary>
		public string ImplicitResourceKey
		{
			get { return _implicitResourceKey; }
			set { _implicitResourceKey = value; }
		}

		/// <summary>
		/// Gets a <see cref="NameValueCollection"/> of explicit resource keys used for localization.
		/// </summary>
		public NameValueCollection ExplicitResourcesKey
		{
			get { return _explicitResourcesKey; }
		}

		/// <summary>
		/// Gets a <see cref="NameValueCollection"/> of additional values to initialize the node.
		/// </summary>
		public NameValueCollection Attributes
		{
			get { return _attributes; }
		}

		/// <summary>
		/// Gets and <see cref="IList"/> of the roles allowed to view the page represented by the node.
		/// </summary>
		public IList Roles
		{
			get { return _roles; }
		}

		/// <summary>
		/// Gets or sets the node description.
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// Gets or sets the node Title.
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		/// Gets or sets the url of the page represented by the node.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		/// <summary>
		/// Gets or sets the lookup key for the node.
		/// </summary>
		public string Key
		{
			get { return _key; }
			set { _key = value; }
		}
	}
}