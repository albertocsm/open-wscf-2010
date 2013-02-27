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

namespace Microsoft.Practices.CompositeWeb.Utility
{
	/// <summary>
	/// Generic arguments class to pass to event handlers that need to receive data.
	/// </summary>
	/// <typeparam name="TData">The type of data to pass.</typeparam>
	public class DataEventArgs<TData> : EventArgs
	{
		private TData _data;

		/// <summary>
		/// Initializes the DataEventArgs class.
		/// </summary>
		/// <param name="data">Information related to the event.</param>
		/// <exception cref="ArgumentNullException">The data is null.</exception>
		public DataEventArgs(TData data)
		{
			Guard.ArgumentNotNull(data, "Data");
			_data = data;
		}

		/// <summary>
		/// Gets the information related to the event.
		/// </summary>
		public TData Data
		{
			get { return _data; }
		}

		/// <summary>
		/// Provides a string representation of the argument data.
		/// </summary>
		public override string ToString()
		{
			return _data.ToString();
		}
	}
}