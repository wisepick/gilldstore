<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="ManageCustomer.aspx.cs" Inherits="GilldStore_New.Account.ManageCustomer" %>
<%@ Register Src="~/Controls/CustomerControl.ascx" TagName="CustmerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <uc1:CustmerControl 
        ID="CustomerControl1" 
        runat="server"/>
</asp:Content>

