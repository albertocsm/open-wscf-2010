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
using OrdersRepository.Interfaces.Services;
using OrderManagement.Orders.Converters;
using Orders.PresentationEntities;
using OrdersRepository.BusinessEntities;
using OrderManagement.Orders.Properties;

namespace OrderManagement.Orders.Views
{
    public class OrderDetailsPresenter : Presenter<IOrderDetails>
    {
        private IOrdersController _controller;
        private IProductService _productService;
        private IBusinessPresentationConverter<OrderDetail, OrderItemLine> _orderDetailsConverter;

        [InjectionConstructor]
        public OrderDetailsPresenter(
            [CreateNew] IOrdersController controller,
            [ServiceDependency] IProductService productService,
           [ServiceDependency] IBusinessPresentationConverter<OrderDetail, OrderItemLine> orderDetailsConverter)
        {
            controller.VerifyOrderEntryFlowIsStarted();

            _controller = controller;
            _productService = productService;
            _orderDetailsConverter = orderDetailsConverter;
        }

        public override void OnViewLoaded()
        {
            View.ShowOrderItemLines(View.OrderItemsLines);
            UpdateTotalPrice();

            base.OnViewLoaded();
        }

        public override void OnViewInitialized()
        {
            ICollection<OrderDetail> details = _controller.CurrentOrder.Details;
            List<OrderItemLine> lines = new List<OrderItemLine>();
            foreach (OrderDetail detail in details)
            {
                OrderItemLine orderItemLine = _orderDetailsConverter.ConvertBusinessToPresentation(detail);
                UpdateOrderLineTotalPrice(orderItemLine);
                lines.Add(orderItemLine);
            }
            View.ShowOrderItemLines(lines);
        }

        public void OnAddOrderItemLine()
        {
            IList<OrderItemLine> lines = View.OrderItemsLines;
            OrderItemLine newLine = new OrderItemLine();
            newLine.Quantity = 1;
            lines.Add(newLine);
            View.ShowOrderItemLines(lines);
        }

        public void OnDeleteOrderItemLines(IList<OrderItemLine> linesToDelete)
        {
            IList<OrderItemLine> lines = View.OrderItemsLines;
            foreach (OrderItemLine lineToDelete in linesToDelete)
            {
                lines.Remove(lineToDelete);
            }
            UpdateTotalPrice();
            View.ShowOrderItemLines(lines);
        }

        public void OnChangedOrderItemLine(OrderItemLine orderItemLine)
        {
            List<OrderItemLine> lines = new List<OrderItemLine>(View.OrderItemsLines);

            OrderItemLine line = lines.Find(delegate(OrderItemLine order)
            {
                return order.Equals(orderItemLine);
            });

            line.Sku = orderItemLine.Sku;
            if (line.Sku != null)
            {
                bool skuAlreadyEntered = SkuAlreadyEntered(orderItemLine.Sku, lines, line);

                if (skuAlreadyEntered)
                {
                    line.Sku = null;
                    line.Name = null;
                    line.Price = null;
                }
                else
                {
                    Product product = _productService.GetProductBySku(line.Sku);
                    if (product != null)
                    {
                        line.ProductId = product.ProductId;
                        line.Name = product.ProductName;
                        line.Price = product.UnitPrice;
                    }
                }
            }
            line.Quantity = orderItemLine.Quantity;
            UpdateOrderLineTotalPrice(line);

            View.ShowOrderItemLines(lines);
            UpdateTotalPrice();
        }

        private static bool SkuAlreadyEntered(string sku,List<OrderItemLine> lines, OrderItemLine editingLine)
        {
            bool skuAlreadyExists = lines.Exists(delegate(OrderItemLine order)
                {
                    return !order.Equals(editingLine) && sku.Equals(order.Sku, StringComparison.InvariantCultureIgnoreCase);
                }
            );
            return skuAlreadyExists;
        }

        private static void UpdateOrderLineTotalPrice(OrderItemLine line)
        {
            line.Total = line.Price * line.Quantity;
        }

        private void UpdateTotalPrice()
        {
            decimal? totalPrice = 0;
            foreach (OrderItemLine line in View.OrderItemsLines)
            {
                if (line.Total != null)
                    totalPrice += line.Total;
            }
            View.ShowOrderTotalPrice(totalPrice);
        }

        public void OnSave()
        {
            UpdateOrderDetailsFromView();
            _controller.SaveCurrentOrderAsDraft();
        }

        private void UpdateOrderDetailsFromView()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (OrderItemLine itemLine in View.OrderItemsLines)
            {
                OrderDetail orderItem = _orderDetailsConverter.ConvertPresentationToBusiness(itemLine);
                if (orderItem != null)
                {
                    orderDetails.Add(orderItem);
                }
            }
            Order order = _controller.CurrentOrder;
            order.Details = orderDetails;
            _controller.CurrentOrder = order;
        }

        public void OnEditOrderItemLine(int rowIndex, bool selectedValue)
        {
            IList<OrderItemLine> lines = View.OrderItemsLines;
            View.OrderItemsLines[rowIndex].Selected = selectedValue;
            View.ShowOrderItemLines(lines);
        }

        public void OnCancel()
        {
            _controller.CancelChanges();
        }

        public void OnPrevious()
        {
            UpdateOrderDetailsFromView();
            _controller.NavigatePreviousFromDetailsView();
        }

        public void OnNext()
        {
            UpdateOrderDetailsFromView();
            _controller.NavigateNextFromDetailsView();
        }

        public void OnProductSelected(string sku)
        {
			if (sku == null)
				return;
            Product product = _productService.GetProductBySku(sku);
            if (product != null)
            {
                View.SetEditingProduct(sku, product.ProductName, product.UnitPrice);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public bool IsSkuValid(string sku, out string errorMessage)
        {
            if (string.IsNullOrEmpty(sku))
            {
                errorMessage = Resources.ErrorEmptySku;
                return false;
            }
            OrderItemLine line = (View.EditingOrderItemLine ?? (new OrderItemLine()));
            List<OrderItemLine> lines = new List<OrderItemLine>(View.OrderItemsLines);
            bool skuAlreadyExists = SkuAlreadyEntered(sku, lines, line);
            if (skuAlreadyExists)
            {
                errorMessage = Resources.ErrorRepeatedSku;
                return false;

            }

            Product product = _productService.GetProductBySku(sku);
            if (product == null)
            {
                errorMessage = Resources.ErrorInvalidSku;
                return false;
            }
            else
            {
                errorMessage = string.Empty;
                return true;
            }
        }
    }
}




