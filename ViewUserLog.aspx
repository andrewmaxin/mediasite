<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ViewUserLog.aspx.vb" Inherits="ViewUserLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" Runat="Server">
<h3>Activity Log for: 
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h3>
    
    <div align="right">
    <asp:Button ID="cmdExport" runat="server" Text="Export" />
    <asp:Button ID="cmdClose" runat="server" Text="Close" />
    </div>

    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
        EnableModelValidation="True" Width="100%"  PageSize="25">
        <Columns>
            <asp:BoundField DataField="DTStamp" HeaderText="DTStamp" 
                SortExpression="DTStamp" />
            <asp:BoundField DataField="Activity" HeaderText="Activity" 
                SortExpression="Activity" />
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="Author1" HeaderText="Author1" 
                SortExpression="Author1" />
            <asp:BoundField DataField="SessionID" HeaderText="SessionID" 
                SortExpression="SessionID" />
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
        
        SelectCommand="SELECT ActivityLog.DTStamp, [Activity Codes].Activity, Songs.Title, Songs.Author1, ActivityLog.SessionID FROM ActivityLog INNER JOIN [Activity Codes] ON ActivityLog.ActionCode = [Activity Codes].ActivityID INNER JOIN Songs ON ActivityLog.SongID = Songs.id WHERE (ActivityLog.UID = @USERID) ORDER BY ActivityLog.DTStamp DESC">
         <SelectParameters>
             <asp:QueryStringParameter DefaultValue="0" Name="UserID" QueryStringField="ID" 
                 Type="Int32" />
        </SelectParameters>
         </asp:SqlDataSource>

</asp:Content>

