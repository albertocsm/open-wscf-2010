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
using System.Xml;
using System.Text;
using System.Collections.Generic;
using EnvDTE;
using System.Diagnostics;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Configuration;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common;

namespace Microsoft.Practices.RecipeFramework.Extensions.Coordinators
{
	/// <summary>
	/// Action coordination for boolean conditions.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	[ServiceDependency(typeof(IActionExecutionService))]
	public class ConditionalCoordinator : SitedComponent, IActionCoordinationService
	{
		/// <summary>
		/// The condition attribute name
		/// </summary>
		public const string ConditionalAttributeName = "Condition";

		private DTE visualStudio;

		/// <summary>
		/// When implemented by a class, allows descendants to
		/// perform processing whenever the component is being sited.
		/// </summary>
		protected override void OnSited()
		{
			base.OnSited();
			visualStudio = GetService<DTE>(true);
		}

		/// <summary>
		/// Runs the coordination using the configuration data specified in the configuration file.
		/// </summary>
		/// <param name="declaredActions">Actions defined in the package configuration file for the currently executing recipe.</param>
		/// <param name="coordinationData">The configuration data used to setup the coordination.</param>
		public void Run(Dictionary<string, Microsoft.Practices.RecipeFramework.Configuration.Action> declaredActions, 
			XmlElement coordinationData)
		{
			IActionExecutionService exec = GetService<IActionExecutionService>(true);
			int amountCompleted = 0;

			try
			{
				foreach (Microsoft.Practices.RecipeFramework.Configuration.Action action in declaredActions.Values)
				{
					amountCompleted++;
					visualStudio.StatusBar.Progress(true, Properties.Resources.StatusBarProgressMessage, amountCompleted, declaredActions.Values.Count);

					bool execute = (action.AnyAttr == null || action.AnyAttr.Length == 0);

					if (!execute)
					{
						IDictionaryService dictservice = (IDictionaryService)GetService(typeof(IDictionaryService));
						ExpressionEvaluationService evaluator = new ExpressionEvaluationService();
						execute = true;
						foreach (XmlAttribute att in action.AnyAttr)
						{
							if (att.Name.Equals(ConditionalCoordinator.ConditionalAttributeName, StringComparison.InvariantCultureIgnoreCase))
							{
								try
								{
									execute = (bool)evaluator.Evaluate(att.Value, new ServiceAdapterDictionary(dictservice));
								}
								catch (Exception e)
								{
									execute = false;
									System.Diagnostics.Trace.TraceWarning(Properties.Resources.InvalidConditionException, e.Message, e.StackTrace);
								}
								break;
							}
						}
					}

					if (execute)
					{
						Trace.TraceInformation(Properties.Resources.ExecutingAction, action.Name);
						exec.Execute(action.Name);
					}
				}
			}
			finally
			{
				visualStudio.StatusBar.Progress(false, "", 0, 0);
			}
		}
	}
}
