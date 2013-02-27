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
using EnvDTE;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Library;
using VsWebSite;
using VSLangProj; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Helper for FileCodeModel manipulation
    /// </summary>
    public static class FileCodeModelHelper
    {
        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        public static void AddAttribute(CodeClass element, string attributeName, string attributeValue)
        {
            if(!HasAttribute(element, attributeName))
            {
                element.AddAttribute(attributeName, attributeValue, 0);
            }
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        public static void AddAttribute(CodeProperty element, string attributeName, string attributeValue)
        {
            if (!HasAttribute(element, attributeName))
            {
                element.AddAttribute(attributeName, attributeValue, 0);
            }
        }

                /// <summary>
        /// Updates the code attribute argument.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="argumentValue">The argument value.</param>
        /// <returns></returns>
        public static bool UpdateCodeAttributeArgument(
            CodeElements attributes,
            string attributeName,
            string argumentName,
            string argumentValue)
        {
            return UpdateCodeAttributeArgument(attributes, attributeName, argumentName, argumentValue, true);
        }

        /// <summary>
        /// Updates the code attribute argument.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="createIfNew">if set to <c>true</c> [create if new].</param>
        /// <returns></returns>
        public static bool UpdateCodeAttributeArgument(
            CodeElements attributes, 
            string attributeName, 
            string argumentName, 
            string argumentValue,
            bool createIfNew)
        {
            Guard.ArgumentNotNull(attributes, "attributes");
            Guard.ArgumentNotNullOrEmptyString(attributeName, "attributeName");
            Guard.ArgumentNotNullOrEmptyString(argumentName, "argumentName");

            bool result = false;
            foreach (CodeElement attribute in attributes)
            {
                CodeAttribute codeAttribute = (CodeAttribute)attribute;
                if (attribute.FullName.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = UpdateCodeAttributeArgument(codeAttribute, argumentName, argumentValue, createIfNew);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the code attribute argument.
        /// </summary>
        /// <param name="codeAttribute">The code attribute.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="createIfNew">if set to <c>true</c> [create if new].</param>
        /// <returns></returns>
        public static bool UpdateCodeAttributeArgument(
            CodeAttribute codeAttribute,
            string argumentName,
            string argumentValue,
            bool createIfNew)
        {
            Guard.ArgumentNotNull(codeAttribute, "codeAttribute");
            Guard.ArgumentNotNullOrEmptyString(argumentName, "argumentName");

            bool result = false;

            EnvDTE80.CodeAttribute2 attribute2 = (EnvDTE80.CodeAttribute2)codeAttribute;
            EnvDTE80.CodeAttributeArgument argumentMatch = null;
            foreach (EnvDTE80.CodeAttributeArgument argument in attribute2.Arguments)
            {
                if (argument.Name.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    argumentMatch = argument;
                    break;
                }
            }
            if (argumentMatch != null)
            {
                argumentMatch.Value = argumentValue;
                result = true;
            }
            else if (createIfNew)
            {
                attribute2.AddArgument(argumentValue, argumentName, attribute2.Arguments.Count);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified element has attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        /// 	<c>true</c> if the specified element has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(CodeClass element, string attributeName)
        {
            if(element.Attributes.Count > 0)
            {
                foreach(CodeElement att in element.Attributes)
                {
                    CodeAttribute codeAttribute = (CodeAttribute)att;

                    if(att.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified element has attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        /// 	<c>true</c> if the specified element has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(CodeInterface element, string attributeName)
        {
            if(element.Attributes.Count > 0)
            {
                foreach(CodeElement att in element.Attributes)
                {
                    CodeAttribute codeAttribute = (CodeAttribute)att;

                    if(att.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// Determines whether the specified element has attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        /// 	<c>true</c> if the specified element has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(CodeProperty element, string attributeName)
        {
            if(element.Attributes.Count > 0)
            {
                foreach(CodeElement att in element.Attributes)
                {
                    CodeAttribute codeAttribute = (CodeAttribute)att;
                    if(att.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the code element.
        /// </summary>
        /// <param name="vs">The vs.</param>
        /// <param name="fileCodeModel">The file code model.</param>
        /// <param name="codeElementType">Type of the code element.</param>
        /// <returns></returns>
        public static object GetCodeElement(DTE vs, EnvDTE.FileCodeModel fileCodeModel, Type codeElementType)
        {
            TextSelection textSelection = (TextSelection)vs.ActiveDocument.Selection;
            TextPoint point = textSelection.ActivePoint;

            object element;

            if(codeElementType.IsAssignableFrom(typeof(CodeNamespace)))
            {
                try
                {
                    element = (CodeNamespace)fileCodeModel.CodeElementFromPoint(
                                point,
                                vsCMElement.vsCMElementNamespace);
                    return element;
                }
                catch
                {
                    return null;
                }
            }

            if(codeElementType.IsAssignableFrom(typeof(CodeAttribute)))
            {
                try
                {
                    element = (CodeAttribute)fileCodeModel.CodeElementFromPoint(
                                point,
                                vsCMElement.vsCMElementAttribute);
                    return element;
                }
                catch
                {
                    return null;
                }
            }

            if(codeElementType.IsAssignableFrom(typeof(CodeClass)))
            {
                try
                {
                    element = (CodeClass)fileCodeModel.CodeElementFromPoint(
                                point,
                                vsCMElement.vsCMElementClass);
                    return element;
                }
                catch
                {
                    return null;
                }
            }

            if(codeElementType.IsAssignableFrom(typeof(CodeProperty)))
            {
                try
                {
                    element = (CodeProperty)fileCodeModel.CodeElementFromPoint(
                                point,
                                vsCMElement.vsCMElementProperty);
                    return element;
                }
                catch
                {
                    return null;
                }
            }

            if(codeElementType.IsAssignableFrom(typeof(CodeFunction)))
            {
                try
                {
                    element = (CodeFunction)fileCodeModel.CodeElementFromPoint(
                                point,
                                vsCMElement.vsCMElementFunction);
                    return element;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="codeClass">The code class.</param>
        /// <param name="var">The var.</param>
        /// <returns></returns>
        public static CodeProperty AddProperty(CodeClass codeClass, CodeVariable var)
        {
            CodeProperty prop = null;

            try
            {
                prop = codeClass.AddProperty(
                    FormatPropertyName(var.Name),
                    FormatPropertyName(var.Name),
                    var.Type.AsFullName, -1,
                    vsCMAccess.vsCMAccessPublic, null);

                EditPoint editPoint = prop.Getter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                //Delete return default(int); added by codeClass.AddProperty
                editPoint.Delete(editPoint.LineLength);

                editPoint.Indent(null, 4);
                editPoint.Insert(string.Format(CultureInfo.InvariantCulture, "return {0};", var.Name));

                editPoint = prop.Setter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                editPoint.Indent(null, 1);
                editPoint.Insert(string.Format(CultureInfo.InvariantCulture, "{0} = value;", var.Name));
                editPoint.SmartFormat(editPoint);

                return prop;
            }
            catch
            {
                //Property already exists
                return null;
            }
        }

        /// <summary>
        /// Finds the type of the code element from the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="elementKind">Kind of the element.</param>
        /// <returns>
        /// The <see cref="CodeElement"/> found or null if no matches.
        /// </returns>
        public static CodeElement FindCodeElementFromType(Project project, string typeName, vsCMElement elementKind)
        {
            if (project == null ||
                string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            // try to find it in the selected project
            CodeElement result = FindCodeElementByFullName(project, typeName, elementKind);

            if (result == null)
            {
                // navigate through the project references and look into each project
                if (!DteHelper.IsWebProject(project))
                {
                    VSProject vsProject = project.Object as VSProject;
                    foreach (Reference reference in vsProject.References)
                    {
                        result = FindCodeElementFromType(reference.SourceProject, typeName, elementKind);
                        if (result != null && 
                            result.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    VSWebSite webProject = project.Object as VSWebSite;
                    if (webProject != null)
                    {
                        foreach (AssemblyReference reference in webProject.References)
                        {
                            Project sourceProject = GetSourceProject(reference);
                            result = FindCodeElementFromType(sourceProject, typeName, elementKind);
                            if (result != null &&
                                result.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the type of the code element from the specified CodeType.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="elementKind">Kind of the element.</param>
        /// <returns></returns>
        public static CodeElement FindCodeElementFromType(CodeElement element, string typeName, vsCMElement elementKind)
        {
            // check if we got it in the specified CodeType
            if (element.Kind == elementKind &&
                element.FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
            {
                return element;
            }

            // look into base types or interfaces
            return InspectBaseCodeElement(element, typeName, elementKind);
        }

        private static CodeElement FindCodeElementByFullName(
            Project project, string targetFullName, vsCMElement elementKind)
        {
            if (project == null)
            {
                return null;
            }

            foreach (ProjectItem projectItem in new DteHelperEx.ProjectItemIterator(project))
            {
                CodeElement element = FindCodeElementByFullName(projectItem, targetFullName, elementKind);
                if (element != null)
                {
                    return element;
                }
            }

            return null;
        }

        private static CodeElement FindCodeElementByFullName(
            ProjectItem projectItem, string targetFullName, vsCMElement elementKind)
        {
            foreach (CodeElement element in new DteHelperEx.CodeElementsIterator(projectItem))
            {
                if (element.Kind == vsCMElement.vsCMElementNamespace)
                {
                    foreach (CodeElement type in ((CodeNamespace)element).Members)
                    {
                        CodeElement result = InspectCodeElement(type, targetFullName, elementKind);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    CodeElement result = InspectCodeElement(element, targetFullName, elementKind);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        private static CodeElement InspectCodeElement(
            CodeElement element, string targetFullName, vsCMElement elementKind)
        {
            if (element.IsCodeType)
            {
                if (element.Kind == elementKind &&
                    element.FullName.Equals(targetFullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return element;
                }
                // look into base types or interfaces
                CodeElement result = InspectBaseCodeElement(element, targetFullName, elementKind);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private static CodeElement InspectBaseCodeElement(
            CodeElement element, string targetFullName, vsCMElement elementKind)
        {
            if (element.Kind == vsCMElement.vsCMElementClass)
            {
                CodeClass target = (CodeClass)element;
                foreach (CodeElement interfaceType in target.ImplementedInterfaces)
                {
                    if (interfaceType.Kind == elementKind &&
                        interfaceType.FullName.Equals(targetFullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return interfaceType;
                    }
                    if (interfaceType.InfoLocation == vsCMInfoLocation.vsCMInfoLocationExternal)
                    {
                        // look in other projects
                        ProjectItem item = DteHelperEx.FindContainingProjectItem(element.ProjectItem.DTE, (CodeType)interfaceType);
                        if (item != null)
                        {
                            return FindCodeElementByFullName(item, targetFullName, elementKind);
                        }
                        return null;
                    }
                    // look in childs
                    CodeElement child = InspectChildren(interfaceType.Children, targetFullName, elementKind);
                    if(child != null)
                    {
                        return child;
                    }
                }
                return null;
            }

            if (element.Kind == vsCMElement.vsCMElementInterface)
            {
                return InspectChildren(element.Children, targetFullName, elementKind);
            }

            return null;
        }

        private static CodeElement InspectChildren(
            CodeElements elements, string targetFullName, vsCMElement elementKind)
        {
            foreach (CodeElement children in elements)
            {
                if (children.Kind == elementKind &&
                    children.FullName.Equals(targetFullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return children;
                }
            }
            return null;
        }

        private static Project GetSourceProject(AssemblyReference reference)
        {
            Project sourceProject = null;

            if (reference.ReferenceKind != AssemblyReferenceType.AssemblyReferenceConfig)
            {
                sourceProject = DteHelperEx.FindProject(reference.DTE, new Predicate<Project>(delegate(Project match)
                {
                    return (match.Kind.Equals(VSLangProj.PrjKind.prjKindCSharpProject, StringComparison.InvariantCultureIgnoreCase) ||
                        match.Kind.Equals(VSLangProj.PrjKind.prjKindVBProject, StringComparison.InvariantCultureIgnoreCase)) &&
                        match.Name.Equals(reference.Name, StringComparison.InvariantCultureIgnoreCase);
                }));
            }
            return sourceProject;
        }

        /// <summary>
        /// Formats the name of the property.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        private static string FormatPropertyName(string variableName)
        {
            StringInfo si = new StringInfo(variableName);
            return si.SubstringByTextElements(0, 1).ToUpperInvariant() + 
                   si.SubstringByTextElements(1, si.LengthInTextElements - 1);
        }
    }
}
