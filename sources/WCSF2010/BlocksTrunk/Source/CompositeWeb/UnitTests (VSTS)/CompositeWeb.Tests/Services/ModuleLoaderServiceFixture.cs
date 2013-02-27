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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Services
{
	[TestClass]
	public class ModuleLoaderServiceFixture
	{
		private static Dictionary<string, Assembly> generatedAssemblies;

		static ModuleLoaderServiceFixture()
		{
			generatedAssemblies = new Dictionary<string, Assembly>();

			generatedAssemblies.Add("TestModule",
			                        CompileFile(
			                        	"Microsoft.Practices.CompositeWeb.Tests.Mocks.Src.TestModule.cs",
			                        	@".\TestModule\TestModule.dll"));

			generatedAssemblies.Add("ModuleThrowingException",
			                        CompileFile(
			                        	"Microsoft.Practices.CompositeWeb.Tests.Mocks.Src.ModuleThrowingException.cs",
			                        	@".\ModuleThrowingException\ModuleThrowingException.dll"));
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void NullCompositionContainerThrows()
		{
			ModuleLoaderService loader = new ModuleLoaderService();
			loader.Load(null, new MockModuleInfo());
		}

		[TestMethod]
		public void LoadCallsLoadOnModuleInitializer()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			ModuleLoaderService loader = new ModuleLoaderService();

			loader.Load(mockContainer,
			            new ModuleInfo("TestModule", generatedAssemblies["TestModule"].FullName, "~/TestModule"));

			bool loadCalled =
				(bool)
				generatedAssemblies["TestModule"].GetType("TestModule.TestModuleInitializer").GetField("LoadCalled").GetValue(null);
			Assert.IsTrue(loadCalled);
		}

		[TestMethod]
		public void InitializationExceptionsAreWrapped()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			ModuleLoaderService loader = new ModuleLoaderService();

			try
			{
				loader.Load(mockContainer,
				            new ModuleInfo("ModuleThrowingException", generatedAssemblies["ModuleThrowingException"].FullName,
				                           "~/ModuleThrowingException"));

				Assert.Fail("ModuleLoadException was not thrown");
			}
			catch (Exception ex)
			{
				if (!(ex is ModuleLoadException))
					Assert.Fail("Exception is not of type ModuleLoadException");

				if (!(((ModuleLoadException) ex).InnerException is NotImplementedException))
					Assert.Fail("The actual inner exception was not wrapped correctly");
			}
		}

		[TestMethod]
		public void LoadCreatesNewContainerForModule()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			ModuleLoaderService loader = new ModuleLoaderService();

			loader.Load(mockContainer,
			            new ModuleInfo("TestModuleName", generatedAssemblies["TestModule"].FullName, "~/TestModule"));

			CompositionContainer moduleContainer = mockContainer.Containers["TestModuleName"];
			Assert.IsNotNull(moduleContainer);
		}


		[TestMethod]
		public void FindModuleInitializerReturnsCorrectInstance()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			ModuleLoaderService loader = new ModuleLoaderService();
			loader.Load(mockContainer,
			            new ModuleInfo("TestModuleName", generatedAssemblies["TestModule"].FullName, "~/TestModule"));

			IModuleInitializer initializer = loader.FindInitializer("TestModuleName");
			Assert.IsNotNull(initializer);
			Assert.AreEqual("TestModule.TestModuleInitializer", initializer.GetType().FullName);
		}


		[TestMethod]
		public void FindModuleInitializerReturnsNullIfnotExists()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			ModuleLoaderService loader = new ModuleLoaderService();
			loader.Load(mockContainer,
			            new ModuleInfo("TestModuleName", generatedAssemblies["TestModule"].FullName, "~/TestModule"));

			IModuleInitializer initializer = loader.FindInitializer("InexistantName");
			Assert.IsNull(initializer);
		}

		[TestMethod]
		public void LoadRegistersServicesUsingIServiceLoaderService()
		{
			CompositionContainer mockContainer = new TestableRootCompositionContainer();
			MockServiceLoaderService serviceLoaderService = new MockServiceLoaderService();
			mockContainer.Services.Add<IServiceLoaderService>(serviceLoaderService);
			ModuleLoaderService loader = new ModuleLoaderService();
			DependantModuleInfo moduleInfo =
				new DependantModuleInfo("TestModule", generatedAssemblies["TestModule"].FullName, "~/TestModule");
			moduleInfo.Services =
				new ServiceInfo[1] {new ServiceInfo(typeof (IMockService), typeof (MockService), ServiceScope.Global)};

			loader.Load(mockContainer, moduleInfo);

			Assert.IsNotNull(serviceLoaderService.UsedServices);
			Assert.AreEqual(1, serviceLoaderService.UsedServices.Length);
			Assert.AreEqual(typeof (IMockService), serviceLoaderService.UsedServices[0].RegisterAs);
			Assert.IsNotNull(serviceLoaderService.UsedCompositionContainer);
			Assert.AreEqual(mockContainer.Containers["TestModule"], serviceLoaderService.UsedCompositionContainer);
		}

		#region Nested type: IMockService

		private interface IMockService
		{
		}

		#endregion

		#region Nested type: MockModuleInfo

		private class MockModuleInfo : IModuleInfo
		{
			#region IModuleInfo Members

			public string Name
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string AssemblyName
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string VirtualPath
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			#endregion
		}

		#endregion

		#region Nested type: MockService

		private class MockService : IMockService
		{
		}

		#region Helper methods

		public static Assembly CompileFile(string input, string output, params string[] references)
		{
			CreateOutput(output);

			List<string> referencedAssemblies = new List<string>(references.Length + 3);

			referencedAssemblies.AddRange(references);
			referencedAssemblies.Add("System.dll");
			referencedAssemblies.Add(typeof (IModuleInitializer).Assembly.CodeBase.Replace(@"file:///", ""));

			CSharpCodeProvider codeProvider = new CSharpCodeProvider();
			CompilerParameters cp = new CompilerParameters(referencedAssemblies.ToArray(), output);

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(input))
			{
				if (stream == null)
				{
					throw new ArgumentException("input");
				}

				StreamReader reader = new StreamReader(stream);
				string source = reader.ReadToEnd();
				CompilerResults results = codeProvider.CompileAssemblyFromSource(cp, source);
				ThrowIfCompilerError(results);
				return results.CompiledAssembly;
			}
		}

		public static void CreateOutput(string output)
		{
			string dir = Path.GetDirectoryName(output);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		public static void ThrowIfCompilerError(CompilerResults results)
		{
			if (results.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Compilation failed.");
				foreach (CompilerError error in results.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				Assert.IsFalse(results.Errors.HasErrors, sb.ToString());
			}
		}

		#endregion

		#endregion
	}
}