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
using OrdersRepository.BusinessEntities;
using OrdersRepository.Interfaces.Services;
using System;
using System.Collections.ObjectModel;

namespace OrderManagement.Orders.Tests.Mocks
{
	public class MockEmployeeService : IEmployeeService
	{
		public IList<Employee> ExpectedGetAllApprovers = new List<Employee>();
        public Employee ReturnedEmployee = new Employee();
        public bool GetEmployeeByIdCalled;

        public ICollection<Employee> AllEmployees
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Employee GetEmployeeById(string id)
        {
            GetEmployeeByIdCalled = true;
            return ReturnedEmployee;
        }

        public ICollection<Employee> AllApprovers
        {
            get
            {
                return ExpectedGetAllApprovers;
            }
        }
    }
}
