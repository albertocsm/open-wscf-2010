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
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.ComponentModel.Design;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
    /// <summary>
    /// Base class for Global persistance values.
    /// </summary>
    [ServiceDependency(typeof(IDictionaryService))]
    public class BaseGlobalsEntryAction : ConfigurableAction
    {
        #region Input Properties

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        [Input(Required = true)]
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        } private string propertyName;

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        [Input(Required = true)]
        public string PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        } private string propertyValue;

        #endregion

        #region ConfigurableAction Implementation

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            this.OnExecute(GetValue());
        }

        /// <summary>
        /// Undoes this instance.
        /// </summary>
        public override void Undo()
        {
            this.OnUndo();
        }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void OnExecute(object value)
        {
        }

        /// <summary>
        /// Called when [undo].
        /// </summary>
        public virtual void OnUndo()
        {
        }

        #endregion

        #region Private Implemantation
         
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        private object GetValue()
        {
            IDictionaryService dictservice = GetService<IDictionaryService>(false);
            if (dictservice == null)
            {
                return this.propertyValue;
            }
            return dictservice.GetValue(this.propertyValue) ?? this.propertyValue;            
        }

        #endregion
    }
}
