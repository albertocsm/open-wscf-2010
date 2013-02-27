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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.Orders.Views;
using Orders.PresentationEntities;
using System.Collections.Generic;
using OrdersRepository.BusinessEntities;

public partial class Orders_OrderInformation : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IOrderInformation
{
    private OrderInformationPresenter _presenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
            WireUpEvents();
        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public OrderInformationPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }

    private void WireUpEvents()
    {
        // Force postback to the SelectButton on the Ok event of the ModalPopup
        CustomerModalPopupExtender.OnOkScript = this.Page.ClientScript.GetPostBackClientHyperlink(SelectButton, "");
    }

    public void ShowOrderNumber(string orderNumber)
    {
        OrderNoTextBox.Text = orderNumber;
    }


    public void LoadApproversList(IList<EmployeeDisplay> approvers)
    {
        ApprovedByDropDown.DataSource = approvers;
        ApprovedByDropDown.DataBind();
    }


    public void LoadStatesList(IEnumerable<State> states)
    {
        StateDropDown.DataSource = states;
        StateDropDown.DataBind();
    }

    public string Approver
    {
        get
        {
            return ApprovedByDropDown.SelectedValue;
        }
        set { ApprovedByDropDown.SelectedValue = value; }
    }


    protected void SaveButton_Click(object sender, EventArgs e)
    {
        _presenter.OnSave();
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        _presenter.OnCancel();
    }

    public string CustomerName
    {
        get
        {
            return customerTextBox.Text;
        }
        set
        {
            customerTextBox.Text = value;
        }
    }

    public string Address
    {
        get
        {
            return StreetTextBox.Text;
        }
        set
        {
            StreetTextBox.Text = value;
        }
    }

    public string City
    {
        get
        {
            return CityTextBox.Text;
        }
        set
        {
            CityTextBox.Text = value;
        }
    }

    public string State
    {
        get
        {
            return StateDropDown.Text;
        }
        set
        {
            StateDropDown.Text = value;
        }
    }

    public string PostalCode
    {
        get
        {
            return PostalCodeTextBox.Text;
        }
        set
        {
            PostalCodeTextBox.Text = value;
        }
    }

    public string OrderName
    {
        get
        {
            return OrderNameTextBox.Text;
        }
        set
        {
            OrderNameTextBox.Text = value;
        }
    }

    public string Description
    {
        get
        {
            return DescriptionTextArea.Text;
        }
        set
        {
            DescriptionTextArea.Text = value;
        }
    }

    protected void NextButton_Click(object sender, EventArgs e)
    {
        this.Page.Validate();
        if (this.Page.IsValid)
        {
            _presenter.OnNext();
        }
    }

    protected void SelectButton_Click(object sender, EventArgs e)
    {
        _presenter.OnCustomerIdSelected(SearchCustomerControl.SelectedCustomerId);
        SearchCustomerControl.ClearView();
    }

    protected void CancelSearchButton_Click(object sender, EventArgs e)
    {
        SearchCustomerControl.ClearView();
    }

    protected void ExistenceCustomerValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = _presenter.IsCustomerValid(args.Value);
    }

    protected void ExistenceApproverValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = _presenter.IsApproverValid(args.Value);
    }

    protected void ExistenceStateValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = _presenter.IsStateValid(args.Value);
    }
}


