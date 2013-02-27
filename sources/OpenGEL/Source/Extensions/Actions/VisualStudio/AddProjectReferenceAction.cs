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
using System.Collections;
using System.ComponentModel;
using System.IO;
using EnvDTE;
using VSLangProj;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using VsWebSite;
using Microsoft.Practices.RecipeFramework.Library;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Adds a reference to a project pointing to another 
    /// project in the same solution. 
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    [ServiceDependency(typeof(DTE))]
    public class AddProjectReferenceAction : Action
    {
        #region Input Properties

        private Project referringProject;
        private string referencedAssembly;
        private Project referencedProject;

        /// <summary>
        /// The project receiving the new reference
        /// </summary>
        [Input(Required = true)]
        public Project ReferringProject
        {
            get { return referringProject; }
            set { referringProject = value; }
        }

        /// <summary>
        /// The assembly reference been added
        /// </summary>
        [Input(Required = false)]
        public string ReferencedAssembly
        {
            get { return referencedAssembly; }
            set { referencedAssembly = value; }
        }

        /// <summary>
        /// The project reference been added
        /// </summary>
        [Input(Required = false)]
        public Project ReferencedProject
        {
            get { return referencedProject; }
            set { referencedProject = value; }
        }

        #endregion

        #region Action Implementation
        /// <summary>
        /// Adds the project reference to the <see cref="ReferencedProject"/>
        /// </summary>
        public override void Execute()
        {
            if(this.referencedProject != null)
            {
                //Add project reference

                if(this.ReferencedProject.UniqueName.Equals(this.ReferringProject.UniqueName, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Do nothing.
                    return;
                }

                // If reference already exists, nothing happens.
                // If referringProject is a VSProject
                VSProject vsProject = referringProject.Object as VSProject;
                if(vsProject != null)
                {
                    if(DteHelper.IsWebProject(referencedProject))
                    {
                        VsWebSite.VSWebSite vsWebSite = (VsWebSite.VSWebSite)(referencedProject.Object);
                        foreach(VsWebSite.WebService webService in vsWebSite.WebServices)
                        {
                            vsProject.AddWebReference(webService.URL);
                        }
                    }
                    else
                    {
                        // No error handling needed here. See documentation for AddProject.
                        vsProject.References.AddProject(referencedProject);
                    }
                }
                else
                {
                    // If the Project recived is not a VsProject
                    // Try it with a webProject
                    VSWebSite webProject = referringProject.Object as VSWebSite;
                    if(webProject != null)
                    {
                        if (DteHelper.IsWebProject(referencedProject))
                        {
                            VSWebSite vsWebSite = (VSWebSite)(referencedProject.Object);
                            foreach(VsWebSite.WebService webService in vsWebSite.WebServices)
                            {
                                webProject.WebReferences.Add(webService.URL, webService.ClassName);
                            }
                        }
                        else
                        {
                            // Check if the reference already exists in the WebProject
                            if(!IsAlreadyReferenced(webProject, referencedProject))
                            {
                                webProject.References.AddFromProject(referencedProject);
                            }
                        }
                    }
                }
            }
            else if(this.referencedAssembly != string.Empty)
            {
                //Add Assembly reference
                VSProject vsProject = referringProject.Object as VSProject;
                string assemblyName = Path.GetFileNameWithoutExtension(this.referencedAssembly);

                if(vsProject != null)
                {
                    if(!IsAlreadyReferenced(vsProject, assemblyName))
                    {
                        vsProject.References.Add(this.referencedAssembly);
                    }
                }
                else
                {
                    VSWebSite webProject = referringProject.Object as VSWebSite;
                    if(webProject != null)
                    {
                        if(!IsAlreadyReferenced(webProject, assemblyName))
                        {
                            webProject.References.AddFromFile(this.referencedAssembly);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //No undo supported as no Remove method exists on the VSProject.References property.
        }

        #endregion

        #region Private Implementation

        private bool IsAlreadyReferenced(VSProject vsProject, string assemblyName)
        {
            foreach(Reference reference in vsProject.References)
            {
                if(reference.Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAlreadyReferenced(VSWebSite webProject, string assemblyName)
        {
            foreach(AssemblyReference reference in webProject.References)
            {
                if(reference.Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAlreadyReferenced(VSWebSite webProject, Project referencedProject)
        {
            foreach(AssemblyReference reference in webProject.References)
            {
                if(reference.Name.Equals(ReferencedProject.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
