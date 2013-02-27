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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Web.UI.WebControls.Utility;
using System.ComponentModel;

namespace Microsoft.Practices.Web.UI.WebControls.Tests.Utility
{
    [TestClass]
    public class GuardFixture
    {
        [TestMethod]
        public void PropertyNotNullDoesNotThrowIfPropertyIsNotNull()
        { 
            PropertyDescriptor property = TypeDescriptor.GetProperties(typeof(SimpleEntity)).Find("Name", false);
            Guard.PropertyNotNull(property, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyNotNullThrowsIfPropertyIsNull()
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(typeof(SimpleEntity)).Find("Unexistant", false);
            Guard.PropertyNotNull(property, String.Empty);
        }

        [TestMethod]
        public void ArumentNotNullDoesNotThrowIfArgumentIsNotNull()
        {
            Guard.ArgumentNotNull(1, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullThrowsIfArgumentIsNull()
        {
            Guard.ArgumentNotNull(null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNonNegativeThrowsIfValueIsNegative()
        {
            Guard.ArgumentNonNegative(-1, String.Empty);
        }

        [TestMethod]
        public void ArgumentNonNegativeDoesNotThrowIfValueIs0()
        {
            Guard.ArgumentNonNegative(0, String.Empty);
        }

        [TestMethod]
        public void ArgumentNonNegativeDoesNotThrowIfValueIsPositive()
        {
            Guard.ArgumentNonNegative(1, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CollectionNotNullNorEmptyThrowsIfCollectionIsEmpty()
        {
            Guard.CollectionNotNullNorEmpty(new object[] { }, String.Empty, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionNotNullNorEmptyThrowsIfCollectionIsNull()
        {
            Guard.CollectionNotNullNorEmpty(null, String.Empty, String.Empty);
        }

        [TestMethod]
        public void CollectionNotNullNorEmptyDoesNotThrowIfCollectionHasItems()
        {
            object[] collection = new object[] { new object() };
            Guard.CollectionNotNullNorEmpty(collection, String.Empty, String.Empty);
        }
    }
}
