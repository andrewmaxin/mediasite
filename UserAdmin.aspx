<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="UserAdmin.aspx.vb" Inherits="UserAdmin" Debug="true" %>

<%@ Register Src="usercontrols/MyTable.ascx" TagName="tablecontrol" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" runat="Server">
    <h3>
        Users:</h3>

        <%If SecInst.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserAdd) Or SecInst.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) Then%>
        
        <a href="UserEdit.aspx?ID=-1">Create User</a><br /><br />
        
        <uc2:tablecontrol ID="MainTableControl1" runat="server" setTableType="UserList" />
        <%End If%>

</asp:Content>
