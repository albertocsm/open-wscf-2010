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
using System.ComponentModel;
using Microsoft.Practices.Common;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter class that can group other <see cref="StringConverter"/> classes. 
    /// The converter types should be added as an attribute with the prefix name 'AddConverter' and any ordinal character.
    /// </summary>
    public class AgregatorStringConverter : StringConverter, IAttributesConfigurable
    {
        #region Constants

        /// <summary>
        /// Add this profix to all your agregated converters as a prefix attribute.
        /// Usage: AddConverter1="MyConverter1Type, MyAssembly" AddConverter2="MyConverter2Type, MyAssembly" ...
        /// </summary>
        public const string AddConverterPrefixAttribute = "AddConverter";

        #endregion

        #region Private Fields

        private List<StringConverter> converters;
        private StringDictionary configuredAttributes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AgregatorStringConverter"/> class.
        /// </summary>
        public AgregatorStringConverter()
            : base()
        {
            this.converters = new List<StringConverter>();
        }

        #endregion

        #region StringConverter overrides

        /// <summary>
        /// Gets a value indicating whether this converter can convert an object in the given source type to a string using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you wish to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (!converter.CanConvertFrom(context, sourceType))
                {
                    return false;
                }
            } 
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (!converter.CanConvertTo(context, destinationType))
                {
                    return false;
                }
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the specified value object to a <see cref="T:System.String"></see> object.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion could not be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            EnsureConfiguration(context);
            object result = base.ConvertFrom(context, culture, value);
            foreach (StringConverter converter in this.converters)
            {
                result = converter.ConvertFrom(context, culture, result);
            }
            return result;
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        /// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            EnsureConfiguration(context);
            object result = base.ConvertTo(context, culture, value, destinationType);
            foreach (StringConverter converter in this.converters)
            {
                result = converter.ConvertTo(context, culture, result, destinationType);
            }
            return result;
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter"></see> is associated with, using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary"></see> of new property values.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> representing the given <see cref="T:System.Collections.IDictionary"></see>, or null if the object cannot be created. This method always returns null.
        /// </returns>
        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetCreateInstanceSupported(context))
                {
                    return converter.CreateInstance(context, propertyValues);
                }
            }
            return base.CreateInstance(context, propertyValues);
        }

        /// <summary>
        /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value; otherwise, false.
        /// </returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetCreateInstanceSupported(context))
                {
                    return true;
                }
            }
            return base.GetCreateInstanceSupported(context);
        }

        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="value">An <see cref="T:System.Object"></see> that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"></see> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> with the properties that are exposed for this data type, or null if there are no properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetPropertiesSupported(context))
                {
                    return converter.GetProperties(context, value, attributes);
                }
            }
            return base.GetProperties(context, value, attributes);
        }

        /// <summary>
        /// Returns whether this object supports properties, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"></see> should be called to find the properties of this object; otherwise, false.
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetPropertiesSupported(context))
                {
                    return true;
                }
            }
            return base.GetPropertiesSupported(context);
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
        /// </returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetStandardValuesSupported(context))
                {
                    return converter.GetStandardValues(context);
                }
            }
            return base.GetStandardValues(context);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetStandardValuesExclusive(context))
                {
                    return true;
                }
            }
            return base.GetStandardValuesExclusive(context);
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (converter.GetStandardValuesSupported(context))
                {
                    return true;
                }
            }
            return base.GetStandardValuesSupported(context);
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            EnsureConfiguration(context);
            foreach (StringConverter converter in this.converters)
            {
                if (!converter.IsValid(context, value))
                {
                    return false;
                }
            }
            return base.IsValid(context, value);
        }

        /// <summary>
        /// Called when <see cref="IAttributesConfigurable.Configure"/> method is called.
        /// </summary>
        /// <remarks>
        /// Override this method if you inherit from this class and want to add some converters to 
        /// the attributes collection.
        /// </remarks>
        /// <param name="attributes">The attributes.</param>
        public virtual void OnConfigure(StringDictionary attributes)
        {
            this.configuredAttributes = attributes;
        }

        #endregion

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(StringDictionary attributes)
        {
            this.OnConfigure(attributes);
        }

        #endregion

        #region Private Implementation

        private void EnsureConfiguration(ITypeDescriptorContext context)
        {
            // check if we already loaded the converters
            if (context == null || 
                this.converters.Count > 0)
            {
                return;
            }

            ITypeResolutionService resolution = (ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));

            if (resolution != null)
            {
                // look for converter types and load them
                foreach (DictionaryEntry attribute in this.configuredAttributes)
                {
                    if (attribute.Key.ToString().StartsWith(AddConverterPrefixAttribute, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Type converterType = resolution.GetType(attribute.Value.ToString(), true, true);
                        ReflectionHelper.EnsureAssignableTo(converterType, typeof(StringConverter));
                        // create an instance of this converter
                        object converter = Activator.CreateInstance(converterType);
                        // check if this converter implements IAttributesConfigurable
                        if (ReflectionHelper.IsAssignableTo(typeof(IAttributesConfigurable), converter))
                        {
                            IAttributesConfigurable configure = converter as IAttributesConfigurable;
                            configure.Configure(this.configuredAttributes);
                        }
                        // store the StringConverter
                        this.converters.Add((StringConverter)converter);
                    }
                }
                // we reverse the collection because the attributes collection 
                // will be ordered backward in the AgregatorStringConverter element .
                this.converters.Reverse();
            }
        }

        #endregion
    }
}
