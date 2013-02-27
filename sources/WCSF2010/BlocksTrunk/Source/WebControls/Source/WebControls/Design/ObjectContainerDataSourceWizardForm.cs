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
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.Web.UI.WebControls.Properties;
using Microsoft.Practices.Web.UI.WebControls.Utility;

namespace Microsoft.Practices.Web.UI.WebControls.Design
{
	/// <summary>
	/// Provides design-time support for the <see cref="ObjectContainerDataSource"/> control.
	/// </summary>
    public partial class ObjectContainerDataSourceWizardForm
    {
        private ObjectContainerDataSourceDesigner _DataSourceControlDesigner;
        private ObjectContainerDataSource _DataSourceControl;
        private ITypeDiscoveryService _discoveryService;
        
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectContainerDataSourceWizardForm"/>.
		/// </summary>
		/// <param name="objectContainerDataSourceDesigner">The <see cref="ObjectContainerDataSource"/> designer.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public ObjectContainerDataSourceWizardForm(ObjectContainerDataSourceDesigner objectContainerDataSourceDesigner)
        {
            Guard.ArgumentNotNull(objectContainerDataSourceDesigner, "objectContainerDataSourceDesigner");
            
            InitializeComponent();

            _DataSourceControlDesigner = objectContainerDataSourceDesigner;
            _DataSourceControl = (ObjectContainerDataSource)_DataSourceControlDesigner.Component;
            Text = String.Format(CultureInfo.CurrentCulture, Resources.FormTitle, _DataSourceControl.ID);
            InitializeTypesComboBox();
        }

        private ITypeDiscoveryService TypeDiscoveryService
        {
            get
            {
                if (this._discoveryService == null)
                {
                    if (this._DataSourceControl.Site != null)
                    {
                        _discoveryService = (ITypeDiscoveryService)this._DataSourceControl.Site.GetService(typeof(ITypeDiscoveryService));
                    }
                }
                return _discoveryService;
            }
        }

        private string DataObjectTypeName
        {
            get { return ((TypeItem)this._typeNameComboBox.SelectedItem).TypeName; }
        }
        
        private static ICollection FilterTypes(ICollection types)
        {
            if ((types == null) || (types.Count == 0))
                return types;

            ArrayList filteredList = new ArrayList(types.Count);
            foreach (Type type in types)
            {
                if (!type.ContainsGenericParameters && !type.IsEnum && !type.IsInterface)
                {
                    filteredList.Add(type);
                }
            }
            return filteredList;
        }

        private void InitializeTypesComboBox()
        {
            if (this.TypeDiscoveryService != null)
            {
                Cursor cursor = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ICollection types = FilterTypes(TypeDiscoveryService.GetTypes(typeof(object), true));
                    _typeNameComboBox.BeginUpdate();
                    _typeNameComboBox.Items.Clear();
                    foreach (Type type in types)
                    {
                        TypeItem typeItem = new TypeItem(type);
                        _typeNameComboBox.Items.Add(typeItem);
                    }
                }
                finally
                {
                    _typeNameComboBox.EndUpdate();
                    Cursor.Current = cursor;
                }
                SelectTypeInComboBox();
            }
        }

        private void SelectTypeInComboBox()
        {
            foreach (TypeItem typeItem in _typeNameComboBox.Items)
            {
                if (typeItem.TypeName == _DataSourceControl.DataObjectTypeName)
                {
                    _typeNameComboBox.SelectedItem = typeItem;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _DataSourceControlDesigner.DataObjectTypeName = DataObjectTypeName;
        }

        private void _typeNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _OKButton.Enabled = true;
        }

        private class TypeItem
        {
            private string _typeName;

            public TypeItem(Type type)
            {
                _typeName = type.FullName;
            }

            public string TypeName
            {
                get { return _typeName; }
            }

            public override string ToString()
            {
                return TypeName;
            }
        }
     }
}
