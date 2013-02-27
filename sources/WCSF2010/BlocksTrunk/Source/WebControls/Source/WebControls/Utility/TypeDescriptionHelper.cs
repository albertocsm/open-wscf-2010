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
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.Web.UI.WebControls.Properties;

namespace Microsoft.Practices.Web.UI.WebControls.Utility
{
	/// <summary>
	/// Utility class to support reflection operations used by <see cref="ObjectContainerDataSourceView"/>.
	/// </summary>
    public static class TypeDescriptionHelper
    {
		/// <summary>
		/// Searches for a valid property.
		/// </summary>
		/// <param name="properties">The <see cref="PropertyDescriptorCollection"/> to search.</param>
		/// <param name="propertyName">The name of the property to look for.</param>
		/// <returns>A valid <see cref="PropertyDescriptor"/> for the requested property name.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="propertyName"/> is null.</exception>
		/// <exception cref="InvalidOperationException">The property is not present in the collection.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static PropertyDescriptor GetValidProperty(PropertyDescriptorCollection properties, string propertyName) 
        {
            Guard.ArgumentNotNull(properties, "properties");

            PropertyDescriptor property = properties.Find(propertyName, false);
            Guard.PropertyNotNull(property, propertyName);
            return property;
        }

		/// <summary>
		/// Sets the properties values on the <paramref name="existing"/> object from the specified values <see cref="IDictionary"/>.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> object with the name/value pairs for the properties.</param>
		/// <param name="existing">The object to set the properties to.</param>
		/// <exception cref="InvalidOperationException">Cannot set the value of a read-only property.</exception>
		/// <exception cref="ArgumentNullException">Either the <paramref name="values"/> or <paramref name="existing"/> arguments are null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static void BuildInstance(IDictionary values, object existing)
        {
            Guard.ArgumentNotNull(values, "values");
            Guard.ArgumentNotNull(existing, "existing");

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(existing);
            foreach (DictionaryEntry entry in values)
            {
                string propertyName = entry.Key.ToString();
                PropertyDescriptor property = GetValidProperty(properties, propertyName);
                if (property.IsReadOnly)
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.PropertyIsReadOnly, propertyName));

                object value = GetObjectValue(entry.Value, property.PropertyType, propertyName);
                property.SetValue(existing, value);
            }
        }

		/// <summary>
		/// Checks that the type <paramref name="item"/> object is the same as the type specified by <paramref name="expectedType"/>.
		/// </summary>
		/// <param name="item">An object to check its type.</param>
		/// <param name="expectedType">The expected type for the object.</param>
		/// <exception cref="InvalidOperationException">The type of the item object is not the expected type.</exception>
		/// <exception cref="ArgumentNullException">Either the <paramref name="item"/> or the <paramref name="expectedType"/> arguments ares null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static void ThrowIfInvalidType(object item, Type expectedType)
        {
            Guard.ArgumentNotNull(item, "item");
            Guard.ArgumentNotNull(expectedType, "expectedType");
            
            if (!expectedType.IsAssignableFrom(item.GetType()))
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidObjectType, expectedType.FullName, item.GetType().FullName));
        }

		/// <summary>
		/// Checks that the specified type has a default constructor.
		/// </summary>
		/// <param name="type">The type to check for.</param>
		/// <exception cref="InvalidOperationException">The specified type has not a default constructor.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static void ThrowIfNoDefaultConstructor(Type type)
        {
            Guard.ArgumentNotNull(type, "type");

            if (type.GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.DefaultConstructorMissing, type.FullName));
        }

		/// <summary>
		/// Converts an object value to the specified type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type to convert the value to.</param>
		/// <param name="paramName">The name of the parameter holding the value.</param>
		/// <returns>The converted value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static object GetObjectValue(object value, Type targetType, string paramName)
        {
            Guard.ArgumentNotNull(targetType, "targetType");

            if ((value == null) || targetType.IsInstanceOfType(value))
                return value;

            value = TransformType(value, targetType, paramName);
            return value;
        }

        private static object TransformType(object value, Type targetType, string paramName)
        {
            Guard.ArgumentNotNull(targetType, "targetType");

            string stringValue = value as string;
            if (stringValue == null)
                return value;

            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            try
            {
				value = converter.ConvertFromString(stringValue);
            }
            catch
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.CannotConvertType, paramName, typeof(string).FullName, targetType.FullName));
            }
            return value;
        }
    }
}
