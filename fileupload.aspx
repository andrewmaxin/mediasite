<%@ Page Language="VB" AutoEventWireup="false" CodeFile="fileupload.aspx.vb" Inherits="fileupload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="template.css" rel="stylesheet" type="text/css" />
<script>
    window.onunload = refreshParent;
    function refreshParent() {
        window.opener.location.reload();
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper" class="wrapper">
        <div id="headerLogo">
            <img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/CDAC2_header.png" /></div>
        <div id="title"> 
                            <br />
            <asp:Label ID="lblTitle" runat="server" Text="File Upload"></asp:Label> 
        </div>
        <div id="content"><asp:Button
                ID="cmdUnloadButton" runat="server" Text="." visible="false" />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <br />
            <asp:FileUpload ID="FileUpload1" runat="server" Width="397px" />
            <asp:Button ID="cmdUpload" runat="server" Text="Upload" Width="67px" />
        </div>
    </form>
</body>
</html>
