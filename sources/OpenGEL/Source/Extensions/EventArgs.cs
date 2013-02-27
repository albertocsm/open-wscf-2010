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

namespace Microsoft.Practices.RecipeFramework.Extensions
{
    /// <summary>
    /// Generic EventArgs
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public class EventArgs<T> : EventArgs         
    {
        private T _data;

        /// <summary>
        /// ctor
        /// </summary>
        public EventArgs()
        {
        }

        /// <summary>
        /// ctor with default data
        /// </summary>
        /// <param name="data"></param>
        public EventArgs(T data)
        {
            _data = data;
        }

        /// <summary>
        /// Returns the data
        /// </summary>
        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }
	
    }
}
