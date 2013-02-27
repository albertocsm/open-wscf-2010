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
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.ComponentModel.Design;
using Microsoft.Practices.ComponentModel; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.ValueProviders.VisualStudio
{
    /// <summary>
    /// Returns a Project property value stored on the Globals section
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IDictionaryService))]
    public class ProjectGlobalsEntryProvider : ValueProvider, IAttributesConfigurable
    {
        #region Constants & Fields

        /// <summary>
        /// The attribute name that specify the project that host the Globals respository.
        /// </summary>
        public const string ProjectAttribute = "ProjectArgument";
        
        /// <summary>
        /// The attribute name that specify the property name that refers 
        /// to the value stored in the <see cref="ProjectAttribute"/> Globals.
        /// </summary>
        public const string PropertyNameAttribute = "PropertyName";

        /// <summary>
        /// The default value will be takne from the solution globals store.
        /// </summary>
        public const string UseSolutionGlobalsDefaultValueAttribute = "UseSolutionGlobalsDefaultValue";

        /// <summary>
        /// The default value in case the current stored value is null or empty.
        /// </summary>
        public const string DefaultPropertyValueAttribute = "DefaultPropertyValue";

        private string projectArgumentName;
        private string propertyName;
        private string defaultPropertyValue;
        private bool useSolutionGlobalsDefaultValue;

        #endregion

        #region ValueProvider Implementation

        /// <summary>
        /// Contains code that will be called when recipe execution begins. This is the first method in the lifecycle.
        /// </summary>
        /// <param name="currentValue">An <see cref="T:System.Object"/> that contains the current value of the argument.</param>
        /// <param name="newValue">When this method returns, contains an <see cref="T:System.Object"/> that contains
        /// the new value of the argument, if the returned value
        /// of the method is <see langword="true"/>. Otherwise, it is ignored.</param>
        /// <returns>
        /// 	<see langword="true"/> if the argument value should be replaced with
        /// the value in <paramref name="newValue"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>By default, always returns <see langword="false"/>, unless overriden by a derived class.</remarks>
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            IDictionaryService dictservice = (IDictionaryService)
                ServiceHelper.GetService(this, typeof(IDictionaryService));

            Project project = dictservice.GetValue(this.projectArgumentName) as Project;

            if(project != null)
            {
                if (!String.IsNullOrEmpty(this.propertyName))
                {
                    if (project.Globals.get_VariableExists(this.propertyName))
                    {
                        newValue = project.Globals[this.propertyName];
                        return true;
                    }
                    if (!string.IsNullOrEmpty(this.defaultPropertyValue))
                    {
                        newValue = dictservice.GetValue(this.defaultPropertyValue);
                        return true;
                    }
                    if (this.useSolutionGlobalsDefaultValue)
                    {
                        DTE dte = (DTE)this.GetService(typeof(DTE));
                        if (dte.Solution.Globals.get_VariableExists(this.propertyName))
                        {
                            newValue = dte.Solution.Globals[this.propertyName];
                            return true;
                        }
                    }
                }
            }

            newValue = null;
            return false;
        }

        /// <summary>
        /// Called when [before actions]. 
        /// This method will update the <see cref="PropertyNameAttribute"/> in the Globals repository of the 
        /// <see cref="ProjectAttribute"/>.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                IDictionaryService dictservice = (IDictionaryService)
                ServiceHelper.GetService(this, typeof(IDictionaryService));
                Project project = dictservice.GetValue(this.projectArgumentName) as Project;
                if (!String.IsNullOrEmpty(this.propertyName))
                {
                    if (project != null)
                    {
                        project.Globals[this.propertyName] = currentValue;
                    }
                    if (this.useSolutionGlobalsDefaultValue)
                    {
                        DTE dte = (DTE)this.GetService(typeof(DTE));
                        dte.Solution.Globals[this.propertyName] = currentValue;
                    }
                }
            }
            newValue = currentValue;
            return false;
        }

        #endregion

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if(attributes.ContainsKey(ProjectAttribute))
            {
                this.projectArgumentName = attributes[ProjectAttribute];
            }

            if(attributes.ContainsKey(PropertyNameAttribute))
            {
                this.propertyName = attributes[PropertyNameAttribute];
            }

            if (attributes.ContainsKey(DefaultPropertyValueAttribute))
            {
                this.defaultPropertyValue = attributes[DefaultPropertyValueAttribute];
            }

            if (attributes.ContainsKey(UseSolutionGlobalsDefaultValueAttribute))
            {
                Boolean.TryParse(attributes[UseSolutionGlobalsDefaultValueAttribute], out this.useSolutionGlobalsDefaultValue);
            }            
        }

        #endregion
    }
}
