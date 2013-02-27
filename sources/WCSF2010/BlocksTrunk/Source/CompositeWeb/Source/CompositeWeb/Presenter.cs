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


namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Base class for presenters
	/// </summary>
	/// <typeparam name="TView">The type of the view this presenter is bound to</typeparam>
	public abstract class Presenter<TView>
	{
		private TView _view;

		/// <summary>
		/// The view this presenter is bound to
		/// </summary>
		public TView View
		{
			get { return _view; }
			set { _view = value; }
		}

		/// <summary>
		/// This method is invoked by the view the first time it loads
		/// </summary>
		public virtual void OnViewInitialized()
		{
		}

		/// <summary>
		/// This method is invoked by the view everytime it loads
		/// </summary>
		public virtual void OnViewLoaded()
		{
		}
	}
}