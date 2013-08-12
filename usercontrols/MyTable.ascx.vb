Imports System.Data


Partial Class usercontrols_MyTable
    Inherits System.Web.UI.UserControl


    Public Enum DISPMode
        ByTitle = 0
        ByArtist = 1
        ByStyle = 2
        ByUse = 3
        Favourites = 4
        NewSongs = 5
        KeyWords = 6
    End Enum

    Public Enum TABLETYPE
        Lookup
        BookMark
        [Set]
        UserList
        LogActivity
    End Enum

    Private mvarPage As Integer
    Private mvarItemsPerPg As Integer = 25
    Private mvarFilterCode As Integer '= 0
    Private mvarKeyWords As String '= ""
    Private mvarDisplayMode As DISPMode '= DISPMode.ByTitle
    Private mvarTableType As TABLETYPE = TABLETYPE.Lookup

    Private mvarObjectCount As Integer = 0
    Private mvarPageCount As Integer = 0

    Public m_objSQLConn As OleDb.OleDbConnection
    Public m_objSec As clsSecurity

    Private adp As OleDb.OleDbDataAdapter
    Private objDS As DataSet
    Private StrSQL As String

#Region "Properties"

    Public Property setTableType As TABLETYPE
        Get
            Return mvarTableType
        End Get
        Set(ByVal value As TABLETYPE)
            mvarTableType = value
            If mvarTableType = TABLETYPE.UserList Then
                navicontrol1.Visible = False
                PagingControl1.Visible = False
            End If


        End Set
    End Property

    Public Property DisplayMode() As DISPMode
        Get
            Return mvarDisplayMode
        End Get
        Set(ByVal value As DISPMode)
            mvarDisplayMode = value
            'Draw()
        End Set
    End Property

    Public Property KeyWords() As String
        Get
            Return mvarKeyWords
        End Get
        Set(ByVal value As String)
            mvarKeyWords = value
            ' draw()
        End Set
    End Property

    Public Property FilterByID() As Integer
        Get
            Return mvarFilterCode
        End Get
        Set(ByVal value As Integer)
            mvarFilterCode = value
            ' draw()
        End Set
    End Property

    Public Property NumPerPage() As Integer
        Get
            Return mvarItemsPerPg
        End Get
        Set(ByVal value As Integer)
            mvarItemsPerPg = value
        End Set
    End Property

    Public Property PageNum() As Integer
        Get
            Return mvarPage
        End Get
        Set(ByVal value As Integer)
            mvarPage = value
        End Set
    End Property

