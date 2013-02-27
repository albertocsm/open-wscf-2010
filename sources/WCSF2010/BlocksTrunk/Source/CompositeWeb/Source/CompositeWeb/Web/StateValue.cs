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
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace Microsoft.Practices.CompositeWeb.Web
{
	/// <summary>
	/// Implements the <see cref="IStateValue"/> interface for a generic type.
	/// </summary>
	/// <typeparam name="T">The type of the value to manage.</typeparam>
	public class StateValue<T> : IStateValue
	{
		private string _keyName;
		private IHttpSessionState _sessionState;
		private object _value;

		/// <summary>
		/// Initializes a new instance of <see cref="StateValue{T}"/>.
		/// </summary>
		public StateValue()
		{
			_value = default(T);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="StateValue{T}"/>.
		/// </summary>
		/// <param name="value">The initial value for the field.</param>
		public StateValue(T value)
		{
			_value = value;
		}

		/// <summary>
		/// Gets or sets the actual value from or to the session state.
		/// </summary>
		public T Value
		{
			get { return (T) ((IStateValue) this).Value; }
			set { ((IStateValue) this).Value = value; }
		}

		#region IStateValue Members

		/// <summary>
		/// Gets or sets the session state.
		/// </summary>
		public IHttpSessionState SessionState
		{
			get { return _sessionState; }
			set { _sessionState = value; }
		}

		/// <summary>
		/// Gets or sets the key of the value in the session state.
		/// </summary>
		public string KeyName
		{
			get { return _keyName; }
			set { _keyName = value; }
		}

		/// <summary>
		/// Sets the default value.
		/// </summary>
		public void SetDefault()
		{
			_value = default(T);
		}


		/// <summary>
		/// Gets or sets the actual value from or to the session state.
		/// </summary>
		object IStateValue.Value
		{
			get
			{
				if (SessionState == null || SessionState[_keyName] == null)
					return _value;
				else
					return SessionState[_keyName];
			}
			set
			{
				_value = value;

				if (SessionState != null)
					SessionState[_keyName] = value;
			}
		}

		#endregion
	}
}