Imports System.Data

Partial Class ToggleLockout
    Inherits System.Web.UI.Page

    Public m_objEditingUser As clsSecurity
    Public m_objUserToEdit As clsSecurity
    Private m_objPageConn As OleDb.OleDbConnection

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim lngID As Integer
        If Not Page.IsPostBack = True Then

            If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

            m_objEditingUser = HttpContext.Current.Session("Security")

            lngID = CInt(Val(Request.QueryString("ID") & ""))

            m_objUserToEdit = New clsSecurity()

            If m_objEditingUser.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = False Then
                Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
            End If

            If lngID = 0 Then
                'No User ID spec'd --- Bounce!
                Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
            End If

            If m_objUserToEdit.LoadUser(lngID) = False Then Response.Write("ERROR! - Couldnt load user!")

            Try

                m_objPageConn = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
                m_objPageConn.Open()


                Dim objCommand As New OleDb.OleDbCommand("SELECT userlocked from Users_New WHERE UserID=" & lngID, m_objPageConn)
                Dim intLocked As Integer = objCommand.ExecuteScalar()
                objCommand.Dispose()
                objCommand = Nothing

                Response.Write("Current State: " & intLocked)
                Dim strSQL As String = "UPDATE Users_New SET userlocked=" & IIf((intLocked = 1), "0", "1") & " WHERE UserID=" & lngID

                Response.Write(strSQL)
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteScalar()

                m_objPageConn.Dispose()
                m_objPageConn = Nothing

                m_objUserToEdit = Nothing

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close(); window.opener.location.reload();", True)

        End If

    End Sub


    'Protected Sub cmdUnloadButton_Click(sender As Object, e As System.EventArgs) Handles cmdUnloadButton.Click
    '    cmdUnloadButton.Attributes.Add("onclick", "javascript: window.close(); return false;")
    'End Sub
End Class
