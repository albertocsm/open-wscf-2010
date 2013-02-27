Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Microsoft.Practices.CompositeWeb
Imports Microsoft.Practices.CompositeWeb.Services
Imports Microsoft.Practices.CompositeWeb.Interfaces

<TestClass()> _
Public Class $ModuleName$ModuleInitializerFixture

    Public Sub New()

    End Sub

    <TestMethod()> _
    Public Sub $ModuleName$RegisteredOnSiteMap()

        Dim ModuleInitializer As TestableModuleInitializer = New TestableModuleInitializer()
        Dim siteMapBuilder As SiteMapBuilderService = New SiteMapBuilderService()

        ModuleInitializer.RegisterSiteMapInformation(siteMapBuilder)

        Dim node As SiteMapNodeInfo = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)(0)

        Assert.AreEqual("$ModuleName$", node.Key)

    End Sub

End Class

Public Class TestableModuleInitializer
    Inherits $ModuleName$ModuleInitializer

    Public Shadows Sub RegisterSiteMapInformation(ByVal siteMapBuilder As ISiteMapBuilderService)
        MyBase.RegisterSiteMapInformation(siteMapBuilder)
    End Sub

End Class
