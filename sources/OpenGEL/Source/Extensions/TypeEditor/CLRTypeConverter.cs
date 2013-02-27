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
using Microsoft.Practices.RecipeFramework.Library.CodeModel.Converters;
using System.ComponentModel;

namespace Microsoft.Practices.RecipeFramework.Extensions.TypeEditor
{
	/// <summary>
	/// TypeConverter used to convert from string and CLRType and viceversa
	/// </summary>
	public class CLRTypeConverter<TCLRType>: TypeConverter
		where TCLRType: CLRType, new()
	{
		/// <summary>
		/// Tells the designer that we know how to convert from CLRType
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
		{
			if (typeof(string) == sourceType)
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Tells the designer that we know how to convert from CLRType
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				TCLRType clrType = new TCLRType();
				clrType.Init((string)value);
				return clrType;
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Tells the designer that we know how to convert to CLRType
		/// </summary>
		/// <param name="context"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
		{
			if (typeof(TCLRType) == destinationType)
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts from string to CLRType
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value is TCLRType)
			{
				return ((TCLRType)value).ToString();
			}
			else if (value is string)
			{
				TCLRType newValue = new TCLRType();
				newValue.Init((string)value);
				return newValue;
			}
			else if (value == null && destinationType == typeof(TCLRType))
			{
				return new TCLRType();
			}
			else if (value == null && destinationType == typeof(string))
			{
				return string.Empty;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Tells the desinger if the current value is valid
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
		{
			return true;
		}

		/// <summary>
		/// Tells the designer that we support CreateInstance
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		/// <summary>
		/// Creates a new instance of <seealso cref="CLRType"/>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="propertyValues"></param>
		/// <returns></returns>
		public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			TCLRType clrType = new TCLRType();
			return clrType;
		}

	}

	/// <summary>
	/// Default TypeConverter for <seealso cref="CLRType"/>
	/// </summary>
	public class CLRTypeConverter : CLRTypeConverter<CLRType>
	{
	}

}
