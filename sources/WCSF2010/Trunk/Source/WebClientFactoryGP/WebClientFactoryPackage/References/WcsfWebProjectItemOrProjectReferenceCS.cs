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
using System.IO;
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Extensions.References.VisualStudio;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.Common;
using Microsoft.Practices.WebClientFactory.Properties;
using System.Security.Permissions;
using VsWebSite;
using Microsoft.Practices.RecipeFramework.Library.Solution;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Microsoft.Practices.RecipeFramework;

namespace Microsoft.Practices.WebClientFactory.References
{
    /// <summary>
    /// UnBoundRecipe that allows to be executed only on a WebProjectItem
    /// </summary>
    [Serializable]
    public class WcsfWebProjectItemOrProjectReferenceCS : UnboundRecipeReference, IAttributesConfigurable
    {
        private const string IsEnabledForPropertyAttribute = "IsEnabledForProperty";
        private const string WebApplicationProjectKind = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        private string _propertyName;

        /// <summary>
        /// Constructor of the <see cref="WcsfWebProjectItemReference"/> that must specify the 
        /// recipe name that will be used by the reference
        /// </summary>
        /// <param name="recipe"></param>
        public WcsfWebProjectItemOrProjectReferenceCS(string recipe)
            : base(recipe)
        {
        }

        public override string AppliesTo
        {
            get
            {
                return Resources.AppliesToWebFolderReference;
            }
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a <see cref="VSWebProjectItem"/> file.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsEnabledFor(object target)
        {
            ProjectItem item = target as ProjectItem;
            Project project = null;

            if (item == null)
            {
                project = target as Project;                
            }
            else 
            {
                project = item.ContainingProject;                
            }

            if (project != null && IsWebSiteOrWebAppProject(project) && HasWebProjectResponsibility(project))
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        private bool HasWebProjectResponsibility(Project project)
        {
            try
            {
                if (project.Globals.get_VariableExists(_propertyName))
                {
                    string propertyValue = (string)project.Globals[_propertyName];
                    bool propertyValueBoolean;

                    if (Boolean.TryParse(propertyValue, out propertyValueBoolean))
                    {
                        return propertyValueBoolean;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsWebSiteOrWebAppProject(Project project)
        {
            return (DteHelper.IsWebProject(project) || IsWebApplicationProject(project));
        }        

        private static bool IsWebApplicationProject(Project project)
        {
            Guard.ArgumentNotNull(project, "project");
            return (project.Kind == WebApplicationProjectKind);
        }


       #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectItemReference"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected WcsfWebProjectItemOrProjectReferenceCS(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                _propertyName = info.GetString(IsEnabledForPropertyAttribute);
            }
        }

        /// <summary>
        /// Provides a serialization member that derived classes can override to add
        /// data to be stored for serialization.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the reference.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(IsEnabledForPropertyAttribute, _propertyName);
        }

        #endregion ISerializable Members
        
        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes != null)
            {
                if (attributes.ContainsKey(IsEnabledForPropertyAttribute))
                {
                    _propertyName = attributes[IsEnabledForPropertyAttribute];
                }
            }
        }
    }
}
