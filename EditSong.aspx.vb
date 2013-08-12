Imports System.Data
Imports System.Data.OleDb

Partial Class EditSong
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

            Dim strID As String = Left(Request.QueryString("dbid"), 10)

            If strID = "" Then
                id.Value = "-1"
            Else
                id.Value = strID
            End If

            If Page.IsPostBack = False Then
                Dim mvarconnSQL As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
                mvarconnSQL.Open()

                If mvarconnSQL.State = ConnectionState.Open Then

                    Dim cmd As New OleDb.OleDbDataAdapter("SELECT * FROM Songs WHERE ID = " & id.Value, mvarconnSQL)
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

                        song_Name.Text = objDR.Item("Title") & ""
                        Author1.Text = objDR.Item("Author1") & ""
                        Author2.Text = objDR.Item("Author2") & ""

                        Notes.Text = objDR.Item("Notes") & ""
                        arrangement.Text = objDR.Item("Order") & ""
                        copydate.Text = objDR.Item("CDate") & ""
                        publisher.Text = objDR.Item("Publish") & ""
                        BRef.Text = objDR.Item("Reference") & ""
                        Youtubelink.Text = objDR.Item("YouTubeLink") & ""
                        key.Text = objDR.Item("Key") & ""
                        cboStyle.SelectedValue = objDR.Item("Style") & ""
                        use1.SelectedValue = objDR.Item("Use1") & ""
                        use2.SelectedValue = objDR.Item("Use2") & ""
                        Tempo.Text = objDR.Item("Tempo") & ""
                        CCLINum.Text = objDR.Item("CCLI") & ""
                        ExtURL.Text = objDR.Item("URLLink") & ""
                        If String.IsNullOrEmpty(objDR.Item("GroupID")) = False Then
                            cboGroup.SelectedValue = objDR.Item("GroupID") & ""

                        End If

                        cboFS.SelectedValue = Trim(objDR.Item("TS") & "")

                        'Decode Stream Content
                        Dim objMM As New MusicMinder.clsMain
                        objMM.Legal = ConfigurationManager.AppSettings("gsLegalChars")
                        objMM.SongDecode(objDR.Item("SongData") & "", "C", "C")

                        part1name.Text = objMM.GetPartName(0)
                        part1.Text = objMM.GetPartContent(0)
                        If Right(part1.Text, 2) = vbCrLf Then
                            part1.Text = Left(part1.Text, Len(part1.Text) - 2)
                        End If

                        part2name.Text = objMM.GetPartName(1)
                        part2.Text = objMM.GetPartContent(1)
                        If Right(part2.Text, 2) = vbCrLf Then
                            part2.Text = Left(part2.Text, Len(part2.Text) - 2)
                        End If

                        part3name.Text = objMM.GetPartName(2)
                        part3.Text = objMM.GetPartContent(2)
                        If Right(part3.Text, 2) = vbCrLf Then
                            part3.Text = Left(part3.Text, Len(part3.Text) - 2)
                        End If

                        part4name.Text = objMM.GetPartName(3)
                        part4.Text = objMM.GetPartContent(3)
                        If Right(part4.Text, 2) = vbCrLf Then
                            part4.Text = Left(part4.Text, Len(part4.Text) - 2)
                        End If

                        part5name.Text = objMM.GetPartName(4)
                        part5.Text = objMM.GetPartContent(4)
                        If Right(part5.Text, 2) = vbCrLf Then
                            part5.Text = Left(part5.Text, Len(part5.Text) - 2)
                        End If

                        part6name.Text = objMM.GetPartName(5)
                        part6.Text = objMM.GetPartContent(5)
                        If Right(part6.Text, 2) = vbCrLf Then
                            part6.Text = Left(part6.Text, Len(part6.Text) - 2)
                        End If

                        part7name.Text = objMM.GetPartName(6)
                        part7.Text = objMM.GetPartContent(6)
                        If Right(part7.Text, 2) = vbCrLf Then
                            part7.Text = Left(part7.Text, Len(part7.Text) - 2)
                        End If

                        part8name.Text = objMM.GetPartName(7)
                        part8.Text = objMM.GetPartContent(7)
                        If Right(part8.Text, 2) = vbCrLf Then
                            part8.Text = Left(part8.Text, Len(part8.Text) - 2)
                        End If


                        objMM = Nothing
                    End If

                    objDR = Nothing

                    mvarconnSQL.Close()
                    mvarconnSQL.Dispose()
                    mvarconnSQL = Nothing

                End If

            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        End Try


    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Save.Click

        Dim strSongName As String = song_Name.Text.Replace("'", "''")
        Dim strAuthor1 As String = Author1.Text.Replace("'", "''")
        Dim strAuthor2 As String = Author2.Text.Replace("'", "''")
        Dim strStyle As String = cboStyle.SelectedValue.Replace("'", "''")
        Dim intCDate As String = copydate.Text
        Dim strPublisher As String = publisher.Text.Replace("'", "''")
        Dim strID As String = id.Value
        Dim strAction As String = action.Value
        Dim strKey As String = key.SelectedValue.Replace("'", "''")
        Dim strNotes As String = Notes.Text.Replace("'", "''")
        Dim strYouTubeLink As String = ""
        Dim strarr As String = arrangement.Text.Replace("'", "''")
        Dim strUse1 As String = use1.SelectedValue.Replace("'", "''")
        Dim strUse2 As String = use2.SelectedValue.Replace("'", "''")
        Dim strTempo As String = Tempo.Text.Replace("'", "''")
        Dim strCCLINum As String = CCLINum.Text.Replace("'", "''")
        Dim strGroupID As String = cboGroup.SelectedValue
        Dim strSongText As String = ""
        Dim strBRef As String = BRef.Text.Replace("'", "''")
        Dim FS As String = cboFS.SelectedValue.Replace("'", "''")
        Dim strXternalURL As String = ExtURL.Text.Replace("'", "''")


        Dim strPart1Name As String = part1name.Text
        Dim strPart1 As String = part1.Text
        Dim strPart2Name As String = part2name.Text
        Dim strPart2 As String = part2.Text
        Dim strPart3Name As String = part3name.Text
        Dim strPart3 As String = part3.Text
        Dim strPart4Name As String = part4name.Text
        Dim strPart4 As String = part4.Text
        Dim strPart5Name As String = part5name.Text
        Dim strPart5 As String = part5.Text
        Dim strPart6Name As String = part6name.Text
        Dim strPart6 As String = part6.Text
        Dim strPart7Name As String = part7name.Text
        Dim strPart7 As String = part7.Text
        Dim strPart8Name As String = part8name.Text
        Dim strPart8 As String = part8.Text

        Dim strSongData As String = "" 'Raw Data Stream

        'Encode Stream

        'Music Minder DLL reference
        Dim objMM As New MusicMinder.clsMain
        objMM.Legal = ConfigurationManager.AppSettings("gsLegalChars")
        objMM.ClearParts()
        If strPart1Name <> "" Then objMM.AddPart(strPart1Name, strPart1)
        If strPart2Name <> "" Then objMM.AddPart(strPart2Name, strPart2)
        If strPart3Name <> "" Then objMM.AddPart(strPart3Name, strPart3)
        If strPart4Name <> "" Then objMM.AddPart(strPart4Name, strPart4)
        If strPart5Name <> "" Then objMM.AddPart(strPart5Name, strPart5)
        If strPart6Name <> "" Then objMM.AddPart(strPart6Name, strPart6)
        If strPart7Name <> "" Then objMM.AddPart(strPart7Name, strPart7)
        If strPart8Name <> "" Then objMM.AddPart(strPart8Name, strPart8)
        strSongData = objMM.SongEncode(strKey)
        strSongText = objMM.WebDecode(strSongData, "0", "0", "C", "C")
        strSongData = strSongData.Replace("'", "''")
        strSongText = strSongText.Replace("'", "''")
        objMM = Nothing

        '
        strYouTubeLink = Youtubelink.Text
        If strYouTubeLink <> "" Then

            If InStr(strYouTubeLink, "http://www.youtube.com/watch?v=") > 0 Then
                strYouTubeLink = Replace(strYouTubeLink, "http://www.youtube.com/watch?v=", "")
            End If

            If InStr(strYouTubeLink, "&feature=") > 0 Then
                strYouTubeLink = Left(strYouTubeLink, InStr(strYouTubeLink, "&feature=") - 1)
            End If

            strYouTubeLink = strYouTubeLink.Replace("'", "''")

        End If

        'Now we have all the data ready to go, lets insert / update it.
        Dim strSQL As String = ""

        If strID = "-1" Then
            'This is a new Entry
            strSQL = "INSERT INTO [songs] ("
            strSQL += "Title,"
            strSQL += "Author1,"
            strSQL += "Author2,"
            strSQL += "Notes,"
            strSQL += "[Order],"
            strSQL += "CDate,"
            strSQL += "Publish,"
            strSQL += "Reference,"
            strSQL += "YouTubeLink,"
            strSQL += "[Key],"
            strSQL += "Style,"
            strSQL += "Use1,"
            strSQL += "Use2,"
            strSQL += "Tempo,"
            strSQL += "CCLI,"
            strSQL += "URLLink,"
            strSQL += "GroupID,"
            strSQL += "TS,"
            strSQL += "songdata,"
            strSQL += "songtext)"
            strSQL += " Values("
            strSQL += "'" & strSongName & "',"
            strSQL += "'" & strAuthor1 & "',"
            strSQL += "'" & strAuthor2 & "',"
            strSQL += "'" & strNotes & "',"
            strSQL += "'" & strarr & "',"
            strSQL += "'" & intCDate & "',"
            strSQL += "'" & strPublisher & "',"
            strSQL += "'" & strBRef & "',"
            strSQL += "'" & strYouTubeLink & "',"
            strSQL += "'" & strKey & "',"
            strSQL += "'" & strStyle & "',"
            strSQL += "'" & strUse1 & "',"
            strSQL += "'" & strUse2 & "',"
            strSQL += "'" & strTempo & "',"
            strSQL += "'" & strCCLINum & "',"
            strSQL += "'" & strXternalURL & "',"
            strSQL += strGroupID & ","
            strSQL += "'" & FS & "',"
            strSQL += "'" & strSongData & "',"
            strSQL += "'" & strSongText & "')"


        Else
            'This entry already Exists, it is an update. 

            strSQL = "UPDATE [songs] SET "
            strSQL += "Title = '" & strSongName & "',"
            strSQL += "Author1 = '" & strAuthor1 & "',"
            strSQL += "Author2 = '" & strAuthor2 & "',"
            strSQL += "Notes = '" & strNotes & "',"
            strSQL += "[Order] = '" & strarr & "',"
            strSQL += "CDate = '" & intCDate & "',"
            strSQL += "Publish = '" & strPublisher & "',"
            strSQL += "Reference = '" & strBRef & "',"
            strSQL += "YouTubeLink = '" & strYouTubeLink & "',"
            strSQL += "[Key] = '" & strKey & "',"
            strSQL += "Style = '" & strStyle & "',"
            strSQL += "Use1 = '" & strUse1 & "',"
            strSQL += "Use2 = '" & strUse2 & "',"
            strSQL += "Tempo = '" & strTempo & "',"
            strSQL += "CCLI = '" & strCCLINum & "',"
            strSQL += "URLLink = '" & strXternalURL & "',"
            strSQL += "GroupID = " & strGroupID & ","
            strSQL += "TS = '" & FS & "',"
            strSQL += "songdata = '" & strSongData & "',"
            strSQL += "songtext = '" & strSongText & "', "
            strSQL += "LastMod = getDate() "
            strSQL += " WHERE ID = " & strID

        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)
        'Response.Write(strSQL)
        Try
            Dim ss As HttpSessionState = HttpContext.Current.Session
            Dim mvarconnSQL As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString").ToString)
            mvarconnSQL.Open()

            Dim cmd As New OleDb.OleDbCommand(strSQL, mvarconnSQL)

            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            cmd = Nothing

            mvarconnSQL.Close()
            mvarconnSQL.Dispose()
            mvarconnSQL = Nothing

            If strID = "-1" Then
                HttpContext.Current.Session("Security").LogUserActivity(clsSecurity.Permission_ActionCodes.act_SongAdd, CLng(Val(strID)))
            Else
                HttpContext.Current.Session("Security").LogUserActivity(clsSecurity.Permission_ActionCodes.act_SongEdit, CLng(Val(strID)))
            End If



        Catch ex As Exception

            Response.Write(ex.ToString)
            Response.Write(strSQL)
            'HttpContext.Current.Response.Write("HI!")

        End Try



    End Sub


End Class
