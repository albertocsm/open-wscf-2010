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
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.RecipeFramework.Extensions.Services
{
    /// <summary>
    /// Logs events to the output window.
    /// </summary>
    public class OutputWindowTraceListener : TraceListener
    {
        private IOutputWindowService outputWindowService;
        private TraceSwitch traceSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OutputWindowTraceListener"/> class.
        /// </summary>
        /// <param name="outputWindow">The output window.</param>
        /// <param name="traceSwitch">The trace switch.</param>
        public OutputWindowTraceListener(IOutputWindowService outputWindow, TraceSwitch traceSwitch)
        {
            Guard.ArgumentNotNull(outputWindow, "outputWindow");
            Guard.ArgumentNotNull(traceSwitch, "traceSwitch");

            this.traceSwitch = traceSwitch;
            this.outputWindowService = outputWindow;
        }

        /// <summary>
        /// Traces the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public override void TraceData(
            TraceEventCache eventCache, 
            string source, 
            TraceEventType severity, 
            int id, 
            object data)
        {
            if (!this.DoNotLog(severity, this.traceSwitch.Level))
            {
                this.WritePrefix(severity);
                base.TraceData(eventCache, string.Empty, severity, id, data);
            }
        }

        /// <summary>
        /// Traces the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public override void TraceData(
            TraceEventCache eventCache,
            string source,
            TraceEventType severity,
            int id,
            params object[] data)
        {
            if (!this.DoNotLog(severity, this.traceSwitch.Level))
            {
                this.WritePrefix(severity);
                base.TraceData(eventCache, string.Empty, severity, id, data);
            }
        }

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="id">The id.</param>
        /// <param name="message">The message.</param>
        public override void TraceEvent(
            TraceEventCache eventCache,
            string source,
            TraceEventType severity,
            int id,
            string message)
        {
            if (!this.DoNotLog(severity, this.traceSwitch.Level))
            {
                this.WritePrefix(severity);
                base.TraceEvent(eventCache, string.Empty, severity, id, message);
            }
        }

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="id">The id.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public override void TraceEvent(
            TraceEventCache eventCache,
            string source,
            TraceEventType severity,
            int id,
            string format,
            params object[] args)
        {
            if (!this.DoNotLog(severity, this.traceSwitch.Level))
            {
                this.WritePrefix(severity);
                base.TraceEvent(eventCache, string.Empty, severity, id, format, args);
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            if (this.traceSwitch.Level != TraceLevel.Off)
            {
                this.outputWindowService.Display(message);
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            if (this.traceSwitch.Level != TraceLevel.Off)
            {
                this.outputWindowService.Display(message);
                this.outputWindowService.Display(Environment.NewLine);
            }
        }
        
        private void WritePrefix(TraceEventType severity)
        {
            this.Write(new string(' ', base.IndentLevel * base.IndentSize));
            if (severity == TraceEventType.Warning)
            {
                this.Write("(!)");
            }
            else if (severity == TraceEventType.Error)
            {
                this.Write("(*)");
            }
        }

        private bool DoNotLog(TraceEventType severity, TraceLevel level)
        {
            TraceLevel newLevel;
            switch (severity)
            {
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    newLevel = TraceLevel.Error;
                    break;
                case TraceEventType.Warning:
                    newLevel = TraceLevel.Warning;
                    break;
                case TraceEventType.Verbose:
                    newLevel = TraceLevel.Verbose;
                    break;
                default:
                    newLevel = TraceLevel.Info;
                    break;
            }

            return (newLevel > level);
        }
    }
}
