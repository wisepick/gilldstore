<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Product_Detail.aspx.cs" Inherits="GilldStore_New.Product_Detail" %>
<%@ Register Src="~/Controls/Product_Detail_Control.ascx" TagName="ProductDetailControl" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Site.Master" %>   

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="mainContent">
        <div class="container">
            <uc1:ProductDetailControl id="ProductDetailControl1" runat="server" OnCartModified ="Cart_Modified">
            </uc1:ProductDetailControl>        
</asp:Content>
