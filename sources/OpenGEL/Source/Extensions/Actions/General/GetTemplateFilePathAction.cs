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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.General
{
    /// <summary>
    /// Action that gets the template path for the guidance package
    /// </summary>
    public class GetTemplateFilePathAction : ConfigurableAction
    {
        #region Input Properties
        private string templateFile;

        /// <summary>
        /// Gets or sets the template file.
        /// </summary>
        /// <value>The template file.</value>
        [Input(Required = true)]
        public string TemplateFile
        {
            get { return templateFile; }
            set { templateFile = value; }
        } 
        #endregion

        #region Output Properties
        private string templateFilePath;

        /// <summary>
        /// Gets or sets the template file path.
        /// </summary>
        /// <value>The template file path.</value>
        [Output]
        public string TemplateFilePath
        {
            get { return templateFilePath; }
            set { templateFilePath = value; }
        } 
        #endregion

        #region ConfigurableAction Implementation
        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            string templatePath = new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;

            TemplateFilePath = Path.Combine(templatePath, templateFile);
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        } 
        #endregion

        #region Private Implementation
        /// <summary>
        /// Gets the base path.
        /// </summary>
        /// <returns></returns>
        protected string GetBasePath()
        {
            TypeResolutionService service = this.GetService(typeof(ITypeResolutionService)) as TypeResolutionService;
            return service.BasePath;
        } 
        #endregion
    }
}
