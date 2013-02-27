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
using System.Collections.ObjectModel;
using System.Configuration;
using GlobalBank.Commercial.EBanking.Modules.EFT.Constants;
using GlobalBank.Commercial.EBanking.Modules.EFT.Services;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.EntityTranslators;
using GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests
{
	[TestClass]
	public class EFTModuleInitializerFixture
	{
		[TestMethod]
		public void EFTModuleInitializerIsIModuleInitializer()
		{
			Assert.IsTrue(new EFTModuleInitializer() is ModuleInitializer);
		}

		[TestMethod]
		public void RegisterSiteMapInformationRegistersOneNodeWithOneChild()
		{
			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			SiteMapBuilderService siteMapBuilderService = new SiteMapBuilderService();

			module.TestRegisterSiteMapInformation(siteMapBuilderService);

			ReadOnlyCollection<SiteMapNodeInfo> nodes = siteMapBuilderService.GetChildren(siteMapBuilderService.RootNode.Key);
			Assert.AreEqual(1, nodes.Count);
			Assert.AreEqual(1, siteMapBuilderService.GetChildren(nodes[0].Key).Count);
		}

		[TestMethod]
		public void RegisterRequiredPermissionsRegistersOnePermissionSetWithTwoActions()
		{
			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			MockPermissionCatalog permissionCatalog = new MockPermissionCatalog();

			module.TestRegisterRequiredPermissions(permissionCatalog);

			Assert.AreEqual(1, permissionCatalog.RegisteredPermissions.Count);
			Assert.AreEqual("Electronic Funds Transfers", permissionCatalog.RegisteredSet.ModuleName);
			Assert.AreEqual(1, permissionCatalog.RegisteredSet.Actions.Count);

			List<Microsoft.Practices.CompositeWeb.Authorization.Action> actions = new List<Microsoft.Practices.CompositeWeb.Authorization.Action>(permissionCatalog.RegisteredSet.Actions);
			Assert.IsTrue(actions.Exists(delegate(Microsoft.Practices.CompositeWeb.Authorization.Action action) { return action.RuleName == Permissions.AllowCreateTransfers; }));
		}

		[TestMethod]
		public void RegisterTranslatorsRegistersFourTranslators()
		{
			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			MockEntityTranslatorService translatorService = new MockEntityTranslatorService();

			module.TestRegisterTranslators(translatorService);

			Assert.AreEqual(4, translatorService.RegisteredTranslators.Count);
			Assert.AreEqual(translatorService.RegisteredTranslators[0].GetType(), typeof(AccountTranslator));
		}

		[TestMethod]
		public void RegisterModuleServicesRegistrersOneServiceThatIsIAccountServiceAgent()
		{
			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			MockServiceCollection serviceCollection = new MockServiceCollection();

			module.TestAddModuleServices(serviceCollection);

			Assert.AreEqual(1, serviceCollection.RegistedServices.Keys.Count);
			Assert.IsTrue(serviceCollection.RegistedServices.ContainsKey(typeof(IAccountServiceAgent)));
		}

		[TestMethod]
		public void ConfigureShouldRegisterAuthorizationRules()
		{
			MockServiceCollection collection = new MockServiceCollection();
			collection.Add(typeof(IAuthorizationRulesService), new MockAuthorizationRulesService());

			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			module.Configure(collection, ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

			MockAuthorizationRulesService authRulesServices =
				(MockAuthorizationRulesService)collection.Get<IAuthorizationRulesService>();

			Assert.AreEqual(1, authRulesServices.RegisteredAuthorizationRules.Count);
			Assert.IsTrue(authRulesServices.RegisteredAuthorizationRules.ContainsKey("Default.aspx"));
			Assert.AreEqual("MockRule01", authRulesServices.RegisteredAuthorizationRules["Default.aspx"]);
		}

		[TestMethod]
		public void ConfigureShouldNotThrowExceptionIfAuthorizationServicesIsNotLoaded()
		{
			MockServiceCollection collection = new MockServiceCollection();

			TestableEFTModuleInitializer module = new TestableEFTModuleInitializer();
			module.Configure(collection, ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

			MockAuthorizationRulesService authRulesServices =
				(MockAuthorizationRulesService)collection.Get<IAuthorizationRulesService>();

			Assert.IsNull(authRulesServices);
		}


		internal class TestableEFTModuleInitializer : EFTModuleInitializer
		{
			public void TestRegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
			{
				RegisterSiteMapInformation(siteMapBuilderService);
			}

			public void TestRegisterRequiredPermissions(IPermissionsCatalog permissionsCatalog)
			{
				RegisterRequiredPermissions(permissionsCatalog);
			}

			public void TestRegisterTranslators(IEntityTranslatorService entityTranslatorService)
			{
				RegisterTranslators(entityTranslatorService);
			}

			public void TestAddModuleServices(IServiceCollection moduleServices)
			{
				AddModuleServices(moduleServices);
			}

			//public void TestRegisterPageFlows()
			//{
			//     RegisterPageFlows();
			//}
		}

		internal class MockAuthorizationRulesService : IAuthorizationRulesService
		{
			public Dictionary<string, string> RegisteredAuthorizationRules = new Dictionary<string, string>();

			#region IAuthorizationRulesService Members

			public void RegisterAuthorizationRule(string urlPath, string rule)
			{
				RegisteredAuthorizationRules[urlPath] = rule;
			}

			public string[] GetAuthorizationRules(string urlPath)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		internal class MockPermissionCatalog : IPermissionsCatalog
		{
			public ModuleActionSet RegisteredSet = null;
			private Dictionary<string, ModuleActionSet> _registeredPermissions = new Dictionary<string, ModuleActionSet>();

			#region IPermissionsCatalog Members

			public void RegisterPermissionSet(ModuleActionSet set)
			{
				RegisteredSet = set;
				_registeredPermissions.Add(set.ModuleName, set);
			}

			public Dictionary<string, ModuleActionSet> RegisteredPermissions
			{
				get { return _registeredPermissions; }
			}

			#endregion
		}

		internal class MockEntityTranslatorService : IEntityTranslatorService
		{
			public List<IEntityTranslator> RegisteredTranslators = new List<IEntityTranslator>();

			#region IEntityTranslatorService Members

			public bool CanTranslate(Type targetType, Type sourceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public bool CanTranslate<TTarget, TSource>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object Translate(Type targetType, object source)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public TTarget Translate<TTarget>(object source)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void RegisterEntityTranslator(IEntityTranslator translator)
			{
				RegisteredTranslators.Add(translator);
			}

			public void RemoveEntityTranslator(IEntityTranslator translator)
			{
			}

			#endregion
		}

		internal class MockServiceCollection : IServiceCollection
		{
			public Dictionary<Type, object> RegistedServices = new Dictionary<Type, object>();

			#region IServiceCollection Members

			public void Add<TService>(TService serviceInstance)
			{
				RegistedServices.Add(typeof(TService), serviceInstance);
			}

			public void Add(Type serviceType, object serviceInstance)
			{
				RegistedServices[serviceType] = serviceInstance;
			}

			public TService AddNew<TService, TRegisterAs>() where TService : TRegisterAs
			{
				RegistedServices.Add(typeof(TRegisterAs), null);
				return default(TService);
			}

			public TService AddNew<TService>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object AddNew(Type serviceType, Type registerAs)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object AddNew(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddOnDemand<TService, TRegisterAs>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddOnDemand<TService>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddOnDemand(Type serviceType, Type registerAs)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void AddOnDemand(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public bool Contains(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public bool Contains<TService>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public bool ContainsLocal(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public object Get(Type serviceType)
			{
				if (RegistedServices.ContainsKey(serviceType))
				{
					return RegistedServices[serviceType];
				}
				return null;
			}

			public object Get(Type serviceType, bool ensureExists)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public TService Get<TService>(bool ensureExists)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public TService Get<TService>()
			{
				if (RegistedServices.ContainsKey(typeof(TService)))
				{
					return (TService)RegistedServices[typeof(TService)];
				}
				return default(TService);
			}

			public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Remove(Type serviceType)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Remove<TService>()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}
	}
}
