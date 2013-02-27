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

namespace Microsoft.Practices.RecipeFramework.Extensions.TypeEditor
{
	/// <summary>
	/// Default configuration for CLRTypeEditor
	/// </summary>
	public class DefaultCLRTypeEditorConfig<TFilter>: ICLRTypeEditorConfig
		where TFilter: ICodeModelEditorFilter
	{
		#region ICLRTypeEditorConfig Members

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.BrowseRoot"/>
		/// </summary>
		public virtual CodeModelEditor.BrowseRoot BrowseRoot
		{
			get { return CodeModelEditor.BrowseRoot.Default; }
		}

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.BrowseKind"/>
		/// </summary>
		public virtual CodeModelEditor.BrowseKind BrowseKind
		{
			get { return CodeModelEditor.BrowseKind.Class; }
		}

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.Filter"/>
		/// </summary>
		public virtual Type Filter
		{
			get { return typeof(TFilter); }
		}

		#endregion
	}

	/// <summary>
	/// Default configuration for CLRTypeEditor
	/// </summary>
	public class DefaultCLRTypeEditorConfig : ICLRTypeEditorConfig
	{
		#region ICLRTypeEditorConfig Members

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.BrowseRoot"/>
		/// </summary>
		public CodeModelEditor.BrowseRoot BrowseRoot
		{
			get { return CodeModelEditor.BrowseRoot.Default; }
		}

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.BrowseKind"/>
		/// </summary>
		public CodeModelEditor.BrowseKind BrowseKind
		{
			get { return CodeModelEditor.BrowseKind.Class; }
		}

		/// <summary>
		/// <seealso cref="ICLRTypeEditorConfig.Filter"/>
		/// </summary>
		public Type Filter
		{
			get { return null; }
		}

		#endregion
	}
}
