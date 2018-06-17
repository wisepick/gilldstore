<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Create_User.aspx.cs" Inherits="GilldStore_New.Account.Create_User" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="CREATION_DATE" HeaderText="Date" />
            <asp:BoundField DataField="USER_NAME" HeaderText="User Name" />
            <asp:BoundField DataField="PIN_CODE" HeaderText="Pin Code" />
            <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email" />            
        </Columns>
    </asp:GridView>
</asp:Content>
