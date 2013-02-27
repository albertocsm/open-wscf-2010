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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ValidationQuickStart.BusinessEntities;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Globalization;
using ValidationQuickStart.Properties;
using System.Net;

namespace ValidationQuickStart
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void AddressCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Profile profile = new Profile();
            Validator profileValidator=ValidationFactory.CreateValidator<Profile>();

            profile.AccountNumber = AccountNumberTextBox.Text;
            profile.Email = EmailTextBox.Text;
            profile.Name = NameTextBox.Text;
            profile.Homepage = HomepageTextBox.Text;
            profile.State = StateTextBox.Text;
            profile.Zip = ZipTextBox.Text;
            //int age;
            //profile.Age = int.TryParse(AgeTextBox.Text, out age) ? (int?)age : null;

            ValidationResults results = profileValidator.Validate(profile);
            results=results.FindAll(TagFilter.Include,"Address");

            args.IsValid=results.IsValid;
            if (!results.IsValid)
            {
                foreach(ValidationResult result in results)
                {
                    AddressCustomValidator.ErrorMessage = result.Message;
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ProfileSavedLabel.Visible = Page.IsValid;
        }

        protected void HompageCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string url = args.Value;
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                args.IsValid = UriExists(uri);
            }
            else
            {
                args.IsValid = false;
            }
        }

		/*
		 * This validation is only here as an example.
		 * This implementation has scalibility and performance concerns, and is not recommended 
		 * for a real-world application due to these concerns.
		 */
		private bool UriExists(Uri uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.MaximumAutomaticRedirections = 4;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

    }
}
