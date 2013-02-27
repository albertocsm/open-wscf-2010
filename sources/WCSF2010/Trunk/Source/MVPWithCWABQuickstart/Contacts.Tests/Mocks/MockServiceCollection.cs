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
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace MVPWithCWABQuickStart.Contacts.Tests.Mocks
{
    public class MockServiceCollection : IServiceCollection
    {
        public Dictionary<Type, object> RegistedServices = new Dictionary<Type, object>();

        #region IServiceCollection Members

        public void Add<TService>(TService serviceInstance)
        {
            RegistedServices.Add(typeof(TService), serviceInstance);
        }

        public void Add(Type serviceType, object serviceInstance)
        {
            RegistedServices[serviceType] = serviceInstance;
        }

        public TService AddNew<TService, TRegisterAs>() where TService : TRegisterAs
        {
            RegistedServices.Add(typeof(TRegisterAs), null);
            return default(TService);
        }

        public TService AddNew<TService>()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AddNew(Type serviceType, Type registerAs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AddNew(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddOnDemand<TService, TRegisterAs>()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddOnDemand<TService>()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddOnDemand(Type serviceType, Type registerAs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddOnDemand(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains<TService>()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ContainsLocal(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object Get(Type serviceType)
        {
            if (RegistedServices.ContainsKey(serviceType))
            {
                return RegistedServices[serviceType];
            }
            return null;
        }

        public object Get(Type serviceType, bool ensureExists)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public TService Get<TService>(bool ensureExists)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public TService Get<TService>()
        {
            if (RegistedServices.ContainsKey(typeof(TService)))
            {
                return (TService)RegistedServices[typeof(TService)];
            }
            return default(TService);
        }

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Remove(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Remove<TService>()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
