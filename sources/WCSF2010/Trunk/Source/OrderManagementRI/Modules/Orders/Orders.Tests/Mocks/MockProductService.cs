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
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Tests.Mocks
{
    public class MockProductService: IProductService
    {
        List<Product> _searchedProducts;
        public bool SearchProductsCalled;
        public bool GetProductBySkuCalled;

        public MockProductService()
        {
            _searchedProducts = new List<Product>();
        }

        public Product GetProductBySku(string sku)
        {
            GetProductBySkuCalled = true;
            if (!_searchedProducts.Exists(delegate (Product p){return p.ProductSku==sku;}))
            {
                return null;
            }
            return _searchedProducts.Find(delegate(Product p) { return p.ProductSku == sku; });
        }

        public void AddProduct(Product product)
        {
            _searchedProducts.Add(product);
        }

        public IList<Product> SearchProducts(string searchText)
        {
            SearchProductsCalled = true;
            return _searchedProducts;
        }

        public Product GetProductById(int productId)
        {
            if (!_searchedProducts.Exists(delegate(Product p) { return p.ProductId == productId; }))
            {
                throw new ArgumentException("The product does not exists.");
            }
            return _searchedProducts.Find(delegate(Product p) { return p.ProductId == productId; });
        }

		public List<Product> SearchedProducts
		{
			get { return _searchedProducts; }
		}
    }
}
