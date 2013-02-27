Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.Practices.CompositeWeb
Imports Microsoft.Practices.CompositeWeb.Services
Imports Microsoft.Practices.CompositeWeb.Interfaces
Imports Microsoft.Practices.CompositeWeb.Configuration
Imports Microsoft.Practices.CompositeWeb.Authorization
Imports Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services

Public Class ShellModuleInitializer
    Inherits ModuleInitializer

    Private Const AuthorizationSectionIdentifier As String = "compositeWeb/authorization"

    Public Overrides Sub Load(ByVal container As CompositionContainer)

        MyBase.Load(container)

        AddGlobalServices(container.Parent.Services)
        AddModuleServices(container.Services)
        RegisterSiteMapInformation(container.Services.Get(Of ISiteMapBuilderService)(True))

    End Sub

    Protected Overridable Sub AddGlobalServices(ByVal globalServices As IServiceCollection)

        globalServices.AddNew(Of EnterpriseLibraryAuthorizationService, IAuthorizationService)()
        globalServices.AddNew(Of SiteMapBuilderService, ISiteMapBuilderService)()
    End Sub

    Protected Overridable Sub AddModuleServices(ByVal moduleServices As IServiceCollection)

        ' TODO: register services that can be accesed only by the Shell module

    End Sub

    Protected Overridable Sub RegisterSiteMapInformation(ByVal siteMapBuilderService As ISiteMapBuilderService)

        Dim moduleNode As SiteMapNodeInfo = New SiteMapNodeInfo("Home", "~/Default.aspx", "Home", "Home")
        siteMapBuilderService.AddNode(moduleNode)

        siteMapBuilderService.RootNode.Url = "~/Default.aspx"
        siteMapBuilderService.RootNode.Title = "$ApplicationNameArgument$"

        ' TODO: register other site map nodes for pages that belong to the website root
    End Sub

    Public Overrides Sub Configure(ByVal services As IServiceCollection, ByVal moduleConfiguration As System.Configuration.Configuration)

        Dim authorizationRuleService As IAuthorizationRulesService = Services.Get(Of IAuthorizationRulesService)()
        If (Not authorizationRuleService Is Nothing) Then
            Dim authorizationSection As AuthorizationConfigurationSection = TryCast(moduleConfiguration.GetSection(AuthorizationSectionIdentifier), AuthorizationConfigurationSection)
            If (Not authorizationSection Is Nothing) Then
                For Each ruleElement As AuthorizationRuleElement In authorizationSection.ModuleRules
                    authorizationRuleService.RegisterAuthorizationRule(ruleElement.AbsolutePath, ruleElement.RuleName)
                Next
            End If
        End If

    End Sub

End Class
