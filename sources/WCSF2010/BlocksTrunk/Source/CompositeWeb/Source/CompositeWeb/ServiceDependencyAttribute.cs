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
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Indicates that property or parameter is a dependency on a service and
	/// should be dependency injected when the class is put into a <see cref="CompositionContainer"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public sealed class ServiceDependencyAttribute : OptionalDependencyAttribute
	{
		private Type _type;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDependencyAttribute"/> class.
		/// </summary>
		public ServiceDependencyAttribute()
		{
		}

		/// <summary>
		/// Gets or sets the type of the service the property expects.
		/// </summary>
		public Type Type
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
		/// </summary>
		public override IParameter CreateParameter(Type memberType)
		{
			return new ServiceDependencyParameter(_type ?? memberType, Required);
		}

		#region Nested type: ServiceDependencyParameter

		private class ServiceDependencyParameter : IParameter
		{
			private bool _ensureExists;
			private Type _serviceType;

			public ServiceDependencyParameter(Type serviceType, bool ensureExists)
			{
				_serviceType = serviceType;
				_ensureExists = ensureExists;
			}

			#region IParameter Members

			public Type GetParameterType(IBuilderContext context)
			{
				return _serviceType;
			}

			[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
				Justification = "Validation done by Guard class.")]
			public object GetValue(IBuilderContext context)
			{
				Guard.ArgumentNotNull(context, "context");
				CompositionContainer container =
					(CompositionContainer) context.Locator.Get(new DependencyResolutionLocatorKey(typeof (CompositionContainer), null));
				return container.Services.Get(_serviceType, _ensureExists);
			}

			#endregion
		}

		#endregion
	}
}