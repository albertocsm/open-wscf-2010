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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Utility
{
	[TestClass]
	public class ListDictionaryFixture
	{
		[TestMethod]
		public void ShouldConstructAsEmpty()
		{
			ListDictionary<string, string> dict = new ListDictionary<string, string>();
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		public void ShouldAddValuesToDifferentKeys()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 5);
			dict.Add("b", 8);
			Assert.AreEqual(2, dict.Count);
			Assert.AreEqual(1, dict["a"].Count);
			Assert.AreEqual(5, dict["a"][0]);
			Assert.AreEqual(1, dict["b"].Count);
			Assert.AreEqual(8, dict["b"][0]);
		}

		[TestMethod]
		public void ShouldAddMultipleValuesToSameKey()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("foo", 13);
			dict.Add("foo", 21);

			Assert.AreEqual(1, dict.Count);
			Assert.AreEqual(2, dict["foo"].Count);
			Assert.AreEqual(13, dict["foo"][0]);
			Assert.AreEqual(21, dict["foo"][1]);
		}

		[TestMethod]
		public void ShouldAddListForKeyWithoutAnyItems()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("bar");

			Assert.AreEqual(1, dict.Count);
			Assert.AreEqual(0, dict["bar"].Count);
		}

		[TestMethod]
		public void ShouldAddViaIDictionaryInterface()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			IDictionary<string, List<int>> idict = dict;
			int[] values = {1, 1, 2, 3, 5, 8, 13};
			idict.Add("foo", new List<int>(values));

			Assert.AreEqual(1, dict.Count);
			Assert.AreEqual(values.Length, dict["foo"].Count);
		}

		[TestMethod]
		public void ShouldAddViaICollectionInterface()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			int[] values = {1, 1, 2, 3, 5, 8, 13};
			KeyValuePair<string, List<int>> item =
				new KeyValuePair<string, List<int>>("foo", new List<int>(values));
			ICollection<KeyValuePair<string, List<int>>> coll = dict;

			coll.Add(item);

			Assert.AreEqual(1, dict.Count);
			Assert.AreEqual(values.Length, dict["foo"].Count);
		}

		[TestMethod]
		public void ShouldBeAbleToRemoveAllValues()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 5);
			dict.Add("b", 8);

			dict.Clear();

			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		public void ShouldReplaceListForAKeyWhenSettingViaIndexer()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);

			List<int> newValue = new List<int>(new int[] {1, 1, 2, 3, 5, 8, 13});
			dict["a"] = newValue;

			Assert.AreSame(newValue, dict["a"]);
		}

		[TestMethod]
		public void ShouldAlwaysReturnAListEvenIfKeyHasNotBeenAdded()
		{
			ListDictionary<string, string> dict = new ListDictionary<string, string>();
			List<string> value = dict["NotAnAddedKey"];

			Assert.IsNotNull(value);
			Assert.AreEqual(0, value.Count);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ShouldThrowIfAttemptingToAddNullKey()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add(null);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ShouldThrowIfAttemptingToSetNullValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict["Foo"] = null;
		}

		[TestMethod]
		public void ShouldBeAbleToAddNullValueToAKey()
		{
			ListDictionary<string, string> dict = new ListDictionary<string, string>();
			dict.Add("a", "foo");
			dict.Add("a", null);
		}

		[TestMethod]
		public void ShouldHaveCaseSensitiveKeys()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 21);
			dict.Add("A", 34);

			Assert.AreEqual(2, dict.Count);
		}

		[TestMethod]
		public void ShouldReturnAllAddedKeys()
		{
			List<string> keys = new List<string>(new string[] {"a one", "b two", "c three", "d four"});
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			foreach (string key in keys)
			{
				dict.Add(key);
			}

			Assert.AreEqual(keys.Count, dict.Count);

			List<string> returnedKeys = new List<string>(dict.Keys);
			returnedKeys.Sort();
			Assert.AreEqual(keys.Count, returnedKeys.Count);
			for (int i = 0; i < keys.Count; ++i)
			{
				Assert.AreEqual(keys[i], returnedKeys[i], "Mismatch at index {0}", i);
			}
		}

		[TestMethod]
		public void ShouldReturnTrueWhenDictionaryContainsRequestedValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			Assert.IsTrue(dict.ContainsValue(3));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenDictionaryDoesNotContainRequestedValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			Assert.IsFalse(dict.ContainsValue(13));
		}

		[TestMethod]
		public void ShouldReturnTrueWhenCallingContainsViaICollectionInterfaceMethodAndDictionaryContainsValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			ICollection<KeyValuePair<string, List<int>>> collection = dict;
			List<int> aValues = dict["b"];
			KeyValuePair<string, List<int>> searchKey = new KeyValuePair<string, List<int>>("b", aValues);

			Assert.IsTrue(collection.Contains(searchKey));
		}

		[TestMethod]
		public void ShouldNotBeReadOnly()
		{
			ListDictionary<string, string> dict = new ListDictionary<string, string>();
			Assert.IsFalse(((ICollection<KeyValuePair<string, List<string>>>) dict).IsReadOnly);
		}

		[TestMethod]
		public void ShouldReturnGenericEnumerator()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			int numKeys = 0;
			foreach (KeyValuePair<string, List<int>> keyValue in dict)
			{
				++numKeys;
			}

			Assert.AreEqual(dict.Count, numKeys);
		}

		[TestMethod]
		public void ShouldReturnNongenericEnumerator()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			IEnumerator itr = ((IEnumerable) dict).GetEnumerator();
			Assert.IsNotNull(itr);

			int numKeys = 0;
			while (itr.MoveNext())
			{
				++numKeys;
			}

			Assert.AreEqual(dict.Count, numKeys);
		}

		[TestMethod]
		public void ShouldReturnFlattenedListOfValues()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			List<int> values = dict.Values;

			int[] expected = {1, 1, 2, 3, 5, 8};
			Assert.AreEqual(expected.Length, values.Count);
			for (int i = 0; i < expected.Length; ++i)
			{
				Assert.AreEqual(expected[i], values[i], "Mismatch at index {0}", i);
			}
		}

		[TestMethod]
		public void ShouldReturnCollectionOfListsAsValues()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			IDictionary<string, List<int>> idict = dict;
			List<List<int>> values = new List<List<int>>(idict.Values);

			Assert.AreEqual(2, values.Count);
		}

		[TestMethod]
		public void ShouldAlwaysReturnTrueFromTryGetValue()
		{
			ListDictionary<string, string> dict = new ListDictionary<string, string>();

			List<string> value;

			bool found = ((IDictionary<string, List<string>>) dict).TryGetValue("foo", out value);
			Assert.IsNotNull(value);
			Assert.IsTrue(found);
		}

		[TestMethod]
		public void ShouldReturnTrueFromContainsKeyIfContainsKeyOrFalseIfDoesnt()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			Assert.IsTrue(dict.ContainsKey("a"));
			Assert.IsTrue(dict.ContainsKey("b"));
			Assert.IsFalse(dict.ContainsKey("Foo"));
		}

		[TestMethod]
		public void ShouldCopyValuesToArray()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			ICollection<KeyValuePair<string, List<int>>> coll = dict;
			KeyValuePair<string, List<int>>[] values = new KeyValuePair<string, List<int>>[4];

			coll.CopyTo(values, 1);

			Assert.IsTrue(string.IsNullOrEmpty(values[0].Key));
			Assert.IsTrue(string.IsNullOrEmpty(values[3].Key));
			Assert.AreNotEqual(values[1].Key, values[2].Key);
			Assert.IsTrue(values[1].Key == "a" || values[1].Key == "b");
			Assert.IsTrue(values[2].Key == "a" || values[2].Key == "b");
		}

		[TestMethod]
		public void ShouldRemoveByKey()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			dict.Remove("b");
			Assert.AreEqual(1, dict.Count);
			Assert.AreEqual(3, dict["a"].Count);
		}

		[TestMethod]
		public void ShouldRemoveByKeyAndValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			dict.Remove("a", 2);

			Assert.AreEqual(2, dict.Count);
			Assert.AreEqual(2, dict["a"].Count);
			Assert.AreEqual(4, dict["b"].Count);
		}

		[TestMethod]
		public void ShouldRemoveAllInstancesOfGivenValue()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			dict.Remove(2);

			Assert.AreEqual(2, dict.Count);
			Assert.AreEqual(2, dict["a"].Count);
			Assert.AreEqual(3, dict["b"].Count);
		}

		[TestMethod]
		public void ShouldRemoveByItem()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			List<int> value = dict["b"];
			KeyValuePair<string, List<int>> item = new KeyValuePair<string, List<int>>("b", value);
			ICollection<KeyValuePair<string, List<int>>> coll = dict;

			coll.Remove(item);

			Assert.AreEqual(1, dict.Count);
		}

		[TestMethod]
		public void ShouldReturnValuesThatMatchPredicate()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("b", 3);
			dict.Add("b", 5);
			dict.Add("b", 8);

			int[] evens = {2, 8};

			List<int> values = new List<int>(dict.FindAllValues(delegate(int i) { return i%2 == 0; }));

			Assert.AreEqual(evens.Length, values.Count);
			for (int i = 0; i < evens.Length; ++i)
			{
				Assert.AreEqual(evens[i], values[i], "Mismatch at index {0}", i);
			}
		}

		[TestMethod]
		public void ShouldReturnValuesWhereKeyMatchesPredicate()
		{
			ListDictionary<string, int> dict = new ListDictionary<string, int>();
			dict.Add("a", 1);
			dict.Add("a", 1);
			dict.Add("a", 2);
			dict.Add("bar", 3);
			dict.Add("bar", 5);
			dict.Add("bar", 8);
			dict.Add("c", 13);
			dict.Add("c", 21);
			dict.Add("dar", 34);

			// We can't assume that the keys come out in sorted order,
			// so we'll keep a set of the expected numbers instead, and
			// pick them out.

			Dictionary<int, int> expected = new Dictionary<int, int>();
			expected[3] = 0;
			expected[5] = 0;
			expected[8] = 0;
			expected[34] = 0;

			foreach (int value in dict.FindAllValuesByKey(
				delegate(string key) { return key.Length == 3; }))
			{
				Assert.IsTrue(expected.ContainsKey(value));
				expected.Remove(value);
			}
			Assert.AreEqual(0, expected.Count);
		}
	}
}