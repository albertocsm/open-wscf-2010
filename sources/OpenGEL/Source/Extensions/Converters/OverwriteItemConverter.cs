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
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.Collections.Specialized;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Microsoft.Practices.RecipeFramework.Library;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.Xml;

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter class that gives the user the option to overwrite  
    /// the specified item if it already exists. 
    /// </summary>
    public class OverwriteItemConverter : StringConverter, IAttributesConfigurable
    {
        /// <summary>
        /// The proyect where is located the selected item
        /// </summary>
        public const string ProjectArgument = "ProjectArgument";

        /// <summary>
        /// Postfix that will be added to the item to overwrite.
        /// </summary>
        public const string ItemPostfixArgument = "ItemPostfix";

        /// <summary>
        /// Validates the specified argument instead of the current argument value.
        /// </summary>
        public const string MonitorArgument = "MonitorArgument";

        private string projectArgument;
        private string itemPostfixArgument;
        private string monitorArgument;

        // state values
        private bool storedItemStatus = false;
        private string storedItemName = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OverwriteItemConverter"/> class.
        /// </summary>
        public OverwriteItemConverter()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OverwriteItemConverter"/> class.
        /// </summary>
        /// <param name="projectArgument">The project argument.</param>
        public OverwriteItemConverter(string projectArgument)
            : this(projectArgument, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OverwriteItemConverter"/> class.
        /// </summary>
        /// <param name="projectArgument">The project argument.</param>
        /// <param name="itemPostfix">The item postfix.</param>
        public OverwriteItemConverter(string projectArgument, string itemPostfix)
        {
            this.projectArgument = projectArgument;
            this.itemPostfixArgument = itemPostfix;
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The context parameter is null.</exception>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return IsValid((IServiceProvider)context, value);
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="service">The <see cref="IServiceProvider"/> value.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The context parameter is null.</exception>
        public virtual bool IsValid(IServiceProvider service, object value)
        {
            // validate input
            Guard.ArgumentNotNull(service, "service");
            // set return value
            bool overwrite = true;

            // check if there is any monitored argument to use instead of the current value 
            IDictionaryService dictionary = (IDictionaryService)service.GetService(typeof(IDictionaryService));
            if(this.monitorArgument != null)
            {
                value = dictionary.GetValue(this.monitorArgument);
            }
            string itemName = value as string;

            if (!string.IsNullOrEmpty(itemName))
            {
                // add the postfix argument value if any.
                itemName += this.itemPostfixArgument;
                // get the project item associated to this value.
                ProjectItem item = null;
                Project project = (Project)dictionary.GetValue(this.projectArgument);
                if (project != null)
                {
                    item = DteHelperEx.FindItemByName(project.ProjectItems, itemName, true);
                }
                if (item != null)
                {
                    // Check if we need to reset the state in case we are running a new instance of the current recipe.
                    ResetStateIfNewInstance(service);
                    // check if we already processed this item 
                    // (this is to avoid recurrent UI showing up because of multiple calls to this method)
                    if (itemName.Equals(storedItemName, StringComparison.InvariantCulture))
                    {
                        overwrite = storedItemStatus;
                    }
                    else
                    {
                        // Ask the user if she wants to overwrite the item or not.
                        IUIService svc = (IUIService)service.GetService(typeof(IUIService));
                        DialogResult userSelection = svc.ShowMessage(
                                     string.Format(CultureInfo.CurrentUICulture, Properties.Resources.OverwriteItemMessage, itemName),
                                     null,
                                     System.Windows.Forms.MessageBoxButtons.YesNo);
                        overwrite = (userSelection == DialogResult.Yes);
                    }
                }
            }

            // Persist state so we may compare with 
            // the current value on the next validation
            storedItemStatus = overwrite;
            storedItemName = itemName;
            
            return overwrite;
        }
       
        /// <summary>
        /// Configures the specified attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        void IAttributesConfigurable.Configure(StringDictionary attributes)
        {
            // get the current project from where to check if the associated item exists or not.
            if (attributes.ContainsKey(ProjectArgument))
            {
                this.projectArgument = attributes[ProjectArgument];
            }

            // get the postfix argument (tipical use is ".cs") 
            if (attributes.ContainsKey(ItemPostfixArgument))
            {
                this.itemPostfixArgument = attributes[ItemPostfixArgument];
            }

            // get any other value that will be used instead of the current argument.
            if( attributes.ContainsKey(MonitorArgument))
            {
                this.monitorArgument = attributes[MonitorArgument];
            }
        }

        // This functions uses some heuristics to reset the state on each new
        // instance of the current recipe
        private void ResetStateIfNewInstance(IServiceProvider service)
        {
            // look for this item in the argument collection and check if it is already loaded
            // if not, reset the values.
            IConfigurationService configuration = (IConfigurationService)service.GetService(typeof(IConfigurationService));
            foreach (Argument arg in configuration.CurrentRecipe.Arguments)
            {
                // filter for arguments that have a converter and a 
                // ProjectArgument argument.
                if (arg.Converter != null && 
                    HasProjectArgument(arg.Converter.AnyAttr))
                {
                    // since we don't know the argument that invoke this converter
                    // we need to reset the state on any argument that honor this filter
                    // and has a value of null (not initialized)                    
                    IDictionaryService dictionary = (IDictionaryService)service.GetService(typeof(IDictionaryService));
                    if (dictionary.GetValue(arg.Name) == null)
                    {
                        // reset the stored values
                        storedItemName = null;
                        storedItemStatus = false;
                        break;
                    }
                }
            }
        }

        private bool HasProjectArgument(XmlAttribute[] attributes)
        {
            bool result = false;
            if (attributes != null)
            {
                foreach (XmlAttribute attribute in attributes)
                {
                    if (attribute.LocalName.Equals(ProjectArgument, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
