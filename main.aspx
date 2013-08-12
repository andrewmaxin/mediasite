<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="main.aspx.vb" Inherits="main" title="Untitled Page" %>

<%@ Register Src="usercontrols/MyTable.ascx" TagName="tablecontrol" TagPrefix="uc2" %>
<%@ Register Src="usercontrols/TabControl.ascx" TagName="TabControl" TagPrefix="uc1" %>
<%@ Register Src="usercontrols/MiniSongListContol.ascx" TagName="MiniSLControl" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" Runat="Server">
    <uc1:TabControl ID="TabControl1" runat="server" />
    <%  Select Case CInt(HttpContext.Current.Session.Item("Tab"))
            
            Case 0
                %>
                <div><h3>Welcome, <%=StrName%></h3></div>
                <div class="content">
                <div class="moduletable">
                    <uc3:MiniSLControl ID="MiniSLControl1" runat="server"   DisplayMode="RecentlyEdited"  />
                </div>
                <br /><br />
                <div class="mostpopular">
                    <uc3:MiniSLControl ID="MiniSLControl2" runat="server"   DisplayMode="MostPopular"  />
                </div>
                <br /><br />
                <div class="rolodex">
                    <uc3:MiniSLControl ID="MiniSLControl4" runat="server"   DisplayMode="Rolodex"  />
                </div>
                <br /><br />
                <div class="thisweeksset">
                     <uc3:MiniSLControl ID="MiniSLControl5" runat="server"   DisplayMode="ThisWeek"  />
                </div>
                <br /><br />
                <div class="songhistory">
                    <uc3:MiniSLControl ID="MiniSLControl3" runat="server"   DisplayMode="PastHistory"  />
                </div>

                </div>
                <%
            Case 1
               %>
               <uc2:tablecontrol ID="MainTableControl1" runat="server" 
        setTableType="Lookup" /> <%
            Case 2
                 %>
               <uc2:tablecontrol ID="MainTableControl2" runat="server" 
         setTableType="BookMark" /> <%
            Case 3
                 %>
               <uc2:tablecontrol ID="MainTableControl3" runat="server" 
         setTableType="Set" /> <%
            
                                   Response.Write(HttpContext.Current.Session("Security").LastError)
            
        End Select%>
    
    


</asp:Content>

