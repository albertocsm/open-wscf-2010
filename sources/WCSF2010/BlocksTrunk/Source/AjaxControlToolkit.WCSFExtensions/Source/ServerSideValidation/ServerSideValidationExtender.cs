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
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.WCSFExtensions.Properties;
using AjaxControlToolkit.WCSFExtensions.Utility;
using System.Globalization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
[assembly: WebResource("AjaxControlToolkit.WCSFExtensions.ServerSideValidation.ServerSideValidationBehavior.js", "text/javascript")]
namespace AjaxControlToolkit.WCSFExtensions
{
    /// <summary>
    /// An extender that provides validation capabilities accessing server resources from the client side
    /// </summary>
	[ClientScriptResource("AjaxControlToolkit.WCSFExtensions.ServerSideValidationBehavior", "AjaxControlToolkit.WCSFExtensions.ServerSideValidation.ServerSideValidationBehavior.js")]
	[RequiredScript(typeof(AjaxControlToolkit.CommonToolkitScripts))]
	[RequiredScript(typeof(CommonToolkitScripts))]
	[TargetControlType(typeof(BaseValidator))]
	public class ServerSideValidationExtender : ExtenderControlBase, ICallbackEventHandler, IPostBackDataHandler, IPostBackEventHandler
	{
		BaseValidator validator;
		private string valueToValidate;
		private Exception callbackEventException;

        /// <summary>
        /// Gets or sets a Boolean value indicating whether empty text should be validated on the client.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if empty text should be validated on the client; otherwise, <see langword="false"/>.
        /// </returns>
        [ExtenderControlProperty]
        [ClientPropertyName("validateEmptyText")]
        [DefaultValue(false)]
        public bool ValidateEmptyText
        {
            get { return GetPropertyValue<bool>("ValidateEmptyText", false); }
            set { SetPropertyValue<bool>("ValidateEmptyText", value); }
        }

		#region ICallbackEventHandler Members
        /// <summary>
        /// Obtains the serialized result message that will be used in the client side
        /// </summary>
        /// <returns> A JSON object</returns>
		public string GetCallbackResult()
		{
			if(callbackEventException == null)
			{
				validator.Validate();
				ServerValidationResults results = new ServerValidationResults(this.valueToValidate, validator.IsValid, validator.ErrorMessage);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ServerValidationResults));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, results);
                string serialized = Encoding.Default.GetString(ms.ToArray());
                ms.Close();
				return serialized;
			}
			else
			{
				throw callbackEventException;
			}
		}

        /// <summary>
        /// Implementation of a Callback event.
        /// </summary>
        /// <param name="eventArgument"> The argument of the event</param>
		public void RaiseCallbackEvent(string eventArgument)
		{
			try
			{
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ServerValidationResults));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(eventArgument));

                ServerValidationResults arguments = ser.ReadObject(ms) as ServerValidationResults;
				this.valueToValidate = arguments.Value;

				this.validator = GetExtendedValidator();
				SetControlValidationValue(validator, validator.ControlToValidate, this.valueToValidate);
			}
			catch(Exception ex)
			{
				callbackEventException = ex;
			}
		}

		#endregion

        /// <summary>
        /// Implementation of OnPreRender that links the Validator in the page with javascript code
        /// </summary>
        /// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			ClientScriptManager cm = Page.ClientScript;
			BaseValidator extendedValidator = this.GetExtendedValidator();
			CustomValidator customValidator = extendedValidator as CustomValidator;

			if (extendedValidator != null)
			{
				if (customValidator != null)
				{
					customValidator.ClientValidationFunction = "CustomServerSideValidationEvaluateIsValid";
				}
				else
				{
					string controlId = extendedValidator.ClientID;
					cm.RegisterExpandoAttribute(controlId, "evaluationfunction", "GenericServerSideValidationEvaluateIsValid", false);
				}
			}

			//Call GetCallbackEventReference to be able to do a Callback on the client-side
			cm.GetCallbackEventReference(this, "arg", "ServerSideValidationValidate", "context");

            //Registers this control as one that requires postback handling when the page is posted back to the server.
            //This is to prevent full page validation when the postback is in fact a callback triggered by this control
            this.Page.RegisterRequiresPostBack(this);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptor"></param>
		protected override void RenderScriptAttributes(ScriptBehaviorDescriptor descriptor)
		{
			base.RenderScriptAttributes(descriptor);
			descriptor.AddProperty("callbackControl", this.UniqueID);
		}

		private static void SetControlValidationValue(Control validator, string controlName, string value)
		{
			Guard.ArgumentNotNull(validator, "validator");

			Control component = validator.NamingContainer.FindControl(controlName);
			if (component == null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.CannotFindControlToValidate, controlName);
				throw new InvalidOperationException(message);
			}
			PropertyDescriptor validationProperty = BaseValidator.GetValidationProperty(component);
			if (validationProperty == null)
			{
                string message = string.Format(CultureInfo.CurrentCulture, Resources.NoValidationProperty, controlName);
				throw new InvalidOperationException(message);
			}
			validationProperty.SetValue(component, value);
		}

		private BaseValidator GetExtendedValidator()
		{
			return (BaseValidator)this.NamingContainer.FindControl(this.TargetControlID);
        }


        #region Required methods to prevent full page validation when handling a callback triggered by this control

        /// <summary>
        /// Required method to prevent full page validation when handling a callback triggered by this control
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            if (postCollection != null)
            {
                // If this control is the control that triggered the postback (which is a callback),
                // register this control to raise a postback event, preventing automatic full page validation (which
                // happens everytime there is a postback but there is no postback handler for that postback)
                if (postCollection["__CALLBACKID"] == this.UniqueID)
                {
                    this.Page.RegisterRequiresRaiseEvent(this);
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
        }

        #endregion
    }
}
