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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GlobalBank.Infrastructure.Testing.UI.Tests
{
	/// <summary>
	/// Summary description for ClientIdMapperFixture
	/// </summary>
	[TestClass]
	public class ClientIdMapperFixture
	{

		[TestMethod]
		public void TestRenderControl()
		{
			Page page = new Page();

			StringWriter sw = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(sw);
			TestableClientIdMapper mapper = new TestableClientIdMapper();

			Panel panel = new Panel();
			panel.ID = "panel";
			TextBox tb = new TextBox();
			tb.ID = "text";
			panel.Controls.Add(tb);

			page.Controls.Add(panel);

			page.Controls.Add(mapper);
			mapper.TestRender(writer);

			string text = sw.GetStringBuilder().ToString();
			Debug.WriteLine("text='" + text + "'");
			Regex re = new Regex(@"<span ServerId=['""](?<serverId>\w+)['""] ClientId=['""](?<clientId>\w+)['""]></span>");
			MatchCollection matches = re.Matches(text);

			Assert.AreEqual(3, matches.Count);
			Assert.AreEqual("panel", matches[0].Groups["serverId"].Value);
			Assert.IsFalse(String.IsNullOrEmpty(matches[0].Groups["clientId"].Value));
			Assert.AreEqual("text", matches[1].Groups["serverId"].Value);
			Assert.IsFalse(String.IsNullOrEmpty(matches[1].Groups["clientId"].Value));
		}
	}

	class TestableClientIdMapper : ClientIdMapper
	{
		public void TestRender(System.Web.UI.HtmlTextWriter writer)
		{
			Render(writer);
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);
		}
	}
}
