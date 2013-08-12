Imports System.Data

Partial Class usercontrols_MiniSongListContol
    Inherits System.Web.UI.UserControl

    Public Enum DISPMode
        MostPopular = 0
        RecentlyEdited = 1
        ThisWeek = 2
        PastHistory = 3
        Rolodex = 4
    End Enum


    Private mvarDisplayMode As DISPMode
    Public m_objSQLConn As OleDb.OleDbConnection
    Public m_objSec As clsSecurity

    Public Property DisplayMode() As DISPMode
        Get
            Return mvarDisplayMode
        End Get
        Set(ByVal value As DISPMode)
            mvarDisplayMode = value
            Draw()
        End Set
    End Property

    Public Sub Draw()

        If Not m_objSQLConn Is Nothing Then

            Select Case mvarDisplayMode

                Case DISPMode.MostPopular, DISPMode.RecentlyEdited
                    DrawMostPopRecent()
                Case DISPMode.ThisWeek
                    DrawThisWeeksSet()
                Case DISPMode.PastHistory
                    DrawPastWeeksSet()
                Case DISPMode.Rolodex
                    DrawRolodex()

            End Select

        End If

    End Sub

#Region "Rolodex Methods"


    Private Function DrawRolodex_Hdr() As String
        Dim strRes As String = ""

        strRes += "<table  width=""100%"">"
        strRes += "<tr bgcolor=""#993300"">"
        strRes += "<td>"
        strRes += "<h3>Ministry Contacts:</h3>"
        strRes += "</td>"
        strRes += "</tr>"
        strRes += "<tr><td>"
        strRes += "<table border=""0"" cellpadding=""2"" cellspacing=""0"" width=""100%"" >"
        strRes += "<thead>"
        strRes += "<tr bgcolor=""#B86E00""> "
        strRes += "<th>First Name</th>"
        strRes += "<th>Last Name</th>"
        strRes += "<th>Email</th>"
        strRes += "<th>Instruments</th>"
        strRes += "</tr>"
        strRes += "</thead>"

        Return strRes



    End Function

    Private Sub DrawRolodex()
        Dim objSB As New StringBuilder
        'Dim objRS As New ADODB.Recordset
        Dim strArr() As String
        Dim I As Integer

        Try

            'Write the header
            objSB.Append(DrawRolodex_Hdr)

            Dim cmd As New OleDb.OleDbCommand("SELECT * FROM Users_New WHERE userlocked = 0 and churchaffil = " & m_objSec.OrgID & " ORDER BY LastName", m_objSQLConn)
            Dim data As OleDb.OleDbDataReader

            cmd.CommandType = CommandType.Text
            data = cmd.ExecuteReader()

            Do While data.Read

                'Write out a row
                objSB.Append("<tr>")
                objSB.Append("<td>")
                objSB.Append(data.Item("FirstName") & "")
                objSB.Append("</td>")
                objSB.Append("<td>")
                objSB.Append(data.Item("LastName") & "")
                objSB.Append("</td>")
                objSB.Append("<td>")
                objSB.Append("<a href=""mailto:" & data.Item("Email") & """>" & data.Item("Email") & "" & "</a>")
                objSB.Append("</td>")
                objSB.Append("<td>")

                strArr = Split(data.Item("AvailInst") & "", "|")
                For I = 0 To UBound(strArr)
                    If strArr(I) <> "" Then objSB.Append(GetPositionNameFromID(strArr(I)))
                    If (I < UBound(strArr)) And I > 0 Then
                        objSB.Append(", ")
                    End If
                Next

                objSB.Append("</td>")

                objSB.Append("</tr>")

                'objRS.MoveNext()
            Loop

            ' objRS.Close()
 

            objSB.Append("</table>")



            PlaceHolder1.Controls.Add(New LiteralControl(objSB.ToString))

            objSB = Nothing
            'objRS = Nothing

            GC.Collect()

        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try


    End Sub

    Private Function GetLeaderNameFromID(ByVal p_intUserID As Integer) As String
        GetLeaderNameFromID = ""
        Try

            Dim cmd As New OleDb.OleDbCommand("SELECT [lastname],[firstname] FROM Users_New WHERE [UserID] = " & p_intUserID, m_objSQLConn)
            Dim data As OleDb.OleDbDataReader

            cmd.CommandType = CommandType.Text
            data = cmd.ExecuteReader()

            While data.Read

                GetLeaderNameFromID = data.Item("firstname") 'data.Item("lastname") & ", " &

            End While

            cmd.Dispose()
            cmd = Nothing
            data = Nothing


        Catch ex As Exception

        End Try


    End Function


    Private Function GetPositionNameFromID(ByVal p_intPosID As Integer) As String
        Dim RS_PosName As OleDb.OleDbCommand

        Try
            RS_PosName = New OleDb.OleDbCommand("SELECT [Name] FROM TeamPositions WHERE [ID] = " & p_intPosID, m_objSQLConn)

            GetPositionNameFromID = RS_PosName.ExecuteScalar

            RS_PosName.Dispose()
            RS_PosName = Nothing


        Catch ex As Exception

        End Try


    End Function


#End Region

#Region "Most Recent/Popular"


    Private Function DrawHeader() As String
        Dim strRes As String = ""

        strRes += "<table>"
        strRes += "<tr bgcolor=""#993300"">"
        strRes += "<td>"
        If mvarDisplayMode = DISPMode.MostPopular Then
            strRes += "<h3>Most Popular Songs:</h3>"
        ElseIf mvarDisplayMode = DISPMode.RecentlyEdited Then
            strRes += "<h3>Recently Edited Songs:</h3>"
        End If
        strRes += "</td>"
        strRes += "</tr>"
        strRes += "<tr><td>"
        strRes += "<table cellpadding=""2"" cellspacing=""0"" width=""100%""   >"
        strRes += "<thead>"
        strRes += "<tr bgcolor=""#B86E00""> "
        strRes += "<th>Title</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Chart</th>"
        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
            strRes += "<th>Edit</th>"
        End If
        strRes += "<th>Mp3</th>"
        strRes += "<th>Video</th>"
        strRes += "</tr>"
        strRes += "</thead>"
        Return strRes

    End Function

    Private Sub DrawMostPopRecent()
        Dim objSB As New StringBuilder
        Dim cmd As OleDb.OleDbCommand
        Dim data As OleDb.OleDbDataReader

        Try

            objSB.Append(DrawHeader)

            If mvarDisplayMode = DISPMode.MostPopular Then
                cmd = New OleDb.OleDbCommand("SELECT TOP 10 * FROM Songs ORDER By Fav DESC", m_objSQLConn)
            Else
                cmd = New OleDb.OleDbCommand("SELECT TOP 10 * FROM Songs ORDER BY LastMod DESC", m_objSQLConn)
            End If

            data = cmd.ExecuteReader


            Do While data.Read

                objSB.Append("<tr>")
                objSB.Append("<td>")
                If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                    objSB.Append("<A HREF=""")
                    objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/songsheet.aspx?dbid=")
                    objSB.Append(data.Item("ID"))
                    objSB.Append(""" target=""_blank""><B>")
                    objSB.Append(data.Item("Title") & " ")
                    objSB.Append("</B></a> ")
                Else
                    objSB.Append(data.Item("Title") & " ")
                End If
                objSB.Append("</td>")
                objSB.Append("<td>")
                objSB.Append(data.Item("Author1") & "&nbsp;")
                objSB.Append("</td>")
                objSB.Append("<td>")
                objSB.Append(data.Item("Author2") & "&nbsp;")
                objSB.Append("</td>")
                objSB.Append("<td>")

                If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                    objSB.Append("<A HREF=""")
                    objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongSheet.aspx?dbid=")
                    objSB.Append(data.Item("ID"))
                    objSB.Append(""" target=""_blank""><B>")
                    objSB.Append("<center>")
                    objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
                    objSB.Append("</center>")
                    objSB.Append("</a>")
                Else
                    objSB.Append("&nbsp;")
                End If

                objSB.Append("</td>")

                If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
                    objSB.Append("<td>")
                    objSB.Append("<A HREF=""")
                    objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/editsong.aspx?dbid=")
                    objSB.Append(data.Item("ID"))
                    objSB.Append(""" target=""_blank""><B>")
                    objSB.Append("<center>")
                    objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_edit.png"" alt=""Click to Edit Chart"" border=""0"" height=""16"" width=""16"">")
                    objSB.Append("</center>")
                    objSB.Append("</a>")
                    objSB.Append("</td>")
                End If

                objSB.Append("<td>")
                If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) = True) And (data.Item("Mp3Link") & "" <> "") Then
                    objSB.Append("<A HREF=""")
                    objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=0&dbid=")
                    objSB.Append(data.Item("ID"))
                    objSB.Append(""" target=""_blank""><B>")
                    objSB.Append("<center>")
                    objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/loudspeaker.png"" alt=""Click to Download Mp3"" border=""0"" height=""16"" width=""16"">")
                    objSB.Append("</center>")
                    objSB.Append("</a>")
                Else

                    objSB.Append("&nbsp;")

                End If
                objSB.Append("</td>")

                If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (data.Item("YouTubeLink") & "" <> "") Then
                    objSB.Append("<td>")
                    objSB.Append("<A HREF=""http://ca.youtube.com/watch?v=")
                    objSB.Append(data.Item("YouTubeLink") & "")
                    objSB.Append(""" target=""_blank""><B>")
                    objSB.Append("<center>")
                    objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/tv.png"" alt=""Click to visit Youtube"" border=""0"" height=""16"" width=""16"">")
                    objSB.Append("</center>")
                    objSB.Append("</a>")
                    objSB.Append("</td>")
                ElseIf (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (data.Item("YouTubeLink") & "" = "") Then
                    objSB.Append("<td>&nbsp;</td>")
                End If



                objSB.Append("</tr>")


            Loop

            data.Close()
            data = Nothing


            objSB.Append("</table>")

            PlaceHolder1.Controls.Add(New LiteralControl(objSB.ToString))

        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try


    End Sub

#End Region

#Region "This Week"

    Private Function DrawThisWeeksSet_Header() As String
        Dim strRes As String = ""

        strRes += "<table width=""100%"">"
        strRes += "<tr bgcolor=""#993300"">"
        strRes += "<td>"
        strRes += "<h3>This Week's Set:</h3>"
        strRes += "</td>"
        strRes += "</tr>"
        strRes += "<tr><td>"
        strRes += "<table border=""0"" cellpadding=""2"" cellspacing=""0""  width=""100%"" >"
        strRes += "<thead>"
        strRes += "<tr bgcolor=""#B86E00""> "
        strRes += "<th>Title</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Chart</th>"
        strRes += "<th>Edit</th>"
        strRes += "<th>Mp3</th>"
        strRes += "<th>Video</th>"
        strRes += "<th>Direct Link</th>"
        strRes += "</tr>"
        strRes += "</thead>"

        Return strRes



    End Function

    Private Sub DrawThisWeeksSet()
        Dim objSB As New StringBuilder
        Dim cmd As New OleDb.OleDbCommand
        Dim data As OleDb.OleDbDataReader

        Dim cmd2 As OleDb.OleDbCommand
        Dim cmd2Rdr As OleDb.OleDbDataReader

        Dim strSQL As String = ""

        Try

            'Write the header
            objSB.Append(DrawThisWeeksSet_Header)

            cmd = New OleDb.OleDbCommand("SELECT TOP 1 * FROM [Set] WHERE  (DATEDIFF(dd, GETDATE(), Created) > 0) ORDER BY Created DESC", m_objSQLConn)
            cmd.CommandType = CommandType.Text
            data = cmd.ExecuteReader()

            Do While data.Read

                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("&nbsp;")
                objSB.Append("</td>")
                objSB.Append("</tr>")

                'Write out a row
                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("Topic: " & data.Item("Description") & " - " & "Led by: <b>" & GetLeaderNameFromID(data.Item("UserID")) & "</b> - Service Date: " & data.Item("PerfDate"))
                objSB.Append("</td>")
                objSB.Append("</tr>")

                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("&nbsp;")
                objSB.Append("</td>")
                objSB.Append("</tr>")

                strSQL = " SELECT SetListItems.SetID, SetListItems.SongID, SetListItems.UserID, SetListItems.PrintOrder, SetListItems.PartNames, SetListItems.SetOrder, SetListItems.Notes, SetListItems.TransKey, SetListItems.FontSize, SetListItems.BookMark, SetListItems.SongNotes, Songs.Title, Songs.Author1, Songs.Author2, Songs.Mp3Link, Songs.YouTubeLink "
                strSQL += "FROM SetListItems LEFT OUTER JOIN Songs ON SetListItems.SongID = Songs.id "
                strSQL += "WHERE     (SetListItems.SetID = " & data.Item("ID") & ") "
                strSQL += "ORDER BY SetListItems.PrintOrder ASC"

                cmd2 = New OleDb.OleDbCommand(strSQL, m_objSQLConn)
                cmd2.CommandType = CommandType.Text
                cmd2Rdr = cmd2.ExecuteReader()

                Do While cmd2Rdr.Read
                    If cmd2Rdr.Item("SongID") > 0 Then
                        objSB.Append("<tr>")
                        objSB.Append("<td>")
                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/songsheet.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append(cmd2Rdr.Item("Title") & " ")
                            objSB.Append("</B></a> ")
                        Else
                            objSB.Append(cmd2Rdr.Item("Title") & " ")
                        End If
                        objSB.Append("</td>")
                        objSB.Append("<td>")
                        objSB.Append(cmd2Rdr.Item("Author1") & "&nbsp;")
                        objSB.Append("</td>")
                        objSB.Append("<td>")
                        objSB.Append(cmd2Rdr.Item("Author2") & "&nbsp;")
                        objSB.Append("</td>")
                        objSB.Append("<td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongSheet.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else
                            objSB.Append("&nbsp;")
                        End If

                        objSB.Append("</td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
                            objSB.Append("<td>")
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/editsong.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_edit.png"" alt=""Click to Edit Chart"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                            objSB.Append("</td>")
                        End If

                        objSB.Append("<td>")
                        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) = True) And (cmd2Rdr.Item("Mp3Link") & "" <> "") Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=0&dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/loudspeaker.png"" alt=""Click to Download Mp3"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else

                            objSB.Append("&nbsp;")

                        End If
                        objSB.Append("</td>")

                        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (cmd2Rdr.Item("YouTubeLink") & "" <> "") Then
                            objSB.Append("<td>")
                            objSB.Append("<A HREF=""http://ca.youtube.com/watch?v=")
                            objSB.Append(cmd2Rdr.Item("YouTubeLink") & "")
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/tv.png"" alt=""Click to visit Youtube"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                            objSB.Append("</td>")
                        ElseIf (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (cmd2Rdr.Item("YouTubeLink") & "" = "") Then
                            objSB.Append("<td>&nbsp;</td>")
                        End If

                        objSB.Append("<td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SheetPrintOut.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append("&ordering=True&notes=" & IIf(cmd2Rdr.Item("SongNotes") = 1, "True", "False") & "&transkey=" & cmd2Rdr.Item("TransKey") & "&PartNames=" & IIf(cmd2Rdr.Item("PartNames") = 1, "True", "False") & "&TS=" & cmd2Rdr.Item("FontSize") & "&NS=" & cmd2Rdr.Item("FontSize") & "&action=1"" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else
                            objSB.Append("&nbsp;")
                        End If
                        objSB.Append("</td>")


                        objSB.Append("</tr>")
                    End If

                Loop

                cmd2.Dispose()
                cmd2 = Nothing
                cmd2Rdr = Nothing

            Loop

            objSB.Append("</table>")



            PlaceHolder1.Controls.Add(New LiteralControl(objSB.ToString))

            objSB = Nothing
            'objRS = Nothing

            GC.Collect()

        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try


    End Sub

#End Region


#Region "History Items"

    Private Function DrawPastWeeksSet_Header() As String
        Dim strRes As String = ""

        strRes += "<table width=""100%"">"
        strRes += "<tr bgcolor=""#993300"">"
        strRes += "<td>"
        strRes += "<h3>Past Week's Sets:</h3>"
        strRes += "</td>"
        strRes += "</tr>"
        strRes += "<tr><td>"
        strRes += "<table border=""0"" cellpadding=""0"" cellspacing=""0""  width=""100%"">"
        strRes += "<thead>"
        strRes += "<tr bgcolor=""#B86E00""> "
        strRes += "<th>Title</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Author</th>"
        strRes += "<th>Chart</th>"
        strRes += "<th>Edit</th>"
        strRes += "<th>Mp3</th>"
        strRes += "<th>Video</th>"
        strRes += "<th>Direct Link</th>"
        strRes += "</tr>"
        strRes += "</thead>"

        Return strRes



    End Function

    Private Sub DrawPastWeeksSet()
        Dim objSB As New StringBuilder
        Dim cmd As New OleDb.OleDbCommand
        Dim data As OleDb.OleDbDataReader

        Dim cmd2 As OleDb.OleDbCommand
        Dim cmd2Rdr As OleDb.OleDbDataReader

        Dim strSQL As String = ""

        Try

            'Write the header
            objSB.Append(DrawPastWeeksSet_Header)

            cmd = New OleDb.OleDbCommand("SELECT TOP 10 * FROM [Set] WHERE (DATEDIFF(dd, GETDATE(), Created) < 0) ORDER BY Created DESC", m_objSQLConn)
            cmd.CommandType = CommandType.Text
            data = cmd.ExecuteReader()

            Do While data.Read

                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("&nbsp;")
                objSB.Append("</td>")
                objSB.Append("</tr>")

                'Write out a row
                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("Topic: " & data.Item("Description") & " - " & "Led by: <b>" & GetLeaderNameFromID(data.Item("UserID")) & "</b> - Service Date: " & data.Item("PerfDate"))
                objSB.Append("</td>")
                objSB.Append("</tr>")

                objSB.Append("<tr>")
                objSB.Append("<td colspan=""7"">")
                objSB.Append("&nbsp;")
                objSB.Append("</td>")
                objSB.Append("</tr>")

                strSQL = " SELECT SetListItems.SetID, SetListItems.SongID, SetListItems.UserID, SetListItems.PrintOrder, SetListItems.PartNames, SetListItems.SetOrder, SetListItems.Notes, SetListItems.TransKey, SetListItems.FontSize, SetListItems.BookMark, SetListItems.SongNotes, Songs.Title, Songs.Author1, Songs.Author2, Songs.Mp3Link, Songs.YouTubeLink "
                strSQL += "FROM SetListItems LEFT OUTER JOIN Songs ON SetListItems.SongID = Songs.id "
                strSQL += "WHERE     (SetListItems.SetID = " & data.Item("ID") & ") "
                strSQL += "ORDER BY SetListItems.PrintOrder ASC"

                cmd2 = New OleDb.OleDbCommand(strSQL, m_objSQLConn)
                cmd2.CommandType = CommandType.Text
                cmd2Rdr = cmd2.ExecuteReader()

                Do While cmd2Rdr.Read
                    If cmd2Rdr.Item("SongID") > 0 Then
                        objSB.Append("<tr>")
                        objSB.Append("<td>")
                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/songsheet.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append(cmd2Rdr.Item("Title") & " ")
                            objSB.Append("</B></a> ")
                        Else
                            objSB.Append(cmd2Rdr.Item("Title") & " ")
                        End If
                        objSB.Append("</td>")
                        objSB.Append("<td>")
                        objSB.Append(cmd2Rdr.Item("Author1") & "&nbsp;")
                        objSB.Append("</td>")
                        objSB.Append("<td>")
                        objSB.Append(cmd2Rdr.Item("Author2") & "&nbsp;")
                        objSB.Append("</td>")
                        objSB.Append("<td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongSheet.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else
                            objSB.Append("&nbsp;")
                        End If

                        objSB.Append("</td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
                            objSB.Append("<td>")
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/editsong.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_edit.png"" alt=""Click to Edit Chart"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                            objSB.Append("</td>")
                        End If

                        objSB.Append("<td>")
                        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) = True) And (cmd2Rdr.Item("Mp3Link") & "" <> "") Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=0&dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/loudspeaker.png"" alt=""Click to Download Mp3"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else

                            objSB.Append("&nbsp;")

                        End If
                        objSB.Append("</td>")

                        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (cmd2Rdr.Item("YouTubeLink") & "" <> "") Then
                            objSB.Append("<td>")
                            objSB.Append("<A HREF=""http://ca.youtube.com/watch?v=")
                            objSB.Append(cmd2Rdr.Item("YouTubeLink") & "")
                            objSB.Append(""" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/tv.png"" alt=""Click to visit Youtube"" border=""0"" height=""16"" width=""16"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                            objSB.Append("</td>")
                        ElseIf (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (cmd2Rdr.Item("YouTubeLink") & "" = "") Then
                            objSB.Append("<td>&nbsp;</td>")
                        End If

                        objSB.Append("<td>")

                        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
                            objSB.Append("<A HREF=""")
                            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SheetPrintOut.aspx?dbid=")
                            objSB.Append(cmd2Rdr.Item("SongID"))
                            objSB.Append("&ordering=True&notes=" & IIf(cmd2Rdr.Item("SongNotes") = 1, "True", "False") & "&transkey=" & cmd2Rdr.Item("TransKey") & "&PartNames=" & IIf(cmd2Rdr.Item("PartNames") = 1, "True", "False") & "&TS=" & cmd2Rdr.Item("FontSize") & "&NS=" & cmd2Rdr.Item("FontSize") & "&action=1"" target=""_blank""><B>")
                            objSB.Append("<center>")
                            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
                            objSB.Append("</center>")
                            objSB.Append("</a>")
                        Else
                            objSB.Append("&nbsp;")
                        End If
                        objSB.Append("</td>")


                        objSB.Append("</tr>")
                    End If

                Loop

                cmd2.Dispose()
                cmd2 = Nothing
                cmd2Rdr = Nothing

            Loop

            objSB.Append("</table>")



            PlaceHolder1.Controls.Add(New LiteralControl(objSB.ToString))

            objSB = Nothing
            'objRS = Nothing

            GC.Collect()

        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try


    End Sub

#End Region



End Class
