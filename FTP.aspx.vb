Imports System.Data
Imports System.Data.OleDb

Partial Class FTP
    Inherits System.Web.UI.Page

    Private m_objSec As clsSecurity

    Public Sub New()




    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

        m_objSec = HttpContext.Current.Session("Security")


        With FileManager1

            .AllowDelete = m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinDelete)
            .AllowUpload = m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinUpload)
            .AllowOverwrite = m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinFolderRename)
            .DownloadOnDoubleClick = True
            .ReadOnly = Not m_objSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinFileCreate)
            .EnableContextMenu = True
            '.ReadOnly = True

        End With
    End Sub

 
End Class
