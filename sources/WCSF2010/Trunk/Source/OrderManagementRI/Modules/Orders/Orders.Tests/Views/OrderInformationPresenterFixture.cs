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
using OrderManagement.Orders.Tests.Mocks;
using OrdersRepository.BusinessEntities;
using Orders.PresentationEntities;
using OrderStatusEnum = OrdersRepository.BusinessEntities.Enums.OrderStatus;

namespace OrderManagement.Orders.Tests
{
    [TestClass]
    public class OrderInformationPresenterTestFixture
    {
        private MockOrderInformation view;
        private OrderInformationPresenter presenter;
        private MockOrdersController controller;
        private MockEmployeeService employeeService;
        private MockCustomerService customerService;
        private MockGenericConverter<Employee, EmployeeDisplay> employeeConverter;
        MockPostalInfoLookupService postalInfoLookupService;

        [TestInitialize]
        public void InitMVP()
        {
            view = new MockOrderInformation();
            controller = new MockOrdersController();
            employeeService = new MockEmployeeService();
            customerService = new MockCustomerService();
            employeeConverter = new MockGenericConverter<Employee, EmployeeDisplay>();
            postalInfoLookupService = new MockPostalInfoLookupService();
            presenter = new OrderInformationPresenter(controller, employeeService, customerService, postalInfoLookupService, employeeConverter);

            presenter.View = view;
        }

        [TestMethod]
        public void ShouldSaveOrderInControllerOnSave()
        {
            MockOrderGeneralInformation order = GetPopulatedDisplayOrder();
            view.Order = order;

            presenter.OnSave();

            Assert.IsTrue(controller.SaveCurrentOrderAsDraftCalled);
            Assert.IsFalse(controller.CurrentOrder.OrderId > 0);
            Assert.AreEqual(order.Approver, controller.CurrentOrder.Approver);
            Assert.AreEqual(order.Address, controller.CurrentOrder.ShipAddress);
            Assert.AreEqual(order.City, controller.CurrentOrder.ShipCity);
            Assert.AreEqual(order.State, controller.CurrentOrder.ShipRegion);
            Assert.AreEqual(order.OrderName, controller.CurrentOrder.OrderName);
            Assert.AreEqual(order.Description, controller.CurrentOrder.Description);
        }

        [TestMethod]
        public void OnViewInitializedShouldSetListOfApproversInView()
        {
            List<Employee> approvers = new List<Employee>(2);
            approvers.Add(new Employee("11", "John", "Smith"));
            approvers.Add(new Employee("12", "Joe", "Black"));
            employeeService.ExpectedGetAllApprovers = approvers;
            employeeConverter.PresentationEntity = new EmployeeDisplay("11", "Smith, John");
            presenter.OnViewInitialized();
            Assert.IsNotNull(view.LoadedApprovals);
            Assert.AreEqual(employeeService.ExpectedGetAllApprovers.Count, view.LoadedApprovals.Count);
            Assert.AreEqual(employeeService.ExpectedGetAllApprovers[0].EmployeeId, view.LoadedApprovals[0].Id);
        }

        [TestMethod]
        public void OnViewInitializedShouldDisplayOrderInLoadOrderFlow()
        {
            Order order = GetPopulatedOrder();
            controller.OrderToLoad = order;
            controller.StartLoadOrderFlow(order.OrderId);

            presenter.OnViewInitialized();

            Assert.AreEqual(order.Approver, view.Approver);
            Assert.AreEqual(order.OrderId.ToString(), view.OrderNumber);
        }

        [TestMethod]
        public void OnViewInitializedShouldDisplayNewOrderInNewOrderFlow()
        {
            Order order = new Order();
            order.OrderStatus = (int)OrderStatusEnum.Draft;
            controller.CurrentOrder = order;

            presenter.OnViewInitialized();

            Assert.IsTrue(String.IsNullOrEmpty(view.CustomerName));
            Assert.AreEqual("Draft", view.OrderNumber);
        }

