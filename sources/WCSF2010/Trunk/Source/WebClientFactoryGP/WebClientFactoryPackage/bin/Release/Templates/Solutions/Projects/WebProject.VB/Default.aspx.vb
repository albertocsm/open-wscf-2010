Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports $ShellNamespace$.Views
Imports Microsoft.Practices.ObjectBuilder

Partial Class ShellDefault
    Inherits Microsoft.Practices.CompositeWeb.Web.UI.Page
    Implements IDefaultView

    Private _presenter As DefaultViewPresenter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        If (Not Me.IsPostBack) Then
            Me._presenter.OnViewInitialized()
        End If

        Me._presenter.OnViewLoaded()

    End Sub

    <CreateNew()> _
    Public Property Presenter() As DefaultViewPresenter
        Get
            Return Me._presenter
        End Get
        Set(ByVal value As DefaultViewPresenter)
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            Me._presenter = value
            Me._presenter.View = Me
        End Set
    End Property

End Class
