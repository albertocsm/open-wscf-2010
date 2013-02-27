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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.BusinessEntities;

namespace OrdersRepository.Services.Tests
{
	/// <summary>
	/// Summary description for PostalInfoLookupServiceFixture
	/// </summary>
	[TestClass]
	public class PostalInfoLookupServiceFixture
	{
		[TestMethod]
		public void ShouldRetrieveAllStates()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
			List<State> states = new List<State>(lookup.AllStates);

			AssertStatesMatchesTable(lookup.TestData.States, states);
		}

		[TestMethod]
		public void ShouldGetAllWashingtonCitiesStartingWithA()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
			List<string> cities = new List<string>(lookup.GetCities("A", "WA", 5));
			Assert.AreEqual(2, cities.Count);
			string[] expectedCities = { "ALGONA", "AUBURN" };
			for(int i = 0; i < expectedCities.Length; ++i)
			{
				Assert.AreEqual(expectedCities[i].ToLowerInvariant(), 
							 cities[i].ToLowerInvariant(),
							 "City mismatch at index {0}", i);
			}
		}

		[TestMethod]
		public void ShouldNotGetDuplicateCityNames()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();

			List<string> cities = new List<string>(lookup.GetCities("be", "WA", 5));
			Assert.AreEqual(1, cities.Count);
			Assert.AreEqual("BELLEVUE", cities[0].ToUpperInvariant());
		}

		[TestMethod]
		public void ShouldGetAllCitiesInStateWithEmptyPrefix()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();

			List<string> cities = new List<string>(lookup.GetCities("", "WA", 10));
			Assert.AreEqual(lookup.NumCitiesInWA, cities.Count);
		}

		[TestMethod]
		public void ShouldNotGetMoreCitiesThenRequested()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
			List<string> cities = new List<string>(lookup.GetCities("", "WA", 1));
			Assert.AreEqual(1, cities.Count);
		}

		[TestMethod]
		public void ShouldGetAllZipCodesInBellevueWAInAscendingSortOrder()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
			List<ZipCode> zips = new List<ZipCode>(lookup.GetZipCodes("", "bellevue", "wa", 5));
			Assert.AreEqual(lookup.NumZipsInBellevue, zips.Count);
			Assert.AreEqual("98005", zips[0].PostalCode);
			Assert.AreEqual("98006", zips[1].PostalCode);
		}

		[TestMethod]
		public void ShouldNotGetAnyZipCodesIfPrefixIsntValid()
		{
			TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
			List<ZipCode> zips = new List<ZipCode>(lookup.GetZipCodes("06", "bellevue", "wa", 5));
			Assert.AreEqual(0, zips.Count);
		}

        [TestMethod]
        public void ShouldRetrieveNullIfStateIdDoesNotExist()
        {
            PostalInfoLookupDataSet ds = new PostalInfoLookupDataSet();
            ds.States.AddStatesRow("CA", "California");
            PostalInfoLookupService lookup = new PostalInfoLookupService(ds);

            State retrievedState = lookup.GetStateById("NA");

            Assert.IsNull(retrievedState);
        }

        [TestMethod]
        public void ShouldRetrieveStateById()
        {
            PostalInfoLookupDataSet ds = new PostalInfoLookupDataSet();
            ds.States.AddStatesRow("CA", "California");
            ds.States.AddStatesRow("OR", "Oregon");
            PostalInfoLookupService lookup = new PostalInfoLookupService(ds);

            State retrievedState = lookup.GetStateById("OR");

            Assert.IsNotNull(retrievedState);
            Assert.AreEqual("Oregon", retrievedState.Name);
        }

        [TestMethod]
        public void ShouldGetAllCitiesInEveryStateWithEmptyState()
        {
            PostalInfoLookupDataSet ds = new PostalInfoLookupDataSet();
            ds.States.AddStatesRow("CA", "California");
            ds.States.AddStatesRow("WA", "Washington");

            ds.Zip.AddZipRow(41, "95340", "RED TOP", "CA");
            ds.Zip.AddZipRow(53, "98005", "BELLEVUE", "WA");
            ds.Zip.AddZipRow(53, "98052", "REDMOND", "WA");

            PostalInfoLookupService lookup = new PostalInfoLookupService(ds);
            List<string> cities = new List<string>(lookup.GetCities("R", "", 10));
            Assert.AreEqual(2, cities.Count);
        }

        [TestMethod]
        public void ShouldGetAllZipCodesFromStateWhenCityNotSpecified()
        {
            TestablePostalInfoLookupService lookup = GetPostalInfoLookupService();
            List<ZipCode> zips = new List<ZipCode>(lookup.GetZipCodes("", "", "wa", 5));
            Assert.AreEqual(4, zips.Count);
        }

		private static void AssertStatesMatchesTable(PostalInfoLookupDataSet.StatesDataTable states, List<State> list)
		{
			Assert.AreEqual(states.Rows.Count, list.Count, 
				"Retrieved list contains different number of entries than source table");
			for(int i = 0; i < states.Rows.Count; ++i)
			{
				AssertRowMatchesObject(states[i], list[i], i);
			}
		}

		private static void AssertRowMatchesObject(PostalInfoLookupDataSet.StatesRow row, 
									 State state, int rowNumber)
		{
			Assert.AreEqual(row.Id, state.Id, "ID does not match at index {0}", rowNumber);
			Assert.AreEqual(row.Name, state.Name, "Name does not match at index {0}", rowNumber);
		}



		private static TestablePostalInfoLookupService GetPostalInfoLookupService()
		{
			PostalInfoLookupDataSet ds = new PostalInfoLookupDataSet();
			ds.States.AddStatesRow("CA", "California");
			ds.States.AddStatesRow("OR", "Oregon");
			ds.States.AddStatesRow("WA", "Washington");

			ds.Zip.AddZipRow(53, "98001", "Algona", "WA");
			ds.Zip.AddZipRow(53, "98002", "AUBURN", "WA");
			ds.Zip.AddZipRow(53, "98005", "BELLEVUE", "WA");
			ds.Zip.AddZipRow(53, "98006", "BELLEVUE", "WA");

			ds.Zip.AddZipRow(41, "97201", "PORTLAND", "OR");

			TestablePostalInfoLookupService lookup = new TestablePostalInfoLookupService(ds, 3, 3, 2);
			return lookup;
		}
	}

	class TestablePostalInfoLookupService : PostalInfoLookupService
	{
		private PostalInfoLookupDataSet _testData;
		private int _numCitiesInWA;
		private int _numStatesInDataSet;
		private int _numZipsInBellevue;

		public TestablePostalInfoLookupService(
			PostalInfoLookupDataSet data,
			int numCitiesInWA,
			int numStatesInDataSet,
			int numZipsInBellevue) 
			: base(data)
		{
			_testData = data;
			_numCitiesInWA = numCitiesInWA;
			_numStatesInDataSet = numStatesInDataSet;
			_numZipsInBellevue = numZipsInBellevue;
		}

		public PostalInfoLookupDataSet TestData
		{
			get { return _testData; }
		}


		public int NumStatesInDataSet
		{
			get { return _numStatesInDataSet; }
		}

		public int NumCitiesInWA
		{
			get { return _numCitiesInWA; }
		}

		public int NumZipsInBellevue
		{
			get { return _numZipsInBellevue; }
		}
	}
}
