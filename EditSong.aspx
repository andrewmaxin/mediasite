<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditSong.aspx.vb" Inherits="EditSong" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Add/Edit Song:</title>
    <link href="template.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
<!--

function TABLE1_onclick() {

}

// -->
</script>
    <style type="text/css">
        .style1
        {
            width: 42px;
        }
        .style2
        {
            height: 156px;
            width: 42px;
        }
        .style4
        {
            width: 750px;
            height: 202px;
        }
        .style5
        {
            height: 202px;
            width: 42px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div id="wrapper" class="wrapper">
<div id="headerLogo"><img src="<%= ConfigurationManager.AppSettings("gsSiteURL") %>/images/CDAC2_header.png" /></div>
    <div id="title"><%
                        If id.Value = "" Then
                            Response.Write("Enter Song")
                        Else
                            Response.Write("Edit Song: <i>" & song_Name.Text & "</i>")
                        End If
                                %> <div id="closebutton">
                    <a href="javascript: window.close();">
                        <img src="<%= ConfigurationManager.AppSettings("gsSiteRoot").ToString %>images/close.gif"
                            alt="close" border="0" /></a></div> </div>
    <div id="content">
   
    
    <table width="100%" border="0" cellspacing="5" cellpadding="5">
        <tr> 
          <td colspan="4" class="normaltitle">General Information:</td>
        </tr>
        <tr> 
          <td width="27%" class="normal">Song Name:</td>
          <td colspan="3"><asp:TextBox runat="server" ID="song_Name" maxlength="50" Columns="50" /> 
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="song_Name"
                  CssClass="text-decoration:blink" Display="Dynamic" ErrorMessage="Song Title is a Required Field"
                  Font-Bold="True"></asp:RequiredFieldValidator></td>
        </tr>
        <tr> 
          <td class="normal">Author #1:</td>
          <td colspan="3"> <asp:TextBox runat="server"  id="Author1" maxlength="50" Columns="50" /> 
          </td>
        </tr>
        <tr> 
          <td class="normal">Author #2:</td>
          <td colspan="3"> <asp:TextBox runat="server" id="Author2" maxlength="50" Columns="50" /> 
          </td>
        </tr>
        <tr> 
          <td class="normal">CCLI #:</td>
          <td colspan="3"><asp:TextBox runat="server" id="CCLINum" maxlength="50" Columns="50" /></td>
        </tr>
        <tr> 
          <td class="normal">Style:</td>
          <td width="10%"><asp:DropDownList runat="server" ID="cboStyle"  
                  DataSourceID="SqlDataSource1" DataTextField="Style" DataValueField="Style" 
                  AppendDataBoundItems="True" AutoPostBack="True" >
              <asp:ListItem> </asp:ListItem>
          
          </asp:DropDownList>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>"
              SelectCommand="SELECT * FROM [Styles] ORDER BY [Style]"></asp:SqlDataSource>
          </td>
        <td width="15%" align="right" class="normal">Song Key:</td>
          <td width="48%"> 
          <asp:DropDownList runat="server" ID="key">
          <asp:ListItem Text="C" Value="C" />
          <asp:ListItem Text="Db" Value="Db" />
          <asp:ListItem Text="D" Value="D" />
          <asp:ListItem Text="Eb" Value="Eb" />
          <asp:ListItem Text="E" Value="E" />
          <asp:ListItem Text="F" Value="F" />
          <asp:ListItem Text="Gb" Value="Gb" />
          <asp:ListItem Text="G" Value="G" />
          <asp:ListItem Text="Ab" Value="Ab" />
          <asp:ListItem Text="A" Value="A" />
          <asp:ListItem Text="Bb" Value="Bb" />
          <asp:ListItem Text="B" Value="B" />
          </asp:DropDownList>
           </td>
        </tr>
                <tr> 
          <td class="normal">Tempo:</td>
          <td colspan="3"><asp:TextBox runat="server" id="Tempo" maxlength="50" Columns="50" /></td>
        </tr>
        <tr> 
          <td class="normal">Use #1:</td>
          <td colspan="3"><asp:DropDownList runat="server" ID="use1" 
                  DataSourceID="SqlDataSource2" DataTextField="Theme" DataValueField="Theme" 
                  AppendDataBoundItems="True" AutoPostBack="True">
              <asp:ListItem></asp:ListItem>
          </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>"
              SelectCommand="SELECT [Theme] FROM [Themes] ORDER BY [Theme]"></asp:SqlDataSource>
          </td>
        </tr>
        <tr> 
          <td class="normal">Use #2:</td>
          <td colspan="3"><asp:DropDownList runat="server" ID="use2" 
                  DataSourceID="SqlDataSource2" DataTextField="Theme" DataValueField="Theme" 
                  AppendDataBoundItems="True" AutoPostBack="True">
              <asp:ListItem></asp:ListItem>
          </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>"
              SelectCommand="SELECT [Theme] FROM [Themes] ORDER BY [Theme]"></asp:SqlDataSource>
          </td>
        </tr>
        <tr> 
          <td class="normal">Copy. Date:</td>
          <td colspan="3"><asp:TextBox runat="server" id="copydate" maxlength="12" Columns="8" /> 
          </td>
        </tr>
        <tr> 
          <td class="normal" style="height: 28px">Bible Reference:</td>
          <td colspan="3" style="height: 28px"><asp:TextBox runat="server" id="BRef" maxlength="255" Columns="25" /></td>
        </tr>
        <tr> 
          <td class="normal">YouTube Link:</td>
          <td colspan="3"> 
              <asp:TextBox ID="Youtubelink" runat="server" Width="322px"></asp:TextBox>
&nbsp;* note: Only code is required.</td>
        </tr>
        <tr> 
          <td class="normal">Publisher: </td>
          <td colspan="3"> <asp:TextBox runat="server" id="publisher" maxlength="255" Columns="50" /> 
          </td>
        </tr>
        <tr> 
          <td class="normal">Notes:</td>
          <td colspan="3"><asp:TextBox runat="server" Columns="75" Rows="5" Wrap="true" id="Notes" TextMode="MultiLine" /> 
          </td>
        </tr>
        <tr> 
          <td class="normal">Arrangement:</td>
          <td colspan="3"><asp:TextBox runat="server" Columns="75" Rows="5" Wrap="true" id="arrangement" TextMode="MultiLine" /></td>
        </tr>
        <tr> 
          <td class="normal">External URL:</td>
          <td colspan="3"><asp:TextBox runat="server" id="ExtURL" Columns="75" MaxLength="255" /></td>
        </tr>
        <tr> 
          <td class="normal" style="height: 26px">Font Size:</td>
          <td colspan="3" style="height: 26px">
		  	<asp:DropDownList runat="server" id="cboFS">
             	 <asp:ListItem Value="8" Text="8pt" />
				 <asp:ListItem Value="9" Text="9pt" />
				 <asp:ListItem Value="10" Text="10pt" />
				 <asp:ListItem Value="11" Text="11pt" />
				 <asp:ListItem Value="12" Text="12pt" />
				 <asp:ListItem Value="13" Text="13pt" />
				 <asp:ListItem Value="14" Text="14pt" />
				 <asp:ListItem Value="15" Text="15pt" />
				 <asp:ListItem Value="16" Text="16pt" />
				 <asp:ListItem Value="17" Text="17pt" />
				 <asp:ListItem Value="18" Text="18pt" />
				 <asp:ListItem Value="19" Text="19pt" />
				 <asp:ListItem Value="20" Text="20pt" />
				 <asp:ListItem Value="21" Text="21pt" />
				 <asp:ListItem Value="22" Text="22pt" />
				 <asp:ListItem Value="23" Text="23pt" />
				 <asp:ListItem Value="24" Text="24pt" />
            </asp:DropDownList></td>
        </tr>  
        <tr> 
          <td class="normal">Song Grouping:</td>
          <td colspan="3"><asp:DropDownList runat="server" ID="cboGroup" 
                  DataSourceID="SqlDataSource4" DataTextField="GroupName" DataValueField="ID" 
                  AppendDataBoundItems="True" AutoPostBack="True" >
              <asp:ListItem Value="0">-</asp:ListItem>
          </asp:DropDownList><asp:SqlDataSource
                  ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>"
                  SelectCommand="SELECT [ID], [GroupName] FROM [Groups] ORDER BY [ID]"></asp:SqlDataSource>
        </td>
        </tr>
         <tr> 
          <td class="normal"><asp:HiddenField runat="server" id="id"  /> 
            <asp:HiddenField runat="server" id="action" value="update" /> </td>
          <td colspan="3"><asp:Button runat="server" id="Save" Text="Save Song" />
              <asp:Button ID="cmdCancel" runat="server" Text="Cancel" />
             </td>
        </tr>
                  
        </table>
        <table width="100%" border="1" cellspacing="2" cellpadding="2">
    <tr>
      <td colspan="2" valign="top" class="normaltitle">Song
        Information: <br>
        <span class="normal"><font color="#FF0000">Note: when entering note lines,
        Use Capital Letters to denote notes, lowercase <strong>b</strong> to indicate
        flats, and <strong>#</strong> to
        indicate sharps. If any incorrect characters are used,
        notes will <strong>not</strong> transpose.</font></span></td>
    </tr>
    <tr>
      <td valign="top" class="style5"><span class="normal">Part&nbsp;#1&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part1name" Columns="8" />
      </td>
      <td valign="top" class="style4"><span class="normal">Part&nbsp;#1:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part1" TextMode="MultiLine" Height="176px" />
      </td>
    </tr>
    <tr>
      <td valign="top" class="style2"><span class="normal">Part&nbsp;#2&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part2name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px; height: 156px"><span class="normal">Part&nbsp;#2:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part2" TextMode="MultiLine" Height="176px" />
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#3&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part3name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#3:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part3" TextMode="MultiLine"  Height="176px" />
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#4&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part4name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#4:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part4" TextMode="MultiLine" Height="176px"/>
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#5&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part5name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#5:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part5" TextMode="MultiLine" Height="176px"/>
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#6&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part6name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#6:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part6" TextMode="MultiLine" Height="176px"/>
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#7&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part7name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#7:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part7" TextMode="MultiLine" Height="176px"/>
      </td>
    </tr>
    <tr>
      <td valign="top" class="style1"><span class="normal">Part&nbsp;#8&nbsp;Name:</span><br>
        <asp:TextBox runat="server" maxlength="50" id="part8name" Columns="8" />
      </td>
      <td valign="top" style="width: 750px"><span class="normal">Part&nbsp;#8:</span><br>
        <asp:TextBox runat="server" Columns="80" Rows="8" Wrap="false" id="part8" TextMode="MultiLine" Height="176px"/>
      </td>
    </tr>
  </table>
        <table  width="100%" border="0" cellspacing="5" cellpadding="5">
  <tr>
    <td><font size="3" face="Verdana, Arial, Helvetica, sans-serif" >&nbsp;</font></td>
  </tr>
</table>
    </div>

    </div>
    </div>

    </form>
    </body>
</html>
