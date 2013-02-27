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

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines a service used to register <see cref="SiteMapNodeInfo"/> objects.
	/// </summary>
	/// <remarks>This service allows the dynamic creation of a <see cref="System.Web.SiteMap"/> during the
	/// module load stage. This service is consumed by the <see cref="Microsoft.Practices.CompositeWeb.Providers.ModuleSiteMapProvider"/> when building
	/// the <see cref="System.Web.SiteMap"/>.</remarks>
	public interface ISiteMapBuilderService
	{
		/// <summary>
		/// Gets the site map root node.
		/// </summary>
		SiteMapNodeInfo RootNode { get; }

		/// <summary>
		/// Adds a node as child of the root node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		void AddNode(SiteMapNodeInfo node);

		/// <summary>
		/// Adds a node as a child of the root node and sets the order with which the node should be displayed.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		void AddNode(SiteMapNodeInfo node, int preferredDisplayOrder);

		/// <summary>
		/// Adds a node as a child of another node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent);

		/// <summary>
		/// Adds a node as a child of another node, and sets the order with which to display the node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, int preferredDisplayOrder);

		/// <summary>
		/// Adds a node with an authorization rule.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		void AddNode(SiteMapNodeInfo node, string authorizationRule);

		/// <summary>
		/// Adds a node with an authorization rule and the specified display order.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="authorizationRule">The authorization rule required to access the node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		void AddNode(SiteMapNodeInfo node, string authorizationRule, int preferredDisplayOrder);

		/// <summary>
		/// Adds a node with an authorization rule as child of another node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule);

		/// <summary>
		/// Adds a node under the parent node, with the specified authorization rule and display order.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule, int preferredDisplayOrder);

		/// <summary>
		/// Gets the children of the specified node.
		/// </summary>
		/// <param name="nodeKey">The key of the parent node.</param>
		/// <returns>A <see cref="ReadOnlyCollection{T}"/> collection of the child nodes.</returns>
		ReadOnlyCollection<SiteMapNodeInfo> GetChildren(string nodeKey);

		/// <summary>
		/// Gets the authorization rule associated with the specified node.
		/// </summary>
		/// <param name="nodeKey">The key to the node.</param>
		/// <returns>The authorization rule associated with the node.</returns>
		string GetAuthorizationRule(string nodeKey);
	}
}