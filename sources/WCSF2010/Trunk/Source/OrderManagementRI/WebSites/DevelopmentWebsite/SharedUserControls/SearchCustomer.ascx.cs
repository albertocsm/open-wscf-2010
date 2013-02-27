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
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using OrderManagement.SharedUserControls.Parts;
using OrdersRepository.BusinessEntities;

public partial class Customers_SearchCustomer : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, ISearchCustomer
{
	private SearchCustomerPresenter _presenter;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!this.IsPostBack)
		{
			this._presenter.OnViewInitialized();
		}
		NoResultsLabel.Visible = false;
		this._presenter.OnViewLoaded();
		RegisterClientScriptClearView();
	}

	[CreateNew]
	public SearchCustomerPresenter Presenter
	{
		set
		{
			this._presenter = value;
			this._presenter.View = this;
		}
	}

	public string SelectedCustomerId
	{
		get
		{
			if (this.CustomersGridView.Rows.Count > 0 && this.CustomersGridView.SelectedIndex >= 0)
				return this.CustomersGridView.SelectedDataKey.Value.ToString();

			return null;
		}
	}

	protected void SearchButton_Click(object sender, EventArgs e)
	{
		_presenter.OnSearchCustomers();
	}

	public string CompanyName
	{
		get { return CompanyNameTextBox.Text; }
	}

	public string City
	{
		get { return CityTextBox.Text; }
	}

	public string State
	{
		get { return StateDropDown.Text; }
	}

	public string ZipCode
	{
		get { return ZipCodeTextBox.Text; }
	}

	public string Address
	{
		get { return AddressTextBox.Text; }
	}

	public void ShowCustomersResults(IList<Customer> customersResults)
	{
		this.CustomersContainerDataSource.DataSource = customersResults;
	}

	public void ResetSelectedCustomer()
	{
		this.CustomersGridView.SelectedIndex = 0;
	}

	public IEnumerable<State> States
	{
		set
		{
			StateDropDown.DataSource = value;
			StateDropDown.DataBind();
		}
	}

	protected void CustomersGridView_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// Javascript event wired over Grid row to select the row without needing to click the Select link.
			string rowPostbackArguments = string.Format("Select${0}", e.Row.RowIndex);
			string postbackCall = this.Page.ClientScript.GetPostBackClientHyperlink(CustomersGridView, rowPostbackArguments);
			e.Row.Attributes.Add("onclick", postbackCall);
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow row in CustomersGridView.Rows)
		{
			string rowPostbackArguments = string.Format("Select${0}", row.RowIndex);
			this.Page.ClientScript.RegisterForEventValidation(CustomersGridView.UniqueID, rowPostbackArguments);
		}

		base.Render(writer);
	}

	// This method lets reuse the MVP pair when used inside a ModalPopupExtender
	public void ClearView()
	{
		CompanyNameTextBox.Text = string.Empty;
		CityTextBox.Text = string.Empty;
		StateDropDown.SelectedIndex = 0;
		ZipCodeTextBox.Text = string.Empty;
		AddressTextBox.Text = string.Empty;
		ShowCustomersResults(new List<Customer>());
	}

	public void RegisterClientScriptClearView()
	{
		StringBuilder script = new StringBuilder("function ClearSearchCustomerView() {");
		script.AppendFormat(CultureInfo.CurrentCulture, "$get('{0}').value='';", CompanyNameTextBox.ClientID);
		script.AppendFormat(CultureInfo.CurrentCulture, "$get('{0}').value='';", CityTextBox.ClientID);
		script.AppendFormat(CultureInfo.CurrentCulture, "$get('{0}').value='';", StateDropDown.ClientID);
		script.AppendFormat(CultureInfo.CurrentCulture, "$get('{0}').value='';", ZipCodeTextBox.ClientID);
		script.AppendFormat(CultureInfo.CurrentCulture, "$get('{0}').value='';", AddressTextBox.ClientID);
		script.Append("}");
		Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ClearView", script.ToString(), true);
	}

	#region ISearchCustomer Members

	public void ShowNoResultsMessage(string message)
	{
		CustomersGridView.SelectedIndex = -1;
		NoResultsLabel.Visible = true;
		NoResultsLabel.Text = message;
	}

	#endregion
}