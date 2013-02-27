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
public class ControlMonitorParameterCollection : Collection<ControlMonitorParameter>
{
    private RealTimeSearchMonitor _owner;

    public ControlMonitorParameterCollection(RealTimeSearchMonitor owner)
    {
        if (owner == null)
        {
            throw new ArgumentNullException("owner");
        }
        _owner = owner;
    }

    protected override void ClearItems()
    {
        foreach (ControlMonitorParameter parameter in this)
        {
            parameter.SetOwner(null);
        }
        base.ClearItems();
    }

    protected override void InsertItem(int index, ControlMonitorParameter item)
    {
        item.SetOwner(this.Owner);
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        base[index].SetOwner(null);
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, ControlMonitorParameter item)
    {
        base[index].SetOwner(null);
        item.SetOwner(this.Owner);

        base.SetItem(index, item);
    }

    public RealTimeSearchMonitor Owner
    {
        get
        {
            return _owner;
        }
    }
}
}
