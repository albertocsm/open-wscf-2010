//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
#region Using Statements
using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Resources;
using Microsoft.Data.ConnectionUI;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Globalization;

#endregion
namespace Microsoft.Practices.RecipeFramework.Extensions.Editors
{
    /// <summary>
    /// Editor for connection strings
    /// </summary>
	public class ConnectionStringEditor : UITypeEditor, IAttributesConfigurable
	{
		/// <summary>
		/// Configuration attribute that can be specified at the editor element 
		/// that determines the argument that holds the data provider used 
		/// in the connection string. This argument is optional and if specified it 
		/// must have the name "ProviderArgument" and point to an argument defined in 
		/// the containing recipe.
		/// </summary>
		public const string ProviderArgument = "ProviderArgument";

		string providerArgument;

        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
        /// </returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ResourceManager resmgr = new ResourceManager("SR", typeof(DataConnectionDialog).Assembly);
			DataConnectionDialog dlg = new DataConnectionDialog();
			DataSource source;
			DataProvider dataprovider;

			DataSource sqldatasource;

			#region Access

			source = new DataSource("access", resmgr.GetString("DataSource_MicrosoftAccess", CultureInfo.InvariantCulture));
			dataprovider = new DataProvider(
				typeof(OleDbConnection).Namespace,
                resmgr.GetString("DataProvider_OleDB", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_OleDB_Short", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_OleDB_AccessDataSource_Description", CultureInfo.InvariantCulture),
				typeof(OleDbConnection),
				typeof(OleDBConnectionUIControl),
				typeof(OleDBConnectionProperties));
			source.Providers.Add(dataprovider);
			dlg.DataSources.Add(source);

			#endregion

			#region ODBC

            source = new DataSource("odbc", resmgr.GetString("DataSource_MicrosoftOdbcDsn", CultureInfo.InvariantCulture));
			dataprovider = new DataProvider(
				typeof(OdbcConnection).Namespace,
                resmgr.GetString("DataProvider_Odbc", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Odbc_Short", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Odbc_DataSource_Description", CultureInfo.InvariantCulture),
				typeof(OdbcConnection),
				typeof(OdbcConnectionUIControl),
				typeof(OdbcConnectionProperties));
			source.Providers.Add(dataprovider);
			dlg.DataSources.Add(source);

			#endregion

			#region MSSQL

            source = new DataSource("mssql", resmgr.GetString("DataSource_MicrosoftSqlServer", CultureInfo.InvariantCulture));
			dataprovider = new DataProvider(
				typeof(SqlConnection).Namespace,
                resmgr.GetString("DataProvider_Sql", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Sql_Short", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Sql_DataSource_Description", CultureInfo.InvariantCulture),
				typeof(SqlConnection),
				typeof(SqlConnectionUIControl),
				typeof(SqlConnectionProperties));
			source.Providers.Add(dataprovider);
			DataProvider defaultprovider = dataprovider;
			dataprovider = new DataProvider(
				typeof(OleDbConnection).Namespace,
                resmgr.GetString("DataProvider_OleDB", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_OleDB_Short", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_OleDB_SqlDataSource_Description", CultureInfo.InvariantCulture),
				typeof(OleDbConnection),
				typeof(OleDBConnectionUIControl),
				typeof(OleDBConnectionProperties));
			source.Providers.Add(dataprovider);
			source.DefaultProvider = defaultprovider;
			dlg.DataSources.Add(source);
			sqldatasource = source;

			#endregion

			#region MSSQL File

            source = new DataSource("mssql_file", resmgr.GetString("DataSource_MicrosoftSqlServerFile", CultureInfo.InvariantCulture));
			dataprovider = new DataProvider(
				typeof(SqlConnection).Namespace,
                resmgr.GetString("DataProvider_Sql", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Sql_Short", CultureInfo.InvariantCulture),
                resmgr.GetString("DataProvider_Sql_FileDataSource_Description", CultureInfo.InvariantCulture),
				typeof(SqlConnection),
				typeof(SqlFileConnectionUIControl),
				typeof(SqlFileConnectionProperties));
			source.Providers.Add(dataprovider);
			dlg.DataSources.Add(source);

			#endregion

			IUIService uisvc = (IUIService)provider.GetService(typeof(IUIService));
			IWin32Window owner = uisvc != null ? uisvc.GetDialogOwnerWindow() : null;

			if (DataConnectionDialog.Show(dlg, owner) == DialogResult.OK)
			{
				if (providerArgument != null)
				{
					IDictionaryService dictionary = (IDictionaryService)provider.GetService(typeof(IDictionaryService));
					if (dictionary != null)
					{
						try
						{
							dictionary.SetValue(providerArgument, dlg.SelectedDataProvider.Name);
						}
						catch { } // Never fail setting it.
					}
				}
				return dlg.ConnectionString;
			}
			else
			{
				return value;
			}
		}

		#region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			providerArgument = attributes[ProviderArgument];
		}

		#endregion
}
}
