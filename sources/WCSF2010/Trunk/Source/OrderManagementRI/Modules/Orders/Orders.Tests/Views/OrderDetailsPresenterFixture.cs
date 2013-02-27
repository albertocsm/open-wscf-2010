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
using OrderManagement.Orders.Views;
using Orders.PresentationEntities;
using OrderManagement.Orders.Tests.Mocks;
using OrderManagement.Orders.Converters;
using OrdersRepository.BusinessEntities;

namespace OrderManagement.Orders.Tests
{
	/// <summary>
	/// Summary description for DetailsPresenterTestFixture
	/// </summary>
	[TestClass]
	public class DetailsPresenterTestFixture
	{
		private MockDetails view;
		private OrderDetailsPresenter presenter;
		private MockOrdersController controller;
		private MockProductService productService;
		private IBusinessPresentationConverter<OrderDetail, OrderItemLine> orderDetailsConverter;


		[TestInitialize]
		public void InitMVP()
		{
			view = new MockDetails();
			controller = new MockOrdersController();
			productService = new MockProductService();
			orderDetailsConverter = new OrderDetailsConverter(productService);
			presenter = new OrderDetailsPresenter(controller, productService, orderDetailsConverter);
			presenter.View = view;
		}

		[TestMethod]
		public void ShouldShowOrderItemLinesOnLoad()
		{
			presenter.OnViewLoaded();

			Assert.IsTrue(view.OrderItemsLinesShown);
		}

		[TestMethod]
		public void ShouldAddAndShowNewLineOnAdd()
		{
			presenter.OnAddOrderItemLine();
			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(1, view.OrderItemsLines.Count);
			Assert.AreEqual((short)1, view.OrderItemsLines[0].Quantity);
		}

		[TestMethod]
		public void ShouldDeleteAndShowRemainingOrderItemLinesOnDelete()
		{
			List<OrderItemLine> lines = new List<OrderItemLine>();
			OrderItemLine orderItemLine = new OrderItemLine();
			view.OrderItemsLines.Add(orderItemLine);
			lines.Add(orderItemLine);

			presenter.OnDeleteOrderItemLines(lines);

			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(0, view.OrderItemsLines.Count);
		}

		[TestMethod]
		public void ShouldDeleteCorrectOrderLinesOnDelete()
		{
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.90m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			OrderItemLine line3 = new OrderItemLine(3, "4356-43239", "#6 Pencil Pack", 1.99m, 10, 19.90m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);
			view.OrderItemsLines.Add(line3);

			List<OrderItemLine> lines = new List<OrderItemLine>();
			lines.Add(line1);
			lines.Add(line3);

			presenter.OnDeleteOrderItemLines(lines);

			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(1, view.OrderItemsLines.Count);
			Assert.AreEqual(line2, view.OrderItemsLines[0]);
		}

		[TestMethod]
		public void ShouldUpdateItemTotalAndOrderTotalOnOrderItemLineChanged()
		{
			decimal? price = 1.99m;

			productService.AddProduct(new Product(1, "1234-56789", "#2 Pencil Pack", 1.99m, null));
			productService.AddProduct(new Product(2, "9876-54321", "Stencil Pad", 0.79m, null));

			//Total set to 0, so it can be updated by the presenter.
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", price, 10, 0m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);

			short newQty = 20;
			OrderItemLine lineUpdated = new OrderItemLine(1, line1.Sku, line1.Name, line1.Price, newQty, 0, false);
			lineUpdated.Id = line1.Id;

			presenter.OnChangedOrderItemLine(lineUpdated);

			Assert.IsTrue(view.OrderItemsLinesRequested);
			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(newQty, view.OrderItemsLines[0].Quantity);
			Assert.AreEqual(price * newQty, view.OrderItemsLines[0].Total);
			Assert.AreEqual(line1.Total + line2.Total, view.OrderTotalPrice);
		}

		[TestMethod]
		public void ShouldUpdateProductPriceOnChangeOrderItemLine()
		{
			productService.AddProduct(new Product(1, "1234-56789", "#2 Pencil Pack", 1.99m, null));
			decimal fakePrice = 0.01m;
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "Fake product name", fakePrice, 10, 0m, false);
			view.OrderItemsLines.Add(line1);

			presenter.OnChangedOrderItemLine(line1);

			Assert.IsTrue(view.OrderItemsLinesRequested);
			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(1.99m, view.OrderItemsLines[0].Price);
			Assert.AreEqual("#2 Pencil Pack", view.OrderItemsLines[0].Name);
		}

