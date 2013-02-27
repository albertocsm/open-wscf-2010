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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.ComponentModel.Design;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.WebClientFactory.Properties;
#endregion

namespace Microsoft.Practices.WebClientFactory.Actions
{

    [ServiceDependency(typeof(ITypeResolutionService))]
    public class ExtendedTextTemplateAction : T4Action
    {
        // Fields
        private string templateExpression;
        private string rendered;
        private string template;

        // Methods
        public override void Execute()
        {
            if (!String.IsNullOrEmpty(this.templateExpression))
            {
                this.Template = (string)ExpressionEvaluationHelper.EvaluateExpression(
                                (IDictionaryService)GetService(typeof(IDictionaryService)),
                                this.templateExpression);
            }
            string path = this.Template;
            string templateCode = string.Empty;
            if (path == null)
            {
                throw new ArgumentNullException("Template");
            }
            string templateBasePath = base.GetTemplateBasePath();
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(templateBasePath, path);
            }
            path = new FileInfo(path).FullName;
            if (!path.StartsWith(templateBasePath))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,Resources.TemplateNotFoundMessage));
            }
            templateCode = File.ReadAllText(path);
            this.Content = base.Render(templateCode, path);
        }

        public override void Undo()
        {
        }


        /// <summary>
        /// Gets or sets the Template path expression
        /// </summary>
        /// <value>The folder.</value>
        [Input(Required = false)]
        public string TemplateExpression
        {
            get { return templateExpression; }
            set { templateExpression = value; }
        }

        // Properties
        [Output]
        public string Content
        {
            get
            {
                return this.rendered;
            }
            set
            {
                this.rendered = value;
            }
        }

        [Input(Required = true)]
        public string Template
        {
            get
            {
                return this.template;
            }
            set
            {
                this.template = value;
            }
        }
    }

}
