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
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Library.CodeModel.Converters;

namespace Microsoft.Practices.RecipeFramework.Extensions.TypeEditor
{
	/// <summary>
	/// Objects of this class represent a CLR Type
	/// </summary>
	[DesignTimeVisible(true)]
	[DefaultValue("Empty")]
	[Editor(typeof(CLRTypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(CLRTypeConverter))]
	public class CLRType: Component
	{
		string typeName;

		/// <summary>
		/// Default constructor
		/// </summary>
		public CLRType()
		{
		}

		/// <summary>
		/// Public initializer method, called from TypeConverters
		/// </summary>
		/// <param name="typeName"></param>
		public void Init(string typeName)
		{
			this.typeName = typeName;
		}

		/// <summary>
		/// Override to display the type full name in the UI and properties window
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return typeName;
		}
	}
}
