<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyTable.ascx.vb" Inherits="usercontrols_MyTable"  %>
<%@ Register Src="navicontrol.ascx" TagName="navicontrol" TagPrefix="uc2" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="uc3" %>

<uc2:navicontrol ID="navicontrol1" runat="server" />
<uc3:PagingControl ID="PagingControl1" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
