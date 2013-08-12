Imports System.Data
Imports System.Data.OleDb

Partial Class SheetPrintOut
    Inherits System.Web.UI.Page

    Public lngSongID As Long
 
    Public blnShowNotes As Boolean
    Public blnShowOrder As Boolean
    Public blnShowPartNames As Boolean
    Public blnIsPreview As Integer
    Public strTransKey As String

    Public intTextSize As Integer = 16
    Public intNoteSize As Integer = 16

    Public Key As String
    Public CopyDate As String
    Public Publish As String
    Public CCLI As String
    Public Author1 As String
    Public Author2 As String
    Public Notes As String
    Public strTitle As String
    Public songData As String
    Public strOrder As String
    Public strDecodedSong As String


    Public I As Integer
    Public mp3Link As String
    Public Tempo As String
    Public SecInst As clsSecurity
    Public intfav As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim intAction As Integer = 1

        'find out who this person is
        If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

        SecInst = HttpContext.Current.Session("Security")

        If SecInst.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = False Then
            Response.Write("Sorry. You do not have permission to render song sheets.")
        End If

        lngSongID = CInt(Val(Request.QueryString("dbid") & ""))
        blnShowOrder = IIf(LCase(Request.QueryString("ordering") & "") = "true", True, False)
        blnShowNotes = IIf(LCase(Request.QueryString("notes") & "") = "true", True, False)
        blnShowPartNames = IIf(LCase(Request.QueryString("partnames") & "") = "true", True, False)
        strTransKey = Left(Trim(Request.QueryString("transkey") & ""), 6)
        intTextSize = CInt(Val(Request.QueryString("TS") & ""))
        intNoteSize = CInt(Val(Request.QueryString("NS") & ""))
        intAction = CInt(Val(Request.QueryString("action") & ""))
        If intTextSize = 0 Then intTextSize = 16
        If intNoteSize = 0 Then intNoteSize = 16

        Response.Write("<!---ID:" & lngSongID & "-->")
        Response.Write("<!---ShowOrder:" & blnShowOrder & "-->")
        Response.Write("<!---ShowNotes:" & blnShowNotes & "-->")
        Response.Write("<!---ShowPartName:" & blnShowPartNames & "-->")
        Response.Write("<!---TransposeKey:" & strTransKey & "-->")
        Response.Write("<!---TextSize:" & intTextSize & "-->")
        Response.Write("<!---NoteSize:" & intNoteSize & "-->")
        Response.Write("<!---ActionCode:" & intAction & "-->")

        If lngSongID = 0 Then

            '%>
            Response.Write("That was an invalid ID Number.")
            Response.End()
            '<%

        Else

            Try
                Dim mvarconnSQL As New System.Data.OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                mvarconnSQL.Open()

                If mvarconnSQL.State = ConnectionState.Open Then

                    Dim cmd As New OleDb.OleDbCommand("SELECT * From [Songs] Where [ID] = " & lngSongID & "", mvarconnSQL)
                    Dim data As OleDb.OleDbDataReader

                    cmd.CommandType = CommandType.Text
                    data = cmd.ExecuteReader()

                    If data.Read = False Then

                        Response.Write("Song Not Found.")
                        Response.End()

                    Else

                        mp3Link = " "
                        Key = data.Item("Key") & ""
                        CopyDate = data.Item("CDATE") & ""
                        Publish = data.Item("Publish") & ""
                        CCLI = data.Item("CCLI") & ""

                        Author1 = data.Item("Author1") & ""
                        Author2 = data.Item("Author2") & ""
                        Notes = data.Item("Notes") & ""
                        Tempo = data.Item("Tempo") & ""

                        strTitle = data.Item("Title") & ""
                        mp3Link = data.Item("Mp3Link") & ""

                        songData = data.Item("SongData") & ""
                        strOrder = data.Item("Order") & ""
                        intfav = data.Item("Fav")

                        If intAction = 1 Then
                            SecInst.RatchetFavoriteCounter(lngSongID, intfav)
                            SecInst.LogUserActivity(clsSecurity.Permission_ActionCodes.act_SongRender, lngSongID)
                        End If

                        If intAction = 2 Then
                            blnIsPreview = True
                        End If

                        Dim objMM As New MusicMinder.clsMain
                        objMM.Legal = ConfigurationManager.AppSettings("gsLegalChars")

                        strDecodedSong = objMM.WebDecode(songData, blnShowNotes, blnShowPartNames, Key, strTransKey)
                        objMM = Nothing

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

      
    End Sub



End Class
