<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DownloadPics.aspx.cs" Inherits="TB.DownloadPics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        URL:<br />
        Folder:<asp:TextBox ID="txtFolder" runat="server"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtDesc" runat="server" Rows="30" TextMode="MultiLine" 
            Width="640px"></asp:TextBox><asp:Button ID="btnDnDesc" runat="server" Text="Download Pics" OnClick="btnDnDesc_Click" />
        
    </div>
</asp:Content>
