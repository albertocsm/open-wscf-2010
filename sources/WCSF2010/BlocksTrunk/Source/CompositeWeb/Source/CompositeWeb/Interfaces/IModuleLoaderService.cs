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
	/// Declares a service which loads the modules into the application.
	/// </summary>
	public interface IModuleLoaderService
	{
		/// <summary>
		/// Loads the specified modules into the given container.
		/// </summary>
		/// <param name="rootContainer">The <see cref="CompositionContainer"/> into which create the container for the modules.</param>
		/// <param name="modulesInfo">The list of modules to load.</param>
		void Load(CompositionContainer rootContainer, IModuleInfo[] modulesInfo);

		/// <summary>
		/// Gets the <see cref="IModuleInitializer"/> for the speficied module name.
		/// </summary>
		/// <param name="moduleName">The name of the module to search for the initializer.</param>
		/// <returns>An <see cref="IModuleInitializer"/> instance.</returns>
		IModuleInitializer FindInitializer(string moduleName);
	}
}