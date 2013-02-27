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

namespace OrdersRepository.BusinessEntities
{
	[Serializable]
	public partial class OrderDetail
	{
		public OrderDetail(Int32 orderId, Int32 productId, Int16 quantity, Decimal unitPrice)
		{
			this.orderIdField = orderId;
			this.productIdField = productId;
			this.quantityField = quantity;
			this.unitPriceField = unitPrice;
		}

		private Int32 orderIdField;

		public Int32 OrderId
		{
			get { return this.orderIdField; }
			set { this.orderIdField = value; }
		}

		private Int32 productIdField;

		public Int32 ProductId
		{
			get { return this.productIdField; }
			set { this.productIdField = value; }
		}

		private Int16 quantityField;

		public Int16 Quantity
		{
			get { return this.quantityField; }
			set { this.quantityField = value; }
		}

		private Decimal unitPriceField;

		public Decimal UnitPrice
		{
			get { return this.unitPriceField; }
			set { this.unitPriceField = value; }
		}

		public decimal LineTotal
		{
			get { return quantityField * unitPriceField; }
		}
	}
}

