
Partial Class login
    Inherits System.Web.UI.Page


    Sub btnLogin_OnClick(ByVal Src As Object, ByVal E As EventArgs)
        Dim objSec As New clsSecurity()
        Dim intRetCode As Integer

        objSec.UserName = txtUsername.Text
        objSec.Password = txtPassword.Text
        objSec.OrgID = CLng(cboChurch.SelectedValue)

        intRetCode = objSec.Login()

        'User Authentication HERE!
        Select Case intRetCode
            Case 0  'Not Found
                lblInvalid.Text = ConfigurationManager.AppSettings("lblAcctNotFound")
            Case -1 ' User Locked
                lblInvalid.Text = ConfigurationManager.AppSettings("lblAcctInActive")
            Case 99 ' Good Login
                HttpContext.Current.Session.Add("Security", objSec)
                ' objSec.LogUserActivity(clsSecurity.Permission_ActionCodes.act_Login, 0)
                FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, True)
            Case Else 'Error
                lblInvalid.Text = ConfigurationManager.AppSettings("lblErrorInLogin") & "(" & intRetCode & ")" & "<br/>" & objSec.LastError
        End Select

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)


    End Sub

    Public Sub New()

        If HttpContext.Current.Request.Params.Item("ReturnUrl").ToLower.Contains("logout.aspx") Then
            HttpContext.Current.Response.Redirect("default.aspx")
             
        End If
    End Sub
End Class
