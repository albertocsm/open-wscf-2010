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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.UI;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.BuilderStrategies;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;
using HttpContext=Microsoft.Practices.CompositeWeb.Web.HttpContext;

namespace Microsoft.Practices.CompositeWeb
{
	/// <summary>
	/// Defines the application class for a composite web application.
	/// </summary>
	public class WebClientApplication : HttpApplication, IWebClientApplication
	{
		private static WCSFBuilder _applicationBuilder;
		private static IHttpContext _currentContext;
		private static object _lockObject = new object();
		private static WCSFBuilder _pageBuilder;
		private static CompositionContainer _rootContainer;

		/// <summary>
		/// Used for testability
		/// </summary>
		protected static IHttpContext CurrentContext
		{
			get { return _currentContext ?? new HttpContext(System.Web.HttpContext.Current); }
			set { _currentContext = value; }
		}

		#region IWebClientApplication Members

		/// <summary>
		/// Gets the <see cref="IBuilder{TStageEnum}"/> used by the application and its modules.
		/// </summary>
		public virtual IBuilder<WCSFBuilderStage> ApplicationBuilder
		{
			get { return _applicationBuilder; }
		}

		/// <summary>
		/// Gets the <see cref="IBuilder{TStageEnum}"/> used to build the requested pages.
		/// </summary>
		public virtual IBuilder<WCSFBuilderStage> PageBuilder
		{
			get { return _pageBuilder; }
		}

		/// <summary>
		/// Gets and sets the root <see cref="CompositionContainer"/>
		/// </summary>
		public virtual CompositionContainer RootContainer
		{
			get { return _rootContainer; }

			protected set { _rootContainer = value; }
		}

		#endregion

		/// <summary>
		/// Handles the <see cref="HttpApplication.PreRequestHandlerExecute"/> event.
		/// </summary>
		/// <param name="sender">The object firing the event.</param>
		/// <param name="e">The event associated data.</param>
		/// <remarks>Handles the <see cref="HttpApplication.PreRequestHandlerExecute"/> event and calls the InnerPreRequestHandlerExecute method, 
		/// that can be overridden to change how to handle web requests or add extra behavior.</remarks>
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected virtual void Application_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			System.Web.HttpContext context = ((HttpApplication) sender).Context;
			InnerPreRequestHandlerExecute(new HttpContext(context));
		}

		/// <summary>
		/// Handles the <see cref="HttpApplication.PostRequestHandlerExecute"/> event.
		/// </summary>
		/// <param name="sender">The object firing the event.</param>
		/// <param name="e">The event associated data.</param>
		/// <remarks>Handles the <see cref="HttpApplication.PostRequestHandlerExecute"/> event and calls the InnerPostRequestHandlerExecute method,
		/// that can be overridden to add extra after the handler was executed.</remarks>
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected virtual void Application_PostRequestHandlerExecute(object sender, EventArgs e)
		{
			System.Web.HttpContext context = ((HttpApplication) sender).Context;
			InnerPostRequestHandlerExecute(new HttpContext(context));
		}

		/// <summary>
		/// Performs the setup operations needed before executing the request handler.
		/// </summary>
		/// <param name="context">The <see cref="IHttpContext"/> on which the request is being processed.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		protected void InnerPreRequestHandlerExecute(IHttpContext context)
		{
			Guard.ArgumentNotNull(context, "context");
			Page page = context.Handler as Page;
			if (page != null)
			{
				PrePageExecute(page);
			}
		}

		/// <summary>
		/// Performs the operation needed after executing the request handler.
		/// </summary>
		/// <param name="context">The <see cref="IHttpContext"/> on which the request is being processed.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class.")]
		protected void InnerPostRequestHandlerExecute(IHttpContext context)
		{
			Guard.ArgumentNotNull(context, "context");
			if (context.Handler is Page)
			{
				PostPageExecute(context.Handler as Page);
			}
		}

		/// <summary>
		/// Override this method to add the desired behavior to execute before the requested <see cref="Page"/> handler is executed.
		/// </summary>
		/// <param name="page">The requested <see cref="Page"/>.</param>
		protected virtual void PrePageExecute(Page page)
		{
		}

