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
using System.Web.UI;
using System.ComponentModel;

namespace AjaxControlToolkit.WCSFExtensions
{
    /// <summary>
    /// A class representing a server control used as context for filtering Autocomplete
    /// </summary>
    public sealed class CompletionContextItem
    {
        private string key;
        private string controlId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CompletionContextItem()
            : this(string.Empty, string.Empty)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> The key that will be used to access a given context item</param>
        /// <param name="controlId"> The controlId of the server control used as context</param>
        public CompletionContextItem(string key, string controlId)
        {
            this.key = key;
            this.controlId = controlId;
        }

        /// <summary>
        /// A key value that is used to access the context control
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// The controlId of the server control used as context
        /// </summary>
        [IDReferenceProperty]
        [DefaultValue("")]
        public string ControlId
        {
            get { return controlId; }
            set { controlId = value; }
        }
    }
}
