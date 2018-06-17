<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Order_Form.aspx.cs" Inherits="GilldStore_New.Account.Order_Form" %>
<%@ Register Src="~/Controls/Order_Form_Control.ascx" TagName="OrderForm" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:OrderForm id="OrderForm1" runat="server">

    </uc1:OrderForm>
</asp:Content>