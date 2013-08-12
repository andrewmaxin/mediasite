<%@ Page Language="VB" AutoEventWireup="false" CodeFile="filedownload.aspx.vb" Inherits="filedownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="template.css" rel="stylesheet" type="text/css" />
     </head>
<body>
    <form id="form1" runat="server">
     <div id="wrapper" class="wrapper">
<div id="headerLogo"><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/CDAC2_header.png" /></div>
    <div id="title">
    <br />
        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        <%
        If blnStreaming = False Then
                lblTitle.Text = "File Download"
        Else
                lblTitle.Text = "Stream File"
        End if
        %>
    </div> 
    <div id="content">
    <% If blnStreaming = False Then%>
        <asp:Label ID="lblMessage" runat="server" Text="" ></asp:Label>
   <%Else%>
   <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="400" height="15" id="xspf_player" align="middle">
<param name="allowScriptAccess" value="sameDomain" />
<param name="movie" value="xspf_player_slim.swf?song_url=<%=ConfigurationManager.AppSettings("gsSiteFilesWebRoot_temp")%><%=strTempFileName %>" />
<param name="quality" value="high" />
<param name="bgcolor" value="#e6e6e6" />
<embed src="xspf_player_slim.swf?song_url=<%=ConfigurationManager.AppSettings("gsSiteFilesWebRoot_temp")%><%=strTempFileName%>&song_title=<%=strSongTitle%>&autoplay=true" quality="high" bgcolor="#e6e6e6" width="400" height="15" name="xspf_player" align="middle" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
</object>
<%End If%>
        </div>
    </form>
</body>
</html>
