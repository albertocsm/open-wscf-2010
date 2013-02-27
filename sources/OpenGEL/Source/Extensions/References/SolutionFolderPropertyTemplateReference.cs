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
using System.Security.Permissions;
using System.Text;
using EnvDTE;
using System.Runtime.Serialization;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using Microsoft.Practices.Common;
using Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.References
{
    /// <summary>
    /// UnboundTemplateReference that allows to be executed only on a Vs Solution Folder
    /// </summary>
    [Serializable]
    public class SolutionFolderPropertyTemplateReference : UnboundTemplateReference, IAttributesConfigurable
    {
        private const string IsEnabledForPropertyAttribute = "IsEnabledForProperty";
        private string propertyName;

        /// <summary>
        /// UnboundTemplateReference that allows to be executed only on a Vs Solution Folder
        /// </summary>
        public SolutionFolderPropertyTemplateReference(string recipe)
            : base(recipe)
        {
        }        

        /// <summary>
        /// Returns a friendly name as Any Solution or Solution Folder
        /// </summary>
        public override string AppliesTo
        {
            get { return Properties.Resources.SolutionFolderReference; }
        }
        
        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a Solution Folder
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsEnabledFor(object target)
        {
            bool isSolutionOrSolutionFolder = false;
            DTE dte = GetService<DTE>();
            Solution solution = target as Solution;

            if (solution == null)
            {
                Project project = target as Project;

                if (project != null)
                {
                    SolutionFolder solutionFolder = project.Object as SolutionFolder;
                    isSolutionOrSolutionFolder = solutionFolder != null;
                }
                else
                {
                    isSolutionOrSolutionFolder = false;
                }
            }
            else
            {
                isSolutionOrSolutionFolder = true;
            }

            if (isSolutionOrSolutionFolder && dte.Solution.Globals.get_VariableExists(propertyName))
            {
                string propertyValue = (string)dte.Solution.Globals[propertyName];
                bool propertyValueBoolean;

                if (Boolean.TryParse(propertyValue, out propertyValueBoolean))
                {
                    return propertyValueBoolean;
                }
            }

            return false;
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionFolderPropertyTemplateReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            propertyName = info.GetString(IsEnabledForPropertyAttribute);
        }

        /// <summary>
        /// 	<seealso cref="M:System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(IsEnabledForPropertyAttribute, propertyName);
        }
        #endregion ISerializable Members

        #region IAttributesConfigurable Members

        /// <summary>
        /// Configures the component using the dictionary of attributes specified
        /// in the configuration file.
        /// </summary>
        /// <param name="attributes">The attributes in the configuration element.</param>
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            if (attributes.ContainsKey(IsEnabledForPropertyAttribute))
            {
                propertyName = attributes[IsEnabledForPropertyAttribute];
            }
        }

        #endregion
    }
}
