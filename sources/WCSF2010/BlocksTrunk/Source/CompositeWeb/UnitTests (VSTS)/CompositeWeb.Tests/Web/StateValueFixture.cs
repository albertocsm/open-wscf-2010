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
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Web
{
	/// <summary>
	/// Summary description for StateValueFixture
	/// </summary>
	[TestClass]
	public class StateValueFixture
	{
		[TestMethod]
		public void TestCanHandleValueTypes()
		{
			StateValue<Guid> stateValue = new StateValue<Guid>();

			Assert.AreEqual(GetDefault<Guid>(), stateValue.Value);
		}

		[TestMethod]
		public void TestCanSaveValueTypeValuesToSession()
		{
			StateValue<bool> stateValue = new StateValue<bool>();

			stateValue.SessionState = new MockSessionState();
			stateValue.Value = true;

			Assert.IsTrue(stateValue.Value);
		}


		[TestMethod]
		public void SetDefaultWorksAsExpected()
		{
			StateValue<Guid> stateValue = new StateValue<Guid>();
			stateValue.Value = Guid.NewGuid();

			Assert.AreNotEqual(GetDefault<Guid>(), stateValue.Value);

			stateValue.SetDefault();

			Assert.AreEqual(GetDefault<Guid>(), stateValue.Value);
		}

		[TestMethod]
		public void CanCreateInstanceSpecifyingValue()
		{
			Guid id = Guid.NewGuid();

			StateValue<Guid> stateValue = new StateValue<Guid>(id);

			Assert.AreEqual(id, stateValue.Value);
		}

		private static T GetDefault<T>()
		{
			return default(T);
		}
	}
}