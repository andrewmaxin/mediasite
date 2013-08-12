<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UserEdit.aspx.vb" Inherits="UserEdit" %>
<%@ Register Src="usercontrols/MyTable.ascx" TagName="tablecontrol" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" Runat="Server">

<h3>User Details:</h3>


<asp:HiddenField ID="UserID" runat="server" Value="<%=intID%>"/>
<div align="center">
   

   
<asp:Table ID="Table1" runat="server" Width="70%" >

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>First Name: </asp:TableCell><asp:TableCell><asp:TextBox  Width="200px" ID="txtFirstName" runat="server"></asp:TextBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Last Name: </asp:TableCell><asp:TableCell><asp:TextBox  Width="200px" ID="txtLastName" runat="server"></asp:TextBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>User Name: </asp:TableCell><asp:TableCell><asp:TextBox  Width="200px" ID="txtUserName" runat="server"></asp:TextBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Password: </asp:TableCell><asp:TableCell><asp:TextBox  Width="200px" ID="txtPassword" runat="server"></asp:TextBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Email: </asp:TableCell><asp:TableCell><asp:TextBox  Width="200px" ID="txtEmail" runat="server"></asp:TextBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>&nbsp; </asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell><strong>Instruments:</strong></asp:TableCell><asp:TableCell>    <asp:ListBox ID="lstInstruments" runat="server" DataSourceID="TeamPositions" 
        DataTextField="Name" DataValueField="ID" SelectionMode="Multiple" Width="200px" Rows="6" >
    </asp:ListBox></asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell> <asp:CheckBoxList ID="CheckBoxList1" runat="server">
    </asp:CheckBoxList></asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>&nbsp; </asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell>
</asp:TableRow>
</asp:Table>

<asp:Panel ID="permPanel" runat="server" Visible="false">
<asp:Table runat="server">

<asp:TableRow HorizontalAlign="left">
<asp:TableCell ><strong>Permissions:</strong></asp:TableCell><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>User Enabled: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkUserEnabled" runat="server" /></asp:TableCell>
</asp:TableRow>


<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Site Administrator: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkSiteAdmin" runat="server" /></asp:TableCell>
<asp:TableCell>Set Administrator: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkSetAdmin" runat="server" /></asp:TableCell>
</asp:TableRow>
 
<asp:TableRow BackColor="#cc9900" Height="2px">
<asp:TableCell ColumnSpan="4">&nbsp; </asp:TableCell> 
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Song View: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkSongView" runat="server" /></asp:TableCell>
<asp:TableCell>Song Edit: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkSongEdit" runat="server" /></asp:TableCell>
</asp:TableRow>

<asp:TableRow BackColor="#cc9900" Height="2px">
<asp:TableCell ColumnSpan="4">&nbsp; </asp:TableCell> 
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>mp3 Upload: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkMp3Upload" runat="server" /></asp:TableCell>
<asp:TableCell>mp3 Download: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkmp3Download" runat="server" /></asp:TableCell>
</asp:TableRow>

<asp:TableRow BackColor="#cc9900" Height="2px">
<asp:TableCell ColumnSpan="4">&nbsp; </asp:TableCell> 
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>PPT Download: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkpptDownload" runat="server" /></asp:TableCell>
<asp:TableCell>PPT Upload: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkpptUpload" runat="server" /></asp:TableCell>
</asp:TableRow>

<asp:TableRow BackColor="#cc9900" Height="2px">
<asp:TableCell ColumnSpan="4">&nbsp; </asp:TableCell> 
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>DocBin Upload: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkDocBinUpload" runat="server" /></asp:TableCell>
<asp:TableCell>DocBin Download: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkDocBinDownload" runat="server" /></asp:TableCell>
</asp:TableRow>

<asp:TableRow BackColor="#cc9900" Height="2px">
<asp:TableCell ColumnSpan="4">&nbsp; </asp:TableCell> 
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Song Delete: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkSongDel" runat="server" /></asp:TableCell>
<asp:TableCell>Allow Reporting Function: </asp:TableCell><asp:TableCell><asp:CheckBox ID="chkAllowReporting" runat="server" /></asp:TableCell>
</asp:TableRow>

<asp:TableRow>
<asp:TableCell>&nbsp; </asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell>
</asp:TableRow>

<asp:TableRow HorizontalAlign="left">
<asp:TableCell>Church Affiliation: </asp:TableCell><asp:TableCell>
    <asp:DropDownList ID="DropDownList1" runat="server">
    <asp:ListItem Selected="True" Value="1" Text="CDAC"></asp:ListItem>
    </asp:DropDownList>
</asp:TableCell>
</asp:TableRow>
</asp:Table>
</asp:Panel>
    <asp:SqlDataSource ID="Church" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
        SelectCommand="SELECT [ID], [church_code], [church_name], [icon_file] FROM [church] WHERE ([active] = @active)">
        <SelectParameters>
            <asp:Parameter DefaultValue="-1" Name="active" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="TeamPositions" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
        SelectCommand="SELECT * FROM [TeamPositions]"></asp:SqlDataSource>
</div>

<asp:Button ID="cmdSave" runat="server" Text="Save" />
<asp:Button ID="cmdCancel" runat="server" Text="Cancel" />

 
</asp:Content>

