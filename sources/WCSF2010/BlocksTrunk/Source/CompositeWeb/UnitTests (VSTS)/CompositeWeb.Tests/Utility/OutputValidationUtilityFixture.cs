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
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Utility
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class OutputValidationUtilityFixture
	{
		[TestMethod]
		public void ShouldEncodeStringProperties()
		{
			MyEntity entityToEncode = new MyEntity("<tag>", 10);

			MyEntity encodedEntity = OutputValidationUtility.Encode<MyEntity>(entityToEncode);

			Assert.AreEqual("&lt;tag&gt;", encodedEntity.PropertyToEncode);
		}

		[TestMethod]
		public void ShouldEncodeStringPropertiesOnCollectionsItems()
		{
			List<MyEntity> entities = new List<MyEntity>();

			entities.Add(new MyEntity("<TAG>", 1));
			entities.Add(new MyEntity("noHTML", 1));

			IList<MyEntity> encodedEntites = OutputValidationUtility.Encode<MyEntity>(entities);

			Assert.AreEqual("&lt;TAG&gt;", encodedEntites[0].PropertyToEncode);
			Assert.AreEqual("noHTML", encodedEntites[1].PropertyToEncode);
		}

		[TestMethod]
		public void ShouldReturnNullWhenObjectToEncodeIsNull()
		{
			MyEntity entity = null;
			MyEntity encodedEntity = OutputValidationUtility.Encode<MyEntity>(entity);

			Assert.IsNull(encodedEntity);
		}
	}

	internal class MyEntity
	{
		private int _propertyNotEncodeable;
		private string _propertyToEncode;

		public MyEntity(string propertyToEncode, int propertyNotEncodeable)
		{
			_propertyToEncode = propertyToEncode;
			_propertyNotEncodeable = propertyNotEncodeable;
		}

		public string PropertyToEncode
		{
			get { return _propertyToEncode; }
			set { _propertyToEncode = value; }
		}

		public int PropertyNotEncodeable
		{
			get { return _propertyNotEncodeable; }
			set { _propertyNotEncodeable = value; }
		}
	}
}