        [TestMethod]
        public void OnSaveGetsAndSetsCurrentOrderToUpdateFromController()
        {
            Order order = GetPopulatedOrder();
            //order.OrderItems.Add(new OrderItem("JP01", "JetPaq", 299.99, 1));
            controller.CurrentOrder = order;
            view.Order = GetPopulatedDisplayOrder();
            view.Order.Address = "TEST ADDRESS";
            controller.SetCurrentOrderCalled = false;
            Assert.IsFalse(controller.GetCurrentOrderCalled);
            Assert.IsFalse(controller.SetCurrentOrderCalled);

            presenter.OnSave();

            Assert.IsTrue(controller.GetCurrentOrderCalled);
            Assert.IsTrue(controller.SetCurrentOrderCalled);
            Assert.AreEqual("TEST ADDRESS", controller.CurrentOrder.ShipAddress);
            //Assert.AreEqual(order.OrderItems.Count, controller.CurrentOrder.OrderItems.Count);
            //Assert.AreEqual(order.OrderItems[0].FirstName, controller.CurrentOrder.OrderItems[0].FirstName);
        }


        [TestMethod]
        public void OnNextGetsAndUpdatesCurrentOrderFromController()
        {
            Order order = GetPopulatedOrder();
            controller.CurrentOrder = order;
            view.Order = GetPopulatedDisplayOrder();
            view.Order.Address = "TEST ADDRESS";
            controller.SetCurrentOrderCalled = false;
            Assert.IsFalse(controller.GetCurrentOrderCalled);
            Assert.IsFalse(controller.SetCurrentOrderCalled);

            presenter.OnNext();

            Assert.IsTrue(controller.GetCurrentOrderCalled);
            Assert.IsTrue(controller.SetCurrentOrderCalled);
            Assert.AreEqual("TEST ADDRESS", controller.CurrentOrder.ShipAddress);
        }

        [TestMethod]
        public void OnNextCallsNavigateNextFromGeneralView()
        {
            Order order = GetPopulatedOrder();
            controller.CurrentOrder = order;
            view.Order = GetPopulatedDisplayOrder();
            Assert.IsFalse(controller.NavigateNextFromGeneralViewCalled);

            presenter.OnNext();

            Assert.IsTrue(controller.NavigateNextFromGeneralViewCalled);
        }

        [TestMethod]
        public void OnCancelCallsCancelChangesFromGeneralView()
        {
            Order order = GetPopulatedOrder();
            controller.CurrentOrder = order;
            view.Order = GetPopulatedDisplayOrder();
            Assert.IsFalse(controller.CancelChangesCalled);

            presenter.OnCancel();

            Assert.IsTrue(controller.CancelChangesCalled);
        }

        [TestMethod]
        public void OnSaveGetsCustomerIdFromService()
        {
            MockOrderGeneralInformation order = GetPopulatedDisplayOrder();
            view.Order = order;

            presenter.OnSave();

            Assert.IsTrue(customerService.GetCustomersByNameCalled);
            Assert.AreEqual(customerService.GetCustomerByName(order.CustomerName).CustomerId, controller.CurrentOrder.CustomerId);
        }

        [TestMethod]
        public void OnCustomerIdSelectedSetCustomerName()
        {
            customerService.GetCustomersByIdCalled = false;
            customerService.ReturnedCustomer.CompanyName = "My test company name";

            presenter.OnCustomerIdSelected("Customer1");

            Assert.IsTrue(customerService.GetCustomersByIdCalled);
            Assert.AreEqual(customerService.ReturnedCustomer.CompanyName, view.CustomerName);
        }

