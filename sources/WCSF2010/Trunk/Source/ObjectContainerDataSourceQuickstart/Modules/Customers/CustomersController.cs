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
using System;
using System.Collections.Generic;
using System.Web;
using ObjectContainerDataSourceQuickstart.Modules.Customers.BusinessEntities;

namespace ObjectContainerDataSourceQuickstart.Modules.Customers
{
    /// <summary>
    /// Simple controller that contains logic to manage and persist Customer objects.
    /// <remarks>
    /// A list of Customer objects is stored in session to simulate a real
    /// data store.
    /// </remarks>
    /// </summary>
    public class CustomersController
    {
        private List<Customer> Customers
        {
            get
            {
                return HttpContext.Current.Session["Customers"] as List<Customer>;
            }
            set
            {
                HttpContext.Current.Session["Customers"] = value;
            }
        }

        public CustomersController()
        {
            if (Customers == null)
            {
                List<Customer> customers = new List<Customer>();
                customers.Add(new Customer(1000, "Enrique", "Gil", CustomerStatus.Pending));
                customers.Add(new Customer(1001, "Paul", "West", CustomerStatus.Pending));
                customers.Add(new Customer(1002, "Nuria", "Gonzalez", CustomerStatus.Pending));
                customers.Add(new Customer(1003, "Gudmundur", "Hansen", CustomerStatus.Pending));
                customers.Add(new Customer(1004, "David", "Ahs", CustomerStatus.Pending));
                customers.Add(new Customer(1005, "Joe", "Healy", CustomerStatus.Pending));
                customers.Add(new Customer(1006, "Sidney", "Higa", CustomerStatus.Pending));
                Customers = customers;
            }
        }

        public virtual Customer GetNextCustomer()
        {
            return Customers.Find(delegate(Customer customer)
            {
                return (customer.Status == CustomerStatus.Pending);
            });
        }

        public virtual IList<Customer> GetCustomers()
        {
            return Customers;
        }

        public IList<Customer> GetCustomers(int startRowIndex, int pageSize, string sortExpression)
        {
            // Simple sorting
            string[] sortExpressionParts = sortExpression.Split(' ');
            Customers.Sort(delegate(Customer c1, Customer c2)
            {
                int comparisonResult;
                switch (sortExpressionParts[0])
                {
                    case ("Id"):
                        comparisonResult = c1.Id.CompareTo(c2.Id);
                        break;
                    case ("Name"):
                        comparisonResult = c1.Name.CompareTo(c2.Name);
                        break;
                    case ("LastName"):
                        comparisonResult = c1.LastName.CompareTo(c2.LastName);
                        break;
                    case ("Status"):
                        comparisonResult = c1.Status.CompareTo(c2.Status);
                        break;
                    default:
                        comparisonResult = c1.Id.CompareTo(c2.Id);
                        break;
                }
                if (sortExpressionParts.Length > 1 && sortExpressionParts[1] == "DESC")
                    comparisonResult *= -1;

                return comparisonResult;
            });

            // Simple paging
            if (CustomersTotalCount > 0 && CustomersTotalCount <= startRowIndex)
            {
                startRowIndex = ((CustomersTotalCount / pageSize) - 1) * pageSize;
            }
            int rowsLeft = CustomersTotalCount - startRowIndex;
            int rowCount = rowsLeft < pageSize ? rowsLeft : pageSize;
            return Customers.GetRange(startRowIndex, rowCount);
        }

        public virtual int CustomersTotalCount
        {
            get { return Customers.Count; }
        }

        public void DeleteCustomer(Customer customer)
        {
            Customer customerFound = FindCustomer(customer);
            Customers.Remove(customerFound);
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer customerFound = FindCustomer(customer);
            customerFound.LastName = customer.LastName;
            customerFound.Name = customer.Name;
            customerFound.Status = customer.Status;
        }

        public void AddCustomer(Customer customer)
        {
            customer.Id = GetNewId();
            Customers.Add(customer);
        }

        private Customer FindCustomer(Customer customer)
        {
            return Customers.Find(delegate(Customer match)
            {
                return customer.Id == match.Id;
            });
        }

        private int GetNewId()
        {
            int highestId = 1000;
            foreach (Customer customer in Customers)
            {
                if (customer.Id > highestId)
                {
                    highestId = customer.Id;
                }
            }
            return ++highestId;
        }
    }
}