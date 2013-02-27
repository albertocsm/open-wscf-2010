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
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Collections;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Defines anCompositionContainer container for services and managed objects.
	/// </summary>
	public class CompositionContainer : IDisposable
	{
		private IBuilder<WCSFBuilderStage> _builder;
		private IManagedObjectCollection<CompositionContainer> _containerCollection;
		private ILifetimeContainer _lifetime = new LifetimeContainer();
		private IReadWriteLocator _locator;
		private CompositionContainer _parent;

		private IServiceCollection _serviceCollection;
		private Dictionary<Type, Type> _typeMappings;

		/// <summary>
		/// Gets or sets the parent container for this container instance.
		/// </summary>
		[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)]
		public CompositionContainer Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		/// <summary>
		/// Gets the root container instance
		/// </summary>
		public CompositionContainer RootContainer
		{
			get
			{
				CompositionContainer currentContainer = this;

				while (currentContainer.Parent != null)
				{
					currentContainer = currentContainer.Parent;
				}

				return currentContainer;
			}
		}

		/// <summary>
		/// Gets the <see cref="IBuilder{TStageEnum}"/> used by this container.
		/// </summary>
		public IBuilder<WCSFBuilderStage> Builder
		{
			get { return _builder; }
		}

		/// <summary>
		/// Gets the <see cref="IReadWriteLocator"/> used by this container.
		/// </summary>
		public IReadWriteLocator Locator
		{
			get { return _locator; }
		}

		/// <summary>
		/// Returns a collection describing the child <see cref="CompositionContainer"/> objects in this Container.
		/// </summary>
		public IManagedObjectCollection<CompositionContainer> Containers
		{
			get { return _containerCollection; }
		}

		/// <summary>
		/// Returns the service collection for this container.
		/// </summary>
		public IServiceCollection Services
		{
			get { return _serviceCollection; }
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes the <see cref="CompositionContainer"/> instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Event fired when a managed object is added to the container.
		/// </summary>
		public event EventHandler<DataEventArgs<object>> ObjectAdded;

		/// <summary>
		/// Event fired when a managed object is removed from the container.
		/// </summary>
		public event EventHandler<DataEventArgs<object>> ObjectRemoved;

		/// <summary>
		/// Add a type mapping to this container. When the container is asked
		/// to build an instance of type TRequested, it'll actually build an instance
		/// of TReturned.
		/// </summary>
		/// <typeparam name="TRequested"><see cref="System.Type"/> that will be requested from the container.</typeparam>
		/// <typeparam name="TReturned">The type of instance to create. TReturned must
		/// be assignable to a variable of type TRequested.</typeparam>
		public void RegisterTypeMapping<TRequested, TReturned>() where TReturned : TRequested
		{
			RegisterTypeMapping(typeof (TRequested), typeof (TReturned));
		}

		/// <summary>
		/// Add a type mapping to this container. When the container is asked
		/// to build an instance of type requested, it'll actually build an instance
		/// of returned.
		/// </summary>
		/// <param name="requested"><see cref="System.Type"/> that will be requested from the container.</param>
		/// <param name="returned">The type of instance that is actually created. objects of
		/// the returned type must be assignable to variables of the requested type.</param>
		public void RegisterTypeMapping(Type requested, Type returned)
		{
			Guard.TypeIsAssignableFromType(requested, returned, "returned");
			_typeMappings[requested] = returned;
		}

		/// <summary>
		/// Return the type mapped to the given requested type. Will check this container,
		/// and any parent containers, for a mapping for the requested type.
		/// </summary>
		/// <remarks>If there is no mapping, the method returns TRequested.</remarks>
		/// <typeparam name="TRequested">Type to find the mapping for.</typeparam>
		/// <returns>The registered mapped type. Returns TRequested if there is no mapping.</returns>
		public Type GetMappedType<TRequested>()
		{
			return GetMappedType(typeof (TRequested));
		}

		/// <summary>
		/// Return the type mapped to the given requested type. Will check this container,
		/// and any parent containers, for a mapping for the requested type.
		/// </summary>
		/// <param name="requested">Type to find the mapping for.</param>
		/// <returns>The registered mapped type. Returns <paramref name="requested"/> if there is no mapping.</returns>
		public Type GetMappedType(Type requested)
		{
			if (_typeMappings.ContainsKey(requested))
			{
				return _typeMappings[requested];
			}
			if (_parent != null)
			{
				return _parent.GetMappedType(requested);
			}
			return requested;
		}

		/// <summary>
		/// Utility method to initialize an object instance without adding it to the container.
		/// </summary>
		/// <param name="builder">The <see cref="IBuilder{TStageEnum}"/> to use to build the object.</param>
		/// <param name="locator">The <see cref="IReadWriteLocator"/> to use in building the object.</param>
		/// <param name="item">The object to build.</param>
		/// <returns>The built and initialized object.</returns>
		public static object BuildItem(IBuilder<WCSFBuilderStage> builder, IReadWriteLocator locator, object item)
		{
			Type itemType = item.GetType();
			string temporaryId = Guid.NewGuid().ToString();

			PolicyList policies = new PolicyList();
			policies.Set<ISingletonPolicy>(new SingletonPolicy(false), itemType, temporaryId);
			policies.Set<ICreationPolicy>(new DefaultCreationPolicy(), itemType, temporaryId);
			policies.Set<IPropertySetterPolicy>(new PropertySetterPolicy(), itemType, temporaryId);

			return builder.BuildUp(locator, itemType, temporaryId, item, policies);
		}

		/// <summary>
		/// Builds a new instance of <paramref name="typeOfItem"/>.
		/// </summary>
		/// <param name="typeOfItem">The <see cref="Type"/> of the object to create and build.</param>
		/// <returns>The new and built object instance.</returns>
		public object BuildNewItem(Type typeOfItem)
		{
			string temporaryID = Guid.NewGuid().ToString();

			PolicyList policies = new PolicyList();
			policies.Set<ISingletonPolicy>(new SingletonPolicy(false), typeOfItem, temporaryID);
			policies.Set<ICreationPolicy>(new DefaultCreationPolicy(), typeOfItem, temporaryID);
			policies.Set<IPropertySetterPolicy>(new PropertySetterPolicy(), typeOfItem, temporaryID);

			return _builder.BuildUp(
				_locator,
				typeOfItem,
				temporaryID,
				null,
				policies);
		}

		/// <summary>
		/// Disposes the object.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> when the user code is disposing; 
		/// <see langword="false"/> when it is the GC.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_builder.TearDown(_locator, this);

				if (_parent != null)
				{
					List<object> ids = new List<object>();

					foreach (KeyValuePair<object, object> pair in _parent.Locator)
						if (pair.Value == this)
							ids.Add(pair.Key);

					foreach (object id in ids)
						_parent.Locator.Remove(id);

					_parent._lifetime.Remove(this);
				}

				_lifetime.Remove(this);

				List<object> lifetimeObjects = new List<object>();
				lifetimeObjects.AddRange(_lifetime);

				foreach (object obj in lifetimeObjects)
					if (_lifetime.Contains(obj))
						_builder.TearDown(_locator, obj);

				_lifetime.Dispose();
			}
		}

		/// <summary>
		/// Initializes the <see cref="CompositionContainer"/> when it is the root of the containment hierarchy.
		/// </summary>
		/// <param name="builder"></param>
		public void InitializeRootContainer(IBuilder<WCSFBuilderStage> builder)
		{
			_builder = builder;
			_locator = new Locator();

			InitializeFields();
			InitializeCollectionFacades();
		}

		/// <summary>
		/// Injection method used to initialize the container instance.
		/// </summary>
		[InjectionMethod]
		public void InitializeContainer()
		{
			InitializeFields();
			InitializeCollectionFacades();
		}

		/// <summary>
		/// Fires the <see cref="ObjectRemoved"/> event.
		/// </summary>
		protected virtual void OnObjectAdded(object item)
		{
			if (ObjectAdded != null)
			{
				ObjectAdded(this, new DataEventArgs<object>(item));
			}
		}

		/// <summary>
		/// Fires the <see cref="ObjectRemoved"/> event.
		/// </summary>
		protected virtual void OnObjectRemoved(object item)
		{
			if (ObjectRemoved != null)
			{
				ObjectRemoved(this, new DataEventArgs<object>(item));
			}
		}

		private void InitializeCollectionFacades()
		{
			if (_serviceCollection == null)
			{
				_serviceCollection = new ServiceCollection(
					_lifetime,
					_locator,
					_builder,
					_parent == null ? null : _parent.Services);
			}

			if (_containerCollection == null)
			{
				ManagedObjectCollection<CompositionContainer> parentContainers =
					(ManagedObjectCollection<CompositionContainer>) (_parent == null ? null : _parent.Containers);
				_containerCollection = new ManagedObjectCollection<CompositionContainer>(
					_lifetime,
					_locator,
					_builder,
					SearchMode.Local,
					null,
					null,
					parentContainers);
			}
			if (_typeMappings == null)
			{
				_typeMappings = new Dictionary<Type, Type>();
			}
		}

		private void InitializeFields()
		{
			if (_builder == null)
			{
				_builder = _parent.Builder;
			}

			if (_locator == null)
			{
				_locator = new Locator(_parent.Locator);
			}

			if (!_locator.Contains(typeof (ILifetimeContainer), SearchMode.Local))
			{
				_locator.Add(typeof (ILifetimeContainer), _lifetime);
			}

			LocateContainer(typeof (CompositionContainer));
			LocateContainer(GetType());
		}

		private void LocateContainer(Type containerType)
		{
			DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(containerType, null);

			if (!_locator.Contains(key, SearchMode.Local))
			{
				_locator.Add(key, this);
			}
		}
	}
}