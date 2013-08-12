<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="FTP.aspx.vb" Inherits="FTP" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentpanel" Runat="Server">
<div id="title"><a href="main.aspx">Back To Main</a></div>
    <div id="content">
        <iz:FileManager ID="FileManager1" runat="server" EnableTheming="true" SkinID="VS2005" Height="600px" Width="930px">
        <RootDirectories>
        <iz:RootDirectory DirectoryPath="~/private/genbin" Text="CDAC Documents" />
        </RootDirectories>
        </iz:FileManager>
    </div>

</asp:Content>

