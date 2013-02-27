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
using System.Collections;
using System.Collections.Specialized;

namespace Microsoft.Practices.Web.UI.WebControls.Tests.Utility
{
    [TestClass]
    public class DictionaryHelperFixture
    {
        [TestMethod]
        public void GetReadOnlyDictionaryReturnsReadOnlyDictionary()
        { 
            OrderedDictionary dictionary = new OrderedDictionary(1);
            dictionary.Add("key", "value");

            IDictionary result = DictionaryHelper.GetReadOnlyDictionary(dictionary);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("value", result["key"]);
            Assert.IsTrue(result.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetReadOnlyDictionaryThrowsIfDictionaryIsNull()
        {
            DictionaryHelper.GetReadOnlyDictionary(null);
        }

        [TestMethod]
        public void MergeNameValueDictionaries()
        {
            OrderedDictionary dictionary1 = new OrderedDictionary(1);
            dictionary1.Add("key1", "value1");
            OrderedDictionary dictionary2 = new OrderedDictionary(1);
            dictionary2.Add("key2", "value2");

            IDictionary result = DictionaryHelper.MergeNameValueDictionaries(dictionary1, dictionary2);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("value1", result["key1"]);
            Assert.AreEqual("value2", result["key2"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeNameValueDictionarysThrowsIfDictionary1IsNull()
        {
            DictionaryHelper.MergeNameValueDictionaries(null, new OrderedDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeNameValueDictionarysThrowsIfDictionary2IsNull()
        {
            DictionaryHelper.MergeNameValueDictionaries(new OrderedDictionary(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MergeNameValueDictionariesThrowsIfDuplicatedKeys()
        {
            OrderedDictionary dictionary1 = new OrderedDictionary(1);
            dictionary1.Add("key1", "value1");
            OrderedDictionary dictionary2 = new OrderedDictionary(1);
            dictionary2.Add("key1", "value2");

            DictionaryHelper.MergeNameValueDictionaries(dictionary1, dictionary2);
        }
    }
}
