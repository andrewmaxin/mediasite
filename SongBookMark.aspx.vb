Imports System.Data

Partial Class SongBookMark
    Inherits System.Web.UI.Page



    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load


        Dim intSongID As Integer = 0
        Dim strUnreg As String = 0
        Dim blnUnBookmark As Boolean = False
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
            strUnreg = Request.QueryString("unbkmark").ToString
        Catch ex As Exception

        End Try

        blnUnBookmark = (strUnreg.ToLower = "true")

        If Not objSec Is Nothing Then

            If blnUnBookmark = True Then
                'Remove
                strSQL = "Delete from Bookmarks WHERE songid=" & intSongID & " AND userid=" & objSec.UserID
            Else
                'Add
                strSQL = "Insert INTO Bookmarks (songid, userid) VALUES(" & intSongID & "," & objSec.UserID & ")"
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

        'If blnUnBookmark Then

        '    Response.Redirect("http://media2.cdac.ca/main.aspx?Tab=2")
        'Else
        '    Response.Redirect("http://media2.cdac.ca/main.aspx?Tab=1")

        'End If
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)

    End Sub

End Class
