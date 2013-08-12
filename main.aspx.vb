Imports System.Data

Partial Class main
    Inherits System.Web.UI.Page

    Public StrName As String = ""
    Public strEmail As String = ""
    Private m_objPageConn As OleDb.OleDbConnection


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objSec As clsSecurity

        Try

            m_objPageConn = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
            m_objPageConn.Open()

            objSec = HttpContext.Current.Session.Item("Security")
            If Not objSec Is Nothing Then
                StrName = objSec.FirstName & " " & objSec.LastName
                strEmail = objSec.EmailAddress
            Else
                Response.Redirect("logout.aspx")
            End If

            If String.IsNullOrEmpty(Request.QueryString("Tab")) = True Then

                HttpContext.Current.Session.Item("Tab") = 0

            Else

                HttpContext.Current.Session.Item("Tab") = CInt(Request.QueryString("Tab").ToString)

            End If

            Select Case HttpContext.Current.Session.Item("Tab")

                Case "0" 'Home

                    MiniSLControl1.m_objSQLConn = m_objPageConn
                    MiniSLControl2.m_objSQLConn = m_objPageConn
                    MiniSLControl3.m_objSQLConn = m_objPageConn
                    MiniSLControl4.m_objSQLConn = m_objPageConn
                    MiniSLControl5.m_objSQLConn = m_objPageConn


                    MiniSLControl1.m_objSec = objSec
                    MiniSLControl2.m_objSec = objSec
                    MiniSLControl3.m_objSec = objSec
                    MiniSLControl4.m_objSec = objSec
                    MiniSLControl5.m_objSec = objSec


                    MiniSLControl1.Draw()
                    MiniSLControl2.Draw()
                    If objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = True Then
                        MiniSLControl3.Draw()
                        MiniSLControl4.Draw()
                        MiniSLControl5.Draw()
                    End If

                Case "1" 'Lookup

                    MainTableControl1.m_objSQLConn = m_objPageConn

                    MainTableControl1.NumPerPage = 25
                    MainTableControl1.setTableType = usercontrols_MyTable.TABLETYPE.Lookup

                    If Page.IsPostBack = False Then
                        MainTableControl1.DisplayMode = usercontrols_MyTable.DISPMode.ByTitle
                        MainTableControl1.FilterByID = 11
                        MainTableControl1.keywords = ""

                    End If

                    MainTableControl1.m_objSec = objSec
                    If Not MainTableControl1.m_objSec Is Nothing Then
                        MainTableControl1.REFRESH()
                    End If



                Case "2" 'BookMark

                    MainTableControl2.m_objSQLConn = m_objPageConn
                    'MainTableControl2.P = 1
                    MainTableControl2.NumPerPage = 25
                    MainTableControl2.DisplayMode = usercontrols_MyTable.DISPMode.ByTitle
                    MainTableControl2.setTableType = usercontrols_MyTable.TABLETYPE.BookMark
                    MainTableControl2.FilterByID = 0

                    MainTableControl2.m_objSec = objSec
                    If Not MainTableControl2.m_objSec Is Nothing Then
                        MainTableControl2.REFRESH()
                    End If

                Case "3" 'Set

                    MainTableControl3.m_objSQLConn = m_objPageConn
                    'MainTableControl3.PageNum = 1
                    MainTableControl3.NumPerPage = 25
                    MainTableControl3.DisplayMode = usercontrols_MyTable.DISPMode.ByTitle
                    MainTableControl3.setTableType = usercontrols_MyTable.TABLETYPE.Set
                    MainTableControl3.FilterByID = 0

                    MainTableControl3.m_objSec = objSec
                    If Not MainTableControl3.m_objSec Is Nothing Then
                        MainTableControl3.REFRESH()
                    End If

            End Select

            'm_objPageConn.Close()
            'm_objPageConn.Dispose()
            'm_objPageConn = Nothing

        Catch ex As Exception

        End Try

    End Sub
End Class
