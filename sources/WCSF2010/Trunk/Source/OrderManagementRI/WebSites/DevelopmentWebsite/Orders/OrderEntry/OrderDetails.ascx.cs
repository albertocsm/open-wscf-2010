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
using AjaxControlToolkit;
using Orders.PresentationEntities;
using System.Collections.Generic;
using Microsoft.Practices.Web.UI.WebControls;
using System.Globalization;

public partial class Orders_OrderDetails : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IOrderDetails
{
    private OrderDetailsPresenter _presenter;
    private decimal? _total;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
        }
        this._presenter.OnViewLoaded();
    }

    [CreateNew]
    public OrderDetailsPresenter Presenter
    {
        set
        {
            this._presenter = value;
            this._presenter.View = this;
        }
    }


    /// <summary>
    /// Order Item Lines to be shown in details page.
    /// </summary>
    public IList<OrderItemLine> OrderItemsLines
    {
        get
        {
            IList<OrderItemLine> items = (IList<OrderItemLine>)ViewState["OrderItemsLines"];
            if (items == null)
            {
                items = new List<OrderItemLine>();
                ViewState["OrderItemsLines"] = items;
            }
            return items;
        }
        set
        {
            ViewState["OrderItemsLines"] = value;
        }
    }

    public void ShowOrderItemLines(IList<OrderItemLine> orderItemLines)
    {
        OrderItemsLines = orderItemLines as List<OrderItemLine>;
        OrderItemContainerDataSource.DataSource = OrderItemsLines;
    }

    public void ShowOrderTotalPrice(decimal? orderTotalPrice)
    {
        _total = orderTotalPrice;
    }

    /// <summary>
    /// RowEditing is handled to retain status of selected checkbox after entering to edit mode.
    /// </summary>
    protected void OrderDetailsGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UpdateRowIfInEditMode();
        GridViewRow row = OrderDetailsGridView.Rows[e.NewEditIndex];
        CheckBox selectedCheckBox = (CheckBox)row.FindControl("SelectedCheckBox");
        if (selectedCheckBox != null)
        {
            _presenter.OnEditOrderItemLine(row.DataItemIndex, selectedCheckBox.Checked);
        }
        OrderDetailsGridView.EditIndex = e.NewEditIndex;
    }

    protected void OrderItemContainerDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
    {
        UpdateRowIfInEditMode();
        List<OrderItemLine> lines = new List<OrderItemLine>();
        lines.Add((OrderItemLine)e.Instance);
        _presenter.OnDeleteOrderItemLines(lines);
        SetLastRowToEditMode();
    }

    protected void OrderItemContainerDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
    {
        _presenter.OnChangedOrderItemLine((OrderItemLine)e.Instance);
    }

    protected void OrderDetailsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label totalLabel = e.Row.FindControl("TotalLabel") as Label;
            if (totalLabel != null)
            {
                totalLabel.Text = _total.HasValue ? _total.Value.ToString("C2") : "0";
            }
        }
    }

    protected void DeleteOrderItemLineButton_Command(object sender, CommandEventArgs e)
    {
        int currentEditIndex = OrderDetailsGridView.EditIndex;
        if (currentEditIndex >= 0 && ((CheckBox)OrderDetailsGridView.Rows[currentEditIndex].FindControl("SelectedCheckBox")).Checked == false)
        {
            UpdateRowIfInEditMode();
        }

        List<OrderItemLine> linesToDelete = new List<OrderItemLine>();
        foreach (GridViewRow row in OrderDetailsGridView.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox selectedCheckBox = (CheckBox)row.FindControl("SelectedCheckBox");
                if (selectedCheckBox != null && selectedCheckBox.Checked)
                {
                    OrderItemLine orderItemLine = OrderItemsLines[row.DataItemIndex];
                    linesToDelete.Add(orderItemLine);
                }
            }
        }
        if (linesToDelete.Count > 0)
        {
            _presenter.OnDeleteOrderItemLines(linesToDelete);
        }
        //SetLastRowToEditMode();
        OrderDetailsGridView.EditIndex = -1;
    }

    protected void AddOrderItemLineButton_Command(object sender, CommandEventArgs e)
    {
        UpdateRowIfInEditMode();
        if (Page.IsValid)
        {
            _presenter.OnAddOrderItemLine();
            SetLastRowToEditMode();
        }
    }

    private void SetLastRowToEditMode()
    {
        if (OrderItemContainerDataSource.Items.Count > 0)
        {
            OrderDetailsGridView.EditIndex = OrderItemContainerDataSource.Items.Count - 1;
        }
        else
        {
            OrderDetailsGridView.EditIndex = -1;
        }
    }

    protected void OrderDetailsGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                string rowPostbackArguments = string.Format("Edit${0}", e.Row.RowIndex);
                string postbackCall = this.Page.ClientScript.GetPostBackClientHyperlink(OrderDetailsGridView, rowPostbackArguments);
                foreach (TableCell cell in e.Row.Cells)
                {
                    // Onclick is handled in each cell with no command buttons, to turn row to edit mode. 
                    if (ShouldAddEditPostbackToCell(cell))
                    {
                        cell.Attributes.Add("onclick", postbackCall);
                    }
                }
            }
            else
            {
                ModalPopupExtender modalPopup = e.Row.FindControl("ProductModalPopupExtender") as ModalPopupExtender;
                if (modalPopup != null)
                {
                    // Force postback to the SelectButton on the Ok event of the ModalPopup
                    modalPopup.OnOkScript = this.Page.ClientScript.GetPostBackClientHyperlink(SelectButton, "");
                }
            }
        }
    }

    private bool ShouldAddEditPostbackToCell(TableCell cell)
    {
        if (cell.Controls.Count == 0)
            return false;

        if (cell.Controls.Count > 1 && cell.Controls[1] is CheckBox)
            return false;

        if (cell.Controls[0] is ImageButton)
            return false;

        return true;
    }

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow row in OrderDetailsGridView.Rows)
        {
            string rowPostbackArguments = string.Format("Edit${0}", row.RowIndex);
            this.Page.ClientScript.RegisterForEventValidation(OrderDetailsGridView.UniqueID, rowPostbackArguments);
        }

        base.Render(writer);
    }


    protected void CancelButton_Click(object sender, EventArgs e)
    {
        _presenter.OnCancel();
    }

    protected void NextButton_Click(object sender, EventArgs e)
    {
        UpdateRowIfInEditMode();
        _presenter.OnNext();
    }

    protected void PreviousButton_Click(object sender, EventArgs e)
    {
        UpdateRowIfInEditMode();
        _presenter.OnPrevious();
    }

    protected void SelectButton_Click(object sender, EventArgs e)
    {
        _presenter.OnProductSelected(searchProductView.SelectedProductSku);
        searchProductView.ClearView();
    }

    protected void CancelSearchButton_Click(object sender, EventArgs e)
    {
        searchProductView.ClearView();
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        UpdateRowIfInEditMode();
        _presenter.OnSave();
    }

    public OrderItemLine EditingOrderItemLine
    {
        get
        {
            GridViewRow editingRow = OrderDetailsGridView.Rows[OrderDetailsGridView.EditIndex];
            OrderItemLine orderItemLine = OrderItemsLines[editingRow.DataItemIndex];
            return orderItemLine;
        }
    }

    private void UpdateRowIfInEditMode()
    {
        if (OrderDetailsGridView.EditIndex >= 0)
        {
            // Force validation in case postback was caused by a control that doesn't cause it.
            Page.Validate();
            if (Page.IsValid)
            {
                OrderDetailsGridView.UpdateRow(OrderDetailsGridView.EditIndex, false);
            }
        }
    }

    protected void SkuCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string sku = args.Value;
        string errorMessage;
        bool isValid = _presenter.IsSkuValid(sku, out errorMessage);
        if (!isValid)
        {
            CustomValidator validator = source as CustomValidator;
            validator.ErrorMessage = errorMessage;        
        }
        args.IsValid = isValid;
    }

    public void SetEditingProduct(string sku, string name, decimal? price)
    {
        GridViewRow editingRow=null;
        if (OrderDetailsGridView.EditIndex >= 0)
        {
            editingRow = OrderDetailsGridView.Rows[OrderDetailsGridView.EditIndex];
        }
        if (editingRow != null)
        {
            TextBox skuTextBox = editingRow.FindControl("SkuTextBox") as TextBox;
            Label nameLabel = editingRow.FindControl("ProductNameLabel") as Label;
            Label priceLabel = editingRow.FindControl("PriceLabel") as Label;

            skuTextBox.Text = sku;
            nameLabel.Text = name;
            priceLabel.Text = string.Format(CultureInfo.CurrentCulture, "{0:F2}", price);
        }
    }
}


