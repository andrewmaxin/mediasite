Imports System.Data
Imports System.Data.OleDb
Imports System.Threading

Partial Class filedownload
    Inherits System.Web.UI.Page

    Public ResMessage As String = ""
    Public strTempFile As String = ""
    Public strTempFileName As String = ""
    Public blnStreaming As Boolean = False
    Public strSongTitle As String = ""

    Public Sub New()

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim intFileID As Integer = 0
        Dim intFileType As Integer = 0
        Dim blnStream As Boolean = False

        Dim objSec As clsSecurity
        Dim blnSecurityCheck As Boolean = False
        Dim strLocalFileName As String = ""
        Dim objCrypto As clsCrypto
        Dim strFileExtension As String = ""

        'contentid=0&dbid

        Try

            If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
            objSec = HttpContext.Current.Session.Item("Security")

            Dim strID As String = Left(Request.QueryString("dbid"), 10)
            Dim strTyp As String = Left(Request.QueryString("contentid"), 10)
            Dim strStream As String = Left(Request.QueryString("stream"), 10)

            If strID <> "" Then
                intFileID = CInt(strID)
            End If

            If strTyp <> "" Then
                intFileType = CInt(strTyp)
            End If

            If strStream <> "" Then
                blnStream = CBool(strStream)
                blnStreaming = True
            End If

            If intFileID > 0 And (intFileType = 0 Or intFileType = 1) Then
                'Input is Valid

                'Check Permissions
                If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinDownload) And intFileType = 1 Then
                    blnSecurityCheck = True
                End If

                If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) And intFileType = 0 Then
                    blnSecurityCheck = True
                End If

                If blnSecurityCheck = True Then
                    'We are Clear to download!

                    Dim mvarconnSQL As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                    mvarconnSQL.Open()

                    If mvarconnSQL.State = ConnectionState.Open Then

                        Dim cmd As New OleDb.OleDbDataAdapter("SELECT * FROM Songs WHERE ID = " & intFileID, mvarconnSQL)
                        Dim objDR As Data.DataRow = Nothing
                        Dim objDT As Data.DataTable
                        Dim objDS As New DataSet

                        cmd.SelectCommand.CommandType = CommandType.Text
                        cmd.Fill(objDS)

                        objDT = objDS.Tables(0)
                        If objDT.Rows.Count > 0 Then
                            objDR = objDT.Rows(0)
                        End If

                        'Populate Field Values
                        If Not objDR Is Nothing Then

                            If intFileType = 0 Then
                                strLocalFileName = objDR.Item("Mp3Link") & ""
                            Else
                                strLocalFileName = objDR.Item("pptLink") & ""
                            End If

                            strLocalFileName = Replace(strLocalFileName, " ", "_")
                            strLocalFileName = Replace(strLocalFileName, "'", "")
                            strLocalFileName = Replace(strLocalFileName, ",", "")
                            strFileExtension = "." & strLocalFileName.Substring(InStrRev(strLocalFileName, "."))
                            strSongTitle = strLocalFileName
                        End If
                        'ResMessage = strLocalFileName
                        mvarconnSQL.Close()
                        mvarconnSQL.Dispose()
                        mvarconnSQL = Nothing

                        Dim strSourcePath As String = ""
                        Dim strDestPath As String = ""
                        Dim strFileNameVal As String = ""
                        Dim lngFileLen As Integer

                        strSourcePath = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot").ToString & "\"
                        strDestPath = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp").ToString & "\"

                        strFileNameVal = clsSharedFns.GetFileNameString(intFileID, intFileType)
                        'ResMessage = strFileNameVal
                        strSourcePath += strFileNameVal & ".dat"
                        strDestPath += strFileNameVal & ".dat"

                        'If System.IO.File.Exists(strDestPath) Then System.IO.File.Delete(strDestPath)
                        clsSharedFns.DeleteFileFromRepos(strDestPath)

                        objCrypto = New clsCrypto
                        objCrypto.FileInput = strSourcePath
                        objCrypto.FileOutput = strDestPath
                        objCrypto.DecryptFile()

                        'Dim I As Integer
                        'For I = 1 To 10
                        '    If System.IO.File.Exists(strDestPath) = True Then Exit For
                        '    Thread.Sleep(100)
                        'Next

                        Try  'Don't let logging screw this up!

                            If intFileType = 0 Then
                                objSec.LogUserActivity(clsSecurity.Permission_ActionCodes.act_Mp3Download, intFileID)
                            Else
                                objSec.LogUserActivity(clsSecurity.Permission_ActionCodes.act_PPT_Download, intFileID)
                            End If

                        Catch ex As Exception

                        End Try


                        Dim objTemp As New System.IO.FileInfo(strDestPath)
                        lngFileLen = objTemp.Length

                        If blnStream = False Then
                            Response.Clear()
                            Response.AddHeader("Content-Disposition", "attachment;filename=""" & strLocalFileName & """")
                            Response.AddHeader("Content-Length", lngFileLen)
                            Response.Charset = "UTF-8"
                            Response.ContentType = "application/octet-stream" '"application/x-msdownload"
                            Response.Buffer = True
                            Response.WriteFile(strDestPath)
                            Response.End()

                            'PICKLE - This delete is not firing properly
                            Try
                                'System.IO.File.Delete(strDestPath)
                                clsSharedFns.DeleteFileFromRepos(strDestPath)
                            Catch ex As Exception

                            End Try

                        Else

                            'Show mp3 Player.
                            If System.IO.File.Exists(ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp").ToString & "\" & strFileNameVal & strFileExtension) = False Then
                                objTemp.MoveTo(ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp").ToString & "\" & strFileNameVal & strFileExtension)
                            End If

                            strTempFile = strFileNameVal & strFileExtension
                            strTempFileName = strTempFile

                        End If


                        objTemp = Nothing

                    Else

                        ResMessage = "Error. Database did not connect for this transaction."

                    End If

                    Else
                        'Security check Failed!
                        ResMessage = "Sorry, you do not have the proper permissions to download this file."

                    End If

            Else

                ResMessage = "Sorry, the request was not deemed valid so the file cannot be downloaded."

            End If
        Catch FNFex As System.IO.FileNotFoundException

            ResMessage = "File not found on server. Please inform the webmaster at <a href=""mailto:" & ConfigurationManager.AppSettings("gsAdminEmail").ToString & """>" & ConfigurationManager.AppSettings("gsAdminEmail").ToString & "</a> to report the broken link for this song. <br/><br/>Sorry for the inconvienience."

        Catch ex As Exception
            ResMessage = ex.ToString

        End Try

        lblMessage.Text = ResMessage

    End Sub

    Protected Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

        Try
            'System.IO.File.Delete(ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp").ToString & "\" & strTempFileName)
        Catch ex As Exception

        End Try

    End Sub
End Class
