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
namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines the contract for the modules deployed in the application.
	/// </summary>
	public interface IModuleInitializer
	{
		/// <summary>
		/// Noifies the module that it has been loaded.
		/// </summary>
		/// <param name="container">A reference to the <see cref="CompositionContainer"/> where the module has been loaded.</param>
		void Load(CompositionContainer container);

		/// <summary>
		/// Allows the module to process its configuration.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection"/> collection of all the services available to the module.</param>
		/// <param name="moduleConfiguration">The <see cref="Configuration"/> available to the module.</param>
		void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration);
	}
}