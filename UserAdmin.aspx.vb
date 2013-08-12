Imports System.Data


Partial Class UserAdmin
    Inherits System.Web.UI.Page
    Public SecInst As clsSecurity
    Private m_objPageConn As OleDb.OleDbConnection

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            Try


                m_objPageConn = New OleDb.OleDbConnection(ConfigurationManager.AppSettings("SQLConnectionString"))
                m_objPageConn.Open()

                'find out who this person is
                If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

                SecInst = HttpContext.Current.Session("Security")
                MainTableControl1.m_objSec = SecInst
                MainTableControl1.m_objSQLConn = m_objPageConn
                MainTableControl1.REFRESH()

            Catch ex As Exception

            End Try

        End If
    End Sub
End Class