        [TestMethod]
        public void ValidateApproverReturnsFalseIfApproverDoesNotExist()
        {
            employeeService.ReturnedEmployee = null;

            bool isValid = presenter.IsApproverValid("77");

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateApproverReturnsTrueIfApproverDoesExist()
        {
            employeeService.ReturnedEmployee = new Employee("11", "John", "Smith");

            bool isValid = presenter.IsApproverValid("11");

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void OnViewInitializedSetStates()
        {
            List<State> states = new List<State>();
            states.Add(new State("id", "name"));
            postalInfoLookupService.AllStates = states;

            presenter.OnViewInitialized();

            Assert.AreEqual(postalInfoLookupService.AllStates, view.LoadedStates);
        }

        [TestMethod]
        public void ValidateCustomerReturnsFalseIfCustomerDoesNotExist()
        {
            customerService.ReturnedCustomer = null;

            bool isValid = presenter.IsCustomerValid("Name");

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateCustomerReturnsTrueIfCustomerDoesExist()
        {
            customerService.ReturnedCustomer = new Customer();
            customerService.ReturnedCustomer.CompanyName = "Name";

            bool isValid = presenter.IsCustomerValid("Name");

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ConstructorCallsVerifyOrderEntryIsStarted()
        {
            Assert.IsTrue(controller.VerifyOrderEntryFlowIsStartedCalled);
        }

        #region Obsolete because OrderNumber is now autogenerated
        //[TestMethod]
        //public void IsOrderNumberValidShouldCheckIfOrderNumberExistsInRepository()
        //{
        //    bool valid = presenter.IsOrderNumberValid("A2");
        //    Assert.IsTrue(controller.OrderExistsCalled);
        //}

        //[TestMethod]
        //public void OnSaveCatchesExceptionWhenOrderNumberExistsAndSetsViewInvalid()
        //{
        //    controller.ThrowExceptionWhenSaving = true;

        //    MockOrderGeneralInformation order = new MockOrderGeneralInformation();
        //    order.Number = "A1";
        //    order.Approver = 11;
        //    view.Order = order;

        //    Assert.IsNull(view.SetOrderNumberExistsValidationStatusArgument);

        //    presenter.OnSave();

        //    Assert.IsTrue(controller.SaveCurrentOrderAsDraftCalled);
        //    Assert.IsNotNull(view.SetOrderNumberExistsValidationStatusArgument);
        //    Assert.IsFalse(view.SetOrderNumberExistsValidationStatusArgument.Value);
        //}
        #endregion

        private Order GetPopulatedOrder()
        {
            Order order = new Order();
            order.OrderId = 1;
            order.Approver = "11";
            order.CustomerId = "100";
            order.ShipAddress = "225 112th NE Street";
            order.ShipCity = "Bellevue";
            order.ShipRegion = "Washington";
            order.ShipPostalCode = "98005";
            order.OrderName = "ORDER1";
            order.Description = "Needs it quick";
            return order;
        }

        private MockOrderGeneralInformation GetPopulatedDisplayOrder()
        {
            MockOrderGeneralInformation order = new MockOrderGeneralInformation();
            order.Number = "1";
            order.Approver = "11";
            order.CustomerName = "John Smith";
            order.Address = "225 112th NE Street";
            order.City = "Bellevue";
            order.State = "Washington";
            order.PostalCode = "98005";
            order.OrderName = "ORDER1";
            order.Description = "Needs it quick";
            return order;
        }
    }

    class MockOrderInformation : IOrderInformation
    {
        public MockOrderGeneralInformation Order;
        public IList<EmployeeDisplay> LoadedApprovals;
        public IEnumerable<State> LoadedStates;

        public void ShowOrderNumber(string orderNumber)
        {
            Order.Number = orderNumber;
        }

        public string OrderNumber
        {
            get
            {
                return Order.Number;
            }
        }

        public void LoadApproversList(IList<EmployeeDisplay> approvers)
        {
            LoadedApprovals = approvers;
        }

        public string Approver
        {
            get { return Order.Approver; }
            set { Order.Approver = value; }
        }

        public string CustomerName
        {
            get { return Order.CustomerName; }
            set { Order.CustomerName = value; }
        }

        public string Address
        {
            get { return Order.Address; }
            set { Order.Address = value; }
        }
        public string City
        {
            get { return Order.City; }
            set { Order.City = value; }
        }
        public string State
        {
            get { return Order.State; }
            set { Order.State = value; }
        }
        public string PostalCode
        {
            get { return Order.PostalCode; }
            set { Order.PostalCode = value; }
        }

        public string OrderName
        {
            get { return Order.OrderName; }
            set { Order.OrderName = value; }
        }

        public string Description
        {
            get { return Order.Description; }
            set { Order.Description = value; }
        }

        public void LoadStatesList(IEnumerable<State> states)
        {
            LoadedStates = states;
        }
    }


    struct MockOrderGeneralInformation
    {
        public string Number;
        public string Approver;
        public string CustomerName;
        public string Address;
        public string City;
        public string State;
        public string PostalCode;
        public string OrderName;
        public string Description;
    }
}

