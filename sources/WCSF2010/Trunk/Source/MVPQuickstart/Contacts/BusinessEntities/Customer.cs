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
using System.Data;
using System.Configuration;
using System.Web;

namespace MVPQuickstart.Contacts.BusinessEntities
{
	public class Customer
	{
		private String _address;
		private String _city;
		private String _name;
		private String _id;
		private String _zip;
		private String _state;

        public Customer()
        {
        }

		public Customer(string id, string name, string address, string city, string state, string zip)
		{
			_id = id;
			_name = name;
			_address = address;
			_city = city;
			_state = state;
			_zip = zip;
		}

		public String Address
		{
			get { return this._address; }
			set { this._address = value; }
		}

		public String City
		{
			get { return this._city; }
			set { this._city = value; }
		}

		public String Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		public String Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		public String Zip
		{
			get { return this._zip; }
			set { this._zip = value; }
		}

		public String State
		{
			get { return this._state; }
			set { this._state = value; }
		}

	}
}