		[TestMethod]
		public void ShouldSubmitOrderItemsToControllerOnSave()
		{
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);

			presenter.OnSave();

			Assert.IsTrue(controller.SaveCurrentOrderAsDraftCalled);
			Assert.AreEqual(2, controller.CurrentOrder.Details.Count);
			Assert.AreEqual(1, controller.CurrentOrder.Details[0].ProductId);
			Assert.AreEqual(2, controller.CurrentOrder.Details[1].ProductId);
		}

		[TestMethod]
		public void ShouldMaintainSelectedValueWhenEditingOrderLine()
		{
			view.OrderItemsLines = new List<OrderItemLine>();
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);

			bool selectedValue = true;
			int rowIndex = 1;

			presenter.OnEditOrderItemLine(rowIndex, selectedValue);

			Assert.IsTrue(view.OrderItemsLinesRequested);
			Assert.IsTrue(view.OrderItemsLinesShown);
			Assert.AreEqual(selectedValue, view.OrderItemsLines[rowIndex].Selected);
		}

		[TestMethod]
		public void OnCancelCallsCancelChangesFromDetailsView()
		{
			Assert.IsFalse(controller.CancelChangesCalled);
			presenter.OnCancel();
			Assert.IsTrue(controller.CancelChangesCalled);
		}

		[TestMethod]
		public void OnPreviousCallsNavigatePreviousFromDetailsView()
		{
			Assert.IsFalse(controller.NavigatePreviousFromDetailsViewCalled);

			presenter.OnPrevious();

			Assert.IsTrue(controller.NavigatePreviousFromDetailsViewCalled);
		}

		[TestMethod]
		public void OnNextCallsNavigateNextFromDetailsView()
		{
			Assert.IsFalse(controller.NavigateNextFromDetailsViewCalled);

			presenter.OnNext();

			Assert.IsTrue(controller.NavigateNextFromDetailsViewCalled);
		}

		[TestMethod]
		public void ShouldUpdateOrderItemSkuOnProductSelected()
		{
			view.OrderItemsLines = new List<OrderItemLine>();
			productService.AddProduct(new Product(1, "1234-56789", "#2 Pencil Pack", 1.99m, null));
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);
			view.EditingOrderItemLine = line1;

			presenter.OnProductSelected("1234-56789");

			Assert.IsTrue(view.SetEditingProductCalled);
			Assert.AreEqual("1234-56789", view.ShownSku);
			Assert.AreEqual("#2 Pencil Pack", view.ShownName);
			Assert.AreEqual(1.99m, view.ShownPrice);
		}

		[TestMethod]
		public void ShouldNotUpdateOrderItemSkuOnNullProductSelected()
		{
			view.OrderItemsLines = new List<OrderItemLine>();
			productService.AddProduct(new Product(1, "1234-56789", "#2 Pencil Pack", 1.99m, null));
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 10, 7.9m, false);
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);
			view.EditingOrderItemLine = line1;

			presenter.OnProductSelected(null);

			Assert.IsFalse(view.SetEditingProductCalled);
		}

		[TestMethod]
		public void OnViewInitializedShouldDisplayOrderDetailsInLoadOrderFlow()
		{
			Order order = new Order();
			order.Details = new List<OrderDetail>();
			order.Details.Add(new OrderDetail(1, 1, 2, 2.95m));
			productService.AddProduct(new Product(1, "1234", "Product A", 2.95m, ""));
			controller.OrderToLoad = order;
			controller.StartLoadOrderFlow(order.OrderId);

			presenter.OnViewInitialized();

			Assert.AreEqual(1, view.OrderItemsLines.Count);
		}

		[TestMethod]
		public void ShouldIgnoreInvalidOrderItemsWhenPassingToControllerOnSave()
		{
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine(2, "9876-54321", "Stencil Pad", null, 10, 7.9m, false); //Invalid, no UnitPrice
			OrderItemLine line3 = new OrderItemLine(2, "9876-54321", "Stencil Pad", .79m, 0, 7.9m, false); //Invalid, quantity is zero
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);
			view.OrderItemsLines.Add(line3);

			presenter.OnSave();

			Assert.AreEqual(1, controller.CurrentOrder.Details.Count);
			Assert.AreEqual(1, controller.CurrentOrder.Details[0].ProductId);
		}

		[TestMethod]
		public void ConstructorCallsVerifyOrderEntryIsStarted()
		{
			Assert.IsTrue(controller.VerifyOrderEntryFlowIsStartedCalled);
		}

		[TestMethod]
		public void AddingLineWithAlreadyExistingSkuWillSetItBackToEmpty()
		{
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			OrderItemLine line2 = new OrderItemLine();
			view.OrderItemsLines.Add(line1);
			view.OrderItemsLines.Add(line2);

			OrderItemLine newLine2 = new OrderItemLine(1, "1234-56789", null, null, 2, null, false);
			newLine2.Id = line2.Id;
			presenter.OnChangedOrderItemLine(newLine2);

			Assert.AreEqual(2, view.OrderItemsLines.Count);
			OrderItemLine updatedLine = view.OrderItemsLines[1];
			Assert.AreEqual(line2.Id, updatedLine.Id);
			Assert.IsTrue(String.IsNullOrEmpty(updatedLine.Sku));
			Assert.IsTrue(String.IsNullOrEmpty(updatedLine.Name));
			Assert.IsNull(updatedLine.Price);
			Assert.IsFalse(updatedLine.ProductId > 0);
		}

		[TestMethod]
		public void SkuIsNotValidIfEmptyOrNull()
		{
			string sku = string.Empty;
			string errorMessage = string.Empty;

			Assert.IsFalse(presenter.IsSkuValid(sku, out errorMessage));
			Assert.IsFalse(string.IsNullOrEmpty(errorMessage));

			sku = null;
			Assert.IsFalse(presenter.IsSkuValid(sku, out errorMessage));
			Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
		}

		[TestMethod]
		public void RepeatedSkuIsNotValid()
		{
			string sku = "1234-56789";
			OrderItemLine line1 = new OrderItemLine(1, sku, "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			view.OrderItemsLines.Add(line1);

			string errorMessage = string.Empty;
			Assert.IsFalse(presenter.IsSkuValid(sku, out errorMessage));
			Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
		}

		[TestMethod]
		public void NotUsedSkuIsValid()
		{
			OrderItemLine line1 = new OrderItemLine(1, "1234-56789", "#2 Pencil Pack", 1.99m, 10, 19.9m, false);
			view.OrderItemsLines.Add(line1);

			Product product = new Product(1, "9876-54321", "#2 Pencil Pack", 1.99m, "#2 Pencil Pack");
			productService.AddProduct(product);

			string errorMessage = string.Empty;
			Assert.IsTrue(presenter.IsSkuValid("9876-54321", out errorMessage));
			Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
		}

		[TestMethod]
		public void SkuIsValidOnlyIfProductExists()
		{
			Product product = new Product(1, "1234-56789", "#2 Pencil Pack", 1.99m, "#2 Pencil Pack");
			productService.AddProduct(product);

			string errorMessage;
			Assert.IsTrue(presenter.IsSkuValid("1234-56789", out errorMessage));
			Assert.IsFalse(presenter.IsSkuValid("9876-54321", out errorMessage));
		}
	}

	class MockDetails : IOrderDetails
	{
		private IList<OrderItemLine> _orderItems = new List<OrderItemLine>();
		private decimal? _orderTotalPrice;
		public bool OrderItemsLinesShown;
		public bool OrderItemsLinesRequested;
		public bool EditingOrderItemLineRequested;
		public bool SetEditingProductCalled;
		private OrderItemLine _editingOrderItemLine;
		public string ShownSku;
		public string ShownName;
		public decimal? ShownPrice;

		public IList<OrderItemLine> OrderItemsLines
		{
			set
			{
				_orderItems = value;
			}
			get
			{
				OrderItemsLinesRequested = true;
				return _orderItems as List<OrderItemLine>;
			}
		}

		public void ShowOrderItemLines(IList<OrderItemLine> orderItemLines)
		{
			_orderItems = orderItemLines;
			OrderItemsLinesShown = true;
		}

		public void ShowOrderTotalPrice(decimal? orderTotalPrice)
		{
			_orderTotalPrice = orderTotalPrice;
		}

		public decimal? OrderTotalPrice
		{
			get { return _orderTotalPrice; }
		}

		public OrderItemLine EditingOrderItemLine
		{
			get
			{
				EditingOrderItemLineRequested = true;
				return _editingOrderItemLine;
			}

			set
			{
				_editingOrderItemLine = value;
			}
		}

		public void SetEditingProduct(string sku, string name, decimal? price)
		{
			ShownSku = sku;
			ShownName = name;
			ShownPrice = price;
			SetEditingProductCalled = true;
		}
	}
}

