<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CreateSet.ascx.vb" Inherits="usercontrols_CreateSet" %>
<asp:Panel ID="Panel1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Date of Performance:"></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" />
    <asp:Calendar ID="Calendar1" runat="server" Visible="False"></asp:Calendar>
    <br />
    Description:
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
</asp:Panel>
