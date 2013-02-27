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
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;

namespace GlobalBank.Infrastructure.Testing.UI
{
    public class ClientIdMapper : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.WriteLine();
            RenderControlMap(writer, Page.Controls);
            writer.RenderEndTag();
        }

        protected void RenderControlMap(HtmlTextWriter writer, ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (String.IsNullOrEmpty(control.ID) == false)
                {
                    writer.AddAttribute("ServerId", control.ID);
                    writer.AddAttribute("ClientId", control.ClientID);
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.RenderEndTag();
                    writer.WriteLine();
                }

                if (control.Controls != null && control.Controls.Count > 0)
                {
                    RenderControlMap(writer, control.Controls);
                }
            }
        }
    }
}
