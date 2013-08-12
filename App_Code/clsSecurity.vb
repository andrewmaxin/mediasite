Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Security.Cryptography

Public Class clsSecurity
    Private Const SaltyGoodness As String = "KDNLKNNFLKVNXJ1415125125NKLNSDVLKNSELVNSLKNV"

    Public Enum Permissions
        siteadmin
        setadmin
        songview
        songedit
        mp3upload
        mp3dwload
        pptdwn
        pptupl
        songdel
        docbinUL
        docbinDL
        reporting
    End Enum

    Public Enum Permission_ActionCodes

        act_Login = 1            'User Logged in
        act_Logout = 99

        act_SongAdd = 2        'User Saved a New Song
        act_SongEdit = 3       'User Edited an Existing Song
        act_SongRender = 4     'USer 'Printed' a Song
        act_SongPDFRender = 5  'User 'Printed' a PDF

        act_Mp3Upload = 6      'User Uploaded an Mp3
        act_Mp3Download = 7    'User Downloaded an Mp3
        act_Mp3Delete = 8      'User Deleted an Mp3 (admin)

        act_PPT_Upload = 9     'User Uploaded a PPT file
        act_PPT_Download = 10  'User Downloaded a PPT file
        act_PPT_Delete = 11    'User Deleted a PPT file (admin)

        act_UserAdd = 12       'User Added	
        act_UserEdit = 13      'User Edited

        act_SetCreate = 14
        act_SetAddSong = 15
        act_SetDistribute = 16

        act_DocBinLogin = 17
        act_DocBinLogout = 18
        act_DocBinUpload = 19
        act_DocBinDownload = 20
        act_DocBinDelete = 21
        act_DocBinFolderCreate = 22
        act_DocBinFileCreate = 23
        act_DocBinFolderRename = 24
        act_DocBinFileRename = 25

        act_CreateCCLIReport = 55

    End Enum

    Private mvarstrUserName As String
    Private mvarstrPassword As String
    Private mvarstrPwdHash As String
    Private mvarlngOrgID As Long
    Private mvarlngUserID As Long
    Private mvarConnStr As String

    Private mvarUserLocked As Boolean
    Private mvarstrFirstName As String
    Private mvarstrLastName As String
    Private mvarstrEmail As String
    Private mvarLAstError As String = ""
    Private mvarstrAvailInst As String = ""

    Private mvarCollPerm As Collection

#Region "Public Parameters"

    Public ReadOnly Property Permission(ByVal p_strCode As Permissions) As Boolean
        Get
            If mvarCollPerm.Contains(p_strCode.ToString) = True Then
                Return mvarCollPerm(p_strCode.ToString)
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property UserLocked As Boolean
        Get
            Return mvarUserLocked
        End Get
    End Property

    Public ReadOnly Property LastError As String
        Get
            Return mvarLAstError
        End Get
    End Property

    Public ReadOnly Property FirstName() As String
        Get
            FirstName = mvarstrFirstName
        End Get
    End Property

    Public ReadOnly Property LastName() As String
        Get
            LastName = mvarstrLastName
        End Get
    End Property

    Public ReadOnly Property EmailAddress() As String
        Get
            EmailAddress = mvarstrEmail
        End Get
    End Property

    Public Property UserName() As String
        Get
            UserName = mvarstrUserName
        End Get
        Set(ByVal value As String)
            mvarstrUserName = value
        End Set
    End Property

    Public Property OrgID() As Long
        Get
            OrgID = mvarlngOrgID
        End Get
        Set(ByVal value As Long)
            mvarlngOrgID = value
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal value As String)
            mvarstrPassword = value
            mvarstrPwdHash = ComputeHash(value)
        End Set
    End Property

    Public ReadOnly Property UserID() As Long
        Get
            UserID = mvarlngUserID
        End Get
    End Property

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            mvarConnStr = value
        End Set
    End Property

    Public Property AvailInst As String
        Get
            Return mvarstrAvailInst
        End Get
        Set(value As String)
            mvarstrAvailInst = value
        End Set
    End Property

#End Region

