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
using System.Threading;
using System.Web;
using GlobalBank.Commercial.EBanking.Modules.EFT.BusinessEntities;
using GlobalBank.Commercial.EBanking.Modules.EFT.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace GlobalBank.Commercial.EBanking.Modules.EFT
{
	public class EFTController
	{
		private IAccountServiceAgent _accountServiceAgent;
		private ISessionStateLocatorService _sessionService;

		[InjectionConstructor]
		public EFTController
			(
			[ServiceDependency] IAccountServiceAgent accountServiceAgent,
			[ServiceDependency] ISessionStateLocatorService sessionService
			)
		{
			_accountServiceAgent = accountServiceAgent;
			_sessionService = sessionService;
		}

		public virtual List<Transfer> Transfers
		{
			get
			{
				List<Transfer> trans = _sessionService.GetSessionState()["CurrentTransfers"] as List<Transfer>;
				if (trans != null)
				{
					return trans;
				}
				return null;
			}
			private set { _sessionService.GetSessionState()["CurrentTransfers"] = value; }
		}

		public virtual List<Account> Accounts
		{
			get
			{
				List<Account> accounts = _sessionService.GetSessionState()["CurrentAccounts"] as List<Account>;
				if (accounts != null)
				{
					return accounts;
				}
				return null;
			}
			private set { _sessionService.GetSessionState()["CurrentAccounts"] = value; }
		}

		public virtual void AddTransferToBatch(Transfer transfer)
		{
			Guard.ArgumentNotNull(transfer, "transfer");

			List<Transfer> transfers = Transfers;

			Account acct = Accounts.Find(delegate(Account match) { return match.Number == transfer.SourceAccount; });
			if (acct != null)
			{
				transfer.SourceAccountName = acct.Name;
			}

			acct = Accounts.Find(delegate(Account match) { return match.Number == transfer.DestinationAccount; });
			if (acct != null)
			{
				transfer.DestinationAccountName = acct.Name;
			}

			transfers.Add(transfer);
			Transfers = transfers;
		}


		public virtual void ChangeTransfers()
		{
			RedirectTo("CreateTransfer.aspx");
		}

		public virtual void ConfirmTransfers()
		{
			RedirectTo("ConfirmTransfersView.aspx");
		}

		public virtual Account[] GetAccounts()
		{
			if (Accounts == null)
			{
				string customer = Thread.CurrentPrincipal.Identity.Name;
				Accounts = new List<Account>(_accountServiceAgent.GetAccounts(customer));
			}
			return new List<Account>(OutputValidationUtility.Encode<Account>(Accounts)).ToArray();
		}

		public virtual Transfer[] GetTransfers()
		{
			if (Transfers == null)
			{
				Transfers = new List<Transfer>();
			}
			return new List<Transfer>(OutputValidationUtility.Encode<Transfer>(Transfers)).ToArray();
		}

		public virtual void RemoveTransferFromBatch(Transfer transfer)
		{
			List<Transfer> tempTransfers = Transfers;
			Transfer found = tempTransfers.Find(delegate(Transfer match) { return match.Id == transfer.Id; });
			if (found != null)
			{
				tempTransfers.Remove(found);
				Transfers = tempTransfers;
			}
		}

		public virtual void SubmitTransfers()
		{
			Transfers = new List<Transfer>(_accountServiceAgent.ProcessTransfers(Transfers.ToArray()));
			RedirectTo("SummaryView.aspx");
		}

		public virtual void TransferFunds()
		{
			RedirectTo("NewTransferView.aspx");
		}

		public virtual void UpdateTransferInBatch(Transfer updated)
		{
			List<Transfer> tempTransfers = Transfers;
			Transfer found = tempTransfers.Find(delegate(Transfer match) { return match.Id == updated.Id; });
			if (found != null)
			{
				found.Amount = updated.Amount;
				found.Description = updated.Description;
				found.DestinationAccount = updated.DestinationAccount;
				found.SourceAccount = updated.SourceAccount;

				Account acct = Accounts.Find(delegate(Account match) { return match.Number == updated.SourceAccount; });
				if (acct != null)
				{
					found.SourceAccountName = acct.Name;
				}

				acct = Accounts.Find(delegate(Account match) { return match.Number == updated.DestinationAccount; });

				if (acct != null)
				{
					found.DestinationAccountName = acct.Name;
				}

				Transfers = tempTransfers;
			}
		}

		public virtual void CreateAccount(Account account)
		{
			account.CustomerId = Thread.CurrentPrincipal.Identity.Name;
			_accountServiceAgent.CreateAccount(account);
		}

		public virtual void FundsTransferComplete()
		{
			Transfers = null;
		}

		protected virtual void RedirectTo(string newAddress)
		{
			Guard.ArgumentNotNullOrEmptyString(newAddress, "newAddress");
			IHttpContext context = new Microsoft.Practices.CompositeWeb.Web.HttpContext(HttpContext.Current);
			context.Server.Transfer(newAddress);
		}
	}
}
