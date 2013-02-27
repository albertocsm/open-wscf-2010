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
using System.Text;
using System.Web.UI;
using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;

[assembly: System.Web.UI.WebResource("RealTimeSearch.RealTimeSearchBehavior.js", "text/javascript")]

namespace RealTimeSearch {
    [ParseChildren(true)]
    [PersistChildren(false)]    
    [Designer(typeof(RealTimeSearchDesigner))]
    public class RealTimeSearchMonitor : Control , IScriptControl {

        public IEnumerable<ScriptDescriptor> GetScriptDescriptors() {
            yield return GetRealTimeSearchMonitorScriptDescriptor;
            yield break;
        }

        public IEnumerable<ScriptReference> GetScriptReferences() {
            yield return new ScriptReference("RealTimeSearch.RealTimeSearchBehavior.js", typeof(RealTimeSearchMonitor).Assembly.FullName);
            yield break;
        }

        [Description("Indicates how often the input control is checked to see if the value has changed.")]
        [DefaultValue(500)]
        public int Interval {
            get { return GetPropertyValue<int>("Interval", 500); }
            set { SetPropertyValue<int>("Interval", value); }
        }

        [Description("Indicates the input controls that are to be monitored.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        [DefaultValue((string)null)]
        public virtual Collection<ControlMonitorParameter> ControlsToMonitor {
            get {
                if (GetPropertyValue<Collection<ControlMonitorParameter>>("ControlsToMonitor", null) == null) {
                    SetPropertyValue<Collection<ControlMonitorParameter>>("ControlsToMonitor", new ControlMonitorParameterCollection(this));
                }
                return GetPropertyValue<Collection<ControlMonitorParameter>>("ControlsToMonitor", null);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member"), Description("Indicates the associated UpdatePanel control.")]
        [TypeConverter("System.Web.UI.Design.UpdateProgressAssociatedUpdatePanelIDConverter")]
        [DefaultValue("")]
        [IDReferenceProperty(typeof(UpdatePanel))]
        public string AssociatedUpdatePanelID {
            get { return GetPropertyValue<string>("AssociatedUpdatePanelID", string.Empty); }
            set { SetPropertyValue<string>("AssociatedUpdatePanelID", value); }
        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            UpdatePanel associatedUpdatePanel = FindControl(AssociatedUpdatePanelID) as UpdatePanel;

            #region Add triggers to target update panel
            if (!DesignMode && associatedUpdatePanel != null){
                // triggers need to be added now since 
                // UpdatePanels use that property in OnLoad
                foreach (ControlMonitorParameter cmp in ControlsToMonitor) {                    
                    AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                    trigger.ControlID = cmp.TargetID;
                    if (!string.IsNullOrEmpty(cmp.EventName)) {
                        trigger.EventName = cmp.EventName;
                    }
                    associatedUpdatePanel.Triggers.Add(trigger);
                }
            }
            #endregion
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            this.ScriptManager.RegisterScriptControl<RealTimeSearchMonitor>(this);
        }

        protected override void Render(HtmlTextWriter writer) {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();

            this.ScriptManager.RegisterScriptDescriptors(this);
        }

        protected virtual ScriptDescriptor GetRealTimeSearchMonitorScriptDescriptor
        {
            get
            {
                ScriptComponentDescriptor s = new ScriptControlDescriptor("RealTimeSearch.RealTimeSearchBehavior", this.ClientID);
                s.AddProperty("interval", Interval);

                List<ControlMonitorInfo> resolved_ControlsToMonitor = new List<ControlMonitorInfo>();
                foreach (ControlMonitorParameter cmp in ControlsToMonitor)
                {
                    resolved_ControlsToMonitor.Add(ResolveControlMonitorParameter(cmp));
                }
                s.AddProperty("controlsToMonitor", resolved_ControlsToMonitor);

                return s;
            }
        }

        protected virtual ControlMonitorInfo ResolveControlMonitorParameter(ControlMonitorParameter cmp) {
            ControlMonitorInfo result = new ControlMonitorInfo();
            Control targetControl = FindControl(cmp.TargetID);
            result.TargetId = targetControl.ClientID;
            result.Callback = Page.ClientScript.GetPostBackClientHyperlink(targetControl, "");

            ICollection<string> validatorsToResolve = cmp.ResolveValidators(Page);
            foreach (string validatorToResolve in validatorsToResolve) {
                result.Validators.Add(FindControl(validatorToResolve).ClientID);
            }
            return result;
        }

        protected virtual ScriptManager ScriptManager {
            get {
                ScriptManager current = ScriptManager.GetCurrent(this.Page);
                if (current == null) {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture,
                        "The control with ID '{0}' requires a ScriptManager on the page. The ScriptManager must appear before any controls that need it.",
                        this.ID));
                }
                return current;
            }
        }

        protected V GetPropertyValue<V>(string propertyName, V nullValue) {
            if (this.ViewState[propertyName] == null) {
                return nullValue;
            }
            return (V)this.ViewState[propertyName];
        }

        protected void SetPropertyValue<V>(string propertyName, V value) {
            this.ViewState[propertyName] = value;
        }
    }
}
