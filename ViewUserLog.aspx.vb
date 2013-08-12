Imports System.Data

Partial Class ViewUserLog
    Inherits System.Web.UI.Page

    Public ID As Integer

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim objSec As clsSecurity
        Dim blnSecurityCheck As Boolean = False

        If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
        objSec = HttpContext.Current.Session.Item("Security")

        Dim strID As String = Left(Request.QueryString("ID"), 10)
        ID = CInt(Val(strID))

        If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = True Or objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserAdd) = True Then

            Label1.Text = GetUserName(ID)

        Else

            Response.Write("<script language='javascript'> { window.close(); }</script>")

        End If



    End Sub

    Protected Sub cmdClose_Click(sender As Object, e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close(); }</script>")
    End Sub

    Private Function GetUserName(p_intID As Integer) As String

        Try
            Dim m_objPageConn As OleDb.OleDbConnection
            Dim strFirstName As String
            Dim strLastName As String


            m_objPageConn = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
            m_objPageConn.Open()


            Dim objCommand As New OleDb.OleDbCommand("SELECT firstname from Users_New WHERE UserID=" & p_intID, m_objPageConn)
            strFirstName = objCommand.ExecuteScalar()
            objCommand.Dispose()

            objCommand = New OleDb.OleDbCommand("SELECT lastname from Users_New WHERE UserID=" & p_intID, m_objPageConn)
            strLastName = objCommand.ExecuteScalar()
            objCommand.Dispose()
            objCommand = Nothing

            GetUserName = strFirstName & " " & strLastName

            m_objPageConn.Dispose()
            m_objPageConn = Nothing

        Catch ex As Exception

        End Try

    End Function

    Protected Sub cmdExport_Click(sender As Object, e As System.EventArgs) Handles cmdExport.Click



    End Sub



End Class
