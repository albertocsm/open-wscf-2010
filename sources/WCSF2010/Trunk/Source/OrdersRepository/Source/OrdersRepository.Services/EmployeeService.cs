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
using System.Globalization;
using System.Data;
using OrdersRepository.Services.Authorization;
using System.Collections.ObjectModel;

namespace OrdersRepository.Services
{
	public class EmployeeService : IEmployeeService
	{
        OrdersManagementDataSet repository;

        public EmployeeService() : this(OrdersManagementRepository.Instance)
        {  }

        public EmployeeService(OrdersManagementDataSet repository)
        {
            this.repository = repository;
        }

        public ICollection<Employee> AllEmployees
        {
            get
            {
                ICollection<Employee> employees = new List<Employee>();
                foreach (OrdersManagementDataSet.EmployeesRow row in repository.Employees.Rows)
                {
                    employees.Add(new Employee(row.EmployeeId, row.FirstName, row.LastName));
                }
                return employees;
            }
        }

        public ICollection<Employee> AllApprovers
        {
            get
            {
                DataRow[] rows = repository.Employees.Select(string.Format(CultureInfo.CurrentCulture, "EmployeeId LIKE '{0}*'", RepositoryRoleProvider.ApproverPrefix));
                ICollection<Employee> employees = new List<Employee>();
                foreach (OrdersManagementDataSet.EmployeesRow employeesRow in rows)
                {
                    employees.Add(TranslateFromEmployeesRowToEmployeeEntity(employeesRow));
                }
                return employees;
            }
        }

        public Employee GetEmployeeById(string id)
        {
            OrdersManagementDataSet.EmployeesRow employeeRow = repository.Employees.FindByEmployeeId(id);
            if (employeeRow == null || !employeeRow.EmployeeId.Equals(id, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return TranslateFromEmployeesRowToEmployeeEntity(employeeRow);
        }

        private static Employee TranslateFromEmployeesRowToEmployeeEntity(OrdersManagementDataSet.EmployeesRow employeeRow)
        {
            Employee employee = new Employee();
            employee.EmployeeId = employeeRow.EmployeeId;
            employee.FirstName = employeeRow.FirstName;
            employee.LastName = employeeRow.LastName;

            return employee;
        }
    }
}
