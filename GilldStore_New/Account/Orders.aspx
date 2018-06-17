<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="GilldStore_New.Account.Orders" %>
<%@ Register Src="~/Controls/Order_Grid.ascx" TagName="OrderGrid" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:OrderGrid id ="OrderGrid1" runat="server">

    </uc1:OrderGrid>
</asp:Content>
