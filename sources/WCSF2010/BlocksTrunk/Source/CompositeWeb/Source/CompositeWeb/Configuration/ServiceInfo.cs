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
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Configuration
{
	/// <summary>
	/// Defines the basic metadata needed to describe a service.
	/// </summary>
	public class ServiceInfo
	{
		private Type _registerAs;
		private ServiceScope _scope;
		private Type _type;

		/// <summary>
		/// Initializes a new instance of a <see cref="ServiceInfo"/>.
		/// </summary>
		public ServiceInfo()
		{
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="ServiceInfo"/> with the given values.
		/// </summary>
		/// <param name="registerAs">The contract of the service.</param>
		/// <param name="type">The type of the service.</param>
		/// <param name="scope">The scope of the service (Global or Module)</param>
		public ServiceInfo(Type registerAs, Type type, string scope)
		{
			Guard.EnumValueIsDefined(typeof (ServiceScope), scope, "scope");

			ServiceScope serviceScope = (ServiceScope) Enum.Parse(typeof (ServiceScope), scope);
			Initialize(registerAs, type, serviceScope);
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="ServiceInfo"/> with the given values.
		/// </summary>
		/// <param name="registerAs">The contract of the service.</param>
		/// <param name="type">The type of the service.</param>
		/// <param name="scope">The scope of the service (Global or Module)</param>
		public ServiceInfo(Type registerAs, Type type, ServiceScope scope)
		{
			Initialize(registerAs, type, scope);
		}

		/// <summary>
		/// Gets or sets the service contract.
		/// </summary>
		public Type RegisterAs
		{
			get { return _registerAs; }
			set { _registerAs = value; }
		}

		/// <summary>
		/// Gets or sets the service type
		/// </summary>
		public Type Type
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// Gets or sets the scope of the service
		/// </summary>
		public ServiceScope Scope
		{
			get { return _scope; }
			set { _scope = value; }
		}

		private void Initialize(Type registerAs, Type type, ServiceScope scope)
		{
			_registerAs = registerAs;
			_type = type;
			_scope = scope;
		}
	}

	/// <summary>
	/// The service's scope.
	/// </summary>
	public enum ServiceScope
	{
		/// <summary>
		/// The service belongs to the root composition container and is available to every module in the application.
		/// </summary>
		Global,

		/// <summary>
		/// The service is scoped to the registering module only.
		/// </summary>
		Module
	}
}