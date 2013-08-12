Imports System.Data
Imports System.Data.OleDb

Partial Class songsheet
    Inherits System.Web.UI.Page

    Public I As Integer
    Public lngID As Long
    Public Key As String
    Public mp3Link As String
    Public CopyDate As String
    Public Publish As String
    Public CCLI As String
    Public USE1 As String
    Public USE2 As String
    Public Style As String
    Public Author1 As String
    Public Author2 As String
    Public Notes As String
    Public Tempo As String
    Public Reference As String
    Public strTitle As String
    Public pptLink As String
    Public TS As String
    Public URLLink As String
    Public YouTubeLink As String
    Public SecInst As clsSecurity
    Public strOrder As String

    Public strSongHTML As String
    Private strRawSongData As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'find out who this person is
        If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

        SecInst = HttpContext.Current.Session("Security")


        lngID = CInt(Val(Request.QueryString("dbid") & ""))
        'response.Write(lngID)
        If lngID = 0 Then

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

                        Dim cmd As New OleDb.OleDbCommand("SELECT * From [Songs] Where [ID] = " & lngID & "", mvarconnSQL)
                        Dim data As OleDb.OleDbDataReader

                        cmd.CommandType = CommandType.Text
                        data = cmd.ExecuteReader()

                        If data.Read = False Then

                            Response.Write("Song Not Found.")
                            Response.End()

                        Else

                            songID.Value = lngID


                            mp3Link = " "
                            Key = data.Item("Key") & ""
                            lblSongKey.Text += Key

                            CopyDate = data.Item("CDATE") & ""
                            Publish = data.Item("Publish") & ""
                            lblCopyright.Text += CopyDate & " " & Publish

                            CCLI = data.Item("CCLI") & ""
                            lnkCCLI.NavigateUrl = "http://www.ccli.com/songselect/skins/visitor/song_detail.cfm?id=" & CCLI & """"
                            lnkCCLI.Text = CCLI

                            lblUse1.Text += data.Item("Use1") & ""
                            lblUse2.Text += data.Item("Use2") & ""

                            lblStyle.Text += data.Item("Style") & ""
                            lblAuthor1.Text += data.Item("Author1") & ""
                            lblAuthor2.Text += data.Item("Author2") & ""
                            lblSongNotes.Text += data.Item("Notes") & ""
                            lblTempo.Text += data.Item("Tempo") & ""

                            Reference = data.Item("Reference") & ""
                            lnkReference.NavigateUrl = "http://bible.gospelcom.net/cgi-bin/bible?language=english&passage=" & Reference & "&version=NIV"
                            lnkReference.Text = Reference

                            lblTitle.Text += data.Item("Title") & ""

                            mp3Link = data.Item("Mp3Link") & ""
                            lblmp3Lnk.Value = mp3Link
                            pptLink = data.Item("pptLink") & ""
                            lblPPTLink.value = pptLink

                            TS = data.Item("TS") & ""
                            URLLink = data.Item("URLLink") & ""

                            lnkExternal.NavigateUrl = URLLink
                            If URLLink <> "" Then lnkExternal.Text = "Click Here"

                            YouTubeLink = data.Item("YouTubeLink") & ""
                            lblYouTubeLink.Value = YouTubeLink

                            strRawSongData = data.Item("SongData") & ""
                            strOrder = data.Item("Order") & ""

                            'Response.Write("SONG KEY: " & Key)

                            Select Case Key

                                Case "C", "D", "E", "F", "G", "A", "B"
                                    cboSongKey.SelectedValue = Key
                                Case "C#", "Db"
                                    cboSongKey.SelectedValue = "C#/Db"

                                Case "D#", "Eb"
                                    cboSongKey.SelectedValue = "D#/Eb"

                                Case "F#", "Gb"
                                    cboSongKey.SelectedValue = "F#/Gb"

                                Case "G#", "Ab"
                                    cboSongKey.SelectedValue = "G#/Ab"

                                Case "A#", "Bb"
                                    cboSongKey.SelectedValue = "A#/Bb"


                            End Select


                            cboTextSize.SelectedValue = TS

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

        cmdDownloadMp3.Attributes.Add("onclick", "window.open('filedownload.aspx?dbid=" & lngID & "&contentid=0', 'Download Audio File','location=1,status=1,scrollbars=0, width=650,height=350'); return false;")
        cmdDeleteMp3.Attributes.Add("onclick", "window.open('filedelete.aspx?dbid=" & lngID & "&contentid=0'); return false;")
        cmdFileUpload.Attributes.Add("onclick", "window.open('fileupload.aspx?dbid=" & lngID & "&contentid=0', 'Upload Audio File','location=1,status=1,scrollbars=0, width=650,height=350'); return false;")

        cmdAttachmentDownload.Attributes.Add("onclick", "window.open('filedownload.aspx?dbid=" & lngID & "&contentid=1', 'Download Attachment File','location=1,status=1,scrollbars=1, width=650,height=350'); return false;")
        cmdAttachDelete.Attributes.Add("onclick", "window.open('filedelete.aspx?dbid=" & lngID & "&contentid=1'); return false;")
        cmdAttachUpload.Attributes.Add("onclick", "window.open('fileupload.aspx?dbid=" & lngID & "&contentid=1', 'Upload Attachment File','location=1,status=1,scrollbars=1, width=650,height=350'); return false;")


    End Sub

    Private Function EncodeNotes(p_StrNoteVal As String) As String

        p_StrNoteVal = p_StrNoteVal.Replace("#", "%23")


        If p_StrNoteVal.Contains("/") Then
            p_StrNoteVal = Left(p_StrNoteVal, InStr(p_StrNoteVal, "/") - 1)
        End If

        Return p_StrNoteVal

    End Function

    Protected Sub cmdGenerateSheet_Click(sender As Object, e As System.EventArgs) Handles cmdGenerateSheet.Click

        Response.Redirect("SheetPrintOut.aspx?dbid=" & lngID & "&ordering=" & chkPrintOrdering.Checked & "&notes=" & chkPrintChords.Checked & "&transkey=" & EncodeNotes(cboSongKey.SelectedValue) & "&PartNames=" & chkPrintPartNames.Checked & "&TS=" & cboTextSize.SelectedValue & "&NS=" & cboTextSize.SelectedValue & "&action=1")

    End Sub

    Protected Sub cmdGeneratePreview_Click(sender As Object, e As System.EventArgs) Handles cmdGeneratePreview.Click

        Response.Redirect("SheetPrintOut.aspx?dbid=" & lngID & "&ordering=" & chkPrintOrdering.Checked & "&notes=" & chkPrintChords.Checked & "&transkey=" & EncodeNotes(cboSongKey.SelectedValue) & "&PartNames=" & chkPrintPartNames.Checked & "&TS=" & cboTextSize.SelectedValue & "&NS=" & cboTextSize.SelectedValue & "&action=2")

    End Sub


    Protected Sub cmdDeleteMp3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeleteMp3.Click
        Response.Redirect("songsheet.aspx?dbid=" & lngID)
    End Sub

    Protected Sub cmdAttachDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAttachDelete.Click
        Response.Redirect("songsheet.aspx?dbid=" & lngID)
    End Sub

End Class
