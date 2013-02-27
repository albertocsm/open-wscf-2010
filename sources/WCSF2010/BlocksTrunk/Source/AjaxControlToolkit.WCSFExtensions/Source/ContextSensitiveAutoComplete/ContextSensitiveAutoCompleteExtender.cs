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
using System.ComponentModel;
using System.Web.UI;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
[assembly: WebResource("AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoComplete.ContextSensitiveAutoCompleteBehavior.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.PopupControl.PopupControlBehavior.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.PopupControl.AutoCompleteBehavior.js", "text/javascript")]

namespace AjaxControlToolkit.WCSFExtensions
{
    /// <summary>
    /// Extends the AutocompleteExtender providing context sensitive filtering.
    /// </summary>
	[Designer("AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteDesigner, AjaxControlToolkit.WCSFExtensions")]
	[ParseChildren(true)]
	[PersistChildren(false)]
    [DefaultProperty("CompletionContextItems")]
	[ClientScriptResource("AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior", "AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoComplete.ContextSensitiveAutoCompleteBehavior.js")]
    [RequiredScript(typeof(AjaxControlToolkit.CommonToolkitScripts))]
	[RequiredScript(typeof(CommonToolkitScripts))]
    [RequiredScript(typeof(PopupExtender))]
    [RequiredScript(typeof(TimerScript))]
    [RequiredScript(typeof(AutoCompleteExtender))]
    [TargetControlType(typeof(ITextControl))]
	public class ContextSensitiveAutoCompleteExtender : AutoCompleteExtender
    {
        
		/// <summary>
        /// This property is stored manually at RenderScriptAttributes at a dictionary.
		/// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("completionContextItems")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        [DefaultValue((string) null)]
        public virtual Collection<CompletionContextItem> CompletionContextItems
		{
			get
			{
                if (GetPropertyValue<Collection<CompletionContextItem>>("CompletionContextItems", null) == null)
				{
                    SetPropertyValue<Collection<CompletionContextItem>>("CompletionContextItems", new Collection<CompletionContextItem>());
				}
                return GetPropertyValue<Collection<CompletionContextItem>>("CompletionContextItems", null);
			}
		}
        /// <summary>
        /// Renders the javascript code required for setting the client attributes.
        /// </summary>
        /// <param name="descriptor"></param>
        protected override void RenderScriptAttributes(ScriptBehaviorDescriptor descriptor)
        {
            base.RenderScriptAttributes(descriptor);
            
            //// This code stores CompletionContextIItems.
            IDictionary<string, string> contextDictionary = new Dictionary<string, string>();
            foreach (CompletionContextItem contextEntry in CompletionContextItems)
            {
                Control c = this.ResolveControl(contextEntry.ControlId);
                string controlId;
                if (c != null)
                {
                    controlId = c.ClientID;
                }
                else
                {
                    controlId = contextEntry.ControlId;
                }

                contextDictionary.Add(contextEntry.Key, controlId);
            }
            descriptor.AddProperty("completionContextItems", contextDictionary);
        }
    }
}
