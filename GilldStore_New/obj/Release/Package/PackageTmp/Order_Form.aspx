<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order_Form.aspx.cs" Inherits="GilldStore_New.Order_Form" %>
<%@ Register Src="~/Controls/Order_Form_Control.ascx" TagName="OrderForm" TagPrefix="uc1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section id="orderform" class="gray-bg padding-top-bottom">
        <uc1:OrderForm id="OrderForm1" runat="server" OnSuccessOrder="OrderForm1_SuccessOrder"></uc1:OrderForm>          
	</section>
</asp:Content>