Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports $ModuleNamespace$.Views
Imports $ModuleTestProjectNamespace$

<TestClass()> _
Public Class DefaultViewPresenterFixture

    Public Sub New()

    End Sub

#Region "Additional test attributes"

    ' You can use the following additional attributes as you write your tests:

    ' Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()> _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub

    ' Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()> _
    'Public Shared Sub MyClassCleanup()
    'End Sub

    ' Use TestInitialize to run code before running each test 
    '<TestInitialize()> _
    'Public Sub MyTestInitialize()
    'End Sub

    ' Use TestCleanup to run code after each test has run
    '<TestCleanup()> _
    'Public Sub MyTestCleanup()
    'End Sub

#End Region

    ' Eg.: this is a unit tests taken and translated from the reference implementation
    '
    '<TestMethod()> _
    'Public Sub OnViewLoadedCallsControllerGetTransfersAndSetsTransfersInView()

    '    Dim controller As MockElectronicFundsTransferController = New MockElectronicFundsTransferController()
    '    Dim transfer As Transfer = New Transfer()
    '    controller.Transfers = New Transfer() {transfer}
    '    Dim presenter As DefaultViewPresenter = New DefaultViewPresenter(controller)
    '    Dim view As MockDefaultView = New MockDefaultView()
    '    presenter.View = view

    '    presenter.OnViewLoaded()

    '    Assert.IsTrue(controller.GetTransfersCalled)
    '    Assert.IsTrue(view.TransfersSet)
    '    Assert.AreEqual(1, view.Transfers.Length)
    '    Assert.AreSame(transfer, view.Transfers(0))
    'End Sub

End Class

Namespace Mocks

    Class MockDefaultView
        Implements IDefaultView

    End Class

End Namespace

