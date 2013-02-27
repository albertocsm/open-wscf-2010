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
using System.Runtime.InteropServices;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.DteWrapper
{
    /// <summary>
    /// ITypeModel implementation for DTE.
    /// </summary>
    public class DteTypeModel : ITypeModel
    {
        private CodeType codeType;
        private object typeModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DteTypeModel"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor used for testing purposes.
        /// </remarks>
        /// <param name="typeModel">The type model.</param>        
        public DteTypeModel(object typeModel)
            : this(typeModel as CodeType)
        {
            this.typeModel = typeModel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DteTypeModel"/> class.
        /// </summary>
        /// <param name="codeType">Type of the code.</param>
        public DteTypeModel(CodeType codeType)
        {
            this.codeType = codeType;
        }

        /// <summary>
        /// Gets the fully qualified type name (namespace + type name)
        /// </summary>
        /// <value></value>
        public string FullName
        {
            get { return codeType.FullName; }
        }

        /// <summary>
        /// Gets the name of the type (without namespace)
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return codeType.Name; }
        }

        /// <summary>
        /// Returns true if the type is public
        /// </summary>
        /// <value></value>
        public bool IsPublic
        {
            get { return codeType.Access == vsCMAccess.vsCMAccessPublic; }
        }

        /// <summary>
        /// Returns true if the type is a class
        /// </summary>
        /// <value></value>
        public bool IsClass
        {
            get { return codeType.Kind == vsCMElement.vsCMElementClass; }
        }

        /// <summary>
        /// Returns true if the type is an interface
        /// </summary>
        /// <value></value>
        public bool IsInterface
        {
            get { return codeType.Kind == vsCMElement.vsCMElementInterface; }
        }

        /// <summary>
        /// Verifies if the type is decorated with the specified custom attribute
        /// </summary>
        /// <param name="attributeFullName">The fully qualified name of the attribute</param>
        /// <returns>
        /// True if the attribute is found, false otherwise
        /// </returns>
        public bool HasAttribute(string attributeFullName)
        {
            return HasAttribute(attributeFullName, false);
        }

        /// <summary>
        /// Verifies if the type is decorated with the specified custom attribute
        /// </summary>
        /// <param name="attributeFullName">The fully qualified name of the attribute</param>
        /// <param name="inherited">Allows to search the attribute in inherited types</param>
        /// <returns>True if the attribute is found, false otherwise</returns>
        public bool HasAttribute(string attributeFullName, bool inherited)
        {
            return InternalHasAttribute(codeType, attributeFullName, inherited);
        }

        /// <summary>
        /// Determines whether the specified attribute has a property with the specified value.
        /// </summary>
        /// <param name="attributeFullName">Full name of the attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>
        /// 	<c>true</c> if the attribute property has the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAttributePropertyValue(string attributeFullName, string propertyName, object propertyValue)
        {
            if (HasAttribute(attributeFullName))
            {
                return InternalHasAttributePropertyValue((CodeElement)codeType, attributeFullName, propertyName, propertyValue);
            }

            CodeElement element = FileCodeModelHelper.FindCodeElementFromType((CodeElement)codeType, attributeFullName, vsCMElement.vsCMElementAttribute);
            if (element != null)
            {
                return InternalHasAttributePropertyValue(element, attributeFullName, propertyName, propertyValue);
            }

            return false;
        }

        /// <summary>
        /// Gets the containing project.
        /// </summary>
        /// <value>The containing project.</value>
        public IProjectModel ContainingProject
        {
            get
            {
                Project project = codeType.ProjectItem.ContainingProject;
                return new DteProjectModel(project, null);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Verifies if the type contains any member with the specified name
        /// </summary>
        /// <param name="memberName">The name of the member</param>
        /// <returns>
        /// True if contains at least one member with the specified name, false otherwise
        /// </returns>
        public bool HasMember(string memberName)
        {
            foreach (CodeElement member in codeType.Members)
            {
                if (member.Name.Equals(memberName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the type model.
        /// </summary>
        /// <value>The type model.</value>
        public object TypeModel
        {
            get { return this.codeType ?? this.typeModel; }
        }

        private bool InternalHasAttributePropertyValue(CodeElement element,
            string attributeFullName, string propertyName, object propertyValue)
        {
            EnvDTE80.CodeAttribute2 attribute;

            if (element is CodeAttribute)
            {
                attribute = (EnvDTE80.CodeAttribute2)element;
            }
            else if (element.IsCodeType)
            {
                attribute = (EnvDTE80.CodeAttribute2)((CodeType)element).Attributes.Item(attributeFullName);
            }
            else
            {
                return false;
            }

            // Accessing item by name if that item is not contained in the collection
            // will crash VS; not even an exception, just GONE. Therefore, rather than
            // try to go directly to the named argument, we loop through them all looking
            // for the one we want.
            CodeElements arguments = attribute.Arguments;
            int numArguments = arguments.Count;
            for (int argNum = 1; argNum <= numArguments; ++argNum)
            {
                CodeAttributeArgument argument = (CodeAttributeArgument)arguments.Item(argNum);
                string argName = argument.Name;
                if (argName == propertyName)
                {
                    return argument.Value.Contains(propertyValue.ToString());
                }
            }

            return false;
        }

        private bool InternalHasAttribute(CodeType codeType, string attributeFullName, bool inherited)
        {
            if (codeType == null)
                return false;

            foreach (CodeAttribute attribute in codeType.Attributes)
            {
                if (attribute.FullName.Equals(attributeFullName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            if (inherited)
            {
                foreach (CodeElement baseElement in codeType.Bases)
                {
                    if (baseElement.IsCodeType)
                    {
                        CodeType realCodeType = GetRealCodeTypeInReferencedProject(codeType.ProjectItem.ContainingProject, baseElement.FullName);
                        if (InternalHasAttribute(realCodeType, attributeFullName, inherited))
                            return true;
                    }
                }

                if (codeType.Kind == vsCMElement.vsCMElementClass)
                {
                    foreach (CodeInterface baseInterface in ((CodeClass)codeType).ImplementedInterfaces)
                    {
                        if (InternalHasAttribute(GetRealCodeTypeInReferencedProject(codeType.ProjectItem.ContainingProject, baseInterface.FullName), attributeFullName, inherited))
                            return true;
                    }
                }
            }

            return false;
        }

        private CodeType GetRealCodeTypeInReferencedProject(Project referralProject, string typeFullName)
        {
            CodeType codeType = referralProject.CodeModel.CodeTypeFromFullName(typeFullName);
            if (codeType != null && codeType.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                return codeType;

            foreach (Project referencedProject in new DteHelperEx.ProjectReferencesIterator(referralProject))
            {
                codeType = referencedProject.CodeModel.CodeTypeFromFullName(typeFullName);
                if (codeType != null && codeType.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                    return codeType;
            }

            return null;
        }
    }
}
