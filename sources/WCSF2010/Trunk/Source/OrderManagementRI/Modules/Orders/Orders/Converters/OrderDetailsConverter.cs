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
using Microsoft.Practices.CompositeWeb;
using Orders.PresentationEntities;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;

namespace OrderManagement.Orders.Converters
{
    public class OrderDetailsConverter : IBusinessPresentationConverter<OrderDetail, OrderItemLine>
    {
        private IProductService _productService;

        public OrderDetailsConverter([ServiceDependency] IProductService productService)
        {
            this._productService = productService;
        }

        public OrderItemLine ConvertBusinessToPresentation(OrderDetail businessEntity)
        {
            OrderItemLine line = new OrderItemLine();
            line.ProductId = businessEntity.ProductId;
            line.Quantity = businessEntity.Quantity;

            Product product = _productService.GetProductById(line.ProductId);
            line.Sku = product.ProductSku;
            line.Name = product.ProductName;
            line.Price = product.UnitPrice;

			line.Total = line.Price * line.Quantity;

            return line;
        }

        public OrderDetail ConvertPresentationToBusiness(OrderItemLine presentationEntity)
        {
            if (presentationEntity.Quantity <= 0 || !presentationEntity.Price.HasValue)
                return null;

            OrderDetail orderItem = new OrderDetail(0, presentationEntity.ProductId, presentationEntity.Quantity, presentationEntity.Price.Value);
            return orderItem;
        }
    }
}
