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
using System.Web.SessionState;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.Tests.Mocks
{
	public class MockSessionState : IHttpSessionState
	{
		public void Abandon()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Add(string name, object value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Clear()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int CodePage
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public System.Web.HttpCookieMode CookieMode
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void CopyTo(Array array, int index)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool IsCookieless
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsNewSession
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsReadOnly
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsSynchronized
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int LCID
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public SessionStateMode Mode
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void Remove(string name)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RemoveAll()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void RemoveAt(int index)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string SessionID
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Web.HttpStaticObjectsCollection StaticObjects
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object SyncRoot
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int Timeout
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public object this[int index]
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		private object savedValue = null;
		public object this[string name]
		{
			get
			{
				return savedValue;
			}
			set
			{
				savedValue = value;
			}
		}
	}
}

