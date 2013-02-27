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
using System.Diagnostics;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE80;

namespace Microsoft.Practices.WebClientFactory.ValueProviders
{
    /// <summary>
    /// ValueProvider that returns the running version of Visual Studio
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class UnitTestFrameworkVersionProvider : ValueProvider
    {
        /// <summary>
        /// Sets the newValue to the first selected project
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE vs = (DTE)GetService(typeof(DTE));
            newValue = "9.0.0.0";
            string vsVersion = vs.Version;
            if (!string.IsNullOrEmpty(vsVersion) && vsVersion.IndexOf(".") > 0)
            {
                // VS Version takes the form of 9.0 for example
                newValue = vsVersion.Substring(0, vsVersion.IndexOf(".")) + ".0.0.0";
            }
            if (newValue != null && newValue as string != currentValue as string)
            {
                return true;
            }
            return false;
        }
    }
}
