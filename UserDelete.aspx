<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UserDelete.aspx.vb" Inherits="UserDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" Runat="Server">

<h3>Delete User Account</h3>

<p>Are you <b>SURE</b> you want to delete this user account: </p>
   
<asp:Label ID="lblUserName" Text="{User}"></asp:Label>

<p>Note: This will delete all activity history as well.</p>

<p>THIS CANNOT BE UNDONE!</p>

<asp:Button ID="cmdConfirm" runat="server" Text="Confirm" UseSubmitBehavior="true"/>
<asp:Button ID="cmdCancel" runat="server" Text="Cancel" />

</asp:Content>

