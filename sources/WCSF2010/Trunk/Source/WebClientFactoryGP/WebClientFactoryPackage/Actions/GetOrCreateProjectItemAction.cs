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
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio;

namespace Microsoft.Practices.WebClientFactory.Actions
{
    public class GetOrCreateProjectItemAction : ConfigurableAction
    {
        // Fields
        private string itemName;
        private Project project;
        private string template;
        private ProjectItem projectItem;
        private bool recursive = false;

        // Methods
        public override void Execute()
        {
            ProjectItem item = DteHelperEx.FindItemByName(this.Project.ProjectItems, this.ItemName, recursive);
            if (item == null)
            {
                TextTemplateAction templateAction = new TextTemplateAction();
                templateAction.Template = this.Template;

                templateAction.Site = this.Site;
                templateAction.Execute();
                string templateContent = templateAction.Content;

                AddItemFromStringToProjectItemByNameAction addItemAction = new AddItemFromStringToProjectItemByNameAction();
                addItemAction.Content = templateContent;
                addItemAction.Project = this.Project;
                addItemAction.TargetFileName = this.ItemName;
                addItemAction.Site = this.Site;
                addItemAction.Execute();

                item = addItemAction.ProjectItem;
            }

            this.ProjectItem = item;
        }

        public override void Undo()
        {
            this.ProjectItem = null;
        }

        // Properties
        [Input(Required = true)]
        public string ItemName
        {
            get
            {
                return this.itemName;
            }
            set
            {
                this.itemName = value;
            }
        }

        [Input(Required = true)]
        public Project Project
        {
            get
            {
                return this.project;
            }
            set
            {
                this.project = value;
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

        [Input(Required = false)]
        public bool Recursive
        {
            get
            {
                return this.recursive;
            }
            set
            {
                this.recursive = value;
            }
        }

        [Output]
        public ProjectItem ProjectItem
        {
            get
            {
                return this.projectItem;
            }
            set
            {
                this.projectItem = value;
            }
        }
    }
}
