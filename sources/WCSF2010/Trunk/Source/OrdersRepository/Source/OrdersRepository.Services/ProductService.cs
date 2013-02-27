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
using System.Globalization;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrdersRepository.Services.Utility;

namespace OrdersRepository.Services
{

    public class ProductService: IProductService
    {
        OrdersManagementDataSet repository;

        public ProductService()
            : this(OrdersManagementRepository.Instance)
        {  }
        
        public ProductService(OrdersManagementDataSet repository)
        {
            this.repository = repository;
        }

        public Product GetProductBySku(string productSku)
        {
			productSku = InputValidator.EncodeQueryStringParameter(productSku);
            OrdersManagementDataSet.ProductsRow[] productRows = repository.Products.Select(string.Format(CultureInfo.InvariantCulture, "ProductSKU = '{0}'", productSku)) as OrdersManagementDataSet.ProductsRow[];
            if (productRows.Length > 0)
            {
                return TranslateFromProductsRowToProductEntity(productRows[0]);
            }

            return null;
        }

        public IList<Product> SearchProducts(string searchText)
        {
			searchText = InputValidator.EncodeQueryStringParameter(searchText);
			OrdersManagementDataSet.ProductsRow[] productRows = repository.Products.Select(string.Format(CultureInfo.InvariantCulture, "ProductName like '*{0}*' OR Description like '*{0}*' OR ProductSku like '{0}*'", searchText)) as OrdersManagementDataSet.ProductsRow[];
            IList<Product> productList = new List<Product>();
            foreach (OrdersManagementDataSet.ProductsRow row in productRows)
            {
                productList.Add(TranslateFromProductsRowToProductEntity(row));
            }

            return productList;
        }

        private static Product TranslateFromProductsRowToProductEntity(OrdersManagementDataSet.ProductsRow productsRow)
        {
            Product product = new Product();
            product.ProductId = productsRow.ProductId;
            product.ProductSku = productsRow.ProductSKU;
            product.ProductName = productsRow.ProductName;
			product.Description = productsRow.Description;
            product.UnitPrice = productsRow.UnitPrice;

            return product;
        }

        public Product GetProductById(int productId)
        {
            OrdersManagementDataSet.ProductsRow productRow = repository.Products.FindByProductId(productId);
            if (productRow == null)
            {
                return null;
            }

            return TranslateFromProductsRowToProductEntity(productRow);
        }
    }
}
