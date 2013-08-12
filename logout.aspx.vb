
Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objSec As clsSecurity = HttpContext.Current.Session.Item("Security")

        If objSec Is Nothing Then

            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage("default.aspx")

        Else
            ' objSec.LogUserActivity(clsSecurity.Permission_ActionCodes.act_Logout, 0)
            objSec.Logout()
            HttpContext.Current.Session.Remove("Security")

        End If
        
    End Sub
End Class
