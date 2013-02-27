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
namespace Microsoft.Practices.CompositeWeb.ObjectBuilder
{
	/// <summary>
	/// Enumeration to represent the object builder stages used by
	/// the WCSF builder.
	/// </summary>
	public enum WCSFBuilderStage
	{
		/// <summary>
		/// Strategies in this stage run before everything else. This is where
		/// type mapping is performed.
		/// </summary>
		TypeMapping,

		/// <summary>
		/// Strategies in this stage run before creation. Typical work done in this stage might
		/// include strategies that use reflection to set policies into the context that other
		/// strategies would later use.
		/// </summary>
		PreCreation,

		/// <summary>
		/// Strategies in this stage create objects. Typically you will only have a single policy-driven
		/// creation strategy in this stage.
		/// </summary>
		Creation,

		/// <summary>
		/// Strategies in this stage work on created objects. Typical work done in this stage might
		/// include setter injection and method calls.
		/// </summary>
		Initialization,

		/// <summary>
		/// Strategies in this stage work on objects that are already initialized. Typical work done in
		/// this stage might include looking to see if the object implements some notification interface
		/// to discover when its initialization stage has been completed.
		/// </summary>
		PostInitialization
	}
}