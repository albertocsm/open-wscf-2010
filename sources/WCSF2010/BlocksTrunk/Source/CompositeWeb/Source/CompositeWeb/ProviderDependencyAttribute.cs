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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Indicates that property or parameter is a dependency on type providing a ProviderBase derived type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public sealed class ProviderDependencyAttribute : OptionalDependencyAttribute
	{
		private string _providerGetterProperty;
		private Type _providerHostType;

		/// <summary>
		/// Initializes a new intance of the <see cref="ProviderDependencyAttribute"/>.
		/// </summary>
		/// <param name="providerHostType">The type from which to get the provider instance.</param>
		/// <remarks>Use this constructor when the provider can be get from a "Provider" property on the <paramref name="providerHostType"/>.</remarks>
		public ProviderDependencyAttribute(Type providerHostType)
			: this(providerHostType, "Provider")
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ProviderDependencyAttribute"/>.
		/// </summary>
		/// <param name="providerHostType">The type from which to get the provider instance.</param>
		/// <param name="providerGetterProperty">The name of the property on the <paramref name="providerHostType"/> to retrieve
		/// the provider from.</param>
		public ProviderDependencyAttribute(Type providerHostType, string providerGetterProperty)
		{
			Guard.ArgumentNotNull(providerHostType, "providerHostType");
			Guard.ArgumentNotNullOrEmptyString(providerGetterProperty, "providerGetterProperty");

			PropertyInfo prop = providerHostType.GetProperty(providerGetterProperty, BindingFlags.Static | BindingFlags.Public);
			if (prop == null)
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentCulture, Resources.ProviderDependencyPropertyNotFound, providerHostType.FullName,
					              providerGetterProperty));
			}

			_providerHostType = providerHostType;
			_providerGetterProperty = providerGetterProperty;
		}

		/// <summary>
		/// Gets the name of the property from which to get the provider from.
		/// </summary>
		public string ProviderGetterProperty
		{
			get { return _providerGetterProperty; }
		}

		/// <summary>
		/// Gets the type to query for the provider.
		/// </summary>
		public Type ProviderHostType
		{
			get { return _providerHostType; }
		}

		/// <summary>
		/// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
		/// </summary>
		public override IParameter CreateParameter(Type memberType)
		{
			return new ProviderDependencyParameter(_providerHostType, memberType, _providerGetterProperty);
		}

		#region Nested type: ProviderDependencyParameter

		private class ProviderDependencyParameter : IParameter
		{
			private string _propertyName;
			private Type _providerHostType;
			private Type _providerType;

			public ProviderDependencyParameter(Type providerHostType, Type providerType, string propertyName)
			{
				_providerHostType = providerHostType;
				_providerType = providerType;
				_propertyName = propertyName;
			}

			#region IParameter Members

			public Type GetParameterType(IBuilderContext context)
			{
				return _providerType;
			}

			[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
				Justification = "Validation done by Guard class.")]
			public object GetValue(IBuilderContext context)
			{
				Guard.ArgumentNotNull(context, "context");
				PropertyInfo prop = _providerHostType.GetProperty(_propertyName, BindingFlags.Static | BindingFlags.Public);
				if (prop == null)
				{
					throw new ArgumentException();
				}

				object value = prop.GetGetMethod().Invoke(_providerType, null);
				string id = Guid.NewGuid().ToString();
				value = context.HeadOfChain.BuildUp(context, _providerType, value, id);
				return value;
			}

			#endregion
		}

		#endregion
	}
}