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
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	/// <summary>
	/// Summary description for CompositionContainerFixture
	/// </summary>
	[TestClass]
	public class CompositionContainerFixture
	{
		[TestMethod]
		public void DisposingContainerCausesContainedObjectsToBeTornDown()
		{
			TestableRootCompositionContainer cc = new TestableRootCompositionContainer();
			MockTearDownStrategy strategy = new MockTearDownStrategy();
			cc.Builder.Strategies.Add(strategy, WCSFBuilderStage.PreCreation);

			cc.Dispose();

			Assert.IsTrue(strategy.TearDownCalled);
		}

		[TestMethod]
		public void TerminatingCompositionContainerCausesItToBeRemovedFromParent()
		{
			TestableRootCompositionContainer parent = new TestableRootCompositionContainer();
			CompositionContainer child = parent.Containers.AddNew<CompositionContainer>();

			Assert.AreEqual(1, parent.Containers.Count);
			child.Dispose();
			Assert.AreEqual(0, parent.Containers.Count);
		}

		[TestMethod]
		public void TestServicesAreTornDownWhenContainerIsDisposed()
		{
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();

			MockService svc = container.Services.AddNew<MockService>();

			container.Dispose();

			Assert.IsTrue(svc.OnTearingDownCalled);
		}

		[TestMethod]
		public void ChildContainerAreDisposedWithParent()
		{
			TestableRootCompositionContainer parent = new TestableRootCompositionContainer();
			CompositionContainer child = parent.Containers.AddNew<CompositionContainer>();

			MockService svc = child.Services.AddNew<MockService>();
			parent.Dispose();

			Assert.IsTrue(svc.OnTearingDownCalled);
		}

		[TestMethod]
		public void RootContainerParentIsNull()
		{
			TestableRootCompositionContainer container = new TestableRootCompositionContainer();
			Assert.IsNull(container.Parent);
		}

		[TestMethod]
		public void ChildContainerHasParent()
		{
			TestableRootCompositionContainer parent = new TestableRootCompositionContainer();
			CompositionContainer child = parent.Containers.AddNew<CompositionContainer>();

			Assert.AreSame(parent, child.Parent);
		}

		[TestMethod]
		public void CanRegisterTypeMappingOnRootContainer()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();

			root.RegisterTypeMapping<IFoo, Foo>();

			Type mappedType = root.GetMappedType<IFoo>();

			Assert.AreEqual(typeof (Foo), mappedType);
		}

		[TestMethod]
		public void CanRegisterTypeMappingViaTypeObjects()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();
			root.RegisterTypeMapping(typeof (IFoo), typeof (Foo));

			Type mappedType = root.GetMappedType(typeof (IFoo));

			Assert.AreEqual(typeof (Foo), mappedType);
		}

		[TestMethod]
		public void RequestingTypeMappingForUnmappedTypeReturnsRequestedType()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();

			Type mappedType = root.GetMappedType<Foo>();

			Assert.AreEqual(typeof (Foo), mappedType);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void TypeMappingsMustBeTypeCompatible()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();
			root.RegisterTypeMapping(typeof (IBar), typeof (Foo));
		}

		// This test is commented out intentionally. The purpose of this test is to show 
		// that if you use the generic version of the RegisterTypeMapping method, if the
		// types aren't compatible it'll fail at compile time. 
		// 
		// This is exactly what happens. However that also means this file won't compile.
		// The test is left in as comments, if you wish to verify this then remove the comments,
		// watch the compile file, and then comment it out again.

		//[TestMethod]
		//public void GenericTypeMappingRegistrationEnforcesCompileTimeCompatibility()
		//{
		//     TestableRootCompositionContainer root = new TestableRootCompositionContainer();
		//     root.RegisterTypeMapping<IBar, Foo>();
		//}

		[TestMethod]
		public void CanRegisterMultipleTypeMappings()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();

			root.RegisterTypeMapping<IFoo, Foo>();
			root.RegisterTypeMapping<IBar, Bar>();

			Assert.AreEqual(typeof (Bar), root.GetMappedType<IBar>());
			Assert.AreEqual(typeof (Foo), root.GetMappedType<IFoo>());
		}

		[TestMethod]
		public void RequestingTypeMappingOnChildReadsFromParent()
		{
			TestableRootCompositionContainer parent = new TestableRootCompositionContainer();
			CompositionContainer child = parent.Containers.AddNew<CompositionContainer>();

			parent.RegisterTypeMapping<IFoo, Foo>();

			Assert.AreEqual(typeof (Foo), child.GetMappedType<IFoo>());
		}

		[TestMethod]
		public void ChildContainersCanOverrideParentTypeMapping()
		{
			TestableRootCompositionContainer parent = new TestableRootCompositionContainer();
			CompositionContainer child = parent.Containers.AddNew<CompositionContainer>();

			parent.RegisterTypeMapping<IFoo, Foo>();
			child.RegisterTypeMapping<IFoo, Foo2>();

			Assert.AreEqual(typeof (Foo), parent.GetMappedType<IFoo>());
			Assert.AreEqual(typeof (Foo2), child.GetMappedType<IFoo>());
		}

		[TestMethod]
		public void ChildContainerCanAccessToRootContainer()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();
			CompositionContainer child = root.Containers.AddNew<CompositionContainer>();
			CompositionContainer innerChild = child.Containers.AddNew<CompositionContainer>();

			Assert.AreEqual(root, innerChild.RootContainer);
		}

		[TestMethod]
		public void RootContainerCanAccessToRootContainer()
		{
			TestableRootCompositionContainer root = new TestableRootCompositionContainer();

			Assert.AreEqual(root, root.RootContainer);
		}

		#region Nested type: MockService

		private class MockService : IBuilderAware
		{
			public bool OnTearingDownCalled = false;

			#region IBuilderAware Members

			public void OnBuiltUp(string id)
			{
			}

			public void OnTearingDown()
			{
				OnTearingDownCalled = true;
			}

			#endregion
		}

		#endregion

		#region Nested type: MockTearDownStrategy

		private class MockTearDownStrategy : BuilderStrategy
		{
			public bool TearDownCalled = false;

			public override object TearDown(IBuilderContext context, object item)
			{
				TearDownCalled = true;
				return base.TearDown(context, item);
			}
		}

		#endregion
	}
}