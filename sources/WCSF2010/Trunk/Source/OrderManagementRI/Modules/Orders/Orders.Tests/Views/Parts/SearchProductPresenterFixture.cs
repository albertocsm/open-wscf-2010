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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.Orders.Views.Parts;
using OrderManagement.Orders.Tests.Mocks;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Orders.Tests
{
	/// <summary>
	/// Summary description for SearchProductPresenterTestFixture
	/// </summary>
	[TestClass]
	public class SearchProductPresenterTestFixture
	{
		SearchProductPresenter presenter;
		MockProductService productService;
		MockSearchProduct view;

		[TestInitialize]
		public void InitMVP()
		{
			view = new MockSearchProduct();
			productService = new MockProductService();
			presenter = new SearchProductPresenter(productService);

			presenter.View = view;
		}

		[TestMethod]
		public void ShouldShowProductsOnSearchTextChanged()
		{
			string searchText = "Product";
			presenter.OnSearchTextChanged(searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
		}

		[TestMethod]
		public void ShouldShow10FirstProductsOnSearchTextChanged()
		{
			string searchText = "Product";
			productService.AddProduct(new Product(1, "1", "Product A", null, null));
			productService.AddProduct(new Product(2, "2", "Product B", null, null));
			productService.AddProduct(new Product(3, "3", "Product C", null, null));
			productService.AddProduct(new Product(4, "4", "Product D", null, null));
			productService.AddProduct(new Product(5, "5", "Product E", null, null));
			productService.AddProduct(new Product(6, "6", "Product F", null, null));
			productService.AddProduct(new Product(7, "7", "Product G", null, null));
			productService.AddProduct(new Product(8, "8", "Product H", null, null));
			productService.AddProduct(new Product(9, "9", "Product I", null, null));
			productService.AddProduct(new Product(10, "10", "Product J", null, null));
			productService.AddProduct(new Product(11, "11", "Product K", null, null));
			productService.AddProduct(new Product(12, "12", "Product L", null, null));
			presenter.OnSearchTextChanged(searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(10, view.Products.Count);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingAscending()
		{
			productService.AddProduct(new Product(1, "1", "Product A", null, null));
			productService.AddProduct(new Product(3, "3", "Product C", null, null));
			productService.AddProduct(new Product(2, "2", "Product B", null, null));
			string searchText = "Product";
			presenter.OnSorting("ProductName", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 1);
			Assert.AreEqual(view.Products[1].ProductId, 2);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingDescending()
		{
			productService.AddProduct(new Product(1, "1", "Product A", null, null));
			productService.AddProduct(new Product(3, "3", "Product C", null, null));
			productService.AddProduct(new Product(2, "2", "Product B", null, null));
			string searchText = "Product";
			presenter.OnSorting("ProductName", false, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 3);
			Assert.AreEqual(view.Products[1].ProductId, 2);
			Assert.AreEqual(view.Products[2].ProductId, 1);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingByPrice()
		{
			productService.AddProduct(new Product(1, "1", null, 1.00m, null));
			productService.AddProduct(new Product(3, "3", null, 3.00m, null));
			productService.AddProduct(new Product(2, "2", null, 2.00m, null));
			string searchText = "Product";
			presenter.OnSorting("UnitPrice", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 1);
			Assert.AreEqual(view.Products[1].ProductId, 2);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingBySku()
		{
			productService.AddProduct(new Product(1, "1", null, null, null));
			productService.AddProduct(new Product(3, "3", null, null, null));
			productService.AddProduct(new Product(2, "2", null, null, null));
			string searchText = "Product";
			presenter.OnSorting("ProductSKU", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 1);
			Assert.AreEqual(view.Products[1].ProductId, 2);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingByDescription()
		{
			string searchText = "Product";
			presenter.OnSorting("Description", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingByPriceOnNullValue()
		{
			productService.AddProduct(new Product(1, "1", null, 1.00m, null));
			productService.AddProduct(new Product(2, "2", null, null, null));
			productService.AddProduct(new Product(3, "3", null, 3.00m, null));
			string searchText = "Product";
			presenter.OnSorting("UnitPrice", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 2);
			Assert.AreEqual(view.Products[1].ProductId, 1);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingBySkuOnNullValue()
		{
			productService.AddProduct(new Product(1, "1", null, null, null));
			productService.AddProduct(new Product(2, null, null, null, null));
			productService.AddProduct(new Product(3, "3", null, null, null));
			string searchText = "Product";
			presenter.OnSorting("ProductSKU", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 2);
			Assert.AreEqual(view.Products[1].ProductId, 1);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingByProductNameOnNullValue()
		{
			productService.AddProduct(new Product(1, "1", "Product A", null, null));
			productService.AddProduct(new Product(2, "2", null, null, null));
			productService.AddProduct(new Product(3, "3", "Product C", null, null));
			string searchText = "Product";
			presenter.OnSorting("ProductName", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 2);
			Assert.AreEqual(view.Products[1].ProductId, 1);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldShowSortedProductsOnSortingByDescriptionOnNullValue()
		{
			productService.AddProduct(new Product(1, "1", null, null, "Description A"));
			productService.AddProduct(new Product(2, "2", null, null, null));
			productService.AddProduct(new Product(3, "3", null, null, "Description C"));
			string searchText = "Product";
			presenter.OnSorting("Description", true, searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.IsTrue(view.ShowProductsCalled);
			Assert.AreEqual(view.Products[0].ProductId, 2);
			Assert.AreEqual(view.Products[1].ProductId, 1);
			Assert.AreEqual(view.Products[2].ProductId, 3);
		}

		[TestMethod]
		public void ShouldSetAscendingOrderOnViewInitilize()
		{
			presenter.OnViewInitialized();

			Assert.IsTrue(view.OrderDirectionAscending);
		}

		[TestMethod]
		public void ShouldSetOrderDirectionOnSorting()
		{
			presenter.OnSorting("UnitPrice", true, "Any search text");

			Assert.IsTrue(view.AscendingOrderChanged);
			Assert.AreEqual(true, view.OrderDirectionAscending);
		}

		[TestMethod]
		public void ShouldShowMessageWhenNoResultsFound()
		{
			string searchText = "Product";
			presenter.OnSearchTextChanged(searchText);

			Assert.IsTrue(productService.SearchProductsCalled);
			Assert.AreEqual("No results were found.", view.MessageText);
		}
	}

	class MockSearchProduct : ISearchProduct
	{
		public bool ShowProductsCalled;
		public IList<Product> Products;
		public bool ascendingOrderField;
		public bool AscendingOrderChanged;
		public string SelectedProductArgument;
		public string MessageText;
		public bool ResetSelectedProductCalled;

		public void ShowProductsResults(IList<Product> products)
		{
			ShowProductsCalled = true;
			Products = products;
		}

		public bool OrderDirectionAscending
		{
			get { return ascendingOrderField; }
			set
			{
				ascendingOrderField = value;
				AscendingOrderChanged = true;
			}
		}

		public void OnProductSelected(string selectedProductSku)
		{
			SelectedProductArgument = selectedProductSku;
		}

		public void ShowNoResultsMessage(string message)
		{
			MessageText = message;
		}

		public void ResetSelectedProduct()
		{
			ResetSelectedProductCalled = true;
		}
	}
}

