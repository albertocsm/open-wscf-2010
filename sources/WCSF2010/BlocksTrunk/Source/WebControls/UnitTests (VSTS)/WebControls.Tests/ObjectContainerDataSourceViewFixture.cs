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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;
using System.Web.UI;
using Microsoft.Practices.Web.UI.WebControls;

namespace Microsoft.Practices.Web.UI.WebControls.Tests
{
    [TestClass]
    public class ObjectContainerDataSourceViewFixture
    {
        #region DataSource Property

        [TestMethod]
        public void SetDataSourceWithSingleObject()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            object obj = new object();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.DataSource = obj;

            Assert.AreEqual(1, view.TestGetData().Count);
            Assert.AreSame(obj, view.TestGetData()[0]);
            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetDataSourceWithMultipleObjects()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            List<object> objects = new List<object>(1);
            object obj = new object();
            objects.Add(obj);
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.DataSource = objects;

            Assert.AreEqual(1, view.TestGetData().Count);
            Assert.AreSame(obj, view.TestGetData()[0]);
            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetDataSourceWithNullClearsDataListAndDoesNotFireDataSourceViewChangedEvent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            view.TestGetData().Add(new object());
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };
            
            view.DataSource = null;

            Assert.AreEqual(0, view.TestGetData().Count);
            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDataSourceWithSingleObjectOfWrongTypeThrows()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.DataSource = new object();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDataSourceWithMultipleObjectsOfWrongTypeThrows()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            List<object> items = new List<object>();
            items.Add(new object());

