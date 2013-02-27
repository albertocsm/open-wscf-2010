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

using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework.Library.Templates.Actions;

#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Action that unfolds a Vs Template
    /// </summary>
    public class UnfoldTemplateIfProjectNotExistsAction : UnfoldTemplateAction
    {
        #region Input Properties
        private bool isWeb = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is web.
        /// </summary>
        /// <value><c>true</c> if this instance is web; otherwise, <c>false</c>.</value>
        [Input(Required=false)]
        public bool IsWeb
        {
            get { return isWeb; }
            set { isWeb = value; }
        }
        #endregion

        #region IAction Members

        /// <summary>
        /// Unfolds the template
        /// </summary>
        public override void Execute()
        {
            Project project = FindProjectByName(ItemName, isWeb);

            if(project != null)
            {
                NewItem = project;
            }
            else
            {
                base.Execute();   
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        }

        #endregion

        #region Private Implementation
        private Project FindProjectByName(string name, bool isWeb)
        {
            DTE dte = (DTE)GetService(typeof(DTE));
            Project project = null;

            if(!isWeb)
            {
                project = DteHelperEx.FindProjectByName(dte, name, isWeb);
            }
            else
            {
                foreach(Project projectTemp in dte.Solution.Projects)
                {
                    if(projectTemp.Name.Contains(name))
                    {
                        project = projectTemp;
                        break;
                    }

                    if(projectTemp.ProjectItems != null)
                    {
                        Project projectTemp1 = FindProjectByName(projectTemp.ProjectItems, name);
                        if(projectTemp1 != null)
                        {
                            project = projectTemp1;
                            break;
                        }
                    }
                }
            }

            return project;
        }

        private Project FindProjectByName(ProjectItems items, string name)
        {
            foreach(ProjectItem item1 in items)
            {
                if((item1.Object is Project) && (((Project)item1.Object).Name.Contains(name)))
                {
                    return (item1.Object as Project);
                }

                if(item1.ProjectItems != null)
                {
                    Project project1 = FindProjectByName(item1.ProjectItems, name);
                    if(project1 != null)
                    {
                        return project1;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
