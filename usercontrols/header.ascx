<%@ Control Language="VB" AutoEventWireup="true" CodeFile="header.ascx.vb" Inherits="usercontrols_WebUserControl" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" >
    <tr>
        <td width="100%">
            <img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/CDAC2_header.png"
                align="left" alt="" />
        </td>
        <td>
            <table border="0"  style="height=40px" cellspacing="0" cellpadding="5">
                <tr   align="center">
                    <% If objHdrSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_SongAdd) = True Then%>
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/EditSong.aspx">
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/document_new.png"
                                border="0" alt="" /><br/>
                            Add&nbsp;Song</h5></a>
                    </td>
                    <%End If%>
                    <!---
                    <% If objHdrSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_CreateCCLIReport) = True Then%>
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/CCLIRpt.aspx"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/Lreports.gif"
                                border="0" alt=""/><br/>
                            Reports</h5></a>
                    </td>
                    <%End If%>
                    --->
                    
                    <% If objHdrSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = True Then%>
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/UserAdmin.aspx"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/users2.png"
                                border="0" alt=""/><br/>
                            Users</h5> </a>
                    </td>
                    <%Else %>
                    <!---
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/UserEdit.aspx?ID=<%=objHdrSec.UserID %>"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/users2.png"
                                border="0" alt=""/><br/>
                            Users</h5> </a>
                    </td>
                    --->
                    <%End If%>
                    
                    <% If objHdrSec.CheckPermission(clsSecurity.Permission_ActionCodes.act_DocBinLogin) = True Then%>
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/FTP.aspx"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/cabinet.png" width="24"
                                height="24" border="0" alt=""><br/>
                            Files</h5></a>
                    </td>
                    <%End If%>
                    <!---
                    <td>
                        <a href="https://support.cdac.sk.ca" target="_blank"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/about.png"
                                border="0" alt=""><br/>
                            Help</h5></a>
                    </td>
                    --->
                    <td>
                        <a href="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/logout.aspx"> 
                            <h5><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/exit.png"
                                border="0" id="IMG1" language="javascript" onclick="return IMG1_onclick()" alt=""><br/>
                            Logout</h5></a>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
