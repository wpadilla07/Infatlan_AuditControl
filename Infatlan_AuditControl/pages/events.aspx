<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="events.aspx.cs" Inherits="Infatlan_AuditControl.pages.events" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div style="height: 850px;">
        <iframe name="myIframe" id="myIframe" width="" height="" src="/pages/calendar.aspx" style="border: none; width: 100%; height: 100%;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
