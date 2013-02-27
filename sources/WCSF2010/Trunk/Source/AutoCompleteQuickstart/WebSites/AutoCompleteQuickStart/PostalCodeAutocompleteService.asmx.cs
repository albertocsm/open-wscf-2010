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
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using AutoCompleteQuickStart.Ziplookup.Services;

namespace AutoCompleteQuickStart
{
	/// <summary>
	/// The PostalCodeAutocompleteService is an XML and JSON web service
	/// that allows a client to get a throttled list of either cities or postal (zip) codes.
	/// This web service heavily utilizes the ZipLookupService.
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	[ScriptService]
	public class PostalCodeAutocompleteService : WebService
	{
		/// <summary>
		/// Get a list of zip codes, based on the provided criteria.
		/// </summary>
		/// <param name="prefixText">Text that the zip codes returned must start with.</param>
		/// <param name="count">The maximum number of zip codes to return.</param>
		/// <param name="contextValues">A Dictionary that should contain a State and a City 
		/// value to be used in the filter</param>
		/// <returns>A string array of zip codes</returns>
		[WebMethod]
		public string[] GetZipCodes(string prefixText, int count, Dictionary<string, string> contextValues)
		{
			string state = contextValues["State"];
			string city = contextValues["City"];
			return ZipLookupService.GetZipCodes(prefixText, count, state, city);
		}

		/// <summary>
		/// Gets a list of cities, based on the provided criteria.
		/// </summary>
		/// <param name="prefixText">Text that cities returned must start with.</param>
		/// <param name="count">The maximum number of cities to return.</param>
		/// <param name="contextValues">A Dictionary that should contain a State
		/// value to be used in the filter</param>
		/// <returns>A string array of cities</returns>
		[WebMethod]
		public string[] GetCities(string prefixText, int count, Dictionary<string, string> contextValues)
		{
			string state = contextValues["State"];
			return ZipLookupService.GetCities(prefixText, count, state);
		}
	}
}