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
using Microsoft.Practices.CompositeWeb;
using OrderManagement.Orders.Properties;
using Orders.PresentationEntities;
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using OrderStatus=OrdersRepository.BusinessEntities.Enums.OrderStatus;
using System.Globalization;

namespace OrderManagement.Orders.Converters
{
    public class OrdersConverter: IBusinessPresentationConverter<Order,OrderInfo>
    {
        private ICustomerService _customerService;
        private IEmployeeService _employeeService;
        private IBusinessPresentationConverter<Employee,EmployeeDisplay> _employeeConverter;

        public OrdersConverter(
            [ServiceDependency] ICustomerService customerService,
            [ServiceDependency] IEmployeeService employeeService,
          [ServiceDependency] IBusinessPresentationConverter<Employee,EmployeeDisplay> employeeConverter
            )
        {
            _customerService = customerService;
            _employeeService = employeeService;
            _employeeConverter = employeeConverter;
        }

        public OrderInfo ConvertBusinessToPresentation(Order businessEntity)
        {
            OrderInfo orderInfo = new OrderInfo();
            if (businessEntity.OrderId > 0)
            {
                orderInfo.OrderId = businessEntity.OrderId.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                orderInfo.OrderId = Resources.DraftStatusDescription;
            }

            orderInfo.OrderName = businessEntity.OrderName;
            orderInfo.Description = businessEntity.Description;
            orderInfo.Address = businessEntity.ShipAddress;
            orderInfo.City = businessEntity.ShipCity;
            orderInfo.PostalCode = businessEntity.ShipPostalCode;
            orderInfo.State = businessEntity.ShipRegion;

            ConvertOrderStatus(businessEntity, orderInfo);

            Customer customer = _customerService.GetCustomerById(businessEntity.CustomerId);
            orderInfo.CustomerName = customer.CompanyName;

            Employee employee = _employeeService.GetEmployeeById(businessEntity.Approver);
            EmployeeDisplay employeeDisplay = _employeeConverter.ConvertBusinessToPresentation(employee);
            orderInfo.Approver = employeeDisplay.Name;

            if (businessEntity.Details != null)
            {
                orderInfo.OrderTotal = businessEntity.OrderTotal;
            }

            return orderInfo;
        }

        private static void ConvertOrderStatus(Order order, OrderInfo orderInfo)
        {
            switch ((OrderStatus)order.OrderStatus)
            {
                case OrderStatus.Approved:
                    orderInfo.OrderStatus = Resources.ApprovedStatusDescription;
                    break;
                case OrderStatus.Draft:
                    orderInfo.OrderStatus = Resources.DraftStatusDescription;
                    break;
                case OrderStatus.Rejected:
                    orderInfo.OrderStatus = Resources.RejectedStatusDescription;
                    break;
                case OrderStatus.Submitted:
                    orderInfo.OrderStatus = Resources.SubmittedStatusDescription;
                    break;
            }
        }

        public Order ConvertPresentationToBusiness(OrderInfo presentationEntity)
        {
            throw new NotImplementedException(Resources.NotImplementedExceptionMessage);
        }
    }
}
