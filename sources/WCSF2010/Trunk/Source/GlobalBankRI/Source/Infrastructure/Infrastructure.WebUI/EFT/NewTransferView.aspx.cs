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
using System.Globalization;
using System.Threading;
using System.Web;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.Views;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.Web.UI.WebControls;

public partial class EFT_NewTransferView : Microsoft.Practices.CompositeWeb.Web.UI.Page, INewTransferView
{
	private NewTransferViewPresenter _presenter;

	protected override void InitializeCulture()
	{
		string[] lang = HttpContext.Current.Request.UserLanguages;
		if (lang != null && lang.Length > 0)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang[0]);
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang[0]);
		}
		base.InitializeCulture();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			_presenter.OnViewInitialized();
		}
		_presenter.OnViewLoaded();
	}

	[CreateNew]
	public NewTransferViewPresenter Presenter
	{
		get { return _presenter; }
		set
		{
			_presenter = value;
			_presenter.View = this;
		}
	}

	#region INewTransferView Members

	public Transfer[] Transfers
	{
		set { TransferBatchDataSource.DataSource = value; }
	}

	public Account[] Accounts
	{
		set { AccountsDataSource.DataSource = value; }
	}

	public bool EnableAddTransfer
	{
		get { return AddTransferView.Enabled; }
		set { AddTransferView.Enabled = value; }
	}

	#endregion

	protected void TransferBatchDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
	{
		_presenter.OnTransferUpdated((Transfer) e.Instance);
	}

	protected void TransferBatchDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
	{
		_presenter.OnTransferDeleted((Transfer) e.Instance);
	}

	protected void TransferBatchDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
	{
		_presenter.OnTransferInserted((Transfer) e.Instance);
	}

	protected void NextButton_Click(object sender, EventArgs e)
	{
		_presenter.OnNext();
	}
}
