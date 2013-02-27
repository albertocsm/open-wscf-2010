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
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Web.UI.Design;

namespace RealTimeSearch {
    public class ControlMonitorParameter {
        string _targetId;
        string _eventName;
        RealTimeSearchMonitor _owner;

        public ControlMonitorParameter() {
            _targetId = string.Empty;
        }

        internal void SetOwner(RealTimeSearchMonitor owner)
        {
            _owner = owner;
        }

        [Browsable(false)]
        public RealTimeSearchMonitor Owner
        {
            get
            {
                return _owner;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member"), TypeConverter(typeof(ControlIDConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string TargetID {
            get { return _targetId; }
            set { _targetId = value; }
        }

        [TypeConverter(typeof(ControlMonitorParameterEventNameConverter))]
        [DefaultValue("")]
        public string EventName {
            get { return _eventName; }
            set { _eventName = value; }
        }

        public ICollection<string> ResolveValidators(Page page)
        {
            ICollection<string> validators = new Collection<string>();
            foreach (IValidator validator in page.Validators)
            {
                BaseValidator baseValidator = validator as BaseValidator;
                if ((baseValidator != null) && (string.Equals(_targetId, baseValidator.ControlToValidate, StringComparison.InvariantCulture)))
                {
                    validators.Add(baseValidator.ID);
                }
            }
            return validators;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.TargetID))
            {
                return "ControlMonitor";
            }
            return ("ControlMonitor: " + this.TargetID + (string.IsNullOrEmpty(this.EventName) ? string.Empty : ("." + this.EventName)));
        }
    }
}
