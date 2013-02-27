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
using System.IO;
using Microsoft.Practices.Common;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Specialized;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services; 

namespace Microsoft.Practices.RecipeFramework.Extensions.Converters
{
    /// <summary>
    /// Converter that validates a path. The path should be rooted and 
    /// if does not exists, prompts the user for creation (if configured) and
    /// alerts the user if the path contains files/folder (if configured).
    /// </summary>
    public class PathConverter : StringConverter, IAttributesConfigurable
    {
        /// <summary>
        /// Construct a new PathConverter object.
        /// </summary>
		public PathConverter()
		{
		}

        /// <summary>
        /// Construct a new PathConverter object.
        /// </summary>
        public PathConverter(bool validateEmptyFolderArgument, bool validateNewFolderArgument)
		{
			this.validateEmptyFolderArgument = validateEmptyFolderArgument;
			this.validateNewFolderArgument = validateNewFolderArgument;
		}

        /// <summary>
        /// Optional configuration attribute that validates the specified path/folder should be empty.
        /// Only returns true if the folder is empty.
        /// </summary>
        public const string ValidateEmptyFolderArgument = "ValidateEmptyFolderArgument";

        /// <summary>
        /// Optional configuration attribute that validates if the folder is new and in that case it will
        /// ask the user to confirm the creation of the new folder.
        /// </summary>
        public const string ValidateNewFolderArgument = "ValidateNewFolderArgument";

        private bool validateEmptyFolderArgument;
        private bool validateNewFolderArgument;
        private string storedPath;
        private bool storedPathStatus;
        private bool afterRecipeExecutionEventAdded; 

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            bool response = false;
            string path = value as string;

            if (path != null)
            {
                response = Path.IsPathRooted(path) && 
                            IsNewDirectory(context, path) && 
                            IsEmptyDirectory(context, path);
            }

            return response;
        }

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(StringDictionary attributes)
        {
            if (attributes.ContainsKey(ValidateEmptyFolderArgument))
            {
                Boolean.TryParse(attributes[ValidateEmptyFolderArgument], out validateEmptyFolderArgument);
            }

            if (attributes.ContainsKey(ValidateNewFolderArgument))
            {
                Boolean.TryParse(attributes[ValidateNewFolderArgument], out validateNewFolderArgument);
            }
        }

        #endregion

        #region Private Implementation

        private bool IsEmptyDirectory(ITypeDescriptorContext context, string path)
        {
            bool result = true;

            if (this.validateEmptyFolderArgument && 
                Directory.Exists(path))
            {
                // Verify if we need to reset the stored values in case this converter instance
                // is being called from a new instance of the recipe.
                ResetStateIfNewInstance((IServiceProvider)context);
                // this comparison is to avoid the reentrancy that make GAX so we won't show up
                // the dialog multiple times for the same folder
                if (path.Equals(storedPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = storedPathStatus;
                }
                else
                {
                    try
                    {
                        result = (Directory.GetFiles(path).Length == 0 &&
                                  Directory.GetDirectories(path).Length == 0);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return false;
                    }

                    if (!result)
                    {
                        // Ask the user if she wants to overwrite the items in this folder or not.
                        IUIService svc = (IUIService)context.GetService(typeof(IUIService));
                        DialogResult userSelection = svc.ShowMessage(
                             string.Format(CultureInfo.CurrentUICulture, Properties.Resources.OverwriteFilesInFolder, path),
                             null,
                             System.Windows.Forms.MessageBoxButtons.YesNo);
                        result = (userSelection == DialogResult.Yes);
                    }
                }
            }
            storedPath = path;
            storedPathStatus = result;

            return result;
        }

        private bool IsNewDirectory(ITypeDescriptorContext context, string path)
        {
            bool result = Directory.Exists(path);

            if (this.validateNewFolderArgument && !result)
            {
                // Ask the user if she wants to create the new folder or not.
                IUIService svc = (IUIService)context.GetService(typeof(IUIService));
                DialogResult userSelection = svc.ShowMessage(
                             string.Format(CultureInfo.CurrentUICulture, Properties.Resources.CreateNewFolderMessage, path),
                             null,
                             System.Windows.Forms.MessageBoxButtons.YesNo);
                if (userSelection == DialogResult.Yes)
                {
                    Directory.CreateDirectory(path);
                    result = true;
                }
            }

            return result;
        }

        //private void ResetStateIfNewInstance(IServiceProvider service)
        //{
        //    // get the current instance of the GatheringServiceData object
        //    // to compare if this is a new instance or not
        //    // so we know when to reset the state.
        //    if (storedInstance == null)
        //    {
        //        // reset the stored values
        //        storedPath = string.Empty;
        //        storedPathStatus = false;
        //        IConfigurationService configService = (IConfigurationService)service.GetService(typeof(IConfigurationService));
        //        storedInstance = configService.CurrentGatheringServiceData;
        //        // note that the instance in storedInstance will be 
        //        // disposed upon the recipe ends.
        //    }
        //}

        private void ResetStateIfNewInstance(IServiceProvider service)
        {
            if (!afterRecipeExecutionEventAdded)
            {
                IRecipeManagerService manager = (IRecipeManagerService)service.GetService(typeof(IRecipeManagerService));
                // TODO: add an event or other method to reset the values in case the recipe were cancelled.
                manager.AfterRecipeExecution += new RecipeEventHandler(OnAfterRecipeExecution);
                afterRecipeExecutionEventAdded = true;
            }
        }

        private void OnAfterRecipeExecution(object sender, RecipeEventArgs e)
        {
            // reset the stored values
            storedPath = null;
            storedPathStatus = false;
        }

        #endregion
    }
}
