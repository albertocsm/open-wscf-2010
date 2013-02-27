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
using Microsoft.Practices.Web.UI.WebControls.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Threading;
using System.Globalization;

namespace Microsoft.Practices.Web.UI.WebControls.Tests
{
    [TestClass]
    public class TypeDescriptionHelperFixture
    {
        [TestMethod]
        public void GetObjectValueStringToInt()
        {
            object result = TypeDescriptionHelper.GetObjectValue("1", typeof(int), String.Empty);

            Assert.IsTrue(result is int);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetObjectValueStringToDoubleWithCultureInfoenUs()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
            object result = TypeDescriptionHelper.GetObjectValue("1.2", typeof(double), String.Empty);

            Assert.IsTrue(result is double);
            Assert.AreEqual((double)1.2, result);
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [TestMethod]
        public void GetObjectValueStringToDoubleWithCultureInfoesAr()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ar");
            object result = TypeDescriptionHelper.GetObjectValue("1,2", typeof(double), String.Empty);

            Assert.IsTrue(result is double);
            Assert.AreEqual((double)1.2, result);
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [TestMethod]
        public void GetObjectValueStringToBool()
        {
            object result = TypeDescriptionHelper.GetObjectValue("true", typeof(bool), String.Empty);

            Assert.IsTrue(result is bool);
            Assert.IsTrue((bool)result);
        }

        [TestMethod]
        public void GetObjectValueIfValueIsInstanceOfTargetTypeReturnsValue()
        {
            object result = TypeDescriptionHelper.GetObjectValue("1", typeof(string), String.Empty);

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetObjectValueNotStringReturnsValue()
        {
            object result = TypeDescriptionHelper.GetObjectValue((int)1, typeof(bool), String.Empty);

            Assert.IsTrue(result is int);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetObjectValueThrowsIfInvalidStringToInt()
        {
            object result = TypeDescriptionHelper.GetObjectValue("a", typeof(int), String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetObjectValueThrowsIfNullTargetType()
        {
            TypeDescriptionHelper.GetObjectValue("2", null, String.Empty);
        }

        [TestMethod]
        public void GetObjectValueReturnsNullIfValueIsNull()
        {
            object result = TypeDescriptionHelper.GetObjectValue(null, typeof(string), String.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetValidPropertyReturnsAValidProperty()
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SimpleEntity));
            
            PropertyDescriptor result = TypeDescriptionHelper.GetValidProperty(properties, "Name");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetValidPropertyThrowsIfPropertyIsInvalid()
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SimpleEntity));

            TypeDescriptionHelper.GetValidProperty(properties, "Unexistant");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValidPropertyThrowsIfPropertiesIsNull()
        {
            TypeDescriptionHelper.GetValidProperty(null, "Unexistant");
        }

        [TestMethod]
        public void BuildInstance()
        {
            SimpleEntity simpleEntity = new SimpleEntity();
            OrderedDictionary values = new OrderedDictionary(3);
            values.Add("Id", 1);
            values.Add("Name", "name");
            values.Add("Age", 10);
            
            TypeDescriptionHelper.BuildInstance(values, simpleEntity);

            Assert.AreEqual(1, simpleEntity.Id);
            Assert.AreEqual("name", simpleEntity.Name);
            Assert.AreEqual(10, simpleEntity.Age);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInstanceThrowsIfValuesIsNull()
        {
            TypeDescriptionHelper.BuildInstance(null, new SimpleEntity());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInstanceThrowsIfExistingIsNull()
        {
            TypeDescriptionHelper.BuildInstance(new OrderedDictionary(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildInstanceThrowsIfPropertyIsReadOnly()
        {
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name");

            TypeDescriptionHelper.BuildInstance(values, new SimpleEntityWithReadOnlyProperty());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildInstanceThrowsIfPropertyIsUnexistant()
        {
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Unexistant", "unexistant");

            TypeDescriptionHelper.BuildInstance(values, new SimpleEntity());
        }

        [TestMethod]
        public void ThrowIfInvalidTypeDoesNotThrowIfObjectTypeIsCorrect()
        {
            TypeDescriptionHelper.ThrowIfInvalidType(new SimpleEntity(), typeof(SimpleEntity));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowIfInvalidTypeDoesThrowsIfObjectTypeIsNotCorrect()
        {
            TypeDescriptionHelper.ThrowIfInvalidType(new object(), typeof(SimpleEntity));
        }

        [TestMethod]
        public void ThrowIfInvalidTypeDoesNotThrowIfObjectExtendsType()
        {
            TypeDescriptionHelper.ThrowIfInvalidType(new DerivedSimpleEntity(), typeof(SimpleEntity));
        }

        [TestMethod]
        public void ThrowIfNoDefaultConstructorDoesNotThrowIfTypeHasDefaultConstructor()
        {
            TypeDescriptionHelper.ThrowIfNoDefaultConstructor(typeof(SimpleEntity));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowIfNoDefaultConstructorThrowsIfTypeDoesNotHaveDefaultConstructor()
        {
            TypeDescriptionHelper.ThrowIfNoDefaultConstructor(typeof(SimpleEntityNoPublicParameterlessConstructor));
        }
    }

    class DerivedSimpleEntity : SimpleEntity
    { }
}
