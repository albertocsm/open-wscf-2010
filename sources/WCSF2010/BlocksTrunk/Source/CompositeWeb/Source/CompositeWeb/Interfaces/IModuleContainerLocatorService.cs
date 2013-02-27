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
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.CompositeWeb.Interfaces
{
	/// <summary>
	/// Defines a service to locate the <see cref="CompositionContainer"/> of a module.
	/// </summary>
	public interface IModuleContainerLocatorService
	{
		/// <summary>
		/// Gets the <see cref="CompositionContainer"/> for the module the requested url belongs to.
		/// </summary>
		/// <param name="relativeRequestUrl">The requested url.</param>
		/// <returns>A <see cref="CompositionContainer"/> instance.</returns>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
		CompositionContainer GetContainer(string relativeRequestUrl);
	}
}