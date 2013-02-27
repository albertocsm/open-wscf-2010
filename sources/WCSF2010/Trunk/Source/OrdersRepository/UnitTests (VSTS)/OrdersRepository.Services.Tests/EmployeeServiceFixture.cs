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

namespace OrdersRepository.Services.Tests
{
    /// <summary>
    /// Summary description for EmployeeServiceFixture
    /// </summary>
    [TestClass]
    public class EmployeeServiceFixture
    {
        [TestMethod]
        public void CanCreateInstanceWithoutPassingRepositoryToConstructor()
        {
            EmployeeService service = new EmployeeService();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ShouldGetAllEmployees()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            ds.Employees.AddEmployeesRow("11", "Black", "Joe");
            ds.Employees.AddEmployeesRow("12", "Doe", "John");
            ds.AcceptChanges();
            EmployeeService employeeService = new EmployeeService(ds);

            List<Employee> employees = new List<Employee>(employeeService.AllEmployees);

            Assert.IsNotNull(employees);
            Assert.AreEqual(2, employees.Count);
            Assert.AreEqual("11", employees[0].EmployeeId);
            Assert.AreEqual("Black", employees[0].LastName);
			Assert.AreEqual("John", employees[1].FirstName);
        }

        [TestMethod]
        public void ShouldGetAllApprovers()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            ds.Employees.AddEmployeesRow("jblack", "Black", "Joe");
            ds.Employees.AddEmployeesRow("a-jdoe", "Doe", "John");
            ds.AcceptChanges();
            EmployeeService employeeService = new EmployeeService(ds);

            List<Employee> employees = new List<Employee>(employeeService.AllApprovers);

            Assert.IsNotNull(employees);
            Assert.AreEqual(1, employees.Count);
            Assert.AreEqual("a-jdoe", employees[0].EmployeeId);
            Assert.AreEqual("Doe", employees[0].LastName);
        }

        [TestMethod]
        public void ShouldGetEmployeeById()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            ds.Employees.AddEmployeesRow("11", "Black", "Joe");
            ds.Employees.AddEmployeesRow("12", "Doe", "John");
            ds.AcceptChanges();
            EmployeeService employeeService = new EmployeeService(ds);

            Employee employee= employeeService.GetEmployeeById("11");

            Assert.IsNotNull(employee);
            Assert.AreEqual("11", employee.EmployeeId);
            Assert.AreEqual("Black", employee.LastName);
        }

        [TestMethod]
        public void ShouldNotGetNotExistingEmployee()
        {
            OrdersManagementDataSet ds = new OrdersManagementDataSet();
            EmployeeService employeeService = new EmployeeService(ds);

            Employee employee = employeeService.GetEmployeeById("11");

            Assert.IsNull(employee);
        }
    }
}
