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
using System.Data;
using System.Globalization;
using AutoCompleteQuickStart.Ziplookup.BusinessEntities;
using AutoCompleteQuickStart.Ziplookup.PostalData;

namespace AutoCompleteQuickStart.Ziplookup.Services
{
	/// <summary>
	/// This is a simple service used to expose the Postal Code database from 1999,
	/// which is the last year the US Census released this information.
	/// This uses a DataSet only for convenience in this example.  
	/// Ideally, this information would be in a full SQL Server database, using the
	/// Enterprise Library Data Access Block.
	/// 
	/// Also, you would NEVER use string concatenation on SQL query strings, like we
	///  show below for simplicity.
	/// The parameters SHOULD be passed as parameters to a SQL query or stored procedure.
	/// </summary>
	public sealed class ZipLookupService
	{
		private ZipLookupService()
		{
		}

		public static string[] GetZipCodes(string prefixText, int count, string state, string city)
		{
			PostalInfoLookupDataSet ds = PostalInfoLookupDataSet.Instance;
			string zipCodeFilter = string.Format(CultureInfo.CurrentCulture,
			                                     @"(State = '{0}' OR '{0}' = '')
									    AND (City = '{1}' OR '{1}' = '')
									    AND ZipCode LIKE '{2}%'"
			                                     , state, city, prefixText);

			DataView dw = new DataView(ds.Zip);
			dw.RowFilter = zipCodeFilter;
			dw.Sort = "ZipCode";

			List<string> resultZipCodes = new List<string>();
			int remainingValues = count;

			foreach (DataRowView rowView in dw)
			{
				PostalInfoLookupDataSet.ZipRow zipRow = (PostalInfoLookupDataSet.ZipRow) rowView.Row;
				resultZipCodes.Add(zipRow.ZipCode);
				remainingValues--;
				if (remainingValues == 0)
					break;
			}
			return resultZipCodes.ToArray();
		}

		public static string[] GetCities(string prefixText, int count, string state)
		{
			PostalInfoLookupDataSet ds = PostalInfoLookupDataSet.Instance;
			string cityFilter = string.Format(CultureInfo.CurrentCulture,
			                                  @"(State = '{0}' OR '{0}' = '') 
                  AND City LIKE '{1}%'"
			                                  , state, prefixText);
			DataView dw = new DataView(ds.Zip);
			dw.RowFilter = cityFilter;
			dw.Sort = "City";

			List<string> resultCities = new List<string>();
			string previousCity = null;
			int remainingValues = count;

			foreach (DataRowView rowView in dw)
			{
				PostalInfoLookupDataSet.ZipRow zipRow = (PostalInfoLookupDataSet.ZipRow) rowView.Row;
				//The following is to remove duplicate cities, because there is no select DISTINCT clause on in-memory datasets.
				if (zipRow.City != previousCity)
				{
					previousCity = zipRow.City;
					resultCities.Add(zipRow.City);
					remainingValues--;
					if (remainingValues == 0)
						break;
				}
			}

			return resultCities.ToArray();
		}

		public static State[] GetStates()
		{
			List<State> statesList = new List<State>();

			foreach (PostalInfoLookupDataSet.StatesRow row in PostalInfoLookupDataSet.Instance.States.Rows)
			{
				statesList.Add(new State(row.Id, row.Name));
			}

			return statesList.ToArray();
		}
	}
}