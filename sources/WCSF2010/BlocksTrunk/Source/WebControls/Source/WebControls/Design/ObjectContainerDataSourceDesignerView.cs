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
using System.Web.UI;
using System.Web.UI.Design;
using Microsoft.Practices.Web.UI.WebControls.Properties;

namespace Microsoft.Practices.Web.UI.WebControls.Design
{
	/// <summary>
	/// Provides design-time view for the <see cref="ObjectContainerDataSourceDesigner"/> designer.
	/// </summary>
	public class ObjectContainerDataSourceDesignerView : DesignerDataSourceView
	{
		ObjectContainerDataSourceDesigner _owner;

		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceDesignerView"/>.
		/// </summary>
		/// <param name="owner">The <see cref="ObjectContainerDataSourceDesigner"/> owning this view.</param>
		/// <param name="viewName">The name of the view.</param>
		public ObjectContainerDataSourceDesignerView(ObjectContainerDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets a schema that describes the data source view that is represented by this view object.
		/// </summary>
		public override IDataSourceViewSchema Schema
		{
			get { return _owner.DataSourceSchema; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// the <see cref="DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary)"/> method.
		/// </summary>
		public override bool CanDelete
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanDelete; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// the <see cref="DataSourceView.ExecuteInsert(System.Collections.IDictionary)"/> method.
		/// </summary>
		public override bool CanInsert
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanInsert; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// the <see cref="DataSourceView.ExecuteUpdate(System.Collections.IDictionary,System.Collections.IDictionary,System.Collections.IDictionary)"/>
		/// method.
		/// </summary>
		public override bool CanUpdate
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanUpdate; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// a sorted view on the underlying data source.
		/// </summary>
		public override bool CanSort
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanSort; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// paging through the data that is retrieved by the <see cref="DataSourceView.ExecuteSelect(System.Web.UI.DataSourceSelectArguments)"/>
		/// method.
		/// </summary>
		public override bool CanPage
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanPage; }
		}

		/// <summary>
		/// Indicates whether the <see cref="DataSourceView"/> object that
		/// is associated with the current <see cref="DataSourceControl"/> object supports
		/// retrieving the total number of data rows, instead of the data.
		/// </summary>
		public override bool CanRetrieveTotalRowCount
		{
			get { return ((IDataSource)_owner.DataSourceControl).GetView(Resources.DefaultViewName).CanRetrieveTotalRowCount; }
		}
	}
}
