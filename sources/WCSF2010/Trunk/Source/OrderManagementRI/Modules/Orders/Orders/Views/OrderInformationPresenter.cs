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
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using System.Collections.ObjectModel;
using System.Globalization;
using OrderManagement.Orders.Properties;

namespace OrderManagement.Orders.Views
{
    public class OrderInformationPresenter : Presenter<IOrderInformation>
    {

        private IOrdersController _controller;
		private IEmployeeService _employeeService;
		private ICustomerService _customerService;
        private IPostalInfoLookupService _postalInfoLookupService;
        private IBusinessPresentationConverter<Employee, EmployeeDisplay> _employeeConverter;

        public OrderInformationPresenter([CreateNew] IOrdersController controller, 
            [ServiceDependency] IEmployeeService employeeService, 
            [ServiceDependency] ICustomerService customerService,
            [ServiceDependency] IPostalInfoLookupService postalInfoLookupService,
            [ServiceDependency] IBusinessPresentationConverter<Employee, EmployeeDisplay> employeeConverter)
        {
            controller.VerifyOrderEntryFlowIsStarted();

            _controller = controller;
			_employeeService = employeeService;
			_customerService = customerService;
            _postalInfoLookupService = postalInfoLookupService;
            _employeeConverter = employeeConverter;
        }

        public override void OnViewInitialized()
        {
            LoadApprovers();
            View.LoadStatesList(_postalInfoLookupService.AllStates);
            UpdateViewFromOrder(_controller.CurrentOrder);
        }

        private void LoadApprovers()
        {

            ICollection<Employee> _employees = _employeeService.AllApprovers;
            IList<EmployeeDisplay> _approvers = new Collection<EmployeeDisplay>();
            foreach (Employee employee in _employees)
            {
                _approvers.Add(_employeeConverter.ConvertBusinessToPresentation(employee));
            }
                
            View.LoadApproversList(_approvers);
        }

        public void OnSave()
        {
            Order order = _controller.CurrentOrder;
            UpdateOrderFromView(order);
            _controller.CurrentOrder = order;

            _controller.SaveCurrentOrderAsDraft();
            UpdateViewFromOrder(_controller.CurrentOrder);
        }

		public void OnCancel()
		{
			_controller.CancelChanges();
		}



        public void OnNext()
        {
            Order order = _controller.CurrentOrder;
            UpdateOrderFromView(order);
            _controller.CurrentOrder = order;
            _controller.NavigateNextFromGeneralView();
        }


        private void UpdateOrderFromView(Order order)
        {
            order.OrderName = View.OrderName;
            order.Approver = View.Approver;
            order.ShipAddress = View.Address;
            order.ShipCity = View.City;
            order.ShipRegion = View.State;
            order.ShipPostalCode = View.PostalCode;
            order.Description = View.Description;
			order.CustomerId = GetCustomerIdFromName(View.CustomerName);
        }

        private void UpdateViewFromOrder(Order order)
        {
            if (order.OrderId > 0)
            {
                View.ShowOrderNumber(order.OrderId.ToString(CultureInfo.CurrentCulture));
            }
            else
            {
                View.ShowOrderNumber(Resources.DraftStatusDescription);
            }
            View.OrderName = order.OrderName;
            View.Approver = order.Approver;
            View.Address = order.ShipAddress;
            View.City = order.ShipCity;
            View.State = order.ShipRegion;
            View.PostalCode = order.ShipPostalCode;
            View.Description = order.Description;

            View.CustomerName = GetNameFromCustomerId(order.CustomerId);
        }

		private string GetCustomerIdFromName(string customerName)
		{
			return _customerService.GetCustomerByName(customerName).CustomerId;
		}

		private string GetNameFromCustomerId(string CustomerId)
		{
            if (String.IsNullOrEmpty(CustomerId))
                return String.Empty;

            return _customerService.GetCustomerById(CustomerId).CompanyName;
		}

        public void OnCustomerIdSelected(string customerId)
        {
            View.CustomerName = GetNameFromCustomerId(customerId);
        }

        public bool IsCustomerValid(string customerName)
        {
            return _customerService.GetCustomerByName(customerName) != null;
        }

        public bool IsApproverValid(string employeeId)
        {
            return _employeeService.GetEmployeeById(employeeId) != null;
        }

        public bool IsStateValid(string stateId)
        {
            return _postalInfoLookupService.GetStateById(stateId) != null;
        }
    }
}




