Imports System.Data

Partial Class SongDelete
    Inherits System.Web.UI.Page


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load


        Dim intSongID As Integer = 0
        Dim strUnreg As String = 0
        Dim blnUnDelete As Boolean = False
        Dim m_objSQLConn As OleDb.OleDbConnection = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
        Dim strSQL As String = ""
        Dim objSec As clsSecurity = Nothing

        Try
            objSec = HttpContext.Current.Session.Item("Security")
        Catch ex As Exception

        End Try

        Try
            intSongID = CInt(Request.QueryString("dbid").ToString)
        Catch ex As Exception

        End Try

        Try
            strUnreg = Request.QueryString("undelete").ToString
        Catch ex As Exception

        End Try

        blnUnDelete = (strUnreg.ToLower = "true")

        If Not objSec Is Nothing Then

            If blnUnDelete = True Then
                'Remove
                strSQL = "UPDATE Songs SET Deleted=0 WHERE id=" & intSongID
            Else
                'Add
                strSQL = "UPDATE Songs SET Deleted=1 WHERE id=" & intSongID
            End If

            m_objSQLConn.Open()
            Dim objCommand As New OleDb.OleDbCommand(strSQL, m_objSQLConn)
            Dim intTotalRecords As Integer

            Try

                intTotalRecords = objCommand.ExecuteScalar

                objCommand.Dispose()
                objCommand = Nothing


            Catch ex As Exception

            End Try

            m_objSQLConn.Close()
            m_objSQLConn.Dispose()
            m_objSQLConn = Nothing

        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)

    End Sub

End Class
