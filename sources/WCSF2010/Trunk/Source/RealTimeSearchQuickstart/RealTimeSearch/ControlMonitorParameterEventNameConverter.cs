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
using System.ComponentModel;
using System.Web.UI;
using System.ComponentModel.Design;
using RealTimeSearch.Util;

namespace RealTimeSearch
{
    public class ControlMonitorParameterEventNameConverter : StringConverter
    {
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                Predicate<IComponent> visitor = null;
                ControlMonitorParameter parameter = context.Instance as ControlMonitorParameter;
                Control targetControl = null;
                if (((parameter != null) && !string.IsNullOrEmpty(parameter.TargetID)) && ((parameter.Owner != null) && (parameter.Owner.Site != null)))
                {
                    IDesignerHost service = (IDesignerHost)parameter.Owner.Site.GetService(typeof(IDesignerHost));
                    Control rootComponent = service.RootComponent as Control;
                    if (rootComponent != null)
                    {
                        if (visitor == null)
                        {
                            visitor = delegate(IComponent component)
                            {
                                Control control = component as Control;
                                if ((control != null) && string.Equals(control.ID, parameter.TargetID, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    targetControl = control;
                                    return true;
                                }
                                return false;
                            };
                        }
                        ControlMonitorParameterUtil.WalkControlTree(rootComponent, visitor, true, parameter.Owner);
                    }
                }
                if (targetControl != null)
                {
                    List<string> values = new List<string>();
                    foreach (EventDescriptor descriptor in TypeDescriptor.GetEvents(targetControl))
                    {
                        values.Add(descriptor.Name);
                    }
                    if (values.Count > 0)
                    {
                        values.Sort(StringComparer.OrdinalIgnoreCase);
                        return new TypeConverter.StandardValuesCollection(values);
                    }
                }
            }
            return base.GetStandardValues(context);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
