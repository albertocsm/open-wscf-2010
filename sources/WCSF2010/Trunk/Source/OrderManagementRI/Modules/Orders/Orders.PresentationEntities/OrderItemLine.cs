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

namespace Orders.PresentationEntities
{
    [Serializable]
    public class OrderItemLine
    {
		private int _productId;
		private string _sku;
        private string _name;
        private decimal? _price;
        private short _quantity;
        private decimal? _total;
        private bool _selected;
        private Guid _id;

        public OrderItemLine()
        {
            _id = Guid.NewGuid();
        }

        public OrderItemLine(int productId, string sku, string name, decimal? price, short quantity, decimal? total, bool selected)
            :this()
        {
			_productId = productId;
            _sku = sku;
            _name = name;
            _price = price;
            _quantity = quantity;
            _total = total;
            _selected = selected;
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }


		public int ProductId
		{
			get { return _productId; }
			set { _productId = value; }
		}

        public string Sku
        {
            get { return _sku; }
            set { _sku = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public decimal? Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public short Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public decimal? Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public override bool Equals(object obj)
        {
            OrderItemLine line = obj as OrderItemLine;
            if (line != null)
            {
                return line.Id == this.Id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
