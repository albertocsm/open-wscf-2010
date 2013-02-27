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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Properties;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Collections
{
	/// <summary>
	/// Collection of services that are contained in a <see cref="CompositionContainer"/>.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers")]
	public class ServiceCollection : ICollection, IEnumerable<KeyValuePair<Type, object>>, IServiceCollection
	{
		private IBuilder<WCSFBuilderStage> _builder;
		private ILifetimeContainer _container;
		private IReadWriteLocator _locator;
		private IServiceCollection _parent;

		/// <summary>
		/// Initializes an instance of the <see cref="ServiceCollection"/> class.
		/// </summary>
		/// <param name="container">The lifetime container the collection will use</param>
		/// <param name="locator">The locator the collection will use</param>
		/// <param name="builder">The builder the collection will use</param>
		/// <param name="parent">The parent collection</param>
		public ServiceCollection(ILifetimeContainer container, IReadWriteLocator locator, IBuilder<WCSFBuilderStage> builder,
		                         IServiceCollection parent)
		{
			_builder = builder;
			_container = container;
			_locator = locator;
			_parent = parent;
		}

		#region ICollection Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		int ICollection.Count
		{
			get { throw new NotImplementedException(); }
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		object ICollection.SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IEnumerable<KeyValuePair<Type,object>> Members

		/// <summary>
		/// Enumerates through all seen types and retrieves a KeyValuePair for each. 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
		{
			IReadableLocator currentLocator = _locator;
			Dictionary<Type, object> seenTypes = new Dictionary<Type, object>();

			do
			{
				foreach (KeyValuePair<object, object> pair in currentLocator)
					if (pair.Key is DependencyResolutionLocatorKey)
					{
						DependencyResolutionLocatorKey depKey = (DependencyResolutionLocatorKey) pair.Key;

						if (depKey.ID == null)
						{
							Type type = depKey.Type;

							if (!seenTypes.ContainsKey(type))
							{
								seenTypes[type] = String.Empty;
								yield return new KeyValuePair<Type, object>(type, pair.Value);
							}
						}
					}
			} while ((currentLocator = currentLocator.ParentLocator) != null);
		}

		#endregion

		#region IServiceCollection Members

		/// <summary>
		/// Adds a service.
		/// </summary>
		/// <typeparam name="TService">The type under which to register the service.</typeparam>
		/// <param name="serviceInstance">The service to register.</param>
		/// <exception cref="ArgumentNullException">serviceInstance is null.</exception>
		/// <exception cref="ArgumentException">A service of the given type is already registered.</exception>
		public void Add<TService>(TService serviceInstance)
		{
			Add(typeof (TService), serviceInstance);
		}

		/// <summary>
		/// Adds a service.
		/// </summary>
		/// <param name="serviceType">The type under which to register the service.</param>
		/// <param name="serviceInstance">The service to register.</param>
		/// <exception cref="ArgumentNullException">serviceInstance is null.</exception>
		/// <exception cref="ArgumentException">A service of the given type is already registered.</exception>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		public void Add(Type serviceType, object serviceInstance)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");
			Guard.ArgumentNotNull(serviceInstance, "serviceInstance");

			Build(serviceInstance.GetType(), serviceType, serviceInstance);
		}

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <typeparam name="TService">The type of service</typeparam>
		public void AddOnDemand<TService>()
		{
			AddOnDemand(typeof (TService), null);
		}

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <typeparam name="TService">The type of service</typeparam>
		/// <typeparam name="TRegisterAs">The type to register the service as</typeparam>
		public void AddOnDemand<TService, TRegisterAs>()
		{
			AddOnDemand(typeof (TService), typeof (TRegisterAs));
		}

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <param name="serviceType">The type of service</param>
		public void AddOnDemand(Type serviceType)
		{
			AddOnDemand(serviceType, null);
		}

		/// <summary>
		/// Adds a service that will not be created until the first time it is requested.
		/// </summary>
		/// <param name="serviceType">The type of service</param>
		/// <param name="registerAs">The type to register the service as</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		public void AddOnDemand(Type serviceType, Type registerAs)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");

			if (registerAs == null)
				registerAs = serviceType;

			DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(registerAs, null);
			if (_locator.Contains(key, SearchMode.Local))
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentCulture, Resources.DuplicateService, registerAs.FullName));

			DemandAddPlaceholder placeholder = new DemandAddPlaceholder(serviceType);
			_locator.Add(key, placeholder);
			_container.Add(placeholder);
		}

		/// <summary>
		/// Creates and adds a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to create. This type is also
		/// the type the service is registered under.</typeparam>
		/// <returns>The new service instance.</returns>
		/// <exception cref="ArgumentException">Object builder cannot find an appropriate
		/// constructor on the object.</exception>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		public TService AddNew<TService>()
		{
			return AddNew<TService, TService>();
		}

		/// <summary>
		/// Creates a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to create.</typeparam>
		/// <typeparam name="TRegisterAs">The type the service is registered under.</typeparam>
		/// <returns>The new service instance.</returns>
		/// <exception cref="ArgumentException">Object builder cannot find an appropriate
		/// constructor on the object.</exception>
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		public TService AddNew<TService, TRegisterAs>()
			where TService : TRegisterAs
		{
			return (TService) AddNew(typeof (TService), typeof (TRegisterAs));
		}

		/// <summary>
		/// Creates a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to create. This type is also
		/// the type the service is registered under.</param>
		/// <returns>The new service instance.</returns>
		/// <exception cref="ArgumentException">Object builder cannot find an appropriate
		/// constructor on the object.</exception>
		public object AddNew(Type serviceType)
		{
			return AddNew(serviceType, serviceType);
		}

		/// <summary>
		/// Creates a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to create.</param>
		/// <param name="registerAs">The type the service is registered under.</param>
		/// <returns>The new service instance.</returns>
		/// <exception cref="ArgumentException">Object builder cannot find an appropriate
		/// constructor on the object.</exception>
		public object AddNew(Type serviceType, Type registerAs)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");
			Guard.ArgumentNotNull(registerAs, "registerAs");

			return Build(serviceType, registerAs, null);
		}

		/// <summary>
		/// Determines whether the given service type exists in the collection.
		/// </summary>
		/// <typeparam name="TService">Type of service to search for.</typeparam>
		/// <returns>
		/// true if the TService exists; 
		/// false otherwise.
		/// </returns>
		public bool Contains<TService>()
		{
			return Contains(typeof (TService));
		}

		/// <summary>
		/// Determines whether the given service type exists in the collection.
		/// </summary>
		/// <param name="serviceType">Type of service to search for.</param>
		/// <returns>
		/// true if the serviceType exists; 
		/// false otherwise.
		/// </returns>
		public bool Contains(Type serviceType)
		{
			return Contains(serviceType, SearchMode.Up);
		}

		/// <summary>
		/// Determines whether the given service type exists directly in the local collection. The parent
		/// collections are not consulted.
		/// </summary>
		/// <param name="serviceType">Type of service to search for.</param>
		/// <returns>
		/// true if the serviceType exists; 
		/// false otherwise.
		/// </returns>
		public bool ContainsLocal(Type serviceType)
		{
			return Contains(serviceType, SearchMode.Local);
		}

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to be found.</typeparam>
		/// <returns>The service instance, if present; null otherwise.</returns>
		public TService Get<TService>()
		{
			return (TService) Get(typeof (TService), false);
		}

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service to be found.</typeparam>
		/// <param name="ensureExists">If true, will throw an exception if the service is not found.</param>
		/// <returns>The service instance, if present; null if not (and ensureExists is false).</returns>
		/// <exception cref="ServiceMissingException">Thrown if ensureExists is true and the service is not found.</exception>
		public TService Get<TService>(bool ensureExists)
		{
			return (TService) Get(typeof (TService), ensureExists);
		}

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to be found.</param>
		/// <returns>The service instance, if present; null otherwise.</returns>
		public object Get(Type serviceType)
		{
			return Get(serviceType, false);
		}

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <param name="serviceType">The type of the service to be found.</param>
		/// <param name="ensureExists">If true, will throw an exception if the service is not found.</param>
		/// <returns>The service instance, if present; null if not (and ensureExists is false).</returns>
		/// <exception cref="ServiceMissingException">Thrown if ensureExists is true and the service is not found.</exception>
		public object Get(Type serviceType, bool ensureExists)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");

			if (Contains(serviceType, SearchMode.Local))
			{
				object result = _locator.Get(new DependencyResolutionLocatorKey(serviceType, null));

				DemandAddPlaceholder placeholder = result as DemandAddPlaceholder;

				if (placeholder != null)
				{
					Remove(serviceType);
					result = Build(placeholder.TypeToCreate, serviceType, null);
				}

				return result;
			}

			if (_parent != null)
				return _parent.Get(serviceType, ensureExists);

			if (ensureExists)
				throw new ServiceMissingException(serviceType);

			return null;
		}

		/// <summary>
		/// Removes a service registration from the <see cref="CompositionContainer"/>.
		/// </summary>
		/// <typeparam name="TService">The service type to remove.</typeparam>
		public void Remove<TService>()
		{
			Remove(typeof (TService));
		}

		/// <summary>
		/// Removes a service registration from the <see cref="CompositionContainer"/>.
		/// </summary>
		/// <param name="serviceType">The service type to remove.</param>
		public void Remove(Type serviceType)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");
			DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(serviceType, null);

			if (_locator.Contains(key, SearchMode.Local))
			{
				object serviceInstance = _locator.Get(key, SearchMode.Local);
				bool isLastInstance = true;

				_locator.Remove(new DependencyResolutionLocatorKey(serviceType, null));

				foreach (KeyValuePair<object, object> kvp in _locator)
				{
					if (ReferenceEquals(kvp.Value, serviceInstance))
					{
						isLastInstance = false;
						break;
					}
				}

				if (isLastInstance)
				{
					_builder.TearDown(_locator, serviceInstance);
					_container.Remove(serviceInstance);

					if (!(serviceInstance is DemandAddPlaceholder))
						OnRemoved(serviceInstance);
				}
			}
		}

		#endregion

		/// <summary>
		/// Fired whenever a new service is added to the container. For demand-add services, the event is
		/// fired when the service is eventually created.
		/// </summary>
		public event EventHandler<DataEventArgs<object>> Added;

		/// <summary>
		/// Fired whenever a service is removed from the container. When the services collection is disposed,
		/// the Removed event is not fired for the services in the container.
		/// </summary>
		public event EventHandler<DataEventArgs<object>> Removed;

		private bool Contains(Type serviceType, SearchMode searchMode)
		{
			Guard.ArgumentNotNull(serviceType, "serviceType");

			return _locator.Contains(new DependencyResolutionLocatorKey(serviceType, null), searchMode);
		}

		/// <summary>
		/// Called when a new service is added to the collection.
		/// </summary>
		/// <param name="service">The service that was added.</param>
		protected virtual void OnAdded(object service)
		{
			if (Added != null)
			{
				Added(this, new DataEventArgs<object>(service));
			}
		}

		/// <summary>
		/// Called when a service is removed from the collection.
		/// </summary>
		/// <param name="service">The service that was removed.</param>
		protected virtual void OnRemoved(object service)
		{
			if (Removed != null)
			{
				Removed(this, new DataEventArgs<object>(service));
			}
		}

		private object Build(Type typeToBuild, Type typeToRegisterAs, object serviceInstance)
		{
			Guard.TypeIsAssignableFromType(typeToRegisterAs, typeToBuild, "typeToBuild");

			if (_locator.Contains(new DependencyResolutionLocatorKey(typeToRegisterAs, null), SearchMode.Local))
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, Resources.DuplicateService, typeToRegisterAs.FullName));

			if (serviceInstance == null)
				serviceInstance = BuildFirstTimeItem(typeToBuild, typeToRegisterAs, null);
			else if (!_container.Contains(serviceInstance))
				serviceInstance = BuildFirstTimeItem(typeToBuild, typeToRegisterAs, serviceInstance);
			else
				BuildRepeatedItem(typeToRegisterAs, serviceInstance);

			return serviceInstance;
		}

		/// <summary>
		/// Used to build a first time item (either an existing one or a new one). The Builder will
		/// end up locating it, and we add it to the container.
		/// </summary>
		private object BuildFirstTimeItem(Type typeToBuild, Type typeToRegisterAs, object item)
		{
			item = _builder.BuildUp(_locator, typeToBuild, null, item);

			if (typeToRegisterAs != typeToBuild)
			{
				_locator.Add(new DependencyResolutionLocatorKey(typeToRegisterAs, null), item);
				_locator.Remove(new DependencyResolutionLocatorKey(typeToBuild, null));
			}

			OnAdded(item);
			return item;
		}

		/// <summary>
		/// Used to "build" an item we've already seen once. We don't use the builder, because that
		/// would do double-injection. Since it's already in the lifetime container, all we need to
		/// do is add a second locator registration for it for the right name.
		/// </summary>
		private void BuildRepeatedItem(Type typeToRegisterAs, object item)
		{
			_locator.Add(new DependencyResolutionLocatorKey(typeToRegisterAs, null), item);
		}

		#region Nested type: DemandAddPlaceholder

		private class DemandAddPlaceholder
		{
			private Type _typeToCreate;

			public DemandAddPlaceholder(Type typeToCreate)
			{
				_typeToCreate = typeToCreate;
			}

			public Type TypeToCreate
			{
				get { return _typeToCreate; }
			}
		}

		#endregion
	}
}