            view.DataSource = items;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDataSourceThrowsIfDataObjectTypeNameNotSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            view.DataSource = new object();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetDataSourceThrowsIfDataObjectTypeNameIsNotValid()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            view.DataObjectTypeName = "UnexistantType";
            view.DataSource = new object();
        }

        #endregion

        [TestMethod]
        public void ItemsReturnsDataAsReadOnly()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            object obj = new object();
            view.TestGetData().Add(obj);

            ReadOnlyCollection<object> result = view.Items;

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(obj, result[0]);
        }

        [TestMethod]
        public void CanDelete()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanDelete);
        }

        [TestMethod]
        public void CanUpdate()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanUpdate);
        }

        [TestMethod]
        public void CanInsert()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanInsert);
        }

        [TestMethod]
        public void CanRetrieveTotalRowCount()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanRetrieveTotalRowCount);
        }

        [TestMethod]
        public void CanSort()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanSort);
        }

        [TestMethod]
        public void CanPage()
        {
            Assert.IsTrue(new TestableObjectContainerDataSourceView().CanPage);
        }

        [TestMethod]
        public void SetTotalRowCount()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.TotalRowCount = 1;

            Assert.IsTrue(dataSourceViewChangedFired);
            Assert.AreEqual(1, view.GetTotalRowCount());
        }

        [TestMethod]
        public void SetTotalRowCountDoesNotFireDataSourceViewChangedEventIfValueIsEqual()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.TotalRowCount = 1;
            dataSourceViewChangedFired = false;
            view.TotalRowCount = 1;

            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetTotalRowCountFiresDataSourceViewChangedEventIfValueIsDifferent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.TotalRowCount = 1;
            dataSourceViewChangedFired = false;
            view.TotalRowCount = 2;

            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetTotalRowCountThrowsIfValueIsLessThan0()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            view.TotalRowCount = -1;
        }

        [TestMethod]
        public void GetUsingServerSortingReturnsWhatWasSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            view.UsingServerSorting = true;

            Assert.IsTrue(view.UsingServerSorting);
        }

        [TestMethod]
        public void SetUsingServerSortingDefaultsToFalse()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            Assert.IsFalse(view.UsingServerSorting);
        }

        [TestMethod]
        public void SetUsingServerSortingDoesNotFireDataSourceViewChangedEventIfValueIsEqual()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.UsingServerSorting = false;

            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetUsingServerSortingFiresDataSourceViewChangedEventIfValueIsDifferent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.UsingServerSorting = true;

            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void GetUsingServerPagingReturnsWhatWasSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            view.UsingServerPaging = true;

            Assert.IsTrue(view.UsingServerPaging);
        }

        [TestMethod]
        public void SetUsingServerPagingDefaultsToFalse()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            Assert.IsFalse(view.UsingServerPaging);
        }

        [TestMethod]
        public void SetUsingServerPagingDoesNotFireDataSourceViewChangedEventIfValueIsEqual()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.UsingServerPaging = false;

            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetUsingServerPagingFiresDataSourceViewChangedEventIfValueIsDifferent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.UsingServerPaging = true;

            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void OwnerDataSourceIsAccessibleThroughProperty()
        {
            ObjectContainerDataSource dataSource = new ObjectContainerDataSource();
            dataSource.ID = "TestID";
            ObjectContainerDataSourceView view = new ObjectContainerDataSourceView(dataSource, "TestView");

            ObjectContainerDataSource owner = view.Owner;

            Assert.IsNotNull(owner);
            Assert.AreEqual("TestID", owner.ID);
        }

        #region DataObjectTypeName Property

        [TestMethod]
        public void GetDataObjectTypeNameReturnsEmptyStringIfNotSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            string result = view.DataObjectTypeName;

            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void GetDataObjectTypeNameReturnsWhatWasSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();

            view.DataObjectTypeName = "System.Object";
            string result = view.DataObjectTypeName;

            Assert.AreEqual("System.Object", result);
        }

        [TestMethod]
        public void SetDataObjectTypeNameFiresDataSourceViewChangedWhenSetForTheFirstTime()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };
            view.DataObjectTypeName = "System.Object";

            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetDataObjectTypeNameFiresDataSourceViewChangedWhenSettingANewValue()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            view.DataObjectTypeName = "System.Object";
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            view.DataObjectTypeName = "Microsoft.Practices.Web.UI.WebControls.Tests.SimpleEntity, Microsoft.Practices.Web.UI.WebControls.ObjectContainerDataSource.Tests";

            Assert.IsTrue(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void SetDataObjectTypeNameDoesNotFireDataSourceViewChangedIfValueHasNotChanged()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView();
            view.DataObjectTypeName = "System.Object";
            bool dataSourceViewChangedFired = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };
           
            view.DataObjectTypeName = "System.Object";

            Assert.IsFalse(dataSourceViewChangedFired);
        }
        
        #endregion

        #region Insert Method

        [TestMethod]
        public void InsertInsertsInstanceToDataListAndReturns1()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary values = new OrderedDictionary(2);
            values.Add("Age", 10);
            values.Add("Name", "name");

            int affectedRows = view.TestInsert(values);

            Assert.AreEqual(1, affectedRows);
            Assert.AreEqual(1, view.TestGetData().Count);
            Assert.AreEqual(10, ((SimpleEntity)view.TestGetData()[0]).Age);
            Assert.AreEqual("name", ((SimpleEntity)view.TestGetData()[0]).Name);
        }

        [TestMethod]
        public void InsertFiresDataSourceViewChangedEventAfterInsertingItemToDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            bool dataSourceViewChangedFiredAfterInserting = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e)
            {
                dataSourceViewChangedFiredAfterInserting = (view.TestGetData().Count == 1); 
            };

            view.TestInsert(new OrderedDictionary());

            Assert.IsTrue(dataSourceViewChangedFiredAfterInserting);
        }

        [TestMethod]
        public void InsertFiresInsertingEventBeforeInsertingItemInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            bool itemInserted = false;
            view.Inserting += delegate(object sender, ObjectContainerDataSourceInsertingEventArgs e) 
            {
                itemInserted = view.TestGetData().Count > 0;
            };

            view.TestInsert(new OrderedDictionary());

            Assert.IsFalse(itemInserted);
        }

        [TestMethod]
        public void InsertCreatesInstanceWithNewValuesInInsertingEvent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(ComplexEntity));
            SimpleEntity simpleEntity = new SimpleEntity();
            view.Inserting += delegate(object sender, ObjectContainerDataSourceInsertingEventArgs e)
            {
                e.NewValues["SimpleEntity"] = simpleEntity;
            };

            view.TestInsert(new OrderedDictionary());

            Assert.AreSame(simpleEntity, ((ComplexEntity)view.TestGetData()[0]).SimpleEntity);
        }

        [TestMethod]
        public void InsertFiresInsertingEventWithValuesAsNewValues()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Age", 10);
            ObjectContainerDataSourceInsertingEventArgs eventArgs = null;
            view.Inserting += delegate(object sender, ObjectContainerDataSourceInsertingEventArgs e) { eventArgs = e; };

            view.TestInsert(values);

            Assert.AreEqual(1, eventArgs.NewValues.Count);
            Assert.AreEqual(10, eventArgs.NewValues["Age"]);
        }

        [TestMethod]
        public void InsertFiresInsertingEventNotCanceled()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            ObjectContainerDataSourceInsertingEventArgs eventArgs = null;
            view.Inserting += delegate(object sender, ObjectContainerDataSourceInsertingEventArgs e) { eventArgs = e; };

            view.TestInsert(new OrderedDictionary());

            Assert.IsFalse(eventArgs.Cancel);
        }

        [TestMethod]
        public void IfInsertingEventCanceledViewDoesNotAddItemNorFiresInsertedEventNorFiresDataSourceViewChangedEventAndReturns0()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            bool insertedFired = false;
            bool dataSourceViewChangedFired = false;
            view.Inserting += delegate(object sender, ObjectContainerDataSourceInsertingEventArgs e) { e.Cancel = true; };
            view.Inserted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { insertedFired = true; };
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            int affectedRows = view.TestInsert(new OrderedDictionary());

            Assert.AreEqual(0, affectedRows);
            Assert.AreEqual(0, view.TestGetData().Count);
            Assert.IsFalse(insertedFired);
            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void InsertFiresInsertedEventAfterAddingItemToDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary values = new OrderedDictionary();
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            bool itemInserted = false;
            view.Inserted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) 
            {
                eventArgs = e; 
                itemInserted = view.TestGetData().Contains(e.Instance); 
            };
            
            view.TestInsert(values);

            Assert.IsTrue(itemInserted);
            Assert.AreEqual(1, eventArgs.AffectedRows);
            Assert.AreSame(view.TestGetData()[0], eventArgs.Instance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InsertThrowsIfTypeDoesntHaveAParameterlessConstructor()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntityNoPublicParameterlessConstructor));
            OrderedDictionary values = new OrderedDictionary();

            view.TestInsert(values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InsertThrowsIfNullValues()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.TestInsert(null);
        }

        #endregion

        #region Delete Method

        [TestMethod]
        public void DeleteRemovesItemFromDataListAndReturns1()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);

            int affectedRows = view.TestDelete(keys, new OrderedDictionary());

            Assert.AreEqual(0, view.TestGetData().Count);
            Assert.AreEqual(1, affectedRows);
        }

        [TestMethod]
        public void DeleteFiresDataSourceViewChangedEventAfterDeletingItemFromDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            bool dataSourceViewChangedFiredAfterDeleting = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e)
            {
                dataSourceViewChangedFiredAfterDeleting = (view.TestGetData().Count == 0);
            };

            int affectedRows = view.TestDelete(keys, new OrderedDictionary());

            Assert.IsTrue(dataSourceViewChangedFiredAfterDeleting);
        }

        [TestMethod]
        public void DeleteFiresDeletingEventBeforeDeletingItemFromDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            bool itemDeleted = false;
            view.Deleting += delegate(object sender, ObjectContainerDataSourceDeletingEventArgs e)
            {
                itemDeleted = view.TestGetData().Count == 0;
            };

            view.TestDelete(keys, new OrderedDictionary());

            Assert.IsFalse(itemDeleted);
        }

        [TestMethod]
        public void DeleteFiresDeletingEventWithReadOnlyKeysAndOldValues()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary oldValues = new OrderedDictionary(1);
            oldValues.Add("Name", "name");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            ObjectContainerDataSourceDeletingEventArgs eventArgs = null;
            view.Deleting += delegate(object sender, ObjectContainerDataSourceDeletingEventArgs e) { eventArgs = e; };

            view.TestDelete(keys, oldValues);

            Assert.AreEqual(1, eventArgs.Keys.Count);
            Assert.AreEqual(1, eventArgs.Keys["Id"]);
            Assert.IsTrue(eventArgs.Keys.IsReadOnly);
            Assert.AreEqual(1, eventArgs.OldValues.Count);
            Assert.AreEqual("name", eventArgs.OldValues["Name"]);
        }

        [TestMethod]
        public void DeleteFiresDeletingEventNotCanceled()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            ObjectContainerDataSourceDeletingEventArgs eventArgs = null;
            view.Deleting += delegate(object sender, ObjectContainerDataSourceDeletingEventArgs e) { eventArgs = e; };

            view.TestDelete(keys, new OrderedDictionary());

            Assert.IsFalse(eventArgs.Cancel);
        }

        [TestMethod]
        public void IfDeletingEventCanceledViewDoesNotRemoveItemNorFiresDeletedEventNorFiresDataSourceViewChangedEventAndReturns0()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            bool deletedFired = false;
            bool dataSourceViewChangedFired = false;
            view.Deleting += delegate(object sender, ObjectContainerDataSourceDeletingEventArgs e) { e.Cancel = true; };
            view.Deleted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { deletedFired = true; };
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            int affectedRows = view.TestDelete(keys, new OrderedDictionary());

            Assert.AreEqual(0, affectedRows);
            Assert.AreEqual(1, view.TestGetData().Count);
            Assert.IsFalse(deletedFired);
            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void DeleteFiresDeletedEventAfterRemovingItemFromDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            SimpleEntity simpleEntity = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity);
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            bool itemDeleted = false;
            view.Deleted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) 
            {
                eventArgs = e;
                itemDeleted = view.TestGetData().Count == 0;
            };

            view.TestDelete(keys, new OrderedDictionary());

            Assert.IsTrue(itemDeleted);
            Assert.AreEqual(1, eventArgs.AffectedRows);
        }

        [TestMethod]
        public void DeleteFiresDeletedEventWithNewInstance()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            SimpleEntity simpleEntity = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity);
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(2);
            values.Add("Name", "name");
            values.Add("Age", 10);
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Deleted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestDelete(keys, values);

            Assert.AreEqual(1, ((SimpleEntity)eventArgs.Instance).Id);
            Assert.AreEqual("name", ((SimpleEntity)eventArgs.Instance).Name);
            Assert.AreEqual(10, ((SimpleEntity)eventArgs.Instance).Age);
            Assert.AreNotSame(simpleEntity, eventArgs.Instance);
        }

        [TestMethod]
        public void IfDeleteDoesNotFindObjectInDataListThenCreatesOneAndThrowsDeletedEvent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(2);
            values.Add("Name", "name");
            values.Add("Age", 10);
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Deleted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestDelete(keys, values);

            Assert.AreEqual(1, ((SimpleEntity)eventArgs.Instance).Id);
            Assert.AreEqual("name", ((SimpleEntity)eventArgs.Instance).Name);
            Assert.AreEqual(10, ((SimpleEntity)eventArgs.Instance).Age);
        }

        [TestMethod]
        public void IfDeleteDoesNotFindObjectInDataListReturns0AffectedRows()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name");
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Deleted += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestDelete(keys, values);

            Assert.AreEqual(0, eventArgs.AffectedRows);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteThrowsIfNullKeys()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.TestDelete(null, new OrderedDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteThrowsIfEmptyKeys()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.TestDelete(new OrderedDictionary(), new OrderedDictionary());
        }

        #endregion

        #region Update Method

        [TestMethod]
        public void UpdateUpdatesDataListAndReturns1()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Age", 11);
            values.Add("Name", "name1");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);

            int affectedRows = view.TestUpdate(keys, values);

            Assert.AreEqual(1, affectedRows);
            Assert.AreEqual(11, ((SimpleEntity)view.TestGetData()[0]).Age);
            Assert.AreEqual("name1", ((SimpleEntity)view.TestGetData()[0]).Name);
        }

        [TestMethod]
        public void UpdateDoesNotChangePositionOfObjectInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Age", 11);
            values.Add("Name", "name1");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 2);

            int affectedRows = view.TestUpdate(keys, values);

            Assert.AreEqual(2, ((SimpleEntity)view.TestGetData()[0]).Id);
        }

        [TestMethod]
        public void UpdateDoesNotUpdateSameInstanceInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            SimpleEntity simpleEntity = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Age", 11);
            values.Add("Name", "name1");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);

            int affectedRows = view.TestUpdate(keys, values);

            Assert.AreNotSame(simpleEntity, view.TestGetData()[0]);
        }

        [TestMethod]
        public void UpdateFiresDataSourceViewChangedEventAfterUpdatingItemInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Age", 11);
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            bool dataSourceViewChangedFiredAfterUpdating = false;
            view.DataSourceViewChanged += delegate(object sender, EventArgs e)
            {
                dataSourceViewChangedFiredAfterUpdating = (((SimpleEntity)view.TestGetData()[0]).Age == 11); 
            };

            int affectedRows = view.TestUpdate(keys, values);

            Assert.IsTrue(dataSourceViewChangedFiredAfterUpdating);
        }

        [TestMethod]
        public void UpdateFiresUpdatingEventBeforeUpdatingItemInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            bool itemUpdated = false;
            view.Updating += delegate(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
            {
                itemUpdated = (((SimpleEntity)view.TestGetData()[0]).Name == "name1");
            };

            view.TestUpdate(keys, values);

            Assert.IsFalse(itemUpdated);
        }

        [TestMethod]
        public void UpdateUpdatesInstanceWithNewValuesInUpdatingEvent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(ComplexEntity));
            view.TestGetData().Add(new ComplexEntity(1));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            SimpleEntity simpleEntity = new SimpleEntity();
            view.Updating += delegate(object sender, ObjectContainerDataSourceUpdatingEventArgs e)
            {
                e.NewValues["SimpleEntity"] = simpleEntity;
            };

            view.TestUpdate(keys, new OrderedDictionary());

            Assert.AreSame(simpleEntity, ((ComplexEntity)view.TestGetData()[0]).SimpleEntity);
        }

        [TestMethod]
        public void UpdateFiresUpdatingEventWithKeysAndNewValuesAndOldValues()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary newValues = new OrderedDictionary(1);
            newValues.Add("Name", "name1");
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary oldValues = new OrderedDictionary(1);
            oldValues.Add("Name", "name");
            ObjectContainerDataSourceUpdatingEventArgs eventArgs = null;
            view.Updating += delegate(object sender, ObjectContainerDataSourceUpdatingEventArgs e) { eventArgs = e; };

            view.TestUpdate(keys, newValues, oldValues);

            Assert.AreEqual(1, eventArgs.NewValues.Count);
            Assert.AreEqual(1, eventArgs.OldValues.Count);
            Assert.AreEqual(1, eventArgs.Keys.Count);
            Assert.AreEqual(1, eventArgs.Keys["Id"]);
            Assert.AreEqual("name1", eventArgs.NewValues["Name"]);
            Assert.AreEqual("name", eventArgs.OldValues["Name"]);
        }

        [TestMethod]
        public void UpdateFiresUpdatingEventNotCanceled()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            ObjectContainerDataSourceUpdatingEventArgs eventArgs = null;
            view.Updating += delegate(object sender, ObjectContainerDataSourceUpdatingEventArgs e) { eventArgs = e; };

            view.TestUpdate(keys, new OrderedDictionary());

            Assert.IsFalse(eventArgs.Cancel);
        }

        [TestMethod]
        public void IfUpdatingEventCanceledViewDoesNotUpdateItemNorFiresUpdatedEventNorFiresDataSourceViewChangedEventAndReturns0()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            bool updatedFired = false;
            bool dataSourceViewChangedFired = false;
            view.Updating += delegate(object sender, ObjectContainerDataSourceUpdatingEventArgs e) { e.Cancel = true; };
            view.Updated += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { updatedFired = true; };
            view.DataSourceViewChanged += delegate(object sender, EventArgs e) { dataSourceViewChangedFired = true; };

            int affectedRows = view.TestUpdate(keys, values);

            Assert.AreEqual(0, affectedRows);
            Assert.AreEqual("name", ((SimpleEntity)view.TestGetData()[0]).Name);
            Assert.IsFalse(updatedFired);
            Assert.IsFalse(dataSourceViewChangedFired);
        }

        [TestMethod]
        public void UpdateFiresUpdatedEventAfterUpdatingTheItemInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            SimpleEntity simpleEntity = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity);
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            bool itemUpdated = false;
            view.Updated += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e)
            {
                itemUpdated = ((SimpleEntity)view.TestGetData()[0]).Name == "name1";
                eventArgs = e;
            };

            view.TestUpdate(keys, values);

            Assert.IsTrue(itemUpdated);
            Assert.AreEqual(1, eventArgs.AffectedRows);
        }

        [TestMethod]
        public void UpdateFiresUpdatedEventWithNewInstance()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            SimpleEntity simpleEntity = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity);
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Updated += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestUpdate(keys, values);

            Assert.AreNotSame(simpleEntity, eventArgs.Instance);
        }

        [TestMethod]
        public void IfUpdateDoesNotFindObjectInDataListThenCreatesOneAndFiresDeletedEvent()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Updated += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestUpdate(keys, values);

            Assert.AreEqual(0, view.TestGetData().Count);
            Assert.AreEqual(1, ((SimpleEntity)eventArgs.Instance).Id);
            Assert.AreEqual("name1", ((SimpleEntity)eventArgs.Instance).Name);
        }

        [TestMethod]
        public void IfUpdateDoesNotFindObjectInDataListReturns0AffectedRows()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Name", "name1");
            ObjectContainerDataSourceStatusEventArgs eventArgs = null;
            view.Updated += delegate(object sender, ObjectContainerDataSourceStatusEventArgs e) { eventArgs = e; };

            view.TestUpdate(keys, values);

            Assert.AreEqual(0, eventArgs.AffectedRows);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateThrowsIfNullKeys()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.TestUpdate(null, new OrderedDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateThrowsIfEmptyKeys()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));

            view.TestUpdate(new OrderedDictionary(), new OrderedDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateThrowsIfValuesContainsUnexistantProperty()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            OrderedDictionary keys = new OrderedDictionary(1);
            keys.Add("Id", 1);
            OrderedDictionary values = new OrderedDictionary(1);
            values.Add("Unexistant", "unexistant");

            view.TestUpdate(keys, values);
        }

        #endregion

        #region Select Method

        [TestMethod]
        public void SelectReturnsItemsInDataList()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            object obj = new object();
            view.TestGetData().Add(obj);

            List<object> result = view.TestSelect<object>();
            
            Assert.IsTrue(result.Contains(obj));
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SelectReturnsItemsNotSortedIfUsingServerSorting()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = true;
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Id"));

            Assert.IsTrue(result[0].Id > result[1].Id);
        }

        [TestMethod]
        public void SelectReturnsItemsSortedIfNotUsingServerSorting()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            
            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Id"));

            Assert.IsTrue(result[0].Id < result[1].Id);
        }

        [TestMethod]
        public void SelectFiresSelectingEventWithDataSourceSelectArguments()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            ObjectContainerDataSourceSelectingEventArgs eventArgs = null;
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs e) { eventArgs = e; };

            DataSourceSelectArguments arguments = new DataSourceSelectArguments();
            view.TestSelect<object>(arguments);

            Assert.AreSame(arguments, eventArgs.Arguments);
        }

        [TestMethod]
        public void SelectFiresSelectingEventNotCanceled()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            ObjectContainerDataSourceSelectingEventArgs eventArgs = null;
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs e) { eventArgs = e; };

            view.TestSelect<object>();

            Assert.IsFalse(eventArgs.Cancel);
        }

        [TestMethod]
        public void SelectIfSelectingCanceledReturnsNull()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs e) { e.Cancel = true; };

            List<object> result = view.TestSelect<object>();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void SelectReturnsFirstPageIfNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);
                        
            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 1));

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(simpleEntity1, result[0]);
        }

        [TestMethod]
        public void SelectReturnsSecondPageIfNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(1, 1));

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(simpleEntity2, result[0]);
        }

        [TestMethod]
        public void SelectReturnsItemsInLastPageIfMaximumRowsExceedsTotalItemCountAndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(1, 10));

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(simpleEntity2, result[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SelectThrowsIfStartRowIndexEqualsDataCountAndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity1);

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SelectThrowsIfStartRowIndexIsGreaterThanDataCountAndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity1);

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(2, 1));
        }

        [TestMethod]
        public void SelectDoesNotThrowIfStartRowIndexIs0AndDataCountIs0AndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 1));
        }

        [TestMethod]
        public void SelectReturnsAllItemsIfMaximumRowsIs0AndStartRowIndexIs0AndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 0));

            Assert.AreEqual(2, result.Count);
            Assert.AreSame(simpleEntity1, result[0]);
            Assert.AreSame(simpleEntity2, result[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectThrowsIfMaximumRowsGreaterThan0AndStartRowIndexLessThan0AndNotUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = false;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(-1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SelectThrowsIfUsingServerPagingAndTotalRowCountWasNotSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = true;

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SelectThrowsIfUsingServerPagingAndSelectingEventWasNotHandled()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = true;
            view.TotalRowCount = 1;

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 1));
        }

        [TestMethod]
        public void SelectDoesNotPageIfUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = true;
            view.TotalRowCount = 2;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs args) { };
            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(0, 1));

            Assert.AreEqual(2, result.Count);
            Assert.AreSame(simpleEntity1, result[0]);
            Assert.AreSame(simpleEntity2, result[1]);
        }

        [TestMethod]
        public void SelectReturnsTotalRowCountSetIfUsingServerPaging()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = true;
            view.TotalRowCount = 10;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            SimpleEntity simpleEntity2 = new SimpleEntity(2, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.TestGetData().Add(simpleEntity2);
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs e) { };

            DataSourceSelectArguments arguments = new DataSourceSelectArguments(0, 1);
            arguments.RetrieveTotalRowCount = true;
            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(arguments);

            Assert.AreEqual(10, arguments.TotalRowCount);
        }

        [TestMethod]
        public void SelectDoesNotThrowIfUsingServerPagingAndRowIndexIsOutOfRange()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerPaging = true;
            view.TotalRowCount = 1;
            SimpleEntity simpleEntity1 = new SimpleEntity(1, "name", 10);
            view.TestGetData().Add(simpleEntity1);
            view.Selecting += delegate(object sender, ObjectContainerDataSourceSelectingEventArgs args) { };

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(10, 1));
        }

        [TestMethod]
        public void SelectReturnsItemsSortedIfNotUsingServerSorting_ASC()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Id ASC"));

            Assert.IsTrue(result[0].Id < result[1].Id);
        }

        [TestMethod]
        public void SelectReturnsItemsSortedIfNotUsingServerSorting_DESC()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Id DESC"));

            Assert.IsTrue(result[0].Id > result[1].Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectThrowsIfSortingExpressionIsNotASCNorDESC()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Id WRONG"));
        }

        [TestMethod]
        public void SelectReturnsItemsNotSortedIfNotUsingServerSortingAndSortExpressionIsEmpty()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new SimpleEntity(2, "name", 10));
            view.TestGetData().Add(new SimpleEntity(1, "name", 10));

            List<SimpleEntity> result = view.TestSelect<SimpleEntity>(new DataSourceSelectArguments(String.Empty));

            Assert.IsTrue(result[0].Id > result[1].Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SelectThrowsIfNotUsingServerSortingAndPropertyIsNotComparable()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(NonComparableSimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new NonComparableSimpleEntity());

            view.TestSelect<object>(new DataSourceSelectArguments("NonComparable"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectThrowsIfNotUsingServerSortingAndPropertyDoesNotExist()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(SimpleEntity));
            view.UsingServerSorting = false;
            view.TestGetData().Add(new SimpleEntity());

            view.TestSelect<SimpleEntity>(new DataSourceSelectArguments("Unexistant"));
        }

        [TestMethod]
        public void SelectReturnsDataListCountIfTotalRowCountWasNotSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            view.TestGetData().Add(new object());
            view.TestGetData().Add(new object());
            DataSourceSelectArguments arguments = new DataSourceSelectArguments();
            arguments.RetrieveTotalRowCount = true;
            
            view.TestSelect<object>(arguments);

            Assert.AreEqual(2, arguments.TotalRowCount);
        }

        [TestMethod]
        public void SelectReturnsTotalRowCountIfTotalRowCountWasSet()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            view.TestGetData().Add(new object());
            view.TestGetData().Add(new object());
            view.TotalRowCount = 10;
            DataSourceSelectArguments arguments = new DataSourceSelectArguments();
            arguments.RetrieveTotalRowCount = true;

            view.TestSelect<object>(arguments);

            Assert.AreEqual(10, arguments.TotalRowCount);
        }

        [TestMethod]
        public void SelectReturnsNegativeTotalRowCountIfTotalRowCountIsNotRequested()
        {
            TestableObjectContainerDataSourceView view = new TestableObjectContainerDataSourceView(typeof(object));
            view.TestGetData().Add(new object());
            DataSourceSelectArguments arguments = new DataSourceSelectArguments();
            arguments.RetrieveTotalRowCount = false;

            view.TestSelect<object>(arguments);

            Assert.IsTrue(arguments.TotalRowCount < 0);
        }

        #endregion
    }

    class TestableObjectContainerDataSourceView : ObjectContainerDataSourceView
    {
        public TestableObjectContainerDataSourceView()
            : base(new ObjectContainerDataSource(), String.Empty)
        {
        }

        public TestableObjectContainerDataSourceView(Type dataObjectType)
            : this()
        {
            DataObjectTypeName = TypeHelper.GetFullTypeName(dataObjectType);
        }

        public List<object> TestGetData()
        {
            return base.Data;
        }

        public int TestInsert(OrderedDictionary values)
        {
            int insertedRows = 0;
            Exception exception = null;
            base.Insert(values, delegate(int affectedRows, Exception ex) { insertedRows = affectedRows; exception = ex; return true; });
            if (exception != null)
                throw exception;

            return insertedRows;
        }

        public int TestDelete(OrderedDictionary keys, OrderedDictionary oldValues)
        {
            int insertedRows = 0;
            Exception exception = null;
            base.Delete(keys, oldValues, delegate(int affectedRows, Exception ex) { insertedRows = affectedRows; exception = ex; return true; });
            if (exception != null)
                throw exception;

            return insertedRows;
        }

        public int TestUpdate(OrderedDictionary keys, OrderedDictionary newValues, OrderedDictionary oldValues)
        {
            int updatedRows = 0;
            Exception exception = null;
            base.Update(keys, newValues, oldValues, delegate(int affectedRows, Exception ex) { updatedRows = affectedRows; exception = ex; return true; });
            if (exception != null)
                throw exception;

            return updatedRows;
        }

        public int TestUpdate(OrderedDictionary keys, OrderedDictionary newValues)
        {
            return TestUpdate(keys, newValues, null);
        }

        public List<T> TestSelect<T>(DataSourceSelectArguments arguments)
        {
            List<T> items = new List<T>();
            IEnumerable returnedData = null;
            base.Select(arguments, delegate(IEnumerable data) 
            {
                returnedData = data;                
            });
            if (returnedData == null)
                return null;

            foreach (object item in returnedData)
                items.Add((T)item);

            return items;
        }

        public List<T> TestSelect<T>()
        {
            DataSourceSelectArguments arguments = new DataSourceSelectArguments();
            return TestSelect<T>(arguments);
        }

        public int GetTotalRowCount()
        {
            return base.TotalRowCount;
        }
    }

    class SimpleEntity
    {
        private int _age;
        private string _name;
        private int _id;

        public SimpleEntity()
        {

        }

        public SimpleEntity(int id, string name, int age)
        {
            _id = id;
            _name = name;
            _age = age;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    class SimpleEntityWithReadOnlyProperty
    {
        private string _name = null;

        public string Name
        {
            get { return _name; }
        }

    }

    class SimpleEntityNoPublicParameterlessConstructor
    {
        private SimpleEntityNoPublicParameterlessConstructor() { }
    }

    class NonComparableSimpleEntity
    {
        private SimpleEntity _nonComparable;

        public SimpleEntity NonComparable
        {
            get { return _nonComparable; }
            set { _nonComparable = value; }
        }
    }

    class ComplexEntity
    {
        private SimpleEntity _simpleEntity = new SimpleEntity();
        private int _id;

        public ComplexEntity()
        {
               
        }

        public ComplexEntity(int id)
        {
            _id = id;
        }
        public SimpleEntity SimpleEntity
        {
            get { return _simpleEntity; }
            set { _simpleEntity = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    static class TypeHelper
    {
        public static string GetFullTypeName(Type type)
        {
            return type.FullName + ", " + type.Assembly.FullName;
        }
    }

}
