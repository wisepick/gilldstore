<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Open_Cart.aspx.cs" Inherits="GilldStore_New.Account.Open_Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <asp:LinkButton ID="View_Detail" runat="server" Text='<%# Eval("CREATION_DATE") %>' 
                        CommandArgument='<%# Eval("SHOPPING_CART_ID") %>'
                        OnCommand="View_Detail_Command">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CREATION_DATE" HeaderText="Date" />
            <asp:BoundField DataField="USER_NAME" HeaderText="User Name" />
            <asp:BoundField DataField="PIN_CODE" HeaderText="Pin Code" />
            <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email" />     
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="Send_Email" runat="server" Text="Send Email" CommandArgument='<%# Eval("CREATION_USER_ID").ToString() + "," + Eval("CREATION_DATE").ToString() %>' OnCommand="Send_Email_Command" CssClass="btn btn-primary"/>
                </ItemTemplate>
            </asp:TemplateField>       
        </Columns>
    </asp:GridView>
    <asp:GridView ID="Shopping_Cart_Details_View" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="PRODUCT_NAME" HeaderText="Product" />
            <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />
            <asp:BoundField DataField="PRICE" HeaderText="Price" />
            <asp:BoundField DataField="SUBTOTAL" HeaderText="Sub Total" />
        </Columns>
    </asp:GridView>
</asp:Content>
