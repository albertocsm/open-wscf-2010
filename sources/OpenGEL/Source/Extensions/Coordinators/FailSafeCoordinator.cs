//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System;
using System.Globalization;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Extensions.Coordinators;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;
using Config = Microsoft.Practices.RecipeFramework.Configuration;

namespace Microsoft.Practices.RecipeFramework.Extensions.Coordinators
{
    /// <summary>
    /// Action coordination that will stop execution without showing up an error dialog and will send 
    /// the error description to the Output window.
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IActionExecutionService))]
    [ServiceDependency(typeof(IConfigurationService))]
    public class FailSafeCoordinator : SitedComponent, IActionCoordinationService
    {
		/// <summary>
		/// Runs the coordination using the configuration data specified in the configuration file.
		/// </summary>
		/// <param name="declaredActions">Actions defined in the package configuration file for the currently executing recipe.</param>
		/// <param name="coordinationData">The configuration data used to setup the coordination.</param>
		public void Run(Dictionary<string, Config.Action> declaredActions, XmlElement coordinationData)
		{
			IActionExecutionService exec = GetService<IActionExecutionService>(true);
			string currentAction = null;

			try
			{
				foreach (Config.Action action in declaredActions.Values)
				{
					currentAction = action.Name;
					exec.Execute(action.Name);
				}
			}
			catch (Exception e)
			{
				IConfigurationService config = GetService<IConfigurationService>(true);
				DteHelperEx.ShowMessageInOutputWindow(
					GetService<DTE>(true),
					string.Format(CultureInfo.CurrentCulture,
						Properties.Resources.FailSafeCoordinatorExceptionMessage,
						config.CurrentRecipe.Caption, config.CurrentRecipe.Name, currentAction, e.Message),
					DteHelperEx.GetPackageFriendlyName(this.Site));
			}
		}
	}
}
