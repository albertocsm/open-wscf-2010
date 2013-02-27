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
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services.Utility;

namespace OrdersRepository.Services
{
	public class PostalInfoLookupService : IPostalInfoLookupService
	{
		private PostalInfoLookupDataSet _data;

		public PostalInfoLookupService(PostalInfoLookupDataSet data)
		{
			_data = data;
		}

		public IEnumerable<State> AllStates
		{
            get
            {
                foreach (PostalInfoLookupDataSet.StatesRow row in _data.States)
                {
                    yield return _data.StatesRowToState(row);
                }
            }
		}

		public IEnumerable<string> GetCities(string prefix, string state, int maxResults)
		{
            string query = string.Format(
                @"(State = '{0}' OR '{0}' = '') 
                  AND City LIKE '{1}%'",
                InputValidator.EncodeQueryStringParameter(state),
                InputValidator.EncodeQueryStringParameter(prefix));

			DataView view = new DataView(_data.Zip);
			view.RowFilter = query;
            view.Sort = "City";

			string previousCity = null;
			int rowsRemaining = maxResults;
			foreach (DataRowView viewRow in view)
			{
				PostalInfoLookupDataSet.ZipRow row = (PostalInfoLookupDataSet.ZipRow)viewRow.Row;
                //The following is to remove duplicate cities, because there is no select DISTINCT clause on in-memory datasets.
                if (row.City != previousCity)
				{
					previousCity = row.City;
					yield return row.City;
					if (--rowsRemaining == 0)
					{
						yield break;
					}
				}
			}
		}

		public IEnumerable<ZipCode> GetZipCodes(string prefix, string city, string state, int maxResults)
		{
            string query = string.Format(@"(State = '{0}' OR '{0}' = '')
									    AND (City = '{1}' OR '{1}' = '')
									    AND ZipCode LIKE '{2}%'",
									 InputValidator.EncodeQueryStringParameter(state),
									 InputValidator.EncodeQueryStringParameter(city),
									 InputValidator.EncodeQueryStringParameter(prefix));

			DataView view = new DataView(_data.Zip);
            view.RowFilter = query;

			for(int i = 0; i < view.Count && i < maxResults; ++i)
			{
				PostalInfoLookupDataSet.ZipRow row =
					(PostalInfoLookupDataSet.ZipRow)( view[i].Row );
				yield return _data.ZipRowToZipCode(row);
			}
		}


        public State GetStateById(string stateId)
        {
            PostalInfoLookupDataSet.StatesRow row = _data.States.FindById(stateId);
            if (row == null)
                return null;

            return _data.StatesRowToState(row);
        }
    }
}
