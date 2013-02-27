Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.Practices.ObjectBuilder
Imports Microsoft.Practices.CompositeWeb

Namespace Views

    Public Class DefaultViewPresenter
        Inherits Presenter(Of IDefaultView)

        Private _controller As I$ModuleName$Controller

        Public Sub New(<CreateNew()> ByVal moduleController As I$ModuleName$Controller)
            _controller = moduleController
        End Sub

    End Class

End Namespace
