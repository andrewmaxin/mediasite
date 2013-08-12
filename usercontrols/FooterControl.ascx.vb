
Partial Class usercontrols_FooterControl
    Inherits System.Web.UI.UserControl

    Public lngResults As Integer = 0
    Public numpages As Integer = 0
    Public intColSpan As Integer = 0
    Public strLoggedIn As String = ""

    Public Property Pages As Integer
        Get
            Return numpages
        End Get
        Set(value As Integer)
            numpages = value
        End Set
    End Property

    Public Property ResultCount As Integer
        Get
            Return lngResults
        End Get
        Set(value As Integer)
            lngResults = value
        End Set
    End Property
    Public Function sql_CountMySongList() As Long
        Return 0
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objSec As clsSecurity

        Try

            objSec = HttpContext.Current.Session.Item("Security")
            strLoggedIn = objSec.UserName

        Catch ex As Exception

        End Try

    End Sub
End Class
