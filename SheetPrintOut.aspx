<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SheetPrintOut.aspx.vb" Inherits="SheetPrintOut"
    Debug="true" EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%= strTitle%></title>
</head>
<body>
    <style type="text/css">
<!--

.SongNotes {
	font-family: "Courier New", Courier, mono;
	font-style: normal;
	font-size: <%=intTextSize%>px;
	text-transform: none;
	line-height: normal;
	font-weight: normal;
	font-variant: normal;
	color: #000000;
	text-decoration: none;
	padding: 0px;
	margin: 0px 0px 20px;
}

.SongNoteLine {
	font-family: "Courier New", Courier, mono;
	font-style: normal;
	font-size: <%=intTextSize%>px;
	line-height: normal;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #000000;
	text-decoration: none;
	padding: 0px;
	margin: 0px 0px 20px;
}

.SongTitle {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 16px;
	font-style: normal;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #000000;
	text-decoration: none;
	text-align: center;
	margin: 0px;
}

.Author {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 9px;
	font-style: normal;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #000000;
	text-decoration: none;
	text-align: center;
	margin: 0px;
}

.CopyRight {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 9px;
	font-style: normal;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #000000;
	text-decoration: none;
	text-align: center;
	clip:  rect(0px auto auto auto);
	margin: 0px;
}

.SongPartTitle {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 10px;
	font-weight: bold;
	margin: 0px;
	color: #000000;
	text-decoration: none;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
}
    
.ArrangementTitle {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 12px;
	font-weight: bold;
	color: #000000;
	text-decoration: none;
}

.SongNoNotes {
	font-family: Arial, Helvetica, sans-serif;
	font-style: normal;
	font-size: <%=intTextSize%>px;
	line-height: normal;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #000000;
	text-decoration: none;
}
-->
</style>
    <p class="SongTitle">
        <%= strTitle%>
        <% If blnIsPreview = True Then%>
        ----- <font color="#FF0000">Preview Copy</font>
        <% End If%>
    </p>
    <p class="Author">
        <%=Author1 %><br>
        <%=Author2 %></p>
    <p class="CopyRight">
        Copyright:
        <%=CopyDate%>
        -
        <%=Publish%></p>
    <p class="CopyRight">
        Song Key:
        <%=Key %></p>
    <p class="CopyRight">
        Song Notes:
        <%=Notes %></p>
    <p class="CopyRight">
        CCLI Number:
        <%=CCLI %></p>
    <% 
        If blnShowOrder = True Then
    %><p class="ArrangementTitle">
        Arrangement:
        <%=strOrder %></p>
    <%
    End If
    
    Response.Write(strDecodedSong)

    %>
</body>
</html>
