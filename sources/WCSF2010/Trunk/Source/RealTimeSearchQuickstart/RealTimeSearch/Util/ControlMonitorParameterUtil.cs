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
using System.ComponentModel.Design;
using System.Web.UI;
using System.ComponentModel;

namespace RealTimeSearch.Util
{
    internal static class ControlMonitorParameterUtil
    {
        // Methods
        internal static bool IsValidTarget(IComponent component, IComponent rootComponent, bool requireSite)
        {
            Control control = component as Control;
            if (((control != null) && (!requireSite || (control.Site != null))) && ((control != rootComponent) && !string.IsNullOrEmpty(control.ID)))
            {
                return (((control is INamingContainer) || (control is IPostBackDataHandler)) || (control is IPostBackEventHandler));
            }
            return false;
        }

        private static bool WalkChildren(IComponent rootComponent, Control control, bool requireSite, Predicate<IComponent> visitor, Control walkLimit)
        {
            if (control != walkLimit)
            {
                if (IsValidTarget(control, rootComponent, requireSite) && visitor(control))
                {
                    return false;
                }
                foreach (Control control2 in control.Controls)
                {
                    if (WalkChildren(rootComponent, control2, requireSite, visitor, walkLimit))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static void WalkControlTree(Control startControl, Predicate<IComponent> visitor, bool requireSite, Control walkLimit)
        {
            IDesignerHost service = (IDesignerHost)startControl.Site.GetService(typeof(IDesignerHost));
            WalkChildren(service.RootComponent, startControl, requireSite, visitor, walkLimit);
        }
    }
}
