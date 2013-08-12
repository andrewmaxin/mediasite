Imports System.Data
Imports System.Data.OleDb

Partial Class filedelete
    Inherits System.Web.UI.Page

    Private intContent As Integer

    Public SecInst As clsSecurity

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'find out who this person is
        If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

        SecInst = HttpContext.Current.Session("Security")


        lngID.Value = CInt(Val(Request.QueryString("dbid") & ""))
        intContent = CInt(Val(Request.QueryString("contentid") & ""))

        'response.Write(lngID)
        If lngID.Value = 0 Then

            '%>
            Response.Write("That was an invalid ID Number.")
            Response.End()
            '<%

        Else

            If IsPostBack = False Then
                Try
                    Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                    mvarconnSQL.Open()

                    If mvarconnSQL.State = ConnectionState.Open Then

                        Dim cmd As New OleDb.OleDbCommand("SELECT * From [Songs] Where [ID] = " & lngID.Value & "", mvarconnSQL)
                        Dim data As OleDb.OleDbDataReader

                        cmd.CommandType = CommandType.Text
                        data = cmd.ExecuteReader()

                        If data.Read = False Then

                            Response.Write("Song Not Found.")
                            Response.End()

                        Else

                            lngID.Value = lngID.Value
                            intFileType.Value = intContent
                            If intContent = 0 Then
                                lblFileName.Text = data.Item("Mp3Link") & ""
                            Else
                                lblFileName.Text = data.Item("pptLink") & ""
                            End If

                            If lblFileName.Text = "" Then
                                Label1.Text = ""
                                lblTitle.Text = "NO FILE FOUND"
                                cmdConfirm.Visible = False
                                cmdCancel.Text = "Close"

                            End If

                        End If

                        data.Close()
                        data = Nothing

                        cmd.Dispose()
                        cmd = Nothing

                        mvarconnSQL.Close()
                        mvarconnSQL = Nothing

                    End If

                Catch ex As Exception

                End Try
            End If

        End If

        'cmdConfirm.Attributes.Add("onclick", "javascript: window.close(); return false;")
        cmdCancel.Attributes.Add("onclick", "javascript: window.close(); return false;")
        

    End Sub

   

    Protected Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        Dim intRes As Integer = 0
        Dim blnSecurityCheck As Boolean = False
        Dim cmd As OleDb.OleDbCommand


        Try

            If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

            SecInst = HttpContext.Current.Session("Security")


            'Check Permissions
            If SecInst.CheckPermission(clsSecurity.Permission_ActionCodes.act_PPT_Delete) And intFileType.Value = 1 Then
                blnSecurityCheck = True
            End If

            If SecInst.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Delete) And intFileType.Value = 0 Then
                blnSecurityCheck = True
            End If


            Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            mvarconnSQL.Open()

            If mvarconnSQL.State = ConnectionState.Open Then

                If intFileType.Value = 0 Then
                    cmd = New OleDb.OleDbCommand("UPDATE [Songs] SET Mp3Link='' Where [ID] = " & lngID.Value & "", mvarconnSQL)
                Else
                    cmd = New OleDb.OleDbCommand("UPDATE [Songs] SET pptLink='' Where [ID] = " & lngID.Value & "", mvarconnSQL)
                End If

                cmd.CommandType = CommandType.Text
                intRes = cmd.ExecuteNonQuery()

                If intRes > 0 Then
                    'Datebase Updated!

                    If clsSharedFns.DeleteFileFromRepos(lngID.Value, intFileType.Value) = True Then
                        lblFileName.Text = "File Deleted"
                    Else
                        lblFileName.Text = "Actual file could not be deleted."
                    End If

                    cmdCancel.Text = "Close"
                    cmdConfirm.Visible = False

                End If


                cmd.Dispose()
                cmd = Nothing

                mvarconnSQL.Close()
                mvarconnSQL = Nothing

            End If

        Catch ex As Exception
            lblFileName.Text = ex.ToString
        End Try


        'lblFileName.Text = "File Deleted"
        cmdCancel.Text = "Close"
        cmdConfirm.Visible = False




        
    End Sub



End Class
