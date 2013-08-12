<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FooterControl.ascx.vb" Inherits="usercontrols_FooterControl" %>
<table border="0" style="width: 100%">
    <thead >
      <TR> 
        <TH align="left">Logged in: <I><%=strLoggedIn%></I> </TH>
        <TH colspan="<%=intColSpan - 1%>" align="right"> 
          <%
              If CInt(Session("permSetAdmin")) = 1 Then
                  Dim intMySongListCount As Long
			
                  intMySongListCount = sql_CountMySongList()
                  If intMySongListCount > 0 Then
					%> <a href="MyListAdmin.asp"><B><%=intMySongListCount%></B> Song(s) in My List</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
          <%
				end if 
		End if
		%> <%=lngResults %> Results, <%= numpages %> pages </TH>
      </TR>
    </thead>
</table>
