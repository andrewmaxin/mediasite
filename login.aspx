<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
	<title><%=ConfigurationManager.AppSettings("gsSiteName")%></title>
    <link href="template.css" rel="stylesheet" type="text/css" />
    <style>

    body { font-size:90%;  background:#352E28 url(../images/bg.gif) top repeat-x; color:#f9f0da; height:100%; margin:0 }

    </style>
</head>
<body onload="document.form1.userid.focus();" ><form id="Form1"  runat="server">

<div id="wrapper">
<div style="text-align:center"><img src="images/CDAC2.png" /></div>
 <div id="content">
			<table border="0" cellpadding="2" cellspacing="2" align="center" width="300">
        <tr> 
          <td align="center" class="homeheader" colspan="2"  ></td>
				</tr>
				<tr><td colspan="2" align="center">&nbsp;<asp:Label id="lblInvalid" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Larger" /></td></tr>
				
				<input type="hidden" name="hname" value="hidden" />
				<input type="hidden" name="ccokie" value="false" />

				<tr>
					<td width="116" align="right" valign="middle" class="bolddark"><%=ConfigurationManager.AppSettings("lblUser_Name")%>: </td>
					<td width="170"><asp:TextBox id="txtUsername" runat="server" /></td>
				</tr>
				<tr>
					<td width="116" align="right" valign="middle" class="bolddark"><%=ConfigurationManager.AppSettings("lblPassword")%>: </td>
					<td width="170"><asp:TextBox id="txtPassword" TextMode="password" runat="server" /></td>
				</tr>
				<tr>
					<td width="116" align="right" valign="middle" class="bolddark"><%=ConfigurationManager.AppSettings("lblChurch")%>: </td>
					<td width="170"><asp:dropdownlist runat="server" id="cboChurch" >
                        <asp:ListItem Value="1">CDAC</asp:ListItem>
									</asp:dropdownlist>
					</td>
				</tr>
				<tr><td align="center" colspan="2"><asp:Button id="btnLogin" runat="server" text="Login" OnClick="btnLogin_OnClick" /></td></tr>
				<tr><td colspan="2">&nbsp;</td></tr>
				<!---<tr><td align="center" colspan="2"><a href="forgotpassword.aspx"><%=ConfigurationManager.AppSettings("lblForgetPassword")%></a></td></tr>--->
				
				<tr>
					<td align="center" colspan="2" class="alert">
						<p>
                            &nbsp;</p>
	
					</td>
				</tr>
			</table>
 </div>
</div>

</form>
	<center><p><a href="https://support.cdac.sk.ca">Help Desk</a></p>
<p><!-- Powered by: Crafty Syntax Live Help        http://www.craftysyntax.com/ -->
<script type="text/javascript" src="http://support.cdac.sk.ca/livehelp/livehelp_js.php?department=2&amp;pingtimes=15"></script>
<!-- copyright 2003 - 2006 by Eric Gerdes -->
						</p>

<%  If CBool(ConfigurationManager.AppSettings("gsShowAcceptPolicy")) = True Then%>
<BR>
	<a href="Media Resources Web Site Acceptable Use Policy.pdf">Acceptable Usage Policy.</a> <p>ver 3.0</p></center>
<% End IF %>


</body>
</html>
