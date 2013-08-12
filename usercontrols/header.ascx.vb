
Partial Class usercontrols_WebUserControl
    Inherits System.Web.UI.UserControl

    Public objHdrSec As clsSecurity


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            objHdrSec = HttpContext.Current.Session.Item("Security")

            If objHdrSec Is Nothing Then

                Response.Redirect("Logout.aspx")

            End If

        Catch ex As Exception

        End Try

    End Sub
End Class
