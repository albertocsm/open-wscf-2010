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
using System.Configuration;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Extensions.Properties;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Validators
{
    /// <summary>
    /// Provides validation for type names
    /// </summary>
    /// <remarks>
    /// Uses the <code>ITypeResolutionService</code> to verify the existence of the type
    /// </remarks>
    public class TypeNameValidator : ConfigurationValidatorBase, IComponent
    {
        private bool multipleTypes;
        private bool optional;

        /// <summary>
        /// Determines whether an object can be validated based on type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>
        /// true if the type parameter value matches the expected type; otherwise, false.
        /// </returns>
        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [multiple types].
        /// </summary>
        /// <value><c>true</c> if [multiple types]; otherwise, <c>false</c>.</value>
        public bool MultipleTypes
        {
            get { return multipleTypes; }
            set { multipleTypes = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TypeNameValidator"/> is optional.
        /// </summary>
        /// <value><c>true</c> if optional; otherwise, <c>false</c>.</value>
        public bool Optional
        {
            get { return optional; }
            set { optional = value; }
        }

        /// <summary>
        /// Determines whether the value of an object is valid.
        /// </summary>
        /// <param name="value">The object value.</param>
        public override void Validate(object value)
        {
            if (value != null && !(value is string))
                throw new ArgumentException(Resources.TypeNameValidatorExpectsString, "value");

            if (string.IsNullOrEmpty((string)value))
            {
                if (!optional)
                    throw new ArgumentException(Resources.TypeNameValidatorValueNotOptional);
            }
            else
            {
                if (multipleTypes)
                {
                    foreach (string typeName in ((string)value).Split(','))
                    {
                        ValidateTypeName(typeName.Trim());
                    }
                }
                else
                {
                    ValidateTypeName((string)value);
                }
            }
        }
        
        private void ValidateTypeName(string typeName)
        {
            DynamicTypeService typeService = (DynamicTypeService)site.GetService(typeof(DynamicTypeService));
            Debug.Assert(typeService != null, "No dynamic type service registered.");

            IVsHierarchy hier = DteHelper.GetCurrentSelection(site);
            Debug.Assert(hier != null, "No active hierarchy is selected.");

            ITypeResolutionService resolution = typeService.GetTypeResolutionService(hier);
            Type type = resolution.GetType(TypeHelper.ParseGenericType(typeName));
            if (type == null)
            {
                throw new TypeLoadException(string.Format(CultureInfo.CurrentUICulture, Resources.TypeNameValidatorTypeNotFound, typeName));
            }
        }

        #region IComponent Members

        /// <summary/>
        public event EventHandler Disposed;
                
        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"></see> associated with the <see cref="T:System.ComponentModel.IComponent"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.ISite"></see> object associated with the component; or null, if the component does not have a site.</returns>
        public ISite Site
        {
            get { return site; }
            set { site = value; }
        } private ISite site;

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if ((this.site != null) && (this.site.Container != null))
                    {
                        this.site.Container.Remove(this);
                    }
                    if (this.Disposed != null)
                    {
                        this.Disposed(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:Microsoft.Practices.RecipeFramework.Extensions.Validators.TypeNameValidator"/> is reclaimed by garbage collection.
        /// </summary>
        ~TypeNameValidator()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
