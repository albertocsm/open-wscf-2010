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
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using EnvDTE;
using VSLangProj;
using VsWebSite;
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Editors.ClassBrowser
{
    /// <summary>
    /// Represents the editor for class names.
    /// </summary>
    public class ClassBrowserEditor : UITypeEditor
    {
        private const string SetAssembliesMethodName = "SetAssemblies";

        #region Method overrides

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            DynamicTypeService typeService = (DynamicTypeService)provider.GetService(typeof(DynamicTypeService));

            IVsHierarchy hier = DteHelper.GetCurrentSelection(provider);

            ITypeDiscoveryService typeDiscovery = typeService.GetTypeDiscoveryService(hier);

            Project project = ToDteProject(hier);
            if (DteHelper.IsWebProject(project))
            {                
                VSWebSite vsProject = (VSWebSite)project.Object;
                List<string> assemblies = new List<string>();
                foreach (AssemblyReference reference in vsProject.References)
                {
                    if (!string.IsNullOrEmpty(reference.FullPath))
                        assemblies.Add(reference.FullPath);
                }

                MethodInfo setAsssembliesMethod = typeDiscovery.GetType().GetMethod(SetAssembliesMethodName, 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                setAsssembliesMethod.Invoke(typeDiscovery, new object[] { assemblies.ToArray() });
            }

            if(typeDiscovery != null)
            {
                List<string> assembliesAdded = new List<string>();
                List<Assembly> assemblies = new List<Assembly>();
                List<Type> types = new List<Type>();

                foreach(Type type in typeDiscovery.GetTypes(typeof(object), false))
                {
                    if(ShouldInclude(type))
                    {
                        if(!assembliesAdded.Contains(type.Assembly.FullName))
                        {
                            assembliesAdded.Add(type.Assembly.FullName);
                            assemblies.Add(type.Assembly);
                        }
                        types.Add(type);
                    }
                }

                IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                ClassBrowserEditorForm form = new ClassBrowserEditorForm(assemblies, types);
				DialogResult result;

				if (svc != null)
				{
					result = svc.ShowDialog(form);
				}
				else
				{
					result = form.ShowDialog();
				}

                if (result == DialogResult.OK)
                {
                    return form.TypeFullName;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion Method overrides

        /// <summary>
        /// Shoulds the include.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
		protected virtual bool ShouldInclude(Type candidate)
		{
			return false;
		}

        private EnvDTE.Project ToDteProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
            {
                throw new ArgumentNullException("hierarchy");
            }

            object prjObject = null;
            if (hierarchy.GetProperty(0xfffffffe, -2027, out prjObject) >= 0)
            {
                return (EnvDTE.Project)prjObject;
            }
            else
            {
                throw new ArgumentException(Properties.Resources.HierarchyArgumentException);
            }
        }
	}
}
