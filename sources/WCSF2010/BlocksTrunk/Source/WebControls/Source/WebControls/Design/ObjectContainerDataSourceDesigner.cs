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
using System.ComponentModel.Design;
using System.Globalization;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.Web.UI.WebControls.Properties;
using ControlDesigner=System.Web.UI.Design.ControlDesigner;

namespace Microsoft.Practices.Web.UI.WebControls.Design
{
	/// <summary>
	/// Provides design-time support in a design host for the <see cref="ObjectContainerDataSource"/> control.
	/// </summary>
    public class ObjectContainerDataSourceDesigner : DataSourceDesigner
    {
        private static readonly string[] _viewNames = { Resources.DefaultViewName };

		/// <summary>
		/// Indicates whether the <see cref="System.Web.UI.Design.DataSourceDesigner.RefreshSchema(System.Boolean)"/> method can be called.
		/// </summary>
        public override bool CanRefreshSchema
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DataObjectTypeName))
                {
                    return TypeServiceAvailable;
                }
                return false;
            }
        }

		/// <summary>
		/// Indicates whether the <see cref="System.Web.UI.Design.DataSourceDesigner.Configure()"/> method can be called.
		/// </summary>
        public override bool CanConfigure
        {
            get { return TypeServiceAvailable; }
        }
        
        internal string DataObjectTypeName
        {
            get
            {
                return DataSourceControl.DataObjectTypeName;
            }
            set
            {
                if (value != DataObjectTypeName)
                {
                    DataSourceControl.DataObjectTypeName = value;
                    UpdateDesignTimeHtml();
                    if (this.CanRefreshSchema)
                    {
                        RefreshSchema(true);
                    }
                    else
                    {
                        OnDataSourceChanged(EventArgs.Empty);
                    }
                }
            }
        }

        private bool TypeServiceAvailable
        {
            get
            {
                IServiceProvider serviceProvider = base.Component.Site;
                if (serviceProvider != null)
                {
                    ITypeResolutionService typeResolutionService = (ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService));
                    ITypeDiscoveryService typeDiscoveryService = (ITypeDiscoveryService)serviceProvider.GetService(typeof(ITypeDiscoveryService));
                    return (typeResolutionService != null || typeDiscoveryService != null);
                }
                return false;
            }
        }

        internal ObjectContainerDataSource DataSourceControl
        {
            get { return (ObjectContainerDataSource)base.Component; }
        }

        internal IDataSourceViewSchema DataSourceSchema
        {
            get { return DesignerState["DataSourceSchema"] as IDataSourceViewSchema; }
            set { DesignerState["DataSourceSchema"] = value; }
        }

		/// <summary>
		/// Retrieves a <see cref="DesignerDataSourceView"/> object that is identified by the view name.
		/// </summary>
		/// <param name="viewName">The name of the view.</param>
		/// <returns>The <see cref="DesignerDataSourceView"/> identified by the view name.</returns>
		/// <exception cref="ArgumentException">The view name is equal to the default view name.</exception>
        public override DesignerDataSourceView GetView(string viewName)
        {
            if (!string.Equals(viewName, Resources.DefaultViewName, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(viewName))
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidViewName, "viewName"));
            
            return new ObjectContainerDataSourceDesignerView(this, viewName);
        }

		/// <summary>
		/// Launches the data source configuration utility in the design host.
		/// </summary>
        public override void Configure()
        {
            InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, "Configure Data Source");
        }

		/// <summary>
		/// Indicates whether to allow events when refreshing the schema.
		/// </summary>
		/// <param name="preferSilent"><see langword=" true"/> to allow events when refreshing the schema; 
		/// <see langword=" false"/> to disable the DataSourceChanged and SchemaRefreshed events when refreshing the schema.</param>
        public override void RefreshSchema(bool preferSilent)
        {
            try
            {
                base.SuppressDataSourceEvents();
                Cursor cursor = Cursor.Current;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Type type = GetType(base.Component.Site, DataObjectTypeName, preferSilent);

                    if (type == null)
                        return;
                    
                    IDataSourceViewSchema[] schemas = new TypeSchema(type).GetViews();
                    if ((schemas == null) || (schemas.Length == 0))
                    {
                        DataSourceSchema = null;
                        return;
                    }

                    IDataSourceViewSchema newSchema = schemas[0];
                    IDataSourceViewSchema oldSchema = DataSourceSchema;
                    if (!ViewSchemasEquivalent(oldSchema, newSchema))
                    {
                        DataSourceSchema = newSchema;
                        OnSchemaRefreshed(EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage(this.DataSourceControl.Site, String.Format(CultureInfo.CurrentCulture, Resources.CannotRefreshSchema, ex.Message));
                }
                finally
                {
                    Cursor.Current = cursor;
                }
            }
            finally
            {
                base.ResumeDataSourceEvents();
            }
        }

		/// <summary>
		/// Retrieves the view names that are available in this data source.
		/// </summary>
		/// <returns>An array of <see langword="string"/>.</returns>
        public override string[] GetViewNames()
        {
            return _viewNames;
        }

        internal static Type GetType(IServiceProvider serviceProvider, string typeName, bool preferSilent)
        {
            ITypeResolutionService typeResolutionService = GetService<ITypeResolutionService>(serviceProvider);
            if (typeResolutionService == null)
                return null;

            try
            {
                return typeResolutionService.GetType(typeName, true, true);
            }
            catch (Exception ex)
            {
                if (!preferSilent)
                {
                    DisplayErrorMessage(serviceProvider, String.Format(CultureInfo.CurrentCulture, Resources.CannotGetType, ex.Message));
                }
                return null;
            }
        }

        private static T GetService<T>(IServiceProvider serviceProvider)
            where T : class
        {
            if (serviceProvider == null)
                return null;

            return (T)serviceProvider.GetService(typeof(T));
        }

        private bool ConfigureDataSourceChangeCallback(object context)
        {
            try
            {
                SuppressDataSourceEvents();
                IUIService uiService = GetService<IUIService>((IServiceProvider)base.Component.Site);
                if (uiService == null)
                    return false;

                ObjectContainerDataSourceWizardForm form = new ObjectContainerDataSourceWizardForm(this);
                DialogResult result = uiService.ShowDialog(form);
                if (result == DialogResult.OK)
                {
                    OnDataSourceChanged(EventArgs.Empty);
                    return true;
                }
            }
            finally
            {
                ResumeDataSourceEvents();
            }
            return false;
        }

        private static void DisplayErrorMessage(IServiceProvider serviceProvider, string message)
        {
            IUIService UIService = GetService<IUIService>(serviceProvider);
            if (UIService != null)
            {
                UIService.ShowError(message);
            }
        }
    }
}
