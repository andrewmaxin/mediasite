Imports System.Data

Partial Class UserDelete
    Inherits System.Web.UI.Page
    Public m_objEditingUser As clsSecurity
    Public m_objUserToEdit As clsSecurity
    Private m_objPageConn As OleDb.OleDbConnection

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



    End Sub

    Protected Sub cmdConfirm_Click(sender As Object, e As System.EventArgs) Handles cmdConfirm.Click
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

            If m_objUserToEdit.LoadUser(lngID) = False Then

                Response.Write("ERROR! - Couldnt load user!")
                Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
            End If

            Try

                m_objPageConn = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
                m_objPageConn.Open()

                Dim strSQL As String = "Delete FROM USERS WHERE UserID=" & lngID

                Dim objCommand As New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                strSQL = "DELETE FROM TeamMember WHERE UserID = " & lngID
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                strSQL = "DELETE FROM Set WHERE UserID = " & lngID
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                strSQL = "DELETE FROM SetListItems WHERE UserID = " & lngID
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                strSQL = "DELETE FROM Bookmarks WHERE UserID = " & lngID
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                strSQL = "DELETE FROM ActivityLog WHERE UID = " & lngID
                objCommand = New OleDb.OleDbCommand(strSQL, m_objPageConn)
                objCommand.ExecuteNonQuery()

                m_objPageConn.Dispose()
                m_objPageConn = Nothing

                m_objUserToEdit = Nothing

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close(); window.opener.location.reload();", True)

        End If

    End Sub



    Protected Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close();", True)

    End Sub
End Class
