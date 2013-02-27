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
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.CompositeWeb.Tests.Utility
{
	/// <summary>
	/// Unit test fixture for the <see cref="Guard"/> class.
	/// </summary>
	[TestClass]
	public class GuardFixture
	{
		private const string argName = "argumentToCheck";

		#region Testing ArgumentNotNullOrEmptyString

		[TestMethod]
		public void ShouldThrowOnNullStringArgument()
		{
			try
			{
				Guard.ArgumentNotNullOrEmptyString(null, argName);
				Assert.Fail("Should have thrown but didn't");
			}
			catch (ArgumentException)
			{
				// If we got here we're ok.
			}
		}

		[TestMethod]
		public void ShouldThrowOnEmptyStringArgument()
		{
			try
			{
				Guard.ArgumentNotNullOrEmptyString(string.Empty, argName);
				Assert.Fail("Should have thrown but didn't");
			}
			catch (ArgumentException)
			{
				// If we got here we're ok.
			}
		}

		[TestMethod]
		public void ShouldNotThrowForNonEmptyArgument()
		{
			Guard.ArgumentNotNullOrEmptyString("Something", argName);
		}

		[TestMethod]
		public void ShouldThrowForNullArgument()
		{
			try
			{
				Guard.ArgumentNotNull(null, argName);
			}
			catch (ArgumentException)
			{
				// If we got here we're ok
			}
		}

		#endregion

		#region Testing ArgumentNotNull

		[TestMethod]
		public void ShouldNotThrowForNonNullArgument()
		{
			Guard.ArgumentNotNull("Hi there", argName);
		}

		#endregion

		#region Testing EnumValueIsDefined

		[TestMethod]
		public void ShouldNotThrowForValidEnumValue()
		{
			Guard.EnumValueIsDefined(typeof (Colors), 2, argName);
		}

		[TestMethod]
		public void ShouldThrowForInvalidEnumValue()
		{
			try
			{
				Guard.EnumValueIsDefined(typeof (Colors), 42, argName);
			}
			catch (ArgumentException)
			{
				// If we get here, we're ok
			}
		}

		private enum Colors
		{
			Black,
			Red,
			Orange,
			Yellow,
			Green,
			Blue,
			Indigo,
			Violet,
			White
		}

		#endregion

		#region Testing EnumValueIsDefined

		[TestMethod]
		public void ShouldThrowIfInterfaceNotImplemented()
		{
			try
			{
				Guard.TypeIsAssignableFromType(typeof (IBar), typeof (Foo), argName);
				Assert.Fail("Should have thrown but didn't");
			}
			catch (ArgumentException)
			{
				// If we got here we're ok.
			}
		}

		[TestMethod]
		public void ShouldNotThrowIfInterfaceIsImplemented()
		{
			Guard.TypeIsAssignableFromType(typeof (IFoo), typeof (Foo), argName);
		}

		#endregion
	}
}