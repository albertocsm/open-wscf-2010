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
using System.Collections.ObjectModel;
using System.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Providers
{
	/// <summary>
	/// Implements a <see cref="SiteMapProvider"/> that build the <see cref="SiteMap"/> based on
	/// the nodes published by the modules using the <see cref="ISiteMapBuilderService"/>.
	/// </summary>
	public class ModuleSiteMapProvider : StaticSiteMapProvider
	{
		private IAuthorizationService _authorizationService;
		private bool _isInitialized = false;
		private object _lockObject = new object();
		private SiteMapNode _rootNode;
		private ISiteMapBuilderService _siteMapBuilder;

		/// <summary>
		/// Gets or sets the <see cref="ISiteMapBuilderService"/> to use when building the <see cref="SiteMap"/>.
		/// </summary>
		public ISiteMapBuilderService SiteMapBuilder
		{
			get
			{
				if (_siteMapBuilder == null)
				{
					//This class is not injected by OB. An instance is created by ASP.NET 
					//using reflection, so we can?t use the HttpContextLocatorService.
					WebClientApplication application = (WebClientApplication) HttpContext.Current.ApplicationInstance;
					CompositionContainer rootContainer = application.RootContainer;
					_siteMapBuilder = rootContainer.Services.Get<ISiteMapBuilderService>(true);
				}
				return _siteMapBuilder;
			}
			set { _siteMapBuilder = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="IAuthorizationService"/> to use to determine the node accessibility.
		/// </summary>
		public IAuthorizationService AuthorizationService
		{
			get
			{
				if (_authorizationService == null)
				{
					WebClientApplication application = (WebClientApplication) HttpContext.Current.ApplicationInstance;
					CompositionContainer rootContainer = application.RootContainer;
					_authorizationService = rootContainer.Services.Get<IAuthorizationService>();
				}
				return _authorizationService;
			}
			set { _authorizationService = value; }
		}

		/// <summary>
		/// Builds the <see cref="SiteMap"/> managed by the provider.
		/// </summary>
		/// <returns>The root <see cref="SiteMapNode"/> for the provider.</returns>
		public override SiteMapNode BuildSiteMap()
		{
			if (!_isInitialized)
			{
				lock (_lockObject)
				{
					if (!_isInitialized)
					{
						SiteMapNodeInfo rootNodeInfo = SiteMapBuilder.RootNode;
						_rootNode = CreateSiteMapNode(rootNodeInfo);
						AddChildNodes(_rootNode, SiteMapBuilder.GetChildren(rootNodeInfo.Key));

						_isInitialized = true;
					}
				}
			}
			return _rootNode;
		}

		private void AddChildNodes(SiteMapNode parent, ReadOnlyCollection<SiteMapNodeInfo> children)
		{
			if (children.Count > 0)
			{
				foreach (SiteMapNodeInfo child in children)
				{
					SiteMapNode childNode = CreateSiteMapNode(child);
					AddNode(childNode, parent);
					AddChildNodes(childNode, SiteMapBuilder.GetChildren(child.Key));
				}
			}
		}

		private SiteMapNode CreateSiteMapNode(SiteMapNodeInfo nodeInfo)
		{
			return new SiteMapNode(this,
			                       nodeInfo.Key,
			                       nodeInfo.Url,
			                       nodeInfo.Title,
			                       nodeInfo.Description,
			                       nodeInfo.Roles,
			                       nodeInfo.Attributes,
			                       nodeInfo.ExplicitResourcesKey,
			                       nodeInfo.ImplicitResourceKey);
		}

		/// <summary>
		/// Retrieves the root node of all the nodes that are currently managed by the provider.
		/// </summary>
		/// <returns></returns>
		protected override SiteMapNode GetRootNodeCore()
		{
			return BuildSiteMap();
		}

		/// <summary>
		/// Determines whether the specified  <see cref="SiteMapNode"/> object can be 
		/// viewed by the user in the specified context.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/> that contains user information.</param>
		/// <param name="node">The <see cref="SiteMapNode"/> that is requested by the user.</param>
		/// <returns><see langword="true"/>	if the node can be viewed by the user; otherwise, <see langword="false"/>.</returns>
		public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			return IsAccessibleToUser(node);
		}

		/// <summary>
		/// Determines whether the specified  <see cref="SiteMapNode"/> object can be 
		/// viewed by the user in the specified context.
		/// </summary>
		/// <param name="node">The <see cref="SiteMapNode"/> that is requested by the user.</param>
		protected virtual bool IsAccessibleToUser(SiteMapNode node)
		{
			bool result = true;
			if (AuthorizationService != null)
			{
				string rule = SiteMapBuilder.GetAuthorizationRule(node.Key);
				if (rule != null)
				{
					result = AuthorizationService.IsAuthorized(rule);
				}
			}
			return result;
		}
	}
}