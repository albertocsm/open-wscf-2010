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
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder
{
	[TestClass]
	public class WCSFBuilderBaseFixture
	{
		[TestMethod]
		public void AddingStrategyByAnyStageRunsItDuringBuild()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = CreateLocator();

			MockStrategy strategy = new MockStrategy();
			builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			builder.BuildUp<object>(locator, null, null);

			Assert.IsTrue(strategy.BuildWasRun);
		}

		[TestMethod]
		public void StrategyStagesRunInProperOrder()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = CreateLocator();

			MockStrategy strategy = new MockStrategy();

			builder.Strategies.Add(new MockStrategy("Creation "), BuilderStage.Creation);
			builder.Strategies.Add(new MockStrategy("PostInitialization "), BuilderStage.PostInitialization);
			builder.Strategies.Add(new MockStrategy("PreCreation "), BuilderStage.PreCreation);
			builder.Strategies.Add(new MockStrategy("Initialization "), BuilderStage.Initialization);

			string s = builder.BuildUp<string>(locator, null, null);

			Assert.AreEqual("PreCreation Creation Initialization PostInitialization ", s);
		}

		[TestMethod]
		public void UnbuildStrategiesRunInOppositeOrderOfBuild()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = CreateLocator();

			MockStrategy strategy = new MockStrategy();

			builder.Strategies.Add(new MockStrategy("Creation "), BuilderStage.Creation);
			builder.Strategies.Add(new MockStrategy("PostInitialization "), BuilderStage.PostInitialization);
			builder.Strategies.Add(new MockStrategy("PreCreation "), BuilderStage.PreCreation);
			builder.Strategies.Add(new MockStrategy("Initialization "), BuilderStage.Initialization);

			string s = builder.TearDown(locator, "");

			Assert.AreEqual("PostInitialization Initialization Creation PreCreation ", s);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CannotUnbuildANullReference()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = CreateLocator();

			builder.TearDown(locator, (object) null);
		}

		[TestMethod]
		[ExpectedException(typeof (InvalidOperationException))]
		public void RunningABuilderBaseWithNoStrategiesThrows()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = CreateLocator();

			builder.BuildUp<Object>(locator, null, null);
		}

		[TestMethod]
		public void CanInitializeBuilderBaseWithConfigurator()
		{
			MockConfigurator config = new MockConfigurator();

			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>(config);

			Assert.IsTrue(config.WasCalled);
		}

		[TestMethod]
		public void BuildUpWithNoLocatorDoesNotThrow()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = null;

			MockStrategy strategy = new MockStrategy();
			builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			builder.BuildUp<object>(locator, null, null);
		}

		[TestMethod]
		public void TearDownWithNoLocatorDoesNotThrow()
		{
			IBuilder<BuilderStage> builder = new WCSFBuilderBase<BuilderStage>();
			Locator locator = null;

			MockStrategy strategy = new MockStrategy();
			builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			builder.TearDown<object>(locator, "");
		}

		#region Helpers

		private Locator CreateLocator()
		{
			Locator locator = new Locator();
			LifetimeContainer lifetime = new LifetimeContainer();
			locator.Add(typeof (ILifetimeContainer), lifetime);
			return locator;
		}

		#region Nested type: MockConfigurator

		private class MockConfigurator : IBuilderConfigurator<BuilderStage>
		{
			public bool WasCalled = false;

			#region IBuilderConfigurator<BuilderStage> Members

			public void ApplyConfiguration(IBuilder<BuilderStage> builder)
			{
				WasCalled = true;
			}

			#endregion
		}

		#endregion

		#region Nested type: MockStrategy

		private class MockStrategy : BuilderStrategy
		{
			public bool BuildWasRun = false;
			public string StringValue;
			public bool UnbuildWasRun = false;

			public MockStrategy()
				: this("")
			{
			}

			public MockStrategy(string value)
			{
				StringValue = value;
			}

			public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
			{
				BuildWasRun = true;
				return base.BuildUp(context, t, AppendString(existing), id);
			}

			public override object TearDown(IBuilderContext context, object item)
			{
				UnbuildWasRun = true;
				return base.TearDown(context, AppendString(item));
			}

			private string AppendString(object item)
			{
				string result;

				if (item == null)
					result = StringValue;
				else
					result = ((string) item) + StringValue;

				return result;
			}
		}

		#endregion

		#endregion
	}
}