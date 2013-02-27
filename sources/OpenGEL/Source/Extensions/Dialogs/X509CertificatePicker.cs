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
using System.Security.Permissions;
using System.Text;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
    /// <summary>
    /// Helper class that displays a UI X509 Certificate selector dialog.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class X509CertificatePicker : IDisposable
    {
        #region Fields
        
        private X509Store store;
        private StoreName storeName;
        private StoreLocation storeLocation;
        private X509Certificate2 x509Cert;
        
        // Track whether Dispose has been called.
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default initializer. The default store name is StoreName.My for the client side.
        /// </summary>
        public X509CertificatePicker() : this(StoreName.My, StoreLocation.CurrentUser)
        {
        }

        /// <summary>
        /// Class initializer with a <see cref="StoreName"/> and the flag that sepcifies the endpoint side.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        public X509CertificatePicker(StoreName storeName, StoreLocation storeLocation)
        {
            if (Enum.IsDefined(typeof(StoreName), storeName) == false)
            {
                throw new ArgumentOutOfRangeException("storeName");
            }

            if (Enum.IsDefined(typeof(StoreLocation), storeLocation) == false)
            {
                throw new ArgumentOutOfRangeException("storeLocation");
            }

            this.store = new X509Store(storeName, storeLocation);
            this.store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            this.storeName = storeName;
            this.storeLocation = storeLocation;
        }

        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:Microsoft.Practices.RecipeFramework.Extensions.Dialogs.X509CertificatePicker"/> is reclaimed by garbage collection.
        /// </summary>
        ~X509CertificatePicker()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets the store.
		/// </summary>
		/// <value>The store.</value>
        public X509Store Store
        {
            get { return this.store; } 
        }

		/// <summary>
		/// Gets the name of the store.
		/// </summary>
		/// <value>The name of the store.</value>
        public StoreName StoreName
        {
            get { return this.storeName; }
        }

		/// <summary>
		/// Gets the store location.
		/// </summary>
		/// <value>The store location.</value>
        public StoreLocation StoreLocation
        {
            get { return this.storeLocation; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pick the selected certificate
        /// </summary>
        /// <returns></returns>
        public X509Certificate2 PickCertificate()
        {
			return PickCertificate(IntPtr.Zero);
        }
        /// <summary>
        /// Pick the selected certificate
        /// </summary>
        /// <param name="hwd"></param>
        /// <returns></returns>
        public X509Certificate2 PickCertificate(IntPtr hwd)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            X509Certificate2Collection selectedCerts =
                X509Certificate2UI.SelectFromCollection(
                    this.store.Certificates,
                    "Select Certificate",
                    String.Format(CultureInfo.CurrentUICulture, 
                        Properties.Resources.ChooseCertificateDielogMessage,
                        this.storeLocation.ToString(),
                        this.ReadableStoreName(this.storeName)),
                    X509SelectionFlag.SingleSelection,
                    hwd);
                        
            if (selectedCerts.Count != 0)
            {
                this.x509Cert = selectedCerts[0];
            }
            else
            {
                this.x509Cert = null;
            }

            return this.x509Cert;
        }

        /// <summary>
        /// Display the selected certificate
        /// </summary>
        public void DisplayCertificate()
        {
            DisplayCertificate(IntPtr.Zero);
        }

        /// <summary>
        /// Display the selected certificate over the parent window handler.
        /// </summary>
        /// <param name="hwd"></param>
        public void DisplayCertificate(IntPtr hwd)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            if (this.x509Cert != null)
            {
                X509Certificate2UI.DisplayCertificate(this.x509Cert, hwd);
            }
        }

        private string ReadableStoreName(StoreName storeName)
        {
            string readable;

            switch (storeName)
            {
                case StoreName.My:
                    readable = "Personal Store";
                    break;
                case StoreName.AddressBook:
                    readable = "Other People Store";
                    break;
                default:
                    readable = storeName.ToString();
                    break;
            }

            return readable;
        } 

        #endregion    

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }
                this.store.Close();
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.

            }
            disposed = true;
        }

        #endregion
    }
}