		/// <summary>
		/// Override this method to add the desired behavior to execute after the requested <see cref="Page"/> handler is executed.
		/// </summary>
		/// <param name="page">The requested <see cref="Page"/>.</param>
		protected virtual void PostPageExecute(Page page)
		{
		}

		/// <summary>
		/// Returns an <see cref="CompositionContainer"/> for the module handling the current request.
		/// </summary>
		/// <param name="context">The <see cref="IHttpContext"/> on which the request is being processed.</param>
		/// <returns>An <see cref="CompositionContainer"/> object.</returns>
		protected virtual CompositionContainer GetModuleContainer(IHttpContext context)
		{
			IModuleContainerLocatorService locatorService = RootContainer.Services.Get<IModuleContainerLocatorService>();
			return locatorService.GetContainer(context.Request.AppRelativeCurrentExecutionFilePath);
		}

		/// <summary>
		/// Handles the <see cref="Start"/> event and defines the application lifecycle.
		/// </summary>
		/// <param name="sender">The object firing the event.</param>
		/// <param name="e">The event associated data.</param>
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected virtual void Application_Start(object sender, EventArgs e)
		{
			// Application lifecycle
			CreateApplicationBuilder();
			CreatePageBuilder();
			AddBuilderStrategies(ApplicationBuilder);
			AddBuilderStrategies(PageBuilder);
			CreateRootContainer();
			AddRequiredServices();
			LoadModules();
			ConfigureModules();
			Start();
		}

		/// <summary>
		/// Adds the required builder strategies to the specified builder.
		/// </summary>
		/// <param name="builder">An <see cref="IBuilder{BuilderStage}"/> instance.</param>
		/// <remarks>Override this method to add or change the used strategies.</remarks>
		protected virtual void AddBuilderStrategies(IBuilder<WCSFBuilderStage> builder)
		{
			builder.Strategies.AddNew<ContainerAwareTypeMappingStrategy>(WCSFBuilderStage.TypeMapping);
			builder.Policies.SetDefault<IContainerAwareTypeMappingPolicy>(
				new ContainerAwareTypeMappingPolicy());
			builder.Strategies.AddNew<SessionStateBindingStrategy>(WCSFBuilderStage.Initialization);
		}

		/// <summary>
		/// Adds the required application services to the root container.
		/// </summary>
		/// <remarks>Override this method to add or change the services available in the root container.</remarks>
		protected virtual void AddRequiredServices()
		{
			AddServiceIfMissing<ModuleConfigurationLocatorService, IModuleConfigurationLocatorService>(RootContainer);
			AddServiceIfMissing<VirtualPathUtilityService, IVirtualPathUtilityService>(RootContainer);
			AddServiceIfMissing<AuthorizationRulesService, IAuthorizationRulesService>(RootContainer);
			AddServiceIfMissing<SessionStateLocatorService, ISessionStateLocatorService>(RootContainer);
			AddServiceIfMissing<HttpContextLocatorService, IHttpContextLocatorService>(RootContainer);
			AddServiceIfMissing<ServiceLoaderService, IServiceLoaderService>(RootContainer);
			AddServiceIfMissing<ModuleLoaderService, IModuleLoaderService>(RootContainer);
			AddServiceIfMissing<WebConfigModuleInfoStore, IModuleInfoStore>(RootContainer);
			AddServiceIfMissing<WebModuleEnumerator, IModuleEnumerator>(RootContainer);
			AddServiceIfMissing<ModuleContainerLocatorService, IModuleContainerLocatorService>(RootContainer);
		}

		/// <summary>
		/// Utility method to add a service to the container only if that service is not already in the container.
		/// </summary>
		/// <typeparam name="TService">The type implementing the service.</typeparam>
		/// <typeparam name="TRegisterAs">The type of service to register.</typeparam>
		/// <param name="container">The container where to add the service.</param>
		/// <returns>The added servie instance on <see langword="null"/> if the service was not added.</returns>
		protected static TService AddServiceIfMissing<TService, TRegisterAs>(CompositionContainer container)
			where TService : TRegisterAs
		{
			if (!container.Services.Contains<TRegisterAs>())
			{
				return container.Services.AddNew<TService, TRegisterAs>();
			}
			return default(TService);
		}

