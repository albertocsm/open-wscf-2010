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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using AjaxControlToolkit;
using System.Collections.ObjectModel;
using System.Collections.Generic;

[assembly: System.Web.UI.WebResource("CustomExtenders.ClientScriptCallBehavior.js", "text/javascript")]

namespace CustomExtenders
{
    [Designer(typeof(ClientScriptCallDesigner))]
	[ClientScriptResource("CustomExtenders.ClientScriptCallBehavior", "CustomExtenders.ClientScriptCallBehavior.js")]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [DefaultProperty("CustomScriptParameters")]
    [TargetControlType(typeof(Control))]
    public class ClientScriptCallExtender : ExtenderControlBase
    {
        [ExtenderControlProperty]
        [ClientPropertyName("customScript")]
        [DefaultValue("")]
        public string CustomScript
        {
            get
            {
                return GetPropertyValue("customScript", "");
            }
            set
            {
                SetPropertyValue("customScript", value);
            }
        }

        [ExtenderControlProperty]
        [ClientPropertyName("customScriptParameters")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        [DefaultValue((string)null)]
        public virtual Collection<CustomScriptParameter> CustomScriptParameters
        {
            get
            {
                if (GetPropertyValue<Collection<CustomScriptParameter>>("CustomScriptParameters", null) == null)
                {
                    SetPropertyValue<Collection<CustomScriptParameter>>("CustomScriptParameters", new Collection<CustomScriptParameter>());
                }
                return GetPropertyValue<Collection<CustomScriptParameter>>("CustomScriptParameters", null);
            }
        }

        protected override void RenderScriptAttributes(ScriptBehaviorDescriptor descriptor)
        {
            base.RenderScriptAttributes(descriptor);

            IList<string> parametersList = new List<string>();
            foreach (CustomScriptParameter parameterEntry in CustomScriptParameters)
            {
				parametersList.Add(ResolveParameter(parameterEntry));
            }
            descriptor.AddProperty("customScriptParameters", parametersList);
            descriptor.AddProperty("customScript", CustomScript);
        }

		protected string ResolveParameter(CustomScriptParameter parameter)
		{
			if (!string.IsNullOrEmpty(parameter.ControlId))
			{
				// the parameter has a ControlId
				Control c = this.ResolveControl(parameter.ControlId);
				string controlId;
				if (c != null)
				{
					controlId = c.ClientID;
				}
				else
				{
					controlId = parameter.ControlId;
				}
				return controlId;
			}
			else
			{
				// the parameter has a Value
				return parameter.Value;
			}
		}
    }
}
