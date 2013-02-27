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
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.Views;
using Microsoft.Practices.ObjectBuilder;

public partial class EFT_ConfirmTransfersView : Microsoft.Practices.CompositeWeb.Web.UI.Page, IConfirmTransfersView
{
	private ConfirmTransfersViewPresenter _presenter;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			_presenter.OnViewInitialized();
		}
		_presenter.OnViewLoaded();
	}

	protected void Page_LoadComplete(object sender, EventArgs e)
	{
		TransferGridView.DataBind();
	}

	[CreateNew]
	public ConfirmTransfersViewPresenter Presenter
	{
		get { return _presenter; }
		set
		{
			_presenter = value;
			_presenter.View = this;
		}
	}

	#region IConfirmTransfersView Members

	public Transfer[] Transfers
	{
		set { TransferGridView.DataSource = value; }
	}

	#endregion

	protected void SubmitButton_Click(object sender, EventArgs e)
	{
		_presenter.OnSubmit();
	}

	protected void PreviousButton_Click(object sender, EventArgs e)
	{
		_presenter.OnPrevious();
	}
}
