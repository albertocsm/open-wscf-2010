Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.Practices.CompositeWeb
Imports Microsoft.Practices.CompositeWeb.Interfaces
Imports Microsoft.Practices.CompositeWeb.Services
Imports Microsoft.Practices.CompositeWeb.Configuration
Imports Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services


Public Class $ModuleName$ModuleInitializer
    Inherits ModuleInitializer

    Public Overrides Sub Load(ByVal container As CompositionContainer)

        MyBase.Load(container)

        AddGlobalServices(container.Services)
    End Sub

    Protected Overridable Sub AddGlobalServices(ByVal globalServices As IServiceCollection)

        ' TODO: add a service that will be visible to any module
    End Sub

    Public Overrides Sub Configure(ByVal services As IServiceCollection, ByVal moduleConfiguration As System.Configuration.Configuration)

    End Sub

End Class
