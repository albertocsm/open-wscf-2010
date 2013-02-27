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

namespace OrdersRepository.BusinessEntities
{
	[Serializable]
	public partial class Order
	{
		public Order()
		{
		}

		public Order(Int32 orderId, String orderName, String customerId, String approver, String description, Int32 orderStatus, Nullable<DateTime> orderDate, String shipAddress, String shipCity, String shipPostalCode, String shipRegion, Nullable<DateTime> shippedDate, String creator)
		{
			this.CustomerIdField = customerId;
			this.descriptionField = description;
			this.approverField = approver;
			this.creatorField = creator;
			this.orderDateField = orderDate;
			this.orderIdField = orderId;
			this.orderNameField = orderName;
			this.orderStatusField = orderStatus;
			this.shipAddressField = shipAddress;
			this.shipCityField = shipCity;
			this.shippedDateField = shippedDate;
			this.shipPostalCodeField = shipPostalCode;
			this.shipRegionField = shipRegion;
		}

		private String CustomerIdField;

		public String CustomerId
		{
			get { return this.CustomerIdField; }
			set { this.CustomerIdField = value; }
		}

		private String descriptionField;

		public String Description
		{
			get { return this.descriptionField; }
			set { this.descriptionField = value; }
		}

		private String approverField;

		public String Approver
		{
			get { return this.approverField; }
			set { this.approverField = value; }
		}

		private String creatorField;

		public String Creator
		{
			get { return creatorField; }
			set { creatorField = value; }
		}

		private Nullable<DateTime> orderDateField;

		public Nullable<DateTime> OrderDate
		{
			get { return this.orderDateField; }
			set { this.orderDateField = value; }
		}

		private Int32 orderIdField;

		public Int32 OrderId
		{
			get { return this.orderIdField; }
			set { this.orderIdField = value; }
		}

		private String orderNameField;

		public String OrderName
		{
			get { return this.orderNameField; }
			set { this.orderNameField = value; }
		}

		private Int32 orderStatusField;

		public Int32 OrderStatus
		{
			get { return this.orderStatusField; }
			set { this.orderStatusField = value; }
		}

		private String shipAddressField;

		public String ShipAddress
		{
			get { return this.shipAddressField; }
			set { this.shipAddressField = value; }
		}

		private String shipCityField;

		public String ShipCity
		{
			get { return this.shipCityField; }
			set { this.shipCityField = value; }
		}

		private Nullable<DateTime> shippedDateField;

		public Nullable<DateTime> ShippedDate
		{
			get { return this.shippedDateField; }
			set { this.shippedDateField = value; }
		}

		private String shipPostalCodeField;

		public String ShipPostalCode
		{
			get { return this.shipPostalCodeField; }
			set { this.shipPostalCodeField = value; }
		}

		private String shipRegionField;

		public String ShipRegion
		{
			get { return this.shipRegionField; }
			set { this.shipRegionField = value; }
		}

		private IList<OrderDetail> _orderDetails = new List<OrderDetail>();

		public IList<OrderDetail> Details
		{
			get { return _orderDetails; }
			set { _orderDetails = value; }
		}

		public decimal OrderTotal
		{
			get
			{
				decimal total = 0;
				foreach (OrderDetail detail in _orderDetails)
				{
					total += detail.LineTotal;
				}
				return total;
			}
		}
	}
}

