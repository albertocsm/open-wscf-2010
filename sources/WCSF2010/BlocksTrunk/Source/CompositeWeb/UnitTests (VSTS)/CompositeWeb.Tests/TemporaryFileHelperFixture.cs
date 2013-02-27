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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests
{
	[TestClass]
	public class TemporaryFileHelperFixture
	{
		[TestMethod]
		public void TestExtractedFileIsRemovedAfterUse()
		{
			Assert.IsFalse(File.Exists("SampleFile.txt"));
			using (TemporaryFileHelper helper = new TemporaryFileHelper())
			{
				helper.ExtractFile("SampleFile.txt");
				Assert.IsTrue(File.Exists("SampleFile.txt"));
			}
			Assert.IsFalse(File.Exists("SampleFile.txt"));
		}


		[TestMethod]
		public void TestCanExtractToDifferentFolder()
		{
			string filename = @"\TestFolder\SampleFile.txt";
			Assert.IsFalse(File.Exists(filename));
			using (TemporaryFileHelper helper = new TemporaryFileHelper())
			{
				helper.ExtractFile("SampleFile.txt", filename);
				Assert.IsTrue(File.Exists(filename));
			}
			Assert.IsFalse(File.Exists(filename));
		}


		[TestMethod]
		public void TestSeveralExtractedFilesAreRemovedAfterUse()
		{
			string filename1 = @"\TestFolder\SampleFile1.txt";
			string filename2 = @"\TestFolder\SubFolder\SampleFile2.txt";
			Assert.IsFalse(File.Exists(filename1));
			Assert.IsFalse(File.Exists(filename2));
			using (TemporaryFileHelper helper = new TemporaryFileHelper())
			{
				helper.ExtractFile("SampleFile.txt", filename1);
				helper.ExtractFile("SampleFile.txt", filename2);
				Assert.IsTrue(File.Exists(filename1));
				Assert.IsTrue(File.Exists(filename2));
			}
			Assert.IsFalse(File.Exists(filename1));
			Assert.IsFalse(File.Exists(filename2));
		}
	}
}