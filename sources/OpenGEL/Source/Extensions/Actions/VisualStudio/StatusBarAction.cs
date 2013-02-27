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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework;
using System.Globalization;

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.VisualStudio
{
	/// <summary>
	/// Updates the VS status bar object.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public abstract class StatusBarAction : ConfigurableAction
	{
		DTE visualStudio;

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
		/// Updates the status.
		/// </summary>
		/// <param name="text">The text.</param>
		protected void UpdateStatus(string text)
		{
			UpdateStatus(text, null);
		}

		/// <summary>
		/// Updates the status.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="args">The args.</param>
		protected void UpdateStatus(string text, params object[] args)
		{
			if (args != null)
			{
				Trace.TraceInformation(text, args);
				visualStudio.StatusBar.Text = String.Format(CultureInfo.CurrentCulture, text, args);
			}
			else
			{
				Trace.TraceInformation(text);
				visualStudio.StatusBar.Text = text;
			}
		}

		/// <summary>
		/// Updates the status warning.
		/// </summary>
		/// <param name="text">The text.</param>
		protected void UpdateStatusWarning(string text)
		{
			UpdateStatusWarning(text, null);
		}

		/// <summary>
		/// Updates the status warning.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="args">The args.</param>
		protected void UpdateStatusWarning(string text, params object[] args)
		{
			if (args != null)
			{
				Trace.TraceWarning(text, args);
				visualStudio.StatusBar.Text = String.Format(CultureInfo.CurrentCulture, text, args);
			}
			else
			{
				Trace.TraceWarning(text);
				visualStudio.StatusBar.Text = text;
			}
		}

		/// <summary>
		/// Progresses the specified in progress.
		/// </summary>
		/// <param name="inProgress">if set to <c>true</c> [in progress].</param>
		/// <param name="label">The label.</param>
		/// <param name="amountCompleted">The amount completed.</param>
		/// <param name="total">The total.</param>
		protected void Progress(bool inProgress, string label, int amountCompleted, int total)
		{
			visualStudio.StatusBar.Progress(inProgress, label, amountCompleted, total);
		}

		/// <summary>
		/// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
		/// </summary>
		public override sealed void Execute()
		{
			try
			{
				OnExecute();
			}
			catch (Exception)
			{
				UpdateStatusWarning(Properties.Resources.StatusBarReadyMessage);
				throw;
			}
		}

		/// <summary>
		/// Called when [execute].
		/// </summary>
		protected virtual void OnExecute() { }

		/// <summary>
		/// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
		/// </summary>
		public override sealed void Undo()
		{
		}

		/// <summary>
		/// Called when [undo].
		/// </summary>
		protected virtual void OnUndo() { }
	}
}

