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
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Properties;

namespace Microsoft.Practices.CompositeWeb.Services
{
	/// <summary>
	/// Exception thrown by <see cref="IModuleLoaderService"/> implementations whenever 
	/// a module fails to load.
	/// </summary>
	[Serializable]
	public class ModuleLoadException : Exception
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public ModuleLoadException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ModuleLoadException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance with a specified error message 
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="exception">The exception that is the cause of the current exception, 
		/// or a null reference if no inner exception is specified.</param>
		public ModuleLoadException(string message, Exception exception)
			: base(message, exception)
		{
		}


		/// <summary>
		/// Initializes the exception with a particular module and error message.
		/// </summary>
		public ModuleLoadException(string moduleAssembly, string message)
			: base(String.Format(CultureInfo.CurrentCulture,
			                     Resources.FailedToLoadModule, moduleAssembly, message))
		{
		}

		/// <summary>
		/// Initializes the exception with a particular module, error message and inner exception 
		/// that happened.
		/// </summary>
		public ModuleLoadException(string moduleAssembly, string message, Exception innerException)
			: base(String.Format(CultureInfo.CurrentCulture,
			                     Resources.FailedToLoadModule, moduleAssembly, message), innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance with serialized data.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		protected ModuleLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}