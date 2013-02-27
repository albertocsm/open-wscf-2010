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
using System.Web.Security;
using System.Web.UI;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;
using GlobalBank.Infrastructure.Testing.UI;

namespace GlobalBank
{
	public class ElectronicBankingApplication : WebClientApplication
	{
		protected override void AddRequiredServices()
		{
            // TBD: Move this line to EFT or move EntityTranslatorService to CWAB and register the service
            // in the Shell module.
            RootContainer.Services.AddNew<EntityTranslatorService, IEntityTranslatorService>();
            base.AddRequiredServices();
		}

		protected override void Start()
		{
			base.Start();			
		}

		protected override void PrePageExecute(Page page)
		{
			page.Init += new EventHandler(OnPageInit);
		}

		protected override void PostPageExecute(Page page)
		{
			page.Init -= new EventHandler(OnPageInit);
		}

		protected void OnPageInit(object sender, EventArgs e)
		{
			((Page)sender).Controls.Add(new ClientIdMapper());
		}
	}
}
