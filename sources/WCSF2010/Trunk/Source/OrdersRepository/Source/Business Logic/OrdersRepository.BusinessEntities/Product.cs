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
    public partial class Product
    {
        public Product()
        {
        }
        public Product(int productID, string productSku, string productName, decimal? unitPrice, string description)
        {
            this.productIdField = productID;
            this.productSkuField = productSku;
            this.productNameField = productName;
            this.unitPriceField = unitPrice;
            this.descriptionField = description;
        }

        //public Product(Nullable<System.Int32> categoryID, System.Boolean discontinued, System.Int32 productID, System.String productName, System.String quantityPerUnit, Nullable<System.Int16> reorderLevel, Nullable<System.Int32> supplierID, Nullable<System.Decimal> unitPrice, Nullable<System.Int16> unitsInStock, Nullable<System.Int16> unitsOnOrder)
        //{
        //    this.categoryIDField = categoryID;
        //    this.discontinuedField = discontinued;
        //    this.productIDField = productID;
        //    this.productNameField = productName;
        //    this.quantityPerUnitField = quantityPerUnit;
        //    this.reorderLevelField = reorderLevel;
        //    this.supplierIDField = supplierID;
        //    this.unitPriceField = unitPrice;
        //    this.unitsInStockField = unitsInStock;
        //    this.unitsOnOrderField = unitsOnOrder;
        //}

        private string productSkuField;

        public string ProductSku
        {
            get { return productSkuField; }
            set { productSkuField = value; }
        }

        private string descriptionField;

        public string Description
        {
            get { return descriptionField; }
            set { descriptionField = value; }
        }
		
        //private Nullable<System.Int32> categoryIDField;

        //public Nullable<System.Int32> CategoryID
        //{
        //    get { return this.categoryIDField; }
        //    set { this.categoryIDField = value; }
        //}

        //private System.Boolean discontinuedField;

        //public System.Boolean Discontinued
        //{
        //    get { return this.discontinuedField; }
        //    set { this.discontinuedField = value; }
        //}

        private Int32 productIdField;

        public Int32 ProductId
        {
            get { return this.productIdField; }
            set { this.productIdField = value; }
        }

        private String productNameField;

        public String ProductName
        {
            get { return this.productNameField; }
            set { this.productNameField = value; }
        }

        //private System.String quantityPerUnitField;

        //public System.String QuantityPerUnit
        //{
        //    get { return this.quantityPerUnitField; }
        //    set { this.quantityPerUnitField = value; }
        //}

        //private Nullable<System.Int16> reorderLevelField;

        //public Nullable<System.Int16> ReorderLevel
        //{
        //    get { return this.reorderLevelField; }
        //    set { this.reorderLevelField = value; }
        //}

        //private Nullable<System.Int32> supplierIDField;

        //public Nullable<System.Int32> SupplierID
        //{
        //    get { return this.supplierIDField; }
        //    set { this.supplierIDField = value; }
        //}

        private Nullable<Decimal> unitPriceField;

        public Nullable<Decimal> UnitPrice
        {
            get { return this.unitPriceField; }
            set { this.unitPriceField = value; }
        }

        //private Nullable<System.Int16> unitsInStockField;

        //public Nullable<System.Int16> UnitsInStock
        //{
        //    get { return this.unitsInStockField; }
        //    set { this.unitsInStockField = value; }
        //}

        //private Nullable<System.Int16> unitsOnOrderField;

        //public Nullable<System.Int16> UnitsOnOrder
        //{
        //    get { return this.unitsOnOrderField; }
        //    set { this.unitsOnOrderField = value; }
        //}

    }
}