#End Region

    Private Function GetFilterSQL() As String

        If mvarFilterCode <> 11 And mvarFilterCode <> 0 Then
            Return "AND [GroupID] = " & mvarFilterCode & " "
        Else
            Return ""
        End If

    End Function

    Public Sub REFRESH()


        Select Case setTableType
            Case TABLETYPE.Set
                DrawSet()

            Case TABLETYPE.BookMark, TABLETYPE.Lookup
                Draw()

            Case TABLETYPE.UserList
                DrawUserList()

            Case TABLETYPE.LogActivity


        End Select

        Try
            PagingControl1.PageCount = mvarPageCount
        Catch ex As Exception

        End Try

    End Sub

    Private Sub GetCounts(ByVal p_strSQL As String)

        Dim objCommand As New OleDb.OleDbCommand(p_strSQL, m_objSQLConn)
        Dim intTotalRecords As Integer

        Try

            intTotalRecords = objCommand.ExecuteScalar

            objCommand.Dispose()
            objCommand = Nothing

            mvarObjectCount = intTotalRecords
            mvarPageCount = Math.Ceiling(intTotalRecords / mvarItemsPerPg)

        Catch ex As Exception
            'Response.Write(ex.ToString)
        End Try

    End Sub

    Private Function GenerateLineColor(p_intUID As Integer, p_blnLocked As Boolean) As String
        Dim strRes As String = ""




        If IsUserActive(p_intUID) = True Then
            strRes = "style=""background-color:#000000;text-decoration: blink;"""
        Else
            If p_blnLocked Then
                strRes = "style=""background-color:maroon;"""
            Else
                ' strRes = "style=""background-color:#000000;"""
            End If
        End If
 

        Return strRes

    End Function

    Private Function IsUserActive(p_intUID As Integer) As Boolean
        Dim sql As String


        sql = "SELECT * FROM ActivityLog WHERE UID = " & p_intUID & " ORDER BY DTSTAMP Desc"

        Dim adp As New OleDb.OleDbDataAdapter(sql, m_objSQLConn)
        Dim objDS As New DataSet

        adp.SelectCommand.CommandType = CommandType.Text
        adp.Fill(objDS, 0, 1000, "UserActivity")

        Dim objDataTbl As DataTable = objDS.Tables(0)
        Dim objDataRow As DataRow

        For Each objDataRow In objDataTbl.Rows

            If DateDiff("n", objDataRow.Item("DTStamp"), Now()) <= 1440 Then
                IsUserActive = True
                Exit For
            End If

        Next

        objDataRow = Nothing
        objDataTbl.Dispose()
        objDataTbl = Nothing

        objDS.Dispose()
        objDS = Nothing

        adp.Dispose()
        adp = Nothing


    End Function

    Private Sub DrawUserList()

        Dim strRes As New StringBuilder
        Dim StrSQL As String = "SELECT * From [Users_New] WHERE [UserID] > 100  ORDER BY [UserName] ASC"
        Dim I As Integer = 1
        Dim intStartRec As Integer = 0
        Dim blnOdd As Boolean = True

        Try
            Dim adp As New OleDb.OleDbDataAdapter(StrSQL, m_objSQLConn)
            Dim objDS As New DataSet

            adp.SelectCommand.CommandType = CommandType.Text
            adp.Fill(objDS, 0, 1000, "UserList")

            Dim objDataTbl As DataTable = objDS.Tables(0)
            Dim objDataRow As DataRow
            strRes.AppendLine("<table summary=""User List"" width=""100%"" align=""left"" cellpadding=""2"" cellspacing=""0"">")

            strRes.AppendLine("<tr>")
            strRes.AppendLine("<th>Edit</th>")
            strRes.AppendLine("<th>UserName</th>")
            strRes.AppendLine("<th>First Name</th>")
            strRes.AppendLine("<th>Last Name</th>")
            strRes.AppendLine("<th>Email</th>")
            strRes.AppendLine("<th>Flag</th>")
            strRes.AppendLine("<th>Log</th>")
            strRes.AppendLine("<th>Enabled</th>")
            strRes.AppendLine("<th>Delete</th>")
            strRes.AppendLine("</tr>")

            For Each objDataRow In objDataTbl.Rows

                strRes.AppendLine(Draw_UserRow(objDataRow))
                strRes.AppendLine("<tr style=""background-color:#000000;""><td colspan=""8"">&nbsp;</td></tr>")

            Next

            strRes.AppendLine("</table>")

            strRes.AppendLine("<br/>")
            strRes.AppendLine("<br/>")
            strRes.AppendLine("<br/>")

            objDataRow = Nothing
            objDataTbl.Dispose()
            objDataTbl = Nothing

            objDS.Dispose()
            objDS = Nothing

            adp.Dispose()
            adp = Nothing

            PlaceHolder1.Controls.Add(New LiteralControl(strRes.ToString))
            navicontrol1.Visible = False
            PagingControl1.Visible = False


        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try

    End Sub


    Private Function Draw_UserRow(ByRef p_objRow As DataRow) As String
        Dim objSB As New StringBuilder
        Dim intCount As Integer = 0
        Dim strDTStamp As String = ""

        objSB.AppendLine("<tr " & GenerateLineColor(p_objRow.Item("UserID"), p_objRow.Item("userlocked")) & ">")


        'Edit Link
        objSB.AppendLine("<td>")
        objSB.AppendLine("<a href=""UserEdit.aspx?ID=" & p_objRow.Item("UserID") & """ target=""_blank"">Edit</a>")
        objSB.AppendLine("</td>")

        'UserName
        objSB.AppendLine("<td>")
        objSB.AppendLine(p_objRow("username") & "&nbsp;")
        objSB.AppendLine("</td>")

        'First Name
        objSB.AppendLine("<td>")
        objSB.AppendLine(p_objRow("firstname") & "&nbsp;")
        objSB.AppendLine("</td>")

        'Last Name
        objSB.AppendLine("<td>")
        objSB.AppendLine(p_objRow("lastname") & "&nbsp;")
        objSB.AppendLine("</td>")

        'Email
        objSB.AppendLine("<td>")
        objSB.AppendLine(p_objRow("email") & "&nbsp;")
        objSB.AppendLine("</td>")

        ''Flag
        objSB.AppendLine("<td>")
        strDTStamp = ""
        If FlagUser(p_objRow.Item("UserID"), intCount, strDTStamp) = True Then
            objSB.AppendLine("<img src=""images/flag_red.png"" title=""User has downloaded " & intCount & " mp3's since: " & strDTStamp & """ width=""16"" />")
        Else
            objSB.AppendLine("&nbsp;")
        End If
        objSB.AppendLine("</td>")

        'View Log
        objSB.AppendLine("<td>")
        objSB.AppendLine("<a href=""ViewUserLog.aspx?ID=" & p_objRow.Item("UserID") & """ target=""_blank"">View</a>")
        objSB.AppendLine("</td>")

        ''Enabled
        objSB.AppendLine("<td>")
        If p_objRow.Item("userlocked") = 0 Then
            objSB.AppendLine("<a href=""ToggleLockout.aspx?ID=" & p_objRow.Item("UserID") & """ target=""_blank"">Disable</a>")
        Else
            objSB.AppendLine("<a href=""ToggleLockout.aspx?ID=" & p_objRow.Item("UserID") & """ target=""_blank"">Enable</a>")
        End If
        objSB.AppendLine("</td>")

        'Delete
        objSB.AppendLine("<td>")
        objSB.AppendLine("<a href=""UserDelete.aspx?ID=" & p_objRow.Item("UserID") & """ target=""_blank"">Delete</a>")
        objSB.AppendLine("</td>")

        objSB.Append("</tr>")

        Draw_UserRow = objSB.ToString
        objSB = Nothing

    End Function

    Private Function FlagUser(p_intID As Integer, ByRef p_intCount As Integer, ByRef p_strInitDT As String) As Boolean
        'Should this user be flagged for being bad?
        FlagUser = False
        Dim strSQL As String = ""


        'Get mp3's down count
        strSQL = "SELECT COUNT(DISTINCT SongID) AS Expr1 FROM ActivityLog WHERE UID=" & p_intID & " AND ActionCode=" & clsSecurity.Permission_ActionCodes.act_Mp3Download

        Dim objCommand As New OleDb.OleDbCommand(strSQL, m_objSQLConn)
        Dim intTotalRecords As Integer

        Try

            intTotalRecords = objCommand.ExecuteScalar

            objCommand.Dispose()
            objCommand = Nothing

            If intTotalRecords > CInt(ConfigurationManager.AppSettings("gsUserAbuseThreshold").ToString) Then FlagUser = True
            p_intCount = intTotalRecords

        Catch ex As Exception

        End Try

        strSQL = "SELECT  TOP (1) DTStamp FROM ActivityLog WHERE UID=" & p_intID & " ORDER BY DTStamp ASC"

        objCommand = New OleDb.OleDbCommand(strSQL, m_objSQLConn)
        Try

            p_strInitDT = objCommand.ExecuteScalar.ToString

            objCommand.Dispose()
            objCommand = Nothing

        Catch ex As Exception

        End Try


    End Function


    Private Sub DrawSet()

        Dim strRes As New StringBuilder
        Dim StrSQL As String = "SELECT * FROM [Set] Where UserID = " & m_objSec.UserID & " ORDER BY PERFDATE DESC"
        Dim I As Integer = 1
        Dim intStartRec As Integer = 0
        Dim blnOdd As Boolean = True

        Try
            Dim adp As New OleDb.OleDbDataAdapter(StrSQL, m_objSQLConn)
            Dim objDS As New DataSet

            adp.SelectCommand.CommandType = CommandType.Text
            adp.Fill(objDS, 0, 25, "Set")

            Dim objDataTbl As DataTable = objDS.Tables(0)
            Dim objDataRow As DataRow
            strRes.AppendLine("<table  summary=""Song Database"" width=""100%"" align=""center"" cellpadding=""2"" cellspacing=""0"">")

            For Each objDataRow In objDataTbl.Rows

                strRes.AppendLine("<tr style=""background-image:/images/title-bg.gif"">")
                strRes.AppendLine("<td >")
                strRes.AppendLine("Description: " & objDataRow.Item("Description").ToString & "  To be performed on: " & objDataRow.Item("PerfDate").ToString)
                strRes.AppendLine("</td>")
                strRes.AppendLine("</tr>")

                strRes.AppendLine("<tr><td>")

                strRes.AppendLine("<!---" & StrSQL & "--->")
                strRes.AppendLine("<table    width=""100%""   align=""center"" cellpadding=""2"" cellspacing=""0"">")

                strRes.AppendLine(DrawHeader2)

                Dim adp2 As New OleDb.OleDbDataAdapter("Select * FROM SetListItems WHERE SetID = " & objDataRow.Item("ID").ToString & " ORDER BY SetORDER", m_objSQLConn)
                Dim objDS2 As New DataSet
                Dim objDataTbl2 As DataTable
                Dim objDataRow2 As DataRow

                adp2.SelectCommand.CommandType = CommandType.Text
                adp2.Fill(objDS2)
                objDataTbl2 = objDS2.Tables(0)

                For Each objDataRow2 In objDataTbl2.Rows

                    'Write Row!
                    strRes.AppendLine(DrawRow_Set(objDataRow2, blnOdd))
                    blnOdd = Not blnOdd
                Next

                strRes.AppendLine("</table>")

                objDataRow2 = Nothing
                objDataTbl2.Dispose()
                objDataTbl2 = Nothing

                objDS2.Dispose()
                objDS2 = Nothing

                adp2.Dispose()
                adp2 = Nothing

                strRes.AppendLine("</td></tr>  <tr><td>&nbsp</td></tr>")

            Next

            strRes.AppendLine("</table>")

            objDataRow = Nothing
            objDataTbl.Dispose()
            objDataTbl = Nothing

            objDS.Dispose()
            objDS = Nothing

            adp.Dispose()
            adp = Nothing

            PlaceHolder1.Controls.Add(New LiteralControl(strRes.ToString))
            navicontrol1.Visible = False
            PagingControl1.Visible = False


        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try

    End Sub

    Private Sub Draw()
        Dim strRes As New StringBuilder

        Dim I As Integer = 1
        Dim intStartRec As Integer = 0
        Dim blnOdd As Boolean = True

        Try

            If Page.IsPostBack = True Then

                Try
                    DisplayMode = navicontrol1.SearchType
                Catch ex As Exception

                End Try

                FilterByID = navicontrol1.GroupID
                KeyWords = navicontrol1.SearchText

            End If
            StrSQL = BuildSQL()
            strRes.AppendLine("<!---Debug Info - SQL:" & StrSQL & "--->")

            strRes.AppendLine("<table   summary=""Song Database"" width=""100%""   align=""center"" cellpadding=""2"" cellspacing=""0"">")

            strRes.AppendLine(DrawHeader)


            'Dim adp As New OleDb.OleDbDataAdapter(StrSQL, m_objSQLConn)
            'Dim objDS As New DataSet
            adp = New OleDb.OleDbDataAdapter(StrSQL, m_objSQLConn)
            adp.SelectCommand.CommandType = CommandType.Text
            objDS = New DataSet

            'End If

            strRes.AppendLine("<!---" & mvarPage & "--->")
            If mvarPage < 1 Then mvarPage = 1
            intStartRec = (mvarPage - 1) * mvarItemsPerPg
            If intStartRec < 1 Then intStartRec = 1

            'PagingControl1.PageNum = mvarPage


            strRes.AppendLine("<!---" & intStartRec - 1 & "--->")
            strRes.AppendLine("<!---" & mvarItemsPerPg & "--->")
            ' If Page.IsPostBack = False Then
            adp.Fill(objDS, intStartRec - 1, mvarItemsPerPg, "Songs")
            'End If

            Dim objDataTbl As DataTable = objDS.Tables(0)
            Dim objDataRow As DataRow

            'PagingControl1.PageCount = CInt((objDataTbl.Rows.Count / mvarItemsPerPg) * 100)
            'mvarObjectCount = objDataTbl.Rows.Count

            For Each objDataRow In objDataTbl.Rows

                'Write Row!
                strRes.AppendLine(DrawRow(objDataRow, blnOdd))
                blnOdd = Not blnOdd
            Next

            'Try
            '    mvarPageCount = mvarObjectCount / mvarItemsPerPg
            'Catch ex As Exception

            'End Try
            strRes.AppendLine(DrawFooter(mvarObjectCount, mvarPageCount))

            strRes.AppendLine("</table>")

            objDataRow = Nothing
            objDataTbl.Dispose()
            objDataTbl = Nothing

            objDS.Dispose()
            objDS = Nothing

            adp.Dispose()
            adp = Nothing

            If PlaceHolder1.Controls.Count > 0 Then
                PlaceHolder1.Controls.Clear()
            End If

            PlaceHolder1.Controls.Add(New LiteralControl(strRes.ToString))

            If setTableType <> TABLETYPE.Lookup Then
                navicontrol1.Visible = False
            End If

            If setTableType = TABLETYPE.Set Then
                navicontrol1.Visible = False
                PagingControl1.Visible = False
            End If

        Catch ex As Exception
            PlaceHolder1.Controls.Add(New LiteralControl(ex.ToString))
        End Try

    End Sub

    Private Function SQLEncode(ByVal p_str As String) As String
        If String.IsNullOrEmpty(p_str) = True Then Exit Function
        p_str = p_str.Replace("'", "''")
        p_str = p_str.Trim
        Return p_str
    End Function

    Private Function BuildSQL() As String

        Dim strSQL As String = ""
        Dim strCountSQL As String = ""
        Dim intUserID As Integer = m_objSec.UserID

        Dim blnIsSiteAdmin As Boolean = m_objSec.Permission(clsSecurity.Permissions.siteadmin)

        Select Case mvarDisplayMode

            Case DISPMode.ByTitle

                Select Case mvarTableType
                    Case TABLETYPE.Lookup

                        If GetFilterSQL() = "" Then
                            strSQL = "SELECT * FROM Songs WHERE [Title] like '%" & SQLEncode(mvarKeyWords) & "%'  AND Deleted = 0 ORDER BY [Title] asc"
                        Else
                            strSQL = "SELECT * From Songs WHERE [Title] like '%" & SQLEncode(mvarKeyWords) & "%' " & GetFilterSQL() & " AND Deleted = 0 ORDER BY [Title] asc"
                        End If

                        If blnIsSiteAdmin = True Then
                            strSQL = Replace(strSQL, "AND Deleted = 0 ", "")
                        End If

                        strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Title] asc", "")

                    Case TABLETYPE.BookMark

                        If GetFilterSQL() = "" Then
                            strSQL = "SELECT * FROM Songs LEFT OUTER JOIN Bookmarks ON Songs.id = Bookmarks.songid WHERE (Bookmarks.userid = " & m_objSec.UserID & ") " 'AND Songs.Title like '%" & SQLEncode(mvarKeyWords) & "%'  and Songs.Deleted = 0 ORDER BY Songs.Title asc"
                        Else
                            strSQL = "SELECT * FROM Songs LEFT OUTER JOIN Bookmarks ON Songs.id = Bookmarks.songid WHERE (Bookmarks.userid = " & m_objSec.UserID & ") " & GetFilterSQL() & " AND Songs.Title like '%" & SQLEncode(mvarKeyWords) & "%' and Songs.Deleted = 0 ORDER BY Songs.Title asc"
                        End If

                        If blnIsSiteAdmin = True Then
                            strSQL = Replace(strSQL, "and Songs.Deleted = 0 ", "")
                        End If

                        strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY Songs.Title asc", "")

                    Case TABLETYPE.Set



                End Select

            Case DISPMode.ByStyle

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM Songs WHERE Deleted = 0 ORDER BY [Style] asc, [Title] asc"
                Else
                    strSQL = "SELECT * FROM Songs " & GetFilterSQL() & " AND Deleted = 0 ORDER BY [Style] asc, [Title] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "WHERE Deleted = 0 ", "")
                    strSQL = Replace(strSQL, "AND Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Style] asc, [Title] asc", "")

            Case DISPMode.ByArtist

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM [Songs] WHERE ([Author1] like '%" & SQLEncode(mvarKeyWords) & "%' OR [Author2] like '%" & mvarKeyWords & "%') and Deleted = 0 ORDER BY [Author1] asc, [Author2] asc, [Title] asc"
                Else
                    strSQL = "SELECT * FROM [Songs] " & GetFilterSQL() & " AND ([Author1] like '%" & SQLEncode(mvarKeyWords) & "%' OR [Author2] like '%" & mvarKeyWords & "%') and Deleted = 0 ORDER BY [Author1] asc, [Author2] asc, [Title] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "and Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Author1] asc, [Author2] asc, [Title] asc", "")

            Case DISPMode.ByUse

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM [Songs] WHERE Deleted = 0 ORDER BY [Use1] asc, [Use2] asc"
                Else
                    strSQL = "SELECT * FROM [Songs] " & GetFilterSQL() & " AND Deleted = 0 ORDER BY [Use1] asc, [Use2] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "WHERE Deleted = 0 ", "")
                    strSQL = Replace(strSQL, "AND Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Use1] asc, [Use2] asc", "")

            Case DISPMode.Favourites

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM [Songs] WHERE Deleted = 0 ORDER BY [Fav] desc, [Title] asc"
                Else
                    strSQL = "SELECT * FROM [Songs] " & GetFilterSQL() & " AND Deleted = 0 ORDER BY [Fav] desc, [Title] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "WHERE Deleted = 0 ", "")
                    strSQL = Replace(strSQL, "AND Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Fav] desc, [Title] asc", "")

            Case DISPMode.KeyWords

                mvarKeyWords = Replace(Trim(mvarKeyWords), " ", "%")

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM Songs WHERE [SongText] like '%" & SQLEncode(mvarKeyWords) & "%' and Deleted = 0 ORDER BY [Title] asc"
                Else
                    strSQL = "SELECT * FROM [Songs] " & GetFilterSQL() & " AND [SongText] like '%" & SQLEncode(mvarKeyWords) & "%' and Deleted = 0 ORDER BY [Title] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "and Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Title] asc", "")

            Case DISPMode.NewSongs

                If GetFilterSQL() = "" Then
                    strSQL = "SELECT * FROM [Songs] WHERE Deleted = 0 ORDER BY [Cdate] desc, [Title] asc"
                Else
                    strSQL = "SELECT * FROM [Songs] " & GetFilterSQL() & " AND Deleted = 0 ORDER BY [Cdate] desc, [Title] asc"
                End If

                If blnIsSiteAdmin = True Then
                    strSQL = Replace(strSQL, "WHERE Deleted = 0 ", "")
                    strSQL = Replace(strSQL, "AND Deleted = 0 ", "")
                End If

                strCountSQL = "SELECT COUNT(*) " & strSQL.Substring(8).Replace(" ORDER BY [Cdate] desc, [Title] asc", "")

        End Select

        If strCountSQL <> "" Then
            'Response.Write(strCountSQL)
            GetCounts(strCountSQL)

        End If

        Return strSQL

    End Function

    Private Function DrawHeader2() As String

        Dim strRes As String = ""


        strRes += "<thead >"
        strRes += "<tr  bgcolor=""#B86E00"">"
        strRes += "<th scope=""col"">Title</th>"

        strRes += "<th width=""20"" scope=""col"">Chart</th>"

        strRes += "<th width=""20"" scope=""col"">Video</th>"



        strRes += "<th width=""20"" scope=""col"">Mp3</th>"

        strRes += "<th scope=""col"" width=""20"">File</th>"


        strRes += "</tr>"
        strRes += "</thead>"

        Return strRes

    End Function


    Private Function DrawHeader() As String

        Dim strRes As String = ""


        strRes += "<thead >"
        strRes += "<tr bgcolor=""#B86E00"" >"
        strRes += "<th scope=""col"">Title</th>"
        strRes += "<th width=""20"" scope=""col"" >Artist</th>"

        strRes += "<th scope=""col"" width=""20"">Artist</th>"

        strRes += "<th width=""20"" scope=""col"">Chart</th>"

        strRes += "<th width=""20"" scope=""col"">Video</th>"

        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
            strRes += "<th width=""20"" scope=""col"">Edit</th>"
        End If

        If m_objSec.Permission(clsSecurity.Permissions.siteadmin) = True Then
            strRes += "<th scope=""col"" width=""20"">Delete</th>"
        End If

        strRes += "<th width=""20"" scope=""col"">Mp3</th>"

        strRes += "<th scope=""col"" width=""20"">File</th>"

        Select Case mvarTableType
            Case TABLETYPE.Lookup
                strRes += "<th scope=""col"" width=""20"">Bookmark</th>"
            Case TABLETYPE.BookMark
                strRes += "<th scope=""col"" width=""20"">UnBookmark</th>"
            Case Else

        End Select

        strRes += "</tr>"
        strRes += "</thead>"

        Return strRes

    End Function

    Private Function DrawRow_Set(ByRef p_objSetRow As DataRow, Optional ByVal p_blnOdd As Boolean = False) As String

        Dim objSB As New StringBuilder
        Dim adp As New OleDb.OleDbDataAdapter("Select * FROM Songs WHERE ID = " & p_objSetRow.Item("SongID").ToString, m_objSQLConn)
        Dim objDS As New DataSet
        adp.SelectCommand.CommandType = CommandType.Text
        adp.Fill(objDS, "SongsDetail")

        Dim objDataTbl As DataTable = objDS.Tables(0)
        Dim objDataRow As DataRow = objDataTbl.Rows(0)


        If p_blnOdd = True Then
            objSB.AppendLine("<tr class=""odd"">")
        Else
            objSB.AppendLine("<tr>")
        End If

        'Title
        objSB.AppendLine("<td>")
        'objSB.AppendLine("<IMG id=""song" & objDataRow("ID") & "_option"" onmouseover=""this.style.cursor='hand'"" onclick=""javascript: toggleWindow('song" & objDataRow("ID") & "');"" height=15 alt=""More Details about this Song.."" src=""/images/navigate_open.png"" width=15 align=absMiddle border=0 name=""song" & objDataRow("ID") & "_option"">")
        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/songsheet.aspx?ID=")
            objSB.Append(objDataRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append(objDataRow("Title") & " ")
            objSB.Append("</B></a> ")
        Else
            objSB.Append(objDataRow("Title") & " ")
        End If
        objSB.Append("</td>")

        'Chart
        objSB.Append("<td>")
        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongSheet.aspx?dbid=")
            objSB.Append(objDataRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
        Else
            objSB.Append("&nbsp;")
        End If
        objSB.Append("</td>")


        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (objDataRow("YouTubeLink") & "" <> "") Then
            objSB.Append("<td>")
            objSB.Append("<A HREF=""http://ca.youtube.com/watch?v=")
            objSB.Append(objDataRow("YouTubeLink") & "")
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/tv.png"" alt=""Click to visit Youtube"" border=""0"" height=""16"" width=""16"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
            objSB.Append("</td>")
        ElseIf (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (objDataRow("YouTubeLink") & "" = "") Then
            objSB.Append("<td>&nbsp;</td>")
        End If

        objSB.Append("<td>")
        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) = True) And (objDataRow("Mp3Link") & "" <> "") Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=0&dbid=")
            objSB.Append(objDataRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/loudspeaker.png"" alt=""Click to Download Mp3"" border=""0"" height=""16"" width=""16"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
        Else

            objSB.Append("&nbsp;")

        End If
        objSB.Append("</td>")

        objSB.Append("<td>")
        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_PPT_Download) = True) And (objDataRow("pptLink") & "" <> "") Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=1&dbid=")
            objSB.Append(objDataRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/f_zip_16.gif"" alt=""Click to Download Attached File"" border=""0"" height=""16"" width=""16"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
        Else

            objSB.Append("&nbsp;")

        End If
        objSB.Append("</td>")
        objSB.Append("</tr>")


        DrawRow_Set = objSB.ToString
        objSB = Nothing

    End Function


    Private Function DrawRow(ByRef p_objRow As DataRow, Optional ByVal p_blnOdd As Boolean = False) As String

        Dim objSB As New StringBuilder

        If p_blnOdd = True Then
            objSB.AppendLine("<tr class=""odd"">")
        Else
            objSB.AppendLine("<tr>")
        End If

        'Title
        objSB.AppendLine("<td>")
        objSB.AppendLine("<IMG id=""song" & p_objRow("ID") & "_option"" onmouseover=""this.style.cursor='hand'"" onclick=""javascript: toggleWindow('song" & p_objRow("ID") & "');"" height=15 alt=""More Details about this Song.."" src=""/images/navigate_open.png"" width=15 align=absMiddle border=0 name=""song" & p_objRow("ID") & "_option"">")
        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/songsheet.aspx?dbid=")
            objSB.Append(p_objRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append(p_objRow("Title") & " ")
            objSB.Append("</B></a> ")
        Else
            objSB.Append(p_objRow("Title") & " ")
        End If
        objSB.Append("</td>")

        'Author1
        objSB.Append("<td>")
        objSB.Append(p_objRow("Author1") & "&nbsp;")
        objSB.Append("</td>")

        'Author2
        objSB.Append("<td>")
        objSB.Append(p_objRow("Author2") & "&nbsp;")
        objSB.Append("</td>")

        'Chart
        objSB.Append("<td>")
        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True Then
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongSheet.aspx?dbid=")
            objSB.Append(p_objRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_music.png"" alt=""Click to View Information"" border=""0"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
        Else
            objSB.Append("&nbsp;")
        End If
        objSB.Append("</td>")

        If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (p_objRow("YouTubeLink") & "" <> "") Then
            objSB.Append("<td>")
            objSB.Append("<A HREF=""http://ca.youtube.com/watch?v=")
            objSB.Append(p_objRow("YouTubeLink") & "")
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/tv.png"" alt=""Click to visit Youtube"" border=""0"" height=""16"" width=""16"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
            objSB.Append("</td>")
        ElseIf (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongRender) = True) And (p_objRow("YouTubeLink") & "" = "") Then
            objSB.Append("<td>&nbsp;</td>")
        End If


        If m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongEdit) = True Then
            objSB.Append("<td>")
            objSB.Append("<A HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/editsong.aspx?dbid=")
            objSB.Append(p_objRow("ID"))
            objSB.Append(""" target=""_blank""><B>")
            objSB.Append("<center>")
            objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/document_edit.png"" alt=""Click to Edit Chart"" border=""0"" height=""16"" width=""16"">")
            objSB.Append("</center>")
            objSB.Append("</a>")
            objSB.Append("</td>")
        End If

        If m_objSec.Permission(clsSecurity.Permissions.siteadmin) = True Then
            objSB.Append("<td>")
            objSB.Append("<A target=""_blank"" HREF=""")
            objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongDelete.aspx?dbid=")
            objSB.Append(p_objRow("ID"))
            If p_objRow("Deleted") = 1 Then
                objSB.Append("&undelete=true"">")
            Else
                objSB.Append(""">")
            End If
            objSB.Append("<center>")
            If p_objRow("Deleted") = 1 Then
                objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/create_file.png"" alt=""Click to Restore This song"" border=""0"" height=""16"" width=""16"">")
            Else
                objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/delete.png"" alt=""Click to Delete This song"" border=""0"" height=""16"" width=""16"">")
            End If
            objSB.Append("</center>")
            objSB.Append("</a>")
            objSB.Append("</td>")
        End If

            objSB.Append("<td>")
            If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_Mp3Download) = True) And (p_objRow("Mp3Link") & "" <> "") Then
                objSB.Append("<A HREF=""")
                objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=0&dbid=")
                objSB.Append(p_objRow("ID"))
                objSB.Append(""" target=""_blank""><B>")
                objSB.Append("<center>")
                objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/loudspeaker.png"" alt=""Click to Download Mp3"" border=""0"" height=""16"" width=""16"">")
                objSB.Append("</center>")
                objSB.Append("</a>")
            Else

                objSB.Append("&nbsp;")

            End If
            objSB.Append("</td>")

            objSB.Append("<td>")
            If (m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_PPT_Download) = True) And (p_objRow("pptLink") & "" <> "") Then
                objSB.Append("<A HREF=""")
                objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/filedownload.aspx?contentid=1&dbid=")
                objSB.Append(p_objRow("ID"))
                objSB.Append(""" target=""_blank""><B>")
                objSB.Append("<center>")
                objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/f_zip_16.gif"" alt=""Click to Download Attached File"" border=""0"" height=""16"" width=""16"">")
                objSB.Append("</center>")
                objSB.Append("</a>")
            Else

                objSB.Append("&nbsp;")

            End If
            objSB.Append("</td>")

            Select Case mvarTableType
                Case TABLETYPE.Set

                Case TABLETYPE.BookMark

                    If m_objSec.Permission(clsSecurity.Permissions.setadmin) = True Then
                        objSB.Append("<td>")
                        objSB.Append("<A HREF=""")
                        objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongBookmark.aspx?dbid=")
                        objSB.Append(p_objRow("ID"))
                        objSB.Append("&unbkmark=true"" target=""_blank"">")
                        objSB.Append("<center>")
                        objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/bookmark_delete.png"" alt=""Click to Delete This song"" border=""0"" height=""16"" width=""16"">")
                        objSB.Append("</center>")
                        objSB.Append("</a>")
                        objSB.Append("</td>")
                    End If

                Case TABLETYPE.Lookup

                    If m_objSec.Permission(clsSecurity.Permissions.setadmin) = True Then
                        objSB.Append("<td>")
                        objSB.Append("<A HREF=""")
                        objSB.Append(ConfigurationManager.AppSettings("gsSiteURL") & "/SongBookmark.aspx?dbid=")
                        objSB.Append(p_objRow("ID"))
                        objSB.Append(""" target=""_blank"">")
                        objSB.Append("<center>")
                        objSB.Append("<img src=""" & ConfigurationManager.AppSettings("gsSiteURL") & "/images/bookmark.png"" alt=""Click to Delete This song"" border=""0"" height=""16"" width=""16"">")
                        objSB.Append("</center>")
                        objSB.Append("</a>")
                        objSB.Append("</td>")
                    End If

            End Select

            objSB.Append("</tr>")

            objSB.Append("<tr class=""odd"" style=""display: none;"" name=""song" & p_objRow("ID") & """ id=""song" & p_objRow("ID") & """> ")

            Select Case mvarTableType
                Case TABLETYPE.Set
                    objSB.Append("<td colspan=""9"">")

                Case TABLETYPE.BookMark, TABLETYPE.Lookup
                    objSB.Append("<td colspan=""10"">")

            End Select

            objSB.Append("<table width=""100%"" border=""0"" cellpadding=""2"">")
            objSB.Append("<tr>")
            objSB.Append("<TD valign=""top""><B>CCLI</B></TD>")
            objSB.Append("<TD valign=""top"" align=""left""><a href=""http://www.ccli.com/songselect/skins/visitor/song_detail.cfm?id="" target=""_blank""></a></TD>")

            objSB.Append("</tr>")
            objSB.Append("<TR>")
            objSB.Append("<TD valign=""top""><B>Song Lyrics:</B></TD>")
            objSB.Append("<TD valign=""top"" align=""left""><p class=""SongNoNotes"">" & p_objRow("SongText") & "</p></TD>")

            objSB.Append("</TR>")
            objSB.Append("</table></td>")

            objSB.Append("</tr>")

            DrawRow = objSB.ToString
            objSB = Nothing

    End Function

    Private Function DrawFooter(ByVal p_intSetSize As Integer, ByVal p_intPageCount As Integer) As String

        Dim strRes As String = ""

        strRes += "<thead >"
        strRes += "<TR> "
        strRes += "<TH align=""left"">Logged in: <I>" & m_objSec.UserName & "</I> </TH>"

        Select Case mvarTableType
            Case TABLETYPE.Set
                strRes += "<TH colspan=""8"" align=""right"">  " & p_intSetSize & " Results, " & p_intPageCount & " pages </TH>"

            Case TABLETYPE.BookMark, TABLETYPE.Lookup
                strRes += "<TH colspan=""9"" align=""right"">  " & p_intSetSize & " Results, " & p_intPageCount & " pages </TH>"

        End Select

        strRes += "</TR>"
        strRes += "</thead>"

        Return strRes

    End Function

    'Public Sub New(ByRef p_objConn As OleDb.OleDbConnection, ByRef p_objSec As clsSecurity)
    '    m_objSQLConn = p_objConn
    '    m_objSec = p_objSec
    'End Sub

    Protected Sub PagingControl1_First() Handles PagingControl1.First
        'PageNum = 1
        'REFRESH()
        'PagingControl1.PageNum = PageNum
    End Sub

    Protected Sub PagingControl1_Go_To(ByVal p_int As Integer) Handles PagingControl1.Go_To
        PageNum = p_int

        REFRESH()

    End Sub

    Protected Sub PagingControl1_goLast() Handles PagingControl1.goLast
        'PageNum = mvarPageCount
        'REFRESH()
        'PagingControl1.PageNum = PageNum
    End Sub

    Protected Sub PagingControl1_goNext() Handles PagingControl1.goNext
        'PageNum += 1
        'REFRESH()
        'PagingControl1.PageNum = PageNum
    End Sub

    Protected Sub PagingControl1_Prev() Handles PagingControl1.Prev
        'PageNum -= 1
        'REFRESH()
        'PagingControl1.PageNum = PageNum
    End Sub

    Protected Sub navicontrol1_Refresh(p_intType As Integer, p_intFilter As Integer, p_strKeywords As String) Handles navicontrol1.Refresh

        Try
            DisplayMode = p_intType
        Catch ex As Exception

        End Try

        FilterByID = p_intFilter
        KeyWords = p_strKeywords
        REFRESH()

    End Sub
End Class
