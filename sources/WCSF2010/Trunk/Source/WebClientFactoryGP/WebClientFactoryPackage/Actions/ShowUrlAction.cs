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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using System.IO;
using Microsoft.Practices.RecipeFramework;
#endregion

namespace Microsoft.Practices.WebClientFactory.Actions
{
    /// <summary>
    /// Action that deletes a folder
    /// </summary>
    public class ShowUrlAction : ConfigurableAction
    {
        private string _rawUrl;
        private bool _ShowDocs;

        [Input(Required = true)]
        public string RawUrl
        {
            get { return _rawUrl; }
            set { _rawUrl = value; }
        }

        [Input(Required = true)]
        public bool ShowDocumentation
        {
            get { return _ShowDocs; }
            set { _ShowDocs = value; }
        }

        public override void Execute()
        {
            if (_ShowDocs)
            {
                object customOut = null;
                DTE dte = GetService<DTE>();
                dte.ExecuteCommand("View.WebBrowser", String.Empty);
                object customIn = _rawUrl;
                dte.Commands.Raise("{E8B06F44-6D01-11D2-AA7D-00C04F990343}", 207, ref customIn, ref customOut);
            }
        }

        public override void Undo()
        {
        }
    }

}
