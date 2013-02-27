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
using System.IO;
using System.Xml.Serialization;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Configuration
{
	[TestClass]
	public class DependantModuleInfoFixture
	{
		[TestMethod]
		public void CanCreateDependantModule()
		{
			DependantModuleInfo info = new DependantModuleInfo();

			Assert.IsNotNull(info is IModuleInfo);
		}

		[TestMethod]
		public void TestDependantModuleInfoIsIModuleInfo()
		{
			DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

			Assert.IsTrue(info is IModuleInfo);
		}


		[TestMethod]
		public void TestName()
		{
			DependantModuleInfo info = new DependantModuleInfo("Module1", "Module1.Implementation", "~/Module1");
			info.Dependencies = new ModuleDependency[] {new ModuleDependency()};
			info.Dependencies[0].Name = "Module0";
			StringWriter sw = new StringWriter();
			XmlSerializer xser = new XmlSerializer(typeof (DependantModuleInfo));

			xser.Serialize(sw, info);
			Console.Write(sw.GetStringBuilder().ToString());
		}

		/*
		[TestMethod]
		public void Test1()
		{
			DependantModuleInfo info = new DependantModuleInfo("Module1", "Module1", "Location");
			info.Dependencies = new ModuleDependency[] { new ModuleDependency() };
			info.Dependencies[0].Name = "Module0";

			info.AuthorizationRules = new ModuleAuthorization();
			info.AuthorizationRules.ModuleRules = new AuthorizationRule[] { new AuthorizationRule() };
			info.AuthorizationRules.ModuleRules[0].RuleName = "Rule1";

			info.AuthorizationRules.UrlRules = new AuthorizationRule[] { new AuthorizationRule() };
			info.AuthorizationRules.UrlRules[0].RuleName = "Rule2";


			StringWriter sw = new StringWriter();
			XmlSerializer xser = new XmlSerializer(typeof(DependantModuleInfo));

			xser.Serialize(sw, info);

			string s = sw.GetStringBuilder().ToString();
		}
		*/
		/*
	[TestMethod]
	public void TestCanAddDependency()
	{
		DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

		info.AddDependency("module");

		Assert.IsNotNull(info.Dependencies);
		Assert.IsTrue(info.Dependencies.Contains("module"));
	}


	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void TestModuleCannotDependOnItself()
	{
		DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

		info.AddDependency("name");
	}


	[TestMethod]
	public void TestDuplicatedDependenciesAreIgnored()
	{
		DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

		info.AddDependency("module");
		info.AddDependency("module");

		Assert.AreEqual(1, info.Dependencies.Count);
	}


	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void TestAddDependencyFailsForEmptyModuleName()
	{
		DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

		info.AddDependency(String.Empty);
	}


	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestAddDependencyFailsForNullModuleName()
	{
		DependantModuleInfo info = new DependantModuleInfo("name", "type", "location");

		info.AddDependency(null);
	}
		 * 
		 * */
	}
}