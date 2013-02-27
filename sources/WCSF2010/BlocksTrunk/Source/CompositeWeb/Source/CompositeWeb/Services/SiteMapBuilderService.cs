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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Service implementing the site map information needed to later build the site map.
	/// </summary>
	public class SiteMapBuilderService : ISiteMapBuilderService
	{
		private Dictionary<string, List<SiteMapNodeInfo>> _childNodes;
		private Dictionary<string, SiteMapNodeInfo> _keyIndex;
		private Dictionary<string, string> _nodeAuthorization;
		private Dictionary<string, int> _nodePreferredOrder;
		private SiteMapNodeInfo _rootNode;


		// We need to replace all calls to _childNodes.Add() to the add sorted method

		/// <summary>
		/// Initialize a new instance of <see cref="SiteMapBuilderService"/>.
		/// </summary>
		public SiteMapBuilderService()
		{
			_rootNode = new SiteMapNodeInfo(Resources.SiteMapRootNodeKey);
			_keyIndex = new Dictionary<string, SiteMapNodeInfo>();
			_childNodes = new Dictionary<string, List<SiteMapNodeInfo>>();
			_nodeAuthorization = new Dictionary<string, string>();
			_nodePreferredOrder = new Dictionary<string, int>();

			_childNodes.Add(_rootNode.Key, new List<SiteMapNodeInfo>());
		}

		#region ISiteMapBuilderService Members

		/// <summary>
		/// Gets the current root node.
		/// </summary>
		public SiteMapNodeInfo RootNode
		{
			get { return _rootNode; }
		}

		/// <summary>
		/// Adds a node as child of the root node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		public void AddNode(SiteMapNodeInfo node)
		{
			AddNode(node, int.MaxValue);
			//_childNodes[RootNode.Key].Add(node);
		}

		/// <summary>
		/// Adds a node as a child of the root node and sets the order with which the node should be displayed.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		public void AddNode(SiteMapNodeInfo node, int preferredDisplayOrder)
		{
			SafeAddNode(node);
			AddNodeWithOrder(RootNode.Key, node, preferredDisplayOrder);
		}

		/// <summary>
		/// Adds a node as a child of another node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent)
		{
			AddNode(node, parent, int.MaxValue);
		}

		/// <summary>
		/// Adds a node as a child of another node, and sets the order with which to display the node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, int preferredDisplayOrder)
		{
			SafeAddNode(node);

			if (!_childNodes.ContainsKey(parent.Key))
			{
				_childNodes.Add(parent.Key, new List<SiteMapNodeInfo>());
			}

			AddNodeWithOrder(parent.Key, node, preferredDisplayOrder);
		}

		/// <summary>
		/// Adds a node with an authorization rule.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		public void AddNode(SiteMapNodeInfo node, string authorizationRule)
		{
			AddNode(node, authorizationRule, int.MaxValue);
		}

		/// <summary>
		/// Adds a node with an authorization rule and the specified display order.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="authorizationRule">The authorization rule required to access the node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		public void AddNode(SiteMapNodeInfo node, string authorizationRule, int preferredDisplayOrder)
		{
			Guard.ArgumentNotNullOrEmptyString(authorizationRule, "authorizationRule");

			AddNode(node, preferredDisplayOrder);
			_nodeAuthorization.Add(node.Key, authorizationRule);
		}

		/// <summary>
		/// Adds a node with an authorization rule as child of another node.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule)
		{
			AddNode(node, parent, authorizationRule, int.MaxValue);
		}

		/// <summary>
		/// Adds a node under the parent node, with the specified authorization rule and display order.
		/// </summary>
		/// <param name="node">The node to add.</param>
		/// <param name="parent">The node under which to add the new node.</param>
		/// <param name="authorizationRule">The authorizarion rule required to access the node.</param>
		/// <param name="preferredDisplayOrder">The node display order.</param>
		public void AddNode(SiteMapNodeInfo node, SiteMapNodeInfo parent, string authorizationRule, int preferredDisplayOrder)
		{
			AddNode(node, parent, preferredDisplayOrder);
			_nodeAuthorization.Add(node.Key, authorizationRule);
		}

		/// <summary>
		/// Gets the children of the specified node.
		/// </summary>
		/// <param name="nodeKey">The key of the parent node.</param>
		/// <returns>A <see cref="ReadOnlyCollection{T}"/> collection of the child nodes.</returns>
		public ReadOnlyCollection<SiteMapNodeInfo> GetChildren(string nodeKey)
		{
			if (_childNodes.ContainsKey(nodeKey))
			{
				return _childNodes[nodeKey].AsReadOnly();
			}

			return new List<SiteMapNodeInfo>().AsReadOnly();
		}

		/// <summary>
		/// Gets the authorization rule associated with the specified node.
		/// </summary>
		/// <param name="nodeKey">The key to the node.</param>
		/// <returns>The authorization rule associated with the node.</returns>
		public string GetAuthorizationRule(string nodeKey)
		{
			if (_nodeAuthorization.ContainsKey(nodeKey))
			{
				return _nodeAuthorization[nodeKey];
			}

			return null;
		}

		#endregion

		private void SafeAddNode(SiteMapNodeInfo node)
		{
			Guard.ArgumentNotNull(node, "node");

			if (_keyIndex.ContainsKey(node.Key))
			{
				throw new ServiceException(Resources.DuplicateKeyExceptionMessage);
			}

			_keyIndex.Add(node.Key, node);
		}

		private void AddNodeWithOrder(string parentKey, SiteMapNodeInfo node, int preferredDisplayOrder)
		{
			_nodePreferredOrder.Add(node.Key, preferredDisplayOrder);
			for (int i = 0; i < _childNodes[parentKey].Count; i++)
			{
				string key = _childNodes[parentKey][i].Key;
				if (_nodePreferredOrder[key] > preferredDisplayOrder)
				{
					_childNodes[parentKey].Insert(i, node);
					return;
				}
			}

			_childNodes[parentKey].Add(node);
		}
	}
}