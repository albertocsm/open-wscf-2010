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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.WebClientFactory.ValueProviders

{
	public class FolderProvider: ValueProvider, IAttributesConfigurable
	{
		private const int SEARCHLEVEL = 4;
        public const string FolderAttribute = "Folder";

        private string _folder;
		
		public override bool OnBeginRecipe(object currentValue, out object newValue)
		{
			if ( string.IsNullOrEmpty(currentValue as string) )
			{
				string basePath = GetService<IConfigurationService>(true).BasePath;
                string probePath = Path.Combine(basePath, _folder);
				int i = 0;
				while (i++ < SEARCHLEVEL && !Directory.Exists(probePath))
				{
                    _folder = @"..\" + _folder;
                    probePath = Path.Combine(basePath, _folder);
				}
				if ( Directory.Exists(probePath) )
				{
					DirectoryInfo info = new DirectoryInfo(probePath);
					newValue = info.FullName;
					return true;
				}
			}
			return base.OnBeginRecipe(currentValue, out newValue);
		}

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey(FolderAttribute))
                {
                    _folder = attributes[FolderAttribute];
                }
            }
        }

        #endregion
    }
}
