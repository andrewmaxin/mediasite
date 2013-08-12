<%@ Page Language="VB" AutoEventWireup="false" CodeFile="songsheet.aspx.vb" Inherits="songsheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="SHORTCUT ICON" href="<%=ConfigurationManager.AppSettings("gsSiteURL").ToString%>/favicon.ico" />
    <title>
        <%=ConfigurationManager.AppSettings("gsSiteName").ToString%></title>
    <meta name="description" content="<%=ConfigurationManager.AppSettings("gsMetaDescription").ToString%>" />
    <meta name="keywords" content="<%=ConfigurationManager.AppSettings("gsMetaKeywords").ToString%>" />
    <link href="template.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div>
        <form id="form1" runat="server">
        <div id="wrapper" class="wrapper">
            <div id="headerLogo">
                <img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/CDAC2_header.png" />
            </div>
            <div id="content">
                <div id="editor">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="contentpaneopen">
                        <tr>
                            <td width="95%" colspan="2" class="contentheading">
                                <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
                            </td>
                            <td width="5%" class="buttonheading">
                                <a href="javascript: window.close();">
                                    <img src="<%= ConfigurationManager.AppSettings("gsSiteRoot").ToString %>images/close.gif" alt="close" border="0" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAuthor1" runat="server" Text="Author: "></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblAuthor2" runat="server" Text="Author: "></asp:Label>&nbsp;
                            </td>
                            <td >
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblStyle" runat="server" Text="Style: "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCopyright" runat="server" Text="Copyright: "></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSongKey" runat="server" Text="Key: "></asp:Label>
                            </td>
                            <td>
                                CCLI Number:
                                <asp:HyperLink runat="server" ID="lnkCCLI"></asp:HyperLink>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUse1" runat="server" Text="Use #1: "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblUse2" runat="server" Text="Use #2: "></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Biblical Reference:<asp:HyperLink ID="lnkReference" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblTempo" runat="server" Text="Tempo: "></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                External URL:
                                <asp:HyperLink ID="lnkExternal" runat="server"></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSongNotes" runat="server" Text="Song Notes: "></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table width="100%">
                        <tr>
                            <td class="contentheading">
                                Print Song Sheet:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    <span class="Copyright">Song Key:</span>
                                    <asp:DropDownList ID="cboSongKey" runat="server" AutoPostBack="True">
                                        <asp:ListItem>C</asp:ListItem>
                                        <asp:ListItem>C#/Db</asp:ListItem>
                                        <asp:ListItem>D</asp:ListItem>
                                        <asp:ListItem>D#/Eb</asp:ListItem>
                                        <asp:ListItem>E</asp:ListItem>
                                        <asp:ListItem>F</asp:ListItem>
                                        <asp:ListItem>F#/Gb</asp:ListItem>
                                        <asp:ListItem>G</asp:ListItem>
                                        <asp:ListItem>G#/Ab</asp:ListItem>
                                        <asp:ListItem>A</asp:ListItem>
                                        <asp:ListItem>A#/Bb</asp:ListItem>
                                        <asp:ListItem>B</asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <span class="Copyright">
                                        <asp:CheckBox ID="chkPrintOrdering" runat="server" Checked Text="Print Song Arrangement"
                                            AutoPostBack="True" /></span>
                                    <br />
                                    <span class="Copyright">
                                        <asp:CheckBox ID="chkPrintChords" runat="server" Checked Text="Print Chords" AutoPostBack="True" /></span>
                                    <br />
                                    <span class="Copyright">
                                        <asp:CheckBox ID="chkPrintPartNames" runat="server" Checked Text="Print Part Names"
                                            AutoPostBack="True" /></span>
                                    <br />
                                    <br />
                                    <span class="Copyright">Text Size:
                                        <asp:DropDownList ID="cboTextSize" runat="server" AutoPostBack="True">
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                            <asp:ListItem>24</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </span>
                                </p>
                                <p>
                                    <asp:Button ID="cmdGenerateSheet" runat="server" Text="Generate Sheet" alt="Press Here to Generate the Sheet"
                                        Width="117px" />
                                    <asp:HiddenField ID="songID" runat="server" Value="<%=lngID%>" />
                                    <asp:Button ID="cmdGeneratePreview" runat="server" Text="Generate Preview" alt="Use this to Check Print for proper size & look" />
                                </p>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="contentheading">
                                Music File Functions:<asp:HiddenField ID="lblmp3Lnk" runat="server" />
                            </td>
                        </tr>
                        <%
                            If SecInst.Permission(clsSecurity.Permissions.mp3dwload) = True Then
                                If lblmp3Lnk.Value <> "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" border="0" cellpadding="0">
                                    <tr>
                                        <td class="SongTitle">
                                            <p>
                                                Download Mp3 File: <b>
                                                    <%= lblmp3Lnk.Value & ""%></b></p>
                                            <asp:Button ID="cmdDownloadMp3" runat="server" Text="Click Here" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%If SecInst.Permission(clsSecurity.Permissions.mp3dwload) = True And UCase(Right(lblmp3Lnk.Value, 3)) = "MP3" Then%>
                        <tr>
                            <td>
                                Play Song:&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <%
                        End If
                    End If
                End If
                        %>
                        <%
                            If SecInst.Permission(clsSecurity.Permissions.mp3upload) = True Then
                                If lblmp3Lnk.Value = "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" border="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="SongTitle">Upload a music file: (<%= ConfigurationManager.AppSettings("gsMusicExt").ToString%>)</span><br>
                                            <br />
                                            <asp:Button ID="cmdFileUpload" runat="server" Text="Upload" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If
                    End If

                    If SecInst.Permission(clsSecurity.Permissions.siteadmin) = True Then
                        If lblmp3Lnk.Value <> "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" border="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="SongTitle">Delete Mp3 File: <b>
                                                <%= lblmp3Lnk.Value & ""%></b></span><br />
                                            <br />
                                            <asp:Button ID="cmdDeleteMp3" runat="server" Text="Delete" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If
                    End If
                        %>
                    </table>
                    <br />
                    <br />
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="contentheading">
                                Attachment File Functions:
                                <asp:HiddenField ID="lblPPTLink" runat="server" />
                            </td>
                        </tr>
                        <%
                            If SecInst.Permission(clsSecurity.Permissions.mp3dwload) = True Then
                                If lblPPTLink.Value <> "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" border="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <p>
                                                Download File: <b>
                                                    <%= lblPPTLink.value & ""%></b></p>
                                            <asp:Button ID="cmdAttachmentDownload" runat="server" Text="Click Here" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If
                    End If
                        %>
                        <%
                            If SecInst.Permission(clsSecurity.Permissions.pptupl) = True Then
                                If lblPPTLink.Value = "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="SongTitle">Upload an attachment File: (<%= ConfigurationManager.AppSettings("gsPPTExt").ToString%>)</span><br>
                                            <br />
                                            <asp:Button ID="cmdAttachUpload" runat="server" Text="Upload" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If
                    End If

                    If SecInst.Permission(clsSecurity.Permissions.siteadmin) = True Then
                        If lblPPTLink.Value <> "" Then
                        %>
                        <tr>
                            <td>
                                <table width="90%" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span>Delete PPT File: <b>
                                                <%= lblPPTLink.value & ""%></b></span><br />
                                            <br />
                                            <asp:Button ID="cmdAttachDelete" runat="server" Text="Delete" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If
                    End If
                        %>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <%
                            If lblYouTubeLink.Value <> "" Then
                        %>
                        <tr>
                            <td>
                                <br />
                                <br />
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="contentheading">
                                            YouTube Attachment:
                                            <asp:HiddenField ID="lblYouTubeLink" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <object width="320" height="265">
                                                <param name="movie" value="http://www.youtube.com/v/<%=lblYouTubeLink.value%>&hl=en&fs=1&rel=0&color1=0x006699&color2=0x54abd6">
                                                </param>
                                                <param name="allowFullScreen" value="true"></param>
                                                <param name="allowscriptaccess" value="always"></param>
                                                <embed src="http://www.youtube.com/v/<%=lblYouTubeLink.value%>&hl=en&fs=1&rel=0&color1=0x006699&color2=0x54abd6"
                                                    type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true"
                                                    width="320" height="265"></embed>
                                            </object>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%
                        End If

                        %>
                    </table>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
