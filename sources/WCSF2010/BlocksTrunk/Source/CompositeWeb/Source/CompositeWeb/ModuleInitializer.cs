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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Defines the base implementation for the <see cref="IModuleInitializer"/>.
	/// </summary>
	public abstract class ModuleInitializer : IModuleInitializer
	{
		#region IModuleInitializer Members

		/// <summary>
		/// Override to perform operation when the module is loaded.
		/// </summary>
		/// <param name="container">The <see cref="CompositionContainer"/> where the module has been loaded.</param>
		public virtual void Load(CompositionContainer container)
		{
			Guard.ArgumentNotNull(container, "container");
		}

		/// <summary>
		/// Override to process the module configuration.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection"/> collection of all the services available to the module.</param>
		/// <param name="moduleConfiguration">The <see cref="Configuration"/> available to the module.</param>
		public virtual void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
		{
		}

		#endregion
	}
}