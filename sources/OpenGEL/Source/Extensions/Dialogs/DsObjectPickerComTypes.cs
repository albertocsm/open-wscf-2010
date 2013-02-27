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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
    /// <summary>
    /// The object picker dialog box.
    /// </summary>
    [ComImport, Guid("17D6CCD8-3B7B-11D2-B9E0-00C04FD8DBF7")]
    internal class DSObjectPicker
    {
    }

    /// <summary>
    /// The IDsObjectPicker interface is used by an application to initialize and display 
    /// an object picker dialog box. 
    /// </summary>
    //[CLSCompliant(false)]
    [ComImport, Guid("0C87E64E-3B7A-11D2-B9E0-00C04FD8DBF7"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IDsObjectPicker
    {
        /// <summary>
        /// Initializes the specified p init info.
        /// </summary>
        /// <param name="pInitInfo">The p init info.</param>
        void Initialize(ref UnsafeNativeMethods.DsopInitInfo pInitInfo);

        /// <summary>
        /// Invokes the dialog.
        /// </summary>
        /// <param name="hwnd">The hwnd.</param>
        /// <param name="lpDataObject">The lp data object.</param>
        void InvokeDialog(IntPtr hwnd, out IDataObject lpDataObject);
    }
}
