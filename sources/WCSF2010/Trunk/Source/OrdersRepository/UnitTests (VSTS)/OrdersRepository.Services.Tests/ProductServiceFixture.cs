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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersRepository.BusinessEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrdersRepository.Services.Tests
{
    /// <summary>
    /// Summary description for OrdersServiceFixture
    /// </summary>
    [TestClass]
    public class ProductServiceFixture
    {
        [TestMethod]
        public void CanCreateInstanceWithoutPassingRepositoryToConstructor()
        {
            ProductService service = new ProductService();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ShouldGetProductBySku()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            OrdersManagementDataSet.ProductsRow _product = _dataset.Products.AddProductsRow("Product A", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product B", "2222-33333", "Description of Product B", 1.00m);

            Product _foundProduct = _service.GetProductBySku("1234-56789");

            Assert.AreEqual(_product.ProductId, _foundProduct.ProductId);
        }

        [TestMethod]
        public void ShouldReturnNullWhenSkuNotExists()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product A", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product B", "2222-33333", "Description of Product B", 1.00m);

            Product _foundProduct = _service.GetProductBySku("0000-00000");

            Assert.IsNull(_foundProduct);
        }

        [TestMethod]
        public void ShouldReturnAllProducts()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product NameA 01", "01", "Description of Product A 01", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 02", "02", "Description of Product A 02", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 03", "03", "Description of Product A 03", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 04", "04", "Description of Product A 04", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 05", "05", "Description of Product A 05", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 06", "06", "Description of Product A 06", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 07", "07", "Description of Product A 07", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 08", "08", "Description of Product A 08", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 09", "09", "Description of Product A 09", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 10", "10", "Description of Product A 10", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 11", "11", "Description of Product A 11", 1.00m);
            _dataset.Products.AddProductsRow("Product NameA 12", "12", "Description of Product A 12", 1.00m);

            ICollection<Product> _foundProducts = _service.SearchProducts("NameA");

            Assert.AreEqual(12, _foundProducts.Count);
        }

        [TestMethod]
        public void ShouldSearchProductsByName()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product NameA", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product Another NameA ", "9876-54321", "Description of the second Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product NameB", "2222-33333", "Description of Product B", 1.00m);

            ICollection<Product> _foundProducts = _service.SearchProducts("NameA");
            List<Product> searchableList = new List<Product>(_foundProducts);

            Assert.AreEqual(2, _foundProducts.Count);
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "1234-56789"; }));
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "9876-54321"; }));
        }

        [TestMethod]
        public void ShouldSearchProductsByDescription()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product NameA", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product Another NameA ", "9876-54321", "Description of the second Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product NameB", "2222-33333", "Description of Product B", 1.00m);

            ICollection<Product> _foundProducts = _service.SearchProducts("Product A");
            List<Product> searchableList = new List<Product>(_foundProducts);

            Assert.AreEqual(2, _foundProducts.Count);
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "1234-56789"; }));
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "9876-54321"; }));
        }

        [TestMethod]
        public void ShouldSearchProductsBySku()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product NameA", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product Another NameA ", "1234-11111", "Description of the second Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product NameB", "2222-33333", "Description of Product B", 1.00m);

            ICollection<Product> _foundProducts = _service.SearchProducts("1234");
            List<Product> searchableList = new List<Product>(_foundProducts);

            Assert.AreEqual(2, _foundProducts.Count);
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "1234-56789"; }));
            Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "1234-11111"; }));
        }

        [TestMethod]
        public void ShouldGetProductById()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            OrdersManagementDataSet.ProductsRow _product = _dataset.Products.AddProductsRow("Product A", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product B", "2222-33333", "Description of Product B", 1.00m);

            Product _foundProduct = _service.GetProductById(_product.ProductId);

            Assert.AreEqual("1234-56789", _foundProduct.ProductSku);
            Assert.AreEqual("Product A", _foundProduct.ProductName);
			Assert.AreEqual("Description of Product A", _foundProduct.Description);
        }

        [TestMethod]
        public void ShouldReturnNullWhenIdNotExists()
        {
            OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
            ProductService _service = new ProductService(_dataset);
            _dataset.Products.AddProductsRow("Product A", "1234-56789", "Description of Product A", 1.00m);
            _dataset.Products.AddProductsRow("Product B", "2222-33333", "Description of Product B", 1.00m);

            Product _foundProduct = _service.GetProductById(-1);

            Assert.IsNull(_foundProduct);
        }

		[TestMethod]
		public void GetProductBySkuWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
			ProductService _service = new ProductService(_dataset);
			OrdersManagementDataSet.ProductsRow _product = _dataset.Products.AddProductsRow("Product A", "1234-56'89", "Description of Product A", 1.00m);
			_dataset.Products.AddProductsRow("Product B", "2222-33333", "Description of Product B", 1.00m);

			Product _foundProduct = _service.GetProductBySku("1234-56'89");

			Assert.AreEqual(_product.ProductId, _foundProduct.ProductId);
		}

		[TestMethod]
		public void ShouldSearchProductsWithQuotesSearchesCorrectlyAndPreventsInjection()
		{
			OrdersManagementDataSet _dataset = new OrdersManagementDataSet();
			ProductService _service = new ProductService(_dataset);
			_dataset.Products.AddProductsRow("Product'A", "1234-56789", "Description of Product A", 1.00m);
			_dataset.Products.AddProductsRow("Product Another'A ", "9876-54321", "Description of the second Product A", 1.00m);
			_dataset.Products.AddProductsRow("Product Name'B", "2222-33333", "Description of Product B", 1.00m);

			ICollection<Product> _foundProducts = _service.SearchProducts("'A");
			List<Product> searchableList = new List<Product>(_foundProducts);

			Assert.AreEqual(2, _foundProducts.Count);
			Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "1234-56789"; }));
			Assert.IsTrue(searchableList.Exists(delegate(Product product) { return product.ProductSku == "9876-54321"; }));
		}
    }
}
