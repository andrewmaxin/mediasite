<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TabControl.ascx.vb" Inherits="usercontrols_TabControl" %>

<div class="navBar">
<%
    If HttpContext.Current.Session.Item("tab") Is Nothing Then
        HttpContext.Current.Session.Add("tab", 0)
    End If
    
    %>
<table  width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr  valign="middle"> 
            <td nowrap="true" height="20" width="12"  >&nbsp;</td>
            <td nowrap="true"  height="20" align="center"  ><a href="<%= ConfigurationManager.AppSettings("gsSiteURL")%>/main.aspx?Tab=0"> 
              <h6><%  If CInt(HttpContext.Current.Session.Item("tab")) = 0 Then
                      Response.Write("<B>Home</B>")
                  Else
                      Response.Write("Home")
                  End If
			%></h6>
              </a> </td>
            <td nowrap="true"   height="20" width="30"  >&nbsp;|&nbsp;</td>
            <td nowrap="true"   height="20" align="center" ><a href="<%= ConfigurationManager.AppSettings("gsSiteURL")%>/main.aspx?Tab=1"> 
              <h6><%  If CInt(HttpContext.Current.Session.Item("tab")) = 1 Then
                      Response.Write("<B>Lookup Music</B>")
                  Else
                      Response.Write("Lookup Music")
                  End If
			%></h6>
              </a> </td>
            <td nowrap="true" height="20"  width="30"  >&nbsp;|&nbsp;</td>
            <td nowrap="true" height="20" align="center"  ><a href="<%= ConfigurationManager.AppSettings("gsSiteURL")%>/main.aspx?Tab=2"> 
              <h6><%  If CInt(HttpContext.Current.Session.Item("tab")) = 2 Then
                      Response.Write("<B>My Bookmarks</B>")
                  Else
                      Response.Write("My Bookmarks")
                  End If
			%></h6>
              </a> </td>
            <td nowrap="true" height="20"  width="30"  >&nbsp;|&nbsp;</td>
            <td nowrap="true" height="20" align="center"  ><a href="<%= ConfigurationManager.AppSettings("gsSiteURL")%>/main.aspx?Tab=3"> 
              <h6><%  If CInt(HttpContext.Current.Session.Item("tab")) = 3 Then
                      Response.Write("<B>My Sets</B>")
                  Else
                      Response.Write("My Sets")
                  End If
			%></h6>
              </a> </td>
            <td nowrap="true" height="20" width="12" >&nbsp;</td>
            <td width="70%" >&nbsp;</td>
          </tr>
        </table></div>