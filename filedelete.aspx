<%@ Page Language="VB" AutoEventWireup="false" CodeFile="filedelete.aspx.vb" Inherits="filedelete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ConfigurationManager.AppSettings("gsSiteName").ToString%></title>
    <meta name="description" content="<%=ConfigurationManager.AppSettings("gsMetaDescription").ToString%>" />
    <meta name="keywords" content="<%=ConfigurationManager.AppSettings("gsMetaKeywords").ToString%>" />
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
        <br/>
            <asp:Label ID="lblTitle" runat="server" Text="Confirm Delete"></asp:Label>
        </div>
        <div id="content">
            <div id="editor">
                <asp:Label ID="Label1" runat="server" Text="Are you sure you want to delete the file:"></asp:Label>
                <br />
                <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                <asp:HiddenField ID="lngID" runat="server" />
                <asp:HiddenField ID="intFileType" runat="server" />
                <br />
                <asp:Button ID="cmdConfirm" runat="server" Text="Confirm" 
                    EnableTheming="True" /><asp:Button ID="cmdCancel"
                    runat="server" Text="Cancel" />
            </div>
        </div>
    </form>
</body>
</html>
