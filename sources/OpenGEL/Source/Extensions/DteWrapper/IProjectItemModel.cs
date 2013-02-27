//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// Interface that provides an abstraction to a <see cref="ProjectItem"/>
    /// </summary>
    public interface IProjectItemModel
    {
        /// <summary>
        /// Returns the underlying <see cref="ProjectItem" />
        /// </summary>
        object ProjectItem { get; }

        /// <summary>
        /// Returns the name of the folder
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the path of the folder
        /// </summary>
        string ItemPath { get; }
    }
}
