Imports System.Data
Imports System.Data.OleDb

Partial Class fileupload
    Inherits System.Web.UI.Page

    Public ResMessage As String = ""
    Public strTempFile As String = ""
    Public strTempFileName As String = ""
    Public blnStreaming As Boolean = False
    Public strSongTitle As String = ""
    Private intFileID As Integer = 0
    Private intfileType As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim intFileID As Integer = 0
        'Dim intFileType As Integer = 0
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

            If strID <> "" Then
                intFileID = CInt(strID)
            End If

            If strTyp <> "" Then
                intfileType = CInt(strTyp)
            End If

            If intFileID > 0 And (intfileType = 0 Or intfileType = 1) Then
                'Input is Valid

                'Check Permissions
                If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinUpload) And intfileType = 1 Then
                    blnSecurityCheck = True
                End If

                If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Upload) And intfileType = 0 Then
                    blnSecurityCheck = True
                End If

                'Check if File already exists
                Dim strFilename As String = clsSharedFns.GetFileNameString(intFileID, intfileType) & ".dat"
                strFilename = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot") & strFilename
                If System.IO.File.Exists(strFilename) = True Then
                    ResMessage = "You cannot upload a file to this location because a file already exists in that location. Notify the system admin to remove the existing file if need be."
                    blnSecurityCheck = False
                End If

                If blnSecurityCheck = True Then
                    'We are Clear to upload!

                    'Dim mvarconnSQL As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                    'mvarconnSQL.Open()

                    'If mvarconnSQL.State = ConnectionState.Open Then



                Else
                    'Security check Failed!
                    If ResMessage = "" Then ResMessage = "Sorry, you do not have the proper permissions to upload this file."

                End If

            Else

                ResMessage = "Sorry, the request was not deemed valid so a file cannot be uploaded."

            End If

        Catch FNFex As System.IO.FileNotFoundException

            ResMessage = "File not found on server. Please inform the webmaster at <a href=""mailto:" & ConfigurationManager.AppSettings("gsAdminEmail").ToString & """>" & ConfigurationManager.AppSettings("gsAdminEmail").ToString & "</a> to report the broken link for this song. <br/><br/>Sorry for the inconvienience."

        Catch ex As Exception
            ResMessage = ex.ToString

        End Try

        lblMessage.Text = ResMessage

    End Sub




    Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click

        If FileUpload1.HasFile = True Then

            Try
                Dim blnBlowAwayTempFile As Boolean = False
                Dim blnBlowAwayEncFile As Boolean = False
                Dim strFileName As String = System.IO.Path.GetFileName(FileUpload1.FileName)
                Dim strFileExtension As String = System.IO.Path.GetExtension(FileUpload1.FileName)
                Dim strUnencodedTempFile As String = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp") & "\" & FileUpload1.FileName
                Dim strEncodedFile As String = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot") & "\" & clsSharedFns.GetFileNameString(intFileID, intfileType) & ".dat"
                Dim strSQL As String = ""

                'System.IO.File.Delete(strUnencodedTempFile)
                clsSharedFns.DeleteFileFromRepos(strUnencodedTempFile)

                FileUpload1.SaveAs(strUnencodedTempFile)

                If System.IO.File.Exists(strUnencodedTempFile) = True Then

                    blnBlowAwayTempFile = True

                    Dim objCrypto As New clsCrypto
                    objCrypto.FileInput = strUnencodedTempFile
                    objCrypto.FileOutput = strEncodedFile
                    objCrypto.EncryptFile()

                    Dim mvarconnSQL As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                    mvarconnSQL.Open()

                    If mvarconnSQL.State = ConnectionState.Open Then

                        If intfileType = 0 Then
                            strSQL = "Update [Songs] SET Mp3Link = '" & Replace(strFileName, "'", "''") & "' WHERE ID = " & intFileID
                        Else
                            strSQL = "Update [Songs] SET pptLink = '" & Replace(strFileName, "'", "''") & "' WHERE ID = " & intFileID
                        End If

                        Dim cmd As New OleDb.OleDbCommand(strSQL, mvarconnSQL)

                        cmd.CommandType = CommandType.Text
                        cmd.ExecuteNonQuery()

                        cmd.Dispose()
                        cmd = Nothing

                        mvarconnSQL.Close()
                        mvarconnSQL.Dispose()
                        mvarconnSQL = Nothing

                    Else
                        ResMessage = "There was an error in uploading this file. The database would not accept the update."
                        blnBlowAwayEncFile = True
                    End If

                Else

                    ResMessage = "There was an error in uploading this file."

                End If


                If blnBlowAwayEncFile = True Then
                    System.IO.File.Delete(strEncodedFile)
                    clsSharedFns.DeleteFileFromRepos(strEncodedFile)
                End If

                If blnBlowAwayTempFile = True Then
                    'System.IO.File.Delete(strUnencodedTempFile)
                    clsSharedFns.DeleteFileFromRepos(strUnencodedTempFile)
                End If
                 
                cmdUnloadButton_Click(Nothing, New EventArgs)
                 
            Catch ex As Exception
                ResMessage = "There was an error in uploading this file: " & ex.ToString
            End Try

        End If

        lblMessage.Text = ResMessage

    End Sub


    Protected Sub cmdUnloadButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUnloadButton.Click
        Response.Write("<script language='javascript'> { window.close(); }</script>")
    End Sub

End Class
