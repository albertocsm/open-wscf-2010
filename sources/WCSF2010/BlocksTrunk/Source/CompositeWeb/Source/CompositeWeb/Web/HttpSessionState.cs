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
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb.Properties;

namespace Microsoft.Practices.CompositeWeb.Web
{
	/// <summary>
	/// Implements the <see cref="IHttpSessionState"/> interface by wrapping <see cref="System.Web.SessionState.HttpSessionState"/>.
	/// </summary>
	public class HttpSessionState : IHttpSessionState
	{
		private System.Web.SessionState.HttpSessionState _session;

		/// <summary>
		/// Initializes a new instance of <see cref="HttpSessionState"/>.
		/// </summary>
		/// <param name="session">The <see cref="System.Web.SessionState.HttpSessionState"/> to wrap.</param>
		public HttpSessionState(System.Web.SessionState.HttpSessionState session)
		{
			_session = session;
		}

		/// <summary>
		/// Gets the wrapped <see cref="System.Web.SessionState.HttpSessionState"/>.
		/// </summary>
		/// <exception cref="Exception">If not SessionState can be found for the current context.</exception>
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
		protected System.Web.SessionState.HttpSessionState Session
		{
			get
			{
				System.Web.SessionState.HttpSessionState result = _session ?? System.Web.HttpContext.Current.Session;
				if (result == null)
				{
					throw new Exception(Resources.HttpSessionStateNotInitialized);
				}
				return result;
			}
		}

		#region IHttpSessionState Members

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public int CodePage
		{
			get { return Session.CodePage; }
			set { Session.CodePage = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public HttpCookieMode CookieMode
		{
			get { return Session.CookieMode; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public int Count
		{
			get { return Session.Count; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public bool IsCookieless
		{
			get { return Session.IsCookieless; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public bool IsNewSession
		{
			get { return Session.IsNewSession; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public bool IsReadOnly
		{
			get { return Session.IsReadOnly; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public bool IsSynchronized
		{
			get { return Session.IsSynchronized; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public NameObjectCollectionBase.KeysCollection Keys
		{
			get { return Session.Keys; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public int LCID
		{
			get { return Session.LCID; }
			set { Session.LCID = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public SessionStateMode Mode
		{
			get { return Session.Mode; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public string SessionID
		{
			get { return Session.SessionID; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public HttpStaticObjectsCollection StaticObjects
		{
			get { return Session.StaticObjects; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public object SyncRoot
		{
			get { return Session.SyncRoot; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public int Timeout
		{
			get { return Session.Timeout; }
			set { Session.Timeout = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public object this[int index]
		{
			get { return Session[index]; }
			set { Session[index] = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public object this[string name]
		{
			get { return Session[name]; }
			set { Session[name] = value; }
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void Abandon()
		{
			Session.Abandon();
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void Add(string name, object value)
		{
			Session.Add(name, value);
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void Clear()
		{
			Session.Clear();
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void CopyTo(Array array, int index)
		{
			Session.CopyTo(array, index);
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return Session.GetEnumerator();
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void Remove(string name)
		{
			Session.Remove(name);
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void RemoveAll()
		{
			Session.RemoveAll();
		}

		/// <summary>
		/// See <see cref="System.Web.SessionState.IHttpSessionState"/> for more information.
		/// </summary>
		public void RemoveAt(int index)
		{
			Session.RemoveAt(index);
		}

		#endregion
	}
}