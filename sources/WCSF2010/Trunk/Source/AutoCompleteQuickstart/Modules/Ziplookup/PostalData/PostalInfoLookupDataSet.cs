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
using System.IO;
using System.Reflection;

namespace AutoCompleteQuickStart.Ziplookup.PostalData
{
	partial class PostalInfoLookupDataSet
	{
		private static PostalInfoLookupDataSet _instance;
		private static object _lockObject = new object();

		internal static PostalInfoLookupDataSet Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lockObject)
					{
						if (_instance == null)
						{
							_instance = CreateLoaded();
						}
					}
				}
				return _instance;
			}
		}

		private static PostalInfoLookupDataSet CreateLoaded()
		{
			PostalInfoLookupDataSet ds = new PostalInfoLookupDataSet();

			Assembly currentAssembly = Assembly.GetExecutingAssembly();
			using (Stream dataStream = currentAssembly.GetManifestResourceStream("AutoCompleteQuickStart.Ziplookup.PostalData.postalData.xml"))
			{
				ds.ReadXml(dataStream);
			}
			return ds;
		}
	}
}