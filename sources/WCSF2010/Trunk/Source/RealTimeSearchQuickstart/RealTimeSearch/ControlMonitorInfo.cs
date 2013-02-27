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
using System.Collections.ObjectModel;

namespace RealTimeSearch
{
    /// <summary>
    /// This class is used to pass additional info to javascript side extender.
    /// A ControlMonitorParameter is translated to a ControlMonitorInfo
    /// </summary>
    public class ControlMonitorInfo
    {
        private string _targetId;
        private string _callback;
        private ICollection<string> _validators;

        public string TargetId
        {
            get { return _targetId; }
            set { _targetId = value; }
        }

        public string Callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        public ICollection<string> Validators
        {
            get
            {
                if (_validators == null)
                    _validators = new Collection<string>();
                return _validators;
            }
            set
            {
                _validators = value;
            }
        }
    }
}