		/// <summary>
		/// Creates the <see cref="IBuilder{TStageEnum}"/> to be used by the application and its modules.
		/// </summary>
		/// <remarks>Override this method to change the builder to use and/or the used policies.</remarks>
		protected virtual void CreateApplicationBuilder()
		{
			if (ApplicationBuilder == null)
			{
				lock (_lockObject)
				{
					if (ApplicationBuilder == null)
					{
						_applicationBuilder = new WCSFBuilder();
						ApplicationBuilder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
					}
				}
			}
		}

		/// <summary>
		/// Creates the <see cref="IBuilder{TStageEnum}"/> to be used to build the requested page handlers.
		/// </summary>
		/// <remarks>Override this method to change the builder to use and/or the used policies.</remarks>
		protected virtual void CreatePageBuilder()
		{
			if (PageBuilder == null)
			{
				lock (_lockObject)
				{
					if (PageBuilder == null)
					{
						_pageBuilder = new WCSFBuilder();
						PageBuilder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(false));
					}
				}
			}
		}

		/// <summary>
		/// Creates the application root container.
		/// </summary>
		/// <remarks>Override this method to change the root container to be used by the application.</remarks>
		protected virtual void CreateRootContainer()
		{
			if (RootContainer == null)
			{
				lock (_lockObject)
				{
					if (RootContainer == null)
					{
						CompositionContainer container = new CompositionContainer();
						container.InitializeRootContainer(ApplicationBuilder);
						RootContainer = container;
					}
				}
			}
		}

		/// <summary>
		/// Loads the application modules into the root container.
		/// </summary>
		protected virtual void LoadModules()
		{
			IModuleLoaderService loader = RootContainer.Services.Get<IModuleLoaderService>(true);
			IModuleEnumerator moduleEnumerator = RootContainer.Services.Get<IModuleEnumerator>(true);

			if (moduleEnumerator != null)
			{
				loader.Load(RootContainer, moduleEnumerator.EnumerateModules());
			}
		}

		/// <summary>
		/// Searches for specific module configuration and configures each module when that configuration is available.
		/// </summary>
		protected virtual void ConfigureModules()
		{
			IModuleEnumerator moduleEnumerator = RootContainer.Services.Get<IModuleEnumerator>();
			IModuleLoaderService moduleLoader = RootContainer.Services.Get<IModuleLoaderService>();
			IModuleConfigurationLocatorService moduleConfigurationLocator =
				RootContainer.Services.Get<IModuleConfigurationLocatorService>();
			if (moduleEnumerator != null)
			{
				foreach (IModuleInfo moduleInfo in moduleEnumerator.EnumerateModules())
				{
					System.Configuration.Configuration configuraton =
						moduleConfigurationLocator.FindModuleConfiguration(moduleInfo.Name);
					if (configuraton != null)
					{
						moduleLoader.FindInitializer(moduleInfo.Name).Configure(RootContainer.Services, configuraton);
					}
				}
			}
		}


		/// <summary>
		/// Override this methos to add behavior to be executed once the application has started.
		/// </summary>
		protected virtual void Start()
		{
		}


		//TODO: this code should be refactored, and probably moved to another utility class


		/// <summary>
		/// Utility method to build up an object without adding it to the container.
		/// It uses the application's PageBuilder and the <see cref="CompositionContainer"/> for the module handling the current request
		/// </summary>
		/// <param name="obj">The object to build.</param>
		public static void BuildItemWithCurrentContext(object obj)
		{
			IHttpContext context = CurrentContext;
			WebClientApplication app = (WebClientApplication) context.ApplicationInstance;
			IBuilder<WCSFBuilderStage> builder = app.PageBuilder;
			CompositionContainer container = app.GetModuleContainer(context);

			CompositionContainer.BuildItem(builder, container.Locator, obj);
		}
	}
}