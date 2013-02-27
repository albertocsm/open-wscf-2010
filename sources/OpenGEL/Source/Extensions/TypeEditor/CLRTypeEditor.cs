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
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.RecipeFramework.Library.CodeModel.Editors;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common;
using System.Collections.Specialized;

namespace Microsoft.Practices.RecipeFramework.Extensions.TypeEditor
{
	/// <summary>
	/// TypeEditor used to select Types and create CLRType objects
	/// </summary>
	/// <typeparam name="ConfigType"></typeparam>
	/// <typeparam name="TCLRType"></typeparam>
	public class CLRTypeEditor<TCLRType, ConfigType> : CodeModelEditor
		where ConfigType : ICLRTypeEditorConfig, new()
		where TCLRType : CLRType, new()
	{
		ICLRTypeEditorConfig config = null;

		/// <summary>
		/// Returns the current configurtation object
		/// </summary>
		public ICLRTypeEditorConfig Config
		{
			get
			{
				if (config == null)
				{
					config = new ConfigType();
				}
				return config;
			}
		}


		/// <summary>
		/// Default constructor
		/// </summary>
		public CLRTypeEditor()
		{
			IAttributesConfigurable configurable = this as IAttributesConfigurable;
			if (configurable != null && this.Config != null)
			{
				StringDictionary parameters = new StringDictionary();
				parameters[CodeModelEditor.BrowseKindAttribute] = Config.BrowseKind.ToString();
				parameters[CodeModelEditor.BrowseRootAttribute] = Config.BrowseRoot.ToString();
				if (Config.Filter != null)
				{
					string typeRefName = Config.Filter.FullName + "," + Config.Filter.Assembly.FullName;
					parameters[CodeModelEditor.FilterAttribute] = typeRefName;
				}
				configurable.Configure(parameters);
			}
		}

		/// <summary>
		/// Displays the selection dialog box and returns a <seealso cref="CLRType"/> object
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			object newValue = base.EditValue(context, provider, value);
			if (newValue is EnvDTE.CodeElement)
			{
				EnvDTE.CodeElement codeElement = newValue as EnvDTE.CodeElement;
				TCLRType newCLRType = new TCLRType();
				newCLRType.Init(codeElement.FullName);
				return newCLRType;
			}
			return newValue;
		}
	}

	/// <summary>
	/// CLRTypeEditor configured with default configuration
	/// </summary>
	/// <typeparam name="TCLRType"></typeparam>
	public class CLRTypeEditor<TCLRType> : CLRTypeEditor<TCLRType, DefaultCLRTypeEditorConfig>
		where TCLRType : CLRType, new()
	{
	}

	/// <summary>
	/// CLRTypeEditor configured with default configuration and custom filter
	/// </summary>
	/// <typeparam name="TCLRType"></typeparam>
	/// <typeparam name="TFilter"></typeparam>
	public class CLRTypeEditorWithFilter<TCLRType, TFilter> : CLRTypeEditor<TCLRType, DefaultCLRTypeEditorConfig<TFilter>>
		where TFilter : ICodeModelEditorFilter
		where TCLRType : CLRType, new()
	{
	}

	/// <summary>
	/// CLRTypeEditor configured with default configuration and the CLRType
	/// </summary>
	public class CLRTypeEditor : CLRTypeEditor<CLRType, DefaultCLRTypeEditorConfig>
	{
	}

}
