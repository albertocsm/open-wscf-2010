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
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Indicates that property or parameter is a dependency on a <see cref="Microsoft.Practices.CompositeWeb.Interfaces.IStateValue"/> and
	/// should be dependency injected when the class is put into a <see cref="CompositionContainer"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public sealed class StateDependencyAttribute : ParameterAttribute
	{
		private readonly string _keyName;

		/// <summary>
		/// Initializes a new instance of the <see cref="StateDependencyAttribute"/> class.
		/// <param name="keyName">The key used to retrieve the state value from the session</param>
		/// </summary>
		public StateDependencyAttribute(string keyName)
		{
			_keyName = keyName;
		}

		/// <summary>
		/// Gets the current key used to retrieve the state value from the session
		/// </summary>
		public string KeyName
		{
			get { return _keyName; }
		}

		/// <summary>
		/// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
		/// </summary>
		public override IParameter CreateParameter(Type memberType)
		{
			Guard.TypeIsAssignableFromType(typeof (IStateValue), memberType, "memberType");

			return new StateDependencyParameter(memberType, KeyName);
		}

		#region Nested type: StateDependencyParameter

		private class StateDependencyParameter : IParameter
		{
			private string _keyName;
			private Type _memberType;

			public StateDependencyParameter(Type memberType, string keyName)
			{
				_memberType = memberType;
				_keyName = keyName;
			}

			#region IParameter Members

			public Type GetParameterType(IBuilderContext context)
			{
				return _memberType;
			}

			public object GetValue(IBuilderContext context)
			{
				ISessionStateLocatorService sessionLocator =
					context.Locator.Get<ISessionStateLocatorService>(
						new DependencyResolutionLocatorKey(typeof (ISessionStateLocatorService), null));
				if (sessionLocator != null)
				{
					IStateValue value = (IStateValue) Activator.CreateInstance(_memberType);
					value.SessionState = sessionLocator.GetSessionState();
					value.KeyName = _keyName;
					return value;
				}
				return null;
			}

			#endregion
		}

		#endregion
	}
}