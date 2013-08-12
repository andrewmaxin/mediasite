<%@ Control Language="VB" AutoEventWireup="false" CodeFile="navicontrol.ascx.vb" Inherits="usercontrols_navicontrol" %>

<div class="navBar">
    <table border="0" style="width: 100%">
        <tr>
            <td nowrap>
                Search By:
                <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem Text="Title" Value="0" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Artist" Value="1" Selected="false"></asp:ListItem>
                    <asp:ListItem Text="Style" Value="2" Selected="false"></asp:ListItem>
                    <asp:ListItem Text="Use" Value="3" Selected="false"></asp:ListItem>
                    <asp:ListItem Text="Favourites" Value="4" Selected="false"></asp:ListItem>
                    <asp:ListItem Text="New Songs" Value="5" Selected="false"></asp:ListItem>
                    <asp:ListItem Text="KeyWords" Value="6" Selected="false"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Search Text:"></asp:Label>&nbsp;&nbsp;<asp:TextBox
                    ID="txtSearchText" runat="server"></asp:TextBox>
            </td>
            <td>
                Filter:
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataSourceID="SqlDataSource1" DataTextField="GroupName" DataValueField="ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    SelectCommand="SELECT [GroupName], [ID] FROM [Groups] ORDER BY [GroupName]">
                </asp:SqlDataSource>
            </td>
            <td>
                <asp:Button ID="cmdGo" runat="server" Text="Go" UseSubmitBehavior="true"  />
                <asp:Button ID="cmdReset" runat="server" Text="Reset" />
            </td>
        </tr>
    </table>
</div>
