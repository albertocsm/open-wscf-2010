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

namespace OrdersRepository.BusinessEntities
{
    public partial class Employee
    {
        public Employee()
        {
        }

        public Employee(String employeeId, String firstName, String lastName)
        {
            this.EmployeeIdField = employeeId;
            this.firstNameField = firstName;
            this.lastNameField = lastName;
        }

        //public Employee(System.String address, Nullable<System.DateTime> birthDate, System.String city, System.String country, System.String EmployeeId, System.String extension, System.String firstName, Nullable<System.DateTime> hireDate, System.String homePhone, System.String lastName, System.String notes, System.String postalCode, System.String region, System.String reportsTo, System.String title, System.String titleOfCourtesy)
        //{
        //    this.addressField = address;
        //    this.birthDateField = birthDate;
        //    this.cityField = city;
        //    this.countryField = country;
        //    this.EmployeeIdField = EmployeeId;
        //    this.extensionField = extension;
        //    this.firstNameField = firstName;
        //    this.hireDateField = hireDate;
        //    this.homePhoneField = homePhone;
        //    this.lastNameField = lastName;
        //    this.notesField = notes;
        //    this.postalCodeField = postalCode;
        //    this.regionField = region;
        //    this.reportsToField = reportsTo;
        //    this.titleField = title;
        //    this.titleOfCourtesyField = titleOfCourtesy;
        //}

        //private System.String addressField;

        //public System.String Address
        //{
        //    get { return this.addressField; }
        //    set { this.addressField = value; }
        //}

        //private Nullable<System.DateTime> birthDateField;

        //public Nullable<System.DateTime> BirthDate
        //{
        //    get { return this.birthDateField; }
        //    set { this.birthDateField = value; }
        //}

        //private System.String cityField;

        //public System.String City
        //{
        //    get { return this.cityField; }
        //    set { this.cityField = value; }
        //}

        //private System.String countryField;

        //public System.String Country
        //{
        //    get { return this.countryField; }
        //    set { this.countryField = value; }
        //}

        private String EmployeeIdField;

        public String EmployeeId
        {
            get { return this.EmployeeIdField; }
            set { this.EmployeeIdField = value; }
        }

        //private System.String extensionField;

        //public System.String Extension
        //{
        //    get { return this.extensionField; }
        //    set { this.extensionField = value; }
        //}

        private String firstNameField;

        public String FirstName
        {
            get { return this.firstNameField; }
            set { this.firstNameField = value; }
        }

        //private Nullable<System.DateTime> hireDateField;

        //public Nullable<System.DateTime> HireDate
        //{
        //    get { return this.hireDateField; }
        //    set { this.hireDateField = value; }
        //}

        //private System.String homePhoneField;

        //public System.String HomePhone
        //{
        //    get { return this.homePhoneField; }
        //    set { this.homePhoneField = value; }
        //}

        private String lastNameField;

        public String LastName
        {
            get { return this.lastNameField; }
            set { this.lastNameField = value; }
        }

        //private System.String notesField;

        //public System.String Notes
        //{
        //    get { return this.notesField; }
        //    set { this.notesField = value; }
        //}

        //private System.String postalCodeField;

        //public System.String PostalCode
        //{
        //    get { return this.postalCodeField; }
        //    set { this.postalCodeField = value; }
        //}

        //private System.String regionField;

        //public System.String Region
        //{
        //    get { return this.regionField; }
        //    set { this.regionField = value; }
        //}

        //private System.String reportsToField;

        //public System.String ReportsTo
        //{
        //    get { return this.reportsToField; }
        //    set { this.reportsToField = value; }
        //}

        //private System.String titleField;

        //public System.String Title
        //{
        //    get { return this.titleField; }
        //    set { this.titleField = value; }
        //}

        //private System.String titleOfCourtesyField;

        //public System.String TitleOfCourtesy
        //{
        //    get { return this.titleOfCourtesyField; }
        //    set { this.titleOfCourtesyField = value; }
        //}

    }
}

