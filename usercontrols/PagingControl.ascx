<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PagingControl.ascx.vb" Inherits="usercontrols_WebUserControl" %>
<asp:Table ID="Table1" runat="server" Width="100%" >
    <asp:TableRow runat="server" VerticalAlign="Top">
    <asp:TableCell Width="16"><asp:ImageButton ID="cmdFirst" runat="server" ImageUrl="/images/First.png" ImageAlign="AbsMiddle" Width="24" ToolTip="Go to First Page" /></asp:TableCell>
    <asp:TableCell Width="16"><asp:ImageButton ID="cmdPrev" runat="server" ImageUrl="/images/Prev.png" ImageAlign="AbsMiddle"  Width="24" ToolTip="Go to Previous Page" /></asp:TableCell>
    <asp:TableCell Width="25"><asp:TextBox Width="25" ID="txtPage" runat="server" Text="1"></asp:TextBox></asp:TableCell>
    <asp:TableCell Width="25"><asp:Label ID="Label1" runat="server" width="25" Text="" Visible="false"></asp:Label></asp:TableCell>
    <asp:TableCell Width="16"><asp:ImageButton ID="cmdNext" runat="server" ImageUrl="/images/Next.png" ImageAlign="AbsMiddle" Width="24"  ToolTip="Go to Next Page" /></asp:TableCell>
    <asp:TableCell Width="16"><asp:ImageButton ID="cmdLast" runat="server" ImageUrl="/images/Last.png" ImageAlign="AbsMiddle" Width="24" ToolTip="Go to Last Page"  /></asp:TableCell>
    <asp:TableCell Width="100%">&nbsp;</asp:TableCell>
    </asp:TableRow>
</asp:Table>




