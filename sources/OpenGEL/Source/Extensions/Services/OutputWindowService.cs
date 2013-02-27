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
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers;

namespace Microsoft.Practices.RecipeFramework.Extensions.Services
{
    /// <summary>
    /// Service that manages logging to the output window.
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IConfigurationService))]
    public class OutputWindowService : SitedComponent, IOutputWindowService
    {
        private TraceListener listener;
        private string name;
        private OutputWindowPane pane;
        private TraceSwitch traceSwitch;
        private OutputWindow outputWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OutputWindowService"/> class.
        /// </summary>
        public OutputWindowService()
        {
            // will initialize when OnSited to get the Output Window Name (from the current runnung package)
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OutputWindowService"/> class.
        /// </summary>
        /// <param name="outputWindowName">Name of the output window.</param>
        /// <param name="traceSwitch">The trace switch.</param>
        public OutputWindowService(string outputWindowName, TraceSwitch traceSwitch)
        {
            InitializeService(outputWindowName, traceSwitch);
        }

        /// <summary>
        /// When implemented by a class, allows descendants to
        /// perform processing whenever the component is being sited.
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            IConfigurationService configuration = this.GetService<IConfigurationService>(true);
            TraceSwitch traceSwitch = new TraceSwitch(configuration.CurrentPackage.Name, 
                configuration.CurrentPackage.Description);
            traceSwitch.Level = RecipeManager.TraceSwitch.Level;
            InitializeService(configuration.CurrentPackage.Caption, traceSwitch);
        }

        /// <summary>
        /// Displays the <paramref name="message"/> in the output window
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void Display(string message)
        {
            if (this.Pane != null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    // just show up the current pane
                    this.Pane.Activate();
                    this.outputWindow.Parent.Activate();
                }
                this.Pane.OutputString(message);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            Trace.Listeners.Remove(this.listener);
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the output window name.
        /// </summary>
        public string WindowName
        {
            get { return this.name; }
        }

        private void InitializeService(string outputWindowName, TraceSwitch traceSwitch)
        {
            this.name = outputWindowName;
            this.traceSwitch = traceSwitch;
            this.listener = new OutputWindowTraceListener(this, traceSwitch);
            Trace.Listeners.Add(this.listener);
        }

        private OutputWindowPane Pane
        {
            get
            {
                if (this.pane == null)
                {
                    DTE dte = this.GetService<DTE>(false);
                    if (dte != null)
                    {
                        this.outputWindow = ((EnvDTE80.DTE2)dte).ToolWindows.OutputWindow;
                        this.pane = GetPane(outputWindow, this.name);
                    }
                }
                return this.pane;
            }
        }

        private static OutputWindowPane GetPane(OutputWindow outputWindow, string panelName)
        {
            if (!string.IsNullOrEmpty(panelName))
            {
                foreach (OutputWindowPane pane in outputWindow.OutputWindowPanes)
                {
                    if (pane.Name.Equals(panelName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return pane;
                    }
                }
                return outputWindow.OutputWindowPanes.Add(panelName);
            }
            return outputWindow.ActivePane;
        }
    }
}
