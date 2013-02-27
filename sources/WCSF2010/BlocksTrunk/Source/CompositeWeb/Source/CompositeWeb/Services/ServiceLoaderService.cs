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
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Implements the <see cref="IServiceLoaderService"/> interface.
	/// </summary>
	public class ServiceLoaderService : IServiceLoaderService
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ServiceLoaderService"/>.
		/// </summary>
		public ServiceLoaderService()
		{
		}

		#region IServiceLoaderService Members

		/// <summary>
		/// Loads the specified services into the given container.
		/// </summary>
		/// <param name="compositionContainer">The container into which create and add the configured services.</param>
		/// <param name="services">The list of services to load.</param>
		public void Load(CompositionContainer compositionContainer, params ServiceInfo[] services)
		{
			Guard.ArgumentNotNull(compositionContainer, "compositionContainer");

			LoadServices(compositionContainer, services);
		}

		#endregion

		private void LoadServices(CompositionContainer container, ServiceInfo[] services)
		{
			foreach (ServiceInfo service in services)
			{
				LoadService(container, service);
			}
		}

		private void LoadService(CompositionContainer container, ServiceInfo service)
		{
			IServiceCollection serviceCollection;

			switch (service.Scope)
			{
				case ServiceScope.Global:
					serviceCollection = container.RootContainer.Services;
					break;

				case ServiceScope.Module:
					serviceCollection = container.Services;
					break;

				default:
					serviceCollection = container.RootContainer.Services;
					break;
			}

			serviceCollection.AddNew(service.Type, service.RegisterAs);
		}
	}
}