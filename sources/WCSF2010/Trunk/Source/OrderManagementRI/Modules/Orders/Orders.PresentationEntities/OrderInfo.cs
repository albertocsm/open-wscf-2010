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
namespace Orders.PresentationEntities
{
    public class OrderInfo
    {
        public OrderInfo()
        {
        }

        public OrderInfo(string orderId, string approver, string customerName, string address, string city, string state, string postalCode, string orderName, string description, decimal orderTotal, string orderStatus)
            : base()
        {
            _orderId = orderId;
            _approver = approver;
            _customerName = customerName;
            _address = address;
            _city = city;
            _state = state;
            _postalCode = postalCode;
            _orderName = orderName;
            _description = description;
            _orderTotal = orderTotal;
            _orderStatus = orderStatus;
        }

        private string _orderId;

        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }
        private string _approver;

        public string Approver
        {
            get { return _approver; }
            set { _approver = value; }
        }
        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        private string _state;

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        private string _postalCode;

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }
        private string _orderName;

        public string OrderName
        {
            get { return _orderName; }
            set { _orderName = value; }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private decimal _orderTotal;

        public decimal OrderTotal
        {
            get { return _orderTotal; }
            set { _orderTotal = value; }
        }

        private string _orderStatus;

        public string OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }
    }
}
