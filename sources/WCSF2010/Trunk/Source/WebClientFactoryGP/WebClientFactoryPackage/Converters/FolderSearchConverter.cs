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
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Common;
using System.IO;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.WebClientFactory.Converters
{
    public class FolderSearchValidator : TypeConverter, IAttributesConfigurable
    {
        private string[] _files = null;

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (_files == null) return true;

            string selectedPath = (string)value;

            if (Directory.Exists((string)selectedPath) == false)
                return false;

            foreach (string file in _files)
            {
                if (File.Exists(Path.Combine(selectedPath, file)) == false)
                    return false;
            }

            return true;
        }

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                string fileList = attributes["Files"];
                if (String.IsNullOrEmpty(fileList) == false)
                    _files = fileList.Split(';');
            }
        }
    }
}
