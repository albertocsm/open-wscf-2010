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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.Practices.Web.UI.WebControls.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Web.UI.WebControls.Tests
{
    [TestClass]
    public class ObjectContainerDataSourceFixture
    {
        [TestMethod]
        public void SetDataSourceForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            
            object obj = new object();
            control.DataSource = obj;

            Assert.AreSame(obj, control._view._dataSource);
        }

        [TestMethod]
        public void GetItemsForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            List<object> items = new List<object>(1);
            object item = new object();
            items.Add(item);
            control._view._items = items;

            ReadOnlyCollection<object> result = control.Items;

            Assert.AreSame(item, result[0]);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetDataObjectTypeNameForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            control._view._dataObjectTypeName = "typeName";

            string result = control.DataObjectTypeName;

            Assert.AreEqual("typeName", result);
        }

        [TestMethod]
        public void SetDataObjectTypeNameForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.DataObjectTypeName = "typeName";

            Assert.AreEqual("typeName", control._view._dataObjectTypeName);
        }

        [TestMethod]
        public void GetUsingServerSortingForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            control._view.UsingServerSorting = true;

            bool result = control.UsingServerSorting;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetUsingServerSortingForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.UsingServerSorting = true;

            Assert.IsTrue(control._view._usingServerSorting);
        }

        [TestMethod]
        public void SetUsingServerPagingForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.UsingServerPaging = true;

            Assert.IsTrue(control._view._usingServerPaging);
        }

        [TestMethod]
        public void GetUsingServerPagingForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            control._view.UsingServerPaging = true;

            bool result = control.UsingServerPaging;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetTotalRowCountForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.TotalRowCount = 5;

            Assert.AreEqual(5, control._view._totalRowCount);
        }

        [TestMethod]
        public void GetViewNamesReturnsDefaultViewOnly()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            ArrayList viewNames = new ArrayList(control.TestGetViewNames());

            Assert.AreEqual(1, viewNames.Count);
            Assert.IsTrue(viewNames.Contains("DefaultView"));
        }

        [TestMethod]
        public void GetViewNamesReturnsAlwaysTheSameCollection()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            ICollection result1 = control.TestGetViewNames();
            ICollection result2 = control.TestGetViewNames();

            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public void GetViewReturnsCorrectView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            DataSourceView result = control.TestGetView("DefaultView");

            Assert.IsTrue(result is ObjectContainerDataSourceView);
            Assert.AreEqual("DefaultView", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetViewThrowsIfNotDefaultView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            DataSourceView result = control.TestGetView("DefaultView1");
        }

        [TestMethod]
        public void GetViewReturnsDefaultViewIfViewNameIsNull()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            DataSourceView result = control.TestGetView(null);

            Assert.IsTrue(result is ObjectContainerDataSourceView);
            Assert.AreEqual("DefaultView", result.Name);
        }

        [TestMethod]
        public void GetViewReturnsDefaultViewIfViewNameIsEmpty()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            DataSourceView result = control.TestGetView(String.Empty);

            Assert.IsTrue(result is ObjectContainerDataSourceView);
            Assert.AreEqual("DefaultView", result.Name);
        }

        [TestMethod]
        public void GetViewReturnsAlwaysTheSameInstance()
        {
            TestableObjectContainerDataSource2 control = new TestableObjectContainerDataSource2();

            DataSourceView result1 = control.TestGetView(String.Empty);
            DataSourceView result2 = control.TestGetView(String.Empty);

            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public void AddDeletedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();
            
            control._view.CallOnDeleted();

            Assert.IsTrue(control._DeletedFired);
        }

        [TestMethod]
        public void AddDeletingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnDeleting();

            Assert.IsTrue(control._DeletingFired);
        }

        [TestMethod]
        public void AddInsertedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnInserted();

            Assert.IsTrue(control._InsertedFired);
        }

        [TestMethod]
        public void AddInsertingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnInserting();

            Assert.IsTrue(control._InsertingFired);
        }

        [TestMethod]
        public void AddUpdatedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnUpdated();

            Assert.IsTrue(control._UpdatedFired);
        }

        [TestMethod]
        public void AddUpdatingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnUpdating();

            Assert.IsTrue(control._UpdatingFired);
        }

        [TestMethod]
        public void AddSelectingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control._view.CallOnSelecting();

            Assert.IsTrue(control._SelectingFired);
        }
        
        [TestMethod]
        public void RemoveDeletedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnDeleted();

            Assert.IsFalse(control._DeletedFired);
        }

        [TestMethod]
        public void RemoveDeletingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnDeleting();

            Assert.IsFalse(control._DeletingFired);
        }

        [TestMethod]
        public void RemoveInsertedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnInserted();

            Assert.IsFalse(control._InsertedFired);
        }

        [TestMethod]
        public void RemoveInsertingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnInserting();

            Assert.IsFalse(control._InsertingFired);
        }
        
        [TestMethod]
        public void RemoveUpdatedEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnUpdated();

            Assert.IsFalse(control._UpdatedFired);
        }

        [TestMethod]
        public void RemoveUpdatingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnUpdating();

            Assert.IsFalse(control._UpdatingFired);
        }

        [TestMethod]
        public void RemoveSelectingEventForwardsToView()
        {
            TestableObjectContainerDataSource control = new TestableObjectContainerDataSource();

            control.RemoveEventSubscriptions();
            control._view.CallOnSelecting();

            Assert.IsFalse(control._UpdatingFired);
        }
    }

    class TestableObjectContainerDataSource : ObjectContainerDataSource
    {
        public MockObjectContainerDataSourceView _view;
        public bool _DeletedFired = false;
        public bool _DeletingFired = false;
        public bool _InsertedFired = false;
        public bool _InsertingFired = false;
        public bool _UpdatedFired = false;
        public bool _UpdatingFired = false;
        public bool _SelectingFired = false;

        public TestableObjectContainerDataSource()
        {
            _view = new MockObjectContainerDataSourceView(this, "DefaultView");
            Deleted += new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Deleted);
            Deleting += new EventHandler<ObjectContainerDataSourceDeletingEventArgs>(TestableObjectContainerDataSource_Deleting);
            Inserted += new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Inserted);
            Inserting += new EventHandler<ObjectContainerDataSourceInsertingEventArgs>(TestableObjectContainerDataSource_Inserting);
            Updated += new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Updated);
            Updating += new EventHandler<ObjectContainerDataSourceUpdatingEventArgs>(TestableObjectContainerDataSource_Updating);
            Selecting += new EventHandler<ObjectContainerDataSourceSelectingEventArgs>(TestableObjectContainerDataSource_Selecting);
        }

        public void RemoveEventSubscriptions()
        {
            Deleted -= new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Deleted);
            Deleting -= new EventHandler<ObjectContainerDataSourceDeletingEventArgs>(TestableObjectContainerDataSource_Deleting);
            Inserted -= new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Inserted);
            Inserting -= new EventHandler<ObjectContainerDataSourceInsertingEventArgs>(TestableObjectContainerDataSource_Inserting);
            Updated -= new EventHandler<ObjectContainerDataSourceStatusEventArgs>(TestableObjectContainerDataSource_Updated);
            Updating -= new EventHandler<ObjectContainerDataSourceUpdatingEventArgs>(TestableObjectContainerDataSource_Updating);
            Selecting -= new EventHandler<ObjectContainerDataSourceSelectingEventArgs>(TestableObjectContainerDataSource_Selecting);
        }

        void TestableObjectContainerDataSource_Updating(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
        {
            _UpdatingFired = true;
        }

        void TestableObjectContainerDataSource_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            _UpdatedFired = true;
        }

        void TestableObjectContainerDataSource_Inserting(object sender, ObjectContainerDataSourceInsertingEventArgs e)
        {
            _InsertingFired = true;
        }

        void TestableObjectContainerDataSource_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            _InsertedFired = true;
        }

        void TestableObjectContainerDataSource_Deleting(object sender, ObjectContainerDataSourceDeletingEventArgs e)
        {
            _DeletingFired = true;
        }

        void TestableObjectContainerDataSource_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            _DeletedFired = true;
        }

        void TestableObjectContainerDataSource_Selecting(object sender, ObjectContainerDataSourceSelectingEventArgs e)
        {
            _SelectingFired = true;
        }

        protected override ObjectContainerDataSourceView View
        {
            get { return _view; }
        }

        public ICollection TestGetViewNames()
        {
            return base.GetViewNames();
        }

        public DataSourceView TestGetView(string viewName)
        {
            return base.GetView(viewName);
        }
    }

    class TestableObjectContainerDataSource2 : ObjectContainerDataSource
    {
        public DataSourceView TestGetView(string viewName)
        {
            return base.GetView(viewName);
        } 
    }


    namespace Mocks
    {
        class MockObjectContainerDataSourceView : ObjectContainerDataSourceView
        {
            public object _dataSource = null;
            public List<object> _items = new List<object>();
            public string _dataObjectTypeName = null;
            public bool _enablePaging = false;
            public IEnumerable _addRangeItems = null;
            public object _addedObject = null;
            public bool _usingServerSorting = false;
            public int _totalRowCount = 0;
            public bool _usingServerPaging = false;

            public MockObjectContainerDataSourceView(ObjectContainerDataSource owner, string name)
            : base(owner, name)
            {
                
            }

            public override object DataSource
            {
                set { _dataSource = value; }
            }

            public override ReadOnlyCollection<object> Items
            {
                get { return _items.AsReadOnly(); }
            }

            public override string DataObjectTypeName
            {
                get { return _dataObjectTypeName; }
                set { _dataObjectTypeName = value; }
            }

            public override bool UsingServerSorting
            {
                get { return _usingServerSorting; }
                set { _usingServerSorting = value; }
            }

            public override bool UsingServerPaging
            {
                get { return _usingServerPaging; }
                set { _usingServerPaging = value; }
            }

            public override int TotalRowCount
            {
                set { _totalRowCount = value; }
            }

            protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void CallOnDeleted()
            {
                base.OnDeleted(new ObjectContainerDataSourceStatusEventArgs(null, 0));
            }

            public void CallOnDeleting()
            {
                base.OnDeleting(new ObjectContainerDataSourceDeletingEventArgs(null, null));
            }

            public void CallOnInserted()
            {
                base.OnInserted(new ObjectContainerDataSourceStatusEventArgs(null, 0));
            }

            public void CallOnInserting()
            {
                base.OnInserting(new ObjectContainerDataSourceInsertingEventArgs(null));
            }

            public void CallOnUpdated()
            {
                base.OnUpdated(new ObjectContainerDataSourceStatusEventArgs(null, 0));
            }

            public void CallOnUpdating()
            {
                base.OnUpdating(new ObjectContainerDataSourceUpdatingEventArgs(null, null, null));
            }

            public void CallOnSelecting()
            {
                base.OnSelecting(new ObjectContainerDataSourceSelectingEventArgs(null));
            }
        }
    }
}