#Region "Public Functions & Methods"

    Private Function ComputeHash(p_strPlainPwd As String) As String

        Try
            Dim strPreHashVal As String = p_strPlainPwd & SaltyGoodness
            Dim strHashedVal As String = FormsAuthentication.HashPasswordForStoringInConfigFile(strPreHashVal, "sha1")
            ComputeHash = strHashedVal
        Catch ex As Exception

        End Try

    End Function

    Public Function LoadUser(p_intID As Integer) As Integer

        Dim intRetVal As Integer = 0

        Try
            Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            mvarconnSQL.Open()

            If mvarconnSQL.State = ConnectionState.Open Then

                Dim cmd As New OleDb.OleDbCommand("SELECT * FROM Users_New WHERE UserID = " & p_intID, mvarconnSQL)
                Dim data As OleDb.OleDbDataReader

                cmd.CommandType = CommandType.Text
                data = cmd.ExecuteReader()
                mvarCollPerm = New Collection

                If data.Read = False Then

                    intRetVal = 0  'Not Found

                Else

                    If Not IsDBNull(data.Item("userlocked")) Then mvarUserLocked = (data.Item("userlocked") = 1)
                    If Not IsDBNull(data.Item("UserID")) Then mvarlngUserID = data.Item("UserID")
                    '****Added recently*** (why did I leave this out again?)
                    If Not IsDBNull(data.Item("Username")) Then mvarstrUserName = data.Item("Username")
                    If Not IsDBNull(data.Item("Password")) Then mvarstrPassword = data.Item("Password")
                    If Not IsDBNull(data.Item("AvailInst")) Then mvarstrAvailInst = data.Item("AvailInst") & ""
                    '******
                    If Not IsDBNull(data.Item("email")) Then mvarstrEmail = data.Item("email")
                    If Not IsDBNull(data.Item("firstname")) Then mvarstrFirstName = data.Item("firstname")
                    If Not IsDBNull(data.Item("lastname")) Then mvarstrLastName = data.Item("lastname")

                    If Not IsDBNull(data.Item("siteadmin")) Then
                        If data.Item("siteadmin") <> 0 Then mvarCollPerm.Add(True, Permissions.siteadmin.ToString)
                    End If

                    If Not IsDBNull(data.Item("setadmin")) Then
                        If data.Item("setadmin") <> 0 Then mvarCollPerm.Add(True, Permissions.setadmin.ToString)
                    End If

                    If Not IsDBNull(data.Item("songview")) Then
                        If data.Item("songview") <> 0 Then mvarCollPerm.Add(True, Permissions.songview.ToString)
                    End If

                    If Not IsDBNull(data.Item("songedit")) Then
                        If data.Item("songedit") <> 0 Then mvarCollPerm.Add(True, Permissions.songedit.ToString)
                    End If

                    If Not IsDBNull(data.Item("mp3upload")) Then
                        If data.Item("mp3upload") <> 0 Then mvarCollPerm.Add(True, Permissions.mp3upload.ToString)
                    End If

                    If Not IsDBNull(data.Item("mp3dwload")) Then
                        If data.Item("mp3dwload") <> 0 Then mvarCollPerm.Add(True, Permissions.mp3dwload.ToString)
                    End If

                    If Not IsDBNull(data.Item("pptdwn")) Then
                        If data.Item("pptdwn") <> 0 Then mvarCollPerm.Add(True, Permissions.pptdwn.ToString)
                    End If

                    If Not IsDBNull(data.Item("pptupl")) Then
                        If data.Item("pptupl") <> 0 Then mvarCollPerm.Add(True, Permissions.pptupl.ToString)
                    End If

                    If Not IsDBNull(data.Item("docbinUL")) Then
                        If data.Item("docbinUL") <> 0 Then mvarCollPerm.Add(True, Permissions.docbinUL.ToString)
                    End If

                    If Not IsDBNull(data.Item("docbinDL")) Then
                        If data.Item("docbinDL") <> 0 Then mvarCollPerm.Add(True, Permissions.docbinDL.ToString)
                    End If

                    If Not IsDBNull(data.Item("reporting")) Then
                        If data.Item("reporting") <> 0 Then mvarCollPerm.Add(True, Permissions.reporting.ToString)
                    End If

                    If Not IsDBNull(data.Item("songdel")) Then
                        If data.Item("songdel") <> 0 Then mvarCollPerm.Add(True, Permissions.songdel.ToString)
                    End If

                    data.Close()
                    intRetVal = 1

                End If

                data = Nothing

                cmd.Dispose()
                cmd = Nothing

                mvarconnSQL.Close()
                mvarconnSQL.Dispose()
                mvarconnSQL = Nothing

            End If

        Catch ex As Exception

            mvarLAstError = ex.ToString
            intRetVal = 69  'Error!

        End Try

        Return intRetVal

    End Function

    Public Function UpdateUser() As Boolean
        Dim intRetVal As Integer = 0
        Dim strSQL As String = ""

        'Bail!
        Return False

        Try
            Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            mvarconnSQL.Open()

            If mvarconnSQL.State = ConnectionState.Open Then

                strSQL = "UPDATE Users_New "




                Dim cmd As New OleDb.OleDbCommand(strSQL, mvarconnSQL)

                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()

                cmd.Dispose()
                cmd = Nothing


            End If

            mvarconnSQL.Close()
            mvarconnSQL.Dispose()
            mvarconnSQL = Nothing

        Catch ex As Exception

        End Try

        Return intRetVal

    End Function

    Public Function Login() As Integer
        'Check the User Table for a match
        Dim intRetVal As Integer = 0

        Try
            Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            mvarconnSQL.Open()

            If mvarconnSQL.State = ConnectionState.Open Then

                Dim cmd As New OleDb.OleDbCommand("SELECT * FROM Users_New WHERE UserName = '" & mvarstrUserName & "' AND PasswordHash = '" & ComputeHash(mvarstrPassword) & "' and churchaffil = " & mvarlngOrgID, mvarconnSQL)
                Dim data As OleDb.OleDbDataReader

                cmd.CommandType = CommandType.Text
                data = cmd.ExecuteReader()
                mvarCollPerm = New Collection

                If data.Read = False Then

                    intRetVal = 0   'Not Found
                Else

                    If data.Item("userlocked") = 1 Then
                        intRetVal = -1
                    Else

                        intRetVal = 99  'Login Good

                        If Not IsDBNull(data.Item("UserID")) Then mvarlngUserID = data.Item("UserID")
                        If Not IsDBNull(data.Item("email")) Then mvarstrEmail = data.Item("email")
                        If Not IsDBNull(data.Item("firstname")) Then mvarstrFirstName = data.Item("firstname")
                        If Not IsDBNull(data.Item("lastname")) Then mvarstrLastName = data.Item("lastname")

                        If Not IsDBNull(data.Item("Username")) Then mvarstrUserName = data.Item("Username")
                        If Not IsDBNull(data.Item("Password")) Then mvarstrPassword = data.Item("Password")
                        If Not IsDBNull(data.Item("AvailInst")) Then mvarstrAvailInst = data.Item("AvailInst") & ""

                        If Not IsDBNull(data.Item("siteadmin")) Then
                            If data.Item("siteadmin") <> 0 Then mvarCollPerm.Add(True, Permissions.siteadmin.ToString)
                        End If

                        If Not IsDBNull(data.Item("setadmin")) Then
                            If data.Item("setadmin") <> 0 Then mvarCollPerm.Add(True, Permissions.setadmin.ToString)
                        End If

                        If Not IsDBNull(data.Item("songview")) Then
                            If data.Item("songview") <> 0 Then mvarCollPerm.Add(True, Permissions.songview.ToString)
                        End If

                        If Not IsDBNull(data.Item("songedit")) Then
                            If data.Item("songedit") <> 0 Then mvarCollPerm.Add(True, Permissions.songedit.ToString)
                        End If

                        If Not IsDBNull(data.Item("mp3upload")) Then
                            If data.Item("mp3upload") <> 0 Then mvarCollPerm.Add(True, Permissions.mp3upload.ToString)
                        End If

                        If Not IsDBNull(data.Item("mp3dwload")) Then
                            If data.Item("mp3dwload") <> 0 Then mvarCollPerm.Add(True, Permissions.mp3dwload.ToString)
                        End If

                        If Not IsDBNull(data.Item("pptdwn")) Then
                            If data.Item("pptdwn") <> 0 Then mvarCollPerm.Add(True, Permissions.pptdwn.ToString)
                        End If

                        If Not IsDBNull(data.Item("pptupl")) Then
                            If data.Item("pptupl") <> 0 Then mvarCollPerm.Add(True, Permissions.pptupl.ToString)
                        End If

                        If Not IsDBNull(data.Item("docbinUL")) Then
                            If data.Item("docbinUL") <> 0 Then mvarCollPerm.Add(True, Permissions.docbinUL.ToString)
                        End If

                        If Not IsDBNull(data.Item("docbinDL")) Then
                            If data.Item("docbinDL") <> 0 Then mvarCollPerm.Add(True, Permissions.docbinDL.ToString)
                        End If

                        If Not IsDBNull(data.Item("reporting")) Then
                            If data.Item("reporting") <> 0 Then mvarCollPerm.Add(True, Permissions.reporting.ToString)
                        End If

                        If Not IsDBNull(data.Item("songdel")) Then
                            If data.Item("songdel") <> 0 Then mvarCollPerm.Add(True, Permissions.songdel.ToString)
                        End If

                        LogUserActivity(Permission_ActionCodes.act_Login, 0)
                    End If

                    data.Close()

                End If

                data = Nothing

                cmd.Dispose()
                cmd = Nothing

                mvarconnSQL.Close()
                mvarconnSQL.Dispose()
                mvarconnSQL = Nothing
            End If

        Catch ex As Exception


            mvarLAstError = ex.ToString
            intRetVal = 69  'Error!

        End Try

        ' CreateInitialPwdHashes()
        'If mvarlngUserID = 1 Then clsSharedFns.PurgeTempFiles()

        Return intRetVal

    End Function

    Public Sub Logout()
        'Write code to db

        LogUserActivity(Permission_ActionCodes.act_Logout, 0)

        FormsAuthentication.SignOut()
        HttpContext.Current.Session.Remove("Security")
        FormsAuthentication.RedirectToLoginPage()

    End Sub

    Private Sub CreateInitialPwdHashes()

        Try
            
            Dim sqlConn As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            sqlConn.Open()
            Dim StrSQL As String = "SELECT UserID, Password From Users_New"

            Dim adp As New OleDb.OleDbDataAdapter(StrSQL, sqlConn)
            Dim objDS As New DataSet

            adp.SelectCommand.CommandType = CommandType.Text
            adp.Fill(objDS, 0, 1000, "Users")

            Dim objDataTbl As DataTable = objDS.Tables(0)
            Dim objDataRow As DataRow
            Dim ID As Integer
            Dim strPassword As String
            Dim strPwdHash As String

            For Each objDataRow In objDataTbl.Rows

                ID = objDataRow.Item("UserID")
                strPassword = objDataRow.Item("Password")

                strPwdHash = ComputeHash(strPassword)

                StrSQL = "UPDATE Users_New SET [PasswordHash] = '" & strPwdHash & "' WHERE [UserID] = " & ID

                Dim cmd As New OleDb.OleDbCommand(StrSQL, sqlConn)

                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()

                cmd.Dispose()
                cmd = Nothing

            Next

            objDataRow = Nothing
            objDataTbl.Dispose()
            objDataTbl = Nothing

            objDS.Dispose()
            objDS = Nothing

            adp.Dispose()
            adp = Nothing



        Catch ex As Exception


        End Try

    End Sub


    Public Sub RatchetFavoriteCounter(ByVal intSongID As Integer, ByVal intFavcnt As Integer)

        Try
            Dim ss As HttpSessionState = HttpContext.Current.Session
            Dim strSQL As String = ""
            Dim sqlConn As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            sqlConn.Open()

            strSQL = "UPDATE [Songs] SET [Fav] = " & (CInt(intFavcnt) + 1) & " WHERE [ID] = " & intSongID

            Dim cmd As New OleDb.OleDbCommand(strSQL, sqlConn)

            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            cmd = Nothing

            sqlConn.Close()
            sqlConn.Dispose()
            sqlConn = Nothing


        Catch ex As Exception

            mvarLAstError = ex.ToString
            'HttpContext.Current.Response.Write("HI!")

        End Try

    End Sub

    Public Sub LogUserActivity(ByVal p_lngActionCode As Permission_ActionCodes, ByVal p_lngSongID As Long)
        Dim strSQL As String = ""

        Try
            Dim ss As HttpSessionState = HttpContext.Current.Session

            Dim sqlConn As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            sqlConn.Open()

            strSQL = "INSERT INTO [ActivityLog] VALUES( GetDate(), " & mvarlngUserID & ", " & CStr(p_lngActionCode) & ", " & CStr(p_lngSongID) & ", '" & ss.SessionID & "')"

            Dim cmd As New OleDb.OleDbCommand(strSQL, sqlConn)

            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            cmd = Nothing

            sqlConn.Close()
            sqlConn.Dispose()
            sqlConn = Nothing


        Catch ex As Exception

            mvarLAstError = "SQL: " & strSQL & "    " & ex.ToString
            'HttpContext.Current.Response.Write(mvarLAstError)

        End Try


    End Sub

    Public Function CheckPermission(ByVal p_lngPermissionCode As Permission_ActionCodes) As Boolean
        'Return whether user can have access to this function

        'This is like a hash function

        Select Case p_lngPermissionCode

            'Site Admin
            Case Permission_ActionCodes.act_PPT_Delete, Permission_ActionCodes.act_Mp3Delete, Permission_ActionCodes.act_UserAdd, Permission_ActionCodes.act_UserEdit
                Return Permission(Permissions.siteadmin)

                'DocBinAdmin
            Case Permission_ActionCodes.act_DocBinUpload, Permission_ActionCodes.act_DocBinDelete, Permission_ActionCodes.act_DocBinFileCreate, Permission_ActionCodes.act_DocBinFileRename, Permission_ActionCodes.act_DocBinFolderCreate, Permission_ActionCodes.act_DocBinFolderRename
                Return Permission(Permissions.docbinUL)

                'Download Mp3
            Case Permission_ActionCodes.act_Mp3Download
                Return Permission(Permissions.mp3dwload)

                'Upload Mp3
            Case Permission_ActionCodes.act_Mp3Upload
                Return Permission(Permissions.mp3upload)

                'Download PDF
            Case Permission_ActionCodes.act_PPT_Download
                Return Permission(Permissions.pptdwn)

                'Upload PDF
            Case Permission_ActionCodes.act_PPT_Upload
                Return Permission(Permissions.pptupl)

                'Set Admin
            Case Permission_ActionCodes.act_SetAddSong
            Case Permission_ActionCodes.act_SetCreate
            Case Permission_ActionCodes.act_SetDistribute
                Return Permission(Permissions.setadmin)

                'Edit Song
            Case Permission_ActionCodes.act_SongAdd
                Return Permission(Permissions.songedit)
            Case Permission_ActionCodes.act_SongEdit
                Return Permission(Permissions.songedit)

                'ViewSong
            Case Permission_ActionCodes.act_SongPDFRender
            Case Permission_ActionCodes.act_SongRender
                Return Permission(Permissions.songview)

                'DocBinAccess
            Case Permission_ActionCodes.act_DocBinDownload
            Case Permission_ActionCodes.act_DocBinLogin
                Return Permission(Permissions.docbinDL)


        End Select

        'remove later!
        Return True

    End Function


#End Region

    Public Sub New()
        mvarConnStr = ConfigurationManager.AppSettings("SQLConnectionString")
        mvarCollPerm = New Collection

    End Sub

    Protected Overrides Sub Finalize()
        mvarCollPerm.Clear()
        mvarCollPerm = Nothing
        MyBase.Finalize()
    End Sub

End Class
