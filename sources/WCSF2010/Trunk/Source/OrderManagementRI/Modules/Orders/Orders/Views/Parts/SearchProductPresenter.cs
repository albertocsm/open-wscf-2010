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
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using OrderManagement.Orders.Properties;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Views.Parts
{
    public class SearchProductPresenter : Presenter<ISearchProduct>
    {
        private IProductService _productService;
        private Dictionary<string, Comparison<Product>> _comparisonDictionary;

        [InjectionConstructor]
        public SearchProductPresenter(
            [ServiceDependency] IProductService productService)
        {
            _productService = productService;
            _comparisonDictionary = new Dictionary<string, Comparison<Product>>();
            _comparisonDictionary.Add("ProductName", new Comparison<Product>(CompareByProductName));
            _comparisonDictionary.Add("UnitPrice", new Comparison<Product>(CompareByUnitPrice));
            _comparisonDictionary.Add("ProductSKU", new Comparison<Product>(CompareByProductSKU));
            _comparisonDictionary.Add("Description", new Comparison<Product>(CompareByDescription));
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.OrderDirectionAscending = true;
        }

        public void OnSearchTextChanged(string searchText)
        {
            View.ResetSelectedProduct();

            List<Product> products = new List<Product>(_productService.SearchProducts(searchText));

            if (products.Count > 10)
            {
                products = products.GetRange(0, 10);
            }

            View.ShowProductsResults(products);

			if (products.Count == 0)
			{
				View.ShowNoResultsMessage(Resources.NoResultsFoundMessage);
			}
        }

        public void OnSorting(string sortExpression, bool ascending, string searchText)
        {
            List<Product> products = new List<Product>(_productService.SearchProducts(searchText));

            products.Sort(_comparisonDictionary[sortExpression]);

            if (!ascending)
            {
                products.Reverse();
            }
            View.OrderDirectionAscending = ascending;
            View.ShowProductsResults(products);
        }

        private static int CompareByProductName(Product product1, Product product2)
        {
            if (product1 == product2)
            {
                return 0;
            }
            if (product1.ProductName == null)
            {
                return -1;
            }
            return product1.ProductName.CompareTo(product2.ProductName);
        }

        private static int CompareByUnitPrice(Product product1, Product product2)
        {
            if (product1 == product2)
            {
                return 0;
            }
            if (!product1.UnitPrice.HasValue)
            {
                return -1;
            }
            return product1.UnitPrice.Value.CompareTo(product2.UnitPrice);
        }

        private static int CompareByProductSKU(Product product1, Product product2)
        {
            if (product1 == product2)
            {
                return 0;
            }
            if (product1 == null || product1.ProductSku == null)
            {
                return -1;
            }
            return product1.ProductSku.CompareTo(product2.ProductSku);
        }

        private static int CompareByDescription(Product product1, Product product2)
        {
            if (product1 == product2)
            {
                return 0;
            }
            if (product1 == null || product1.Description == null)
            {
                return -1;
            }
            return product1.Description.CompareTo(product2.Description);
        }
    }
}




