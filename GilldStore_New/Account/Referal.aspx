﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Referal.aspx.cs" Inherits="GilldStore_New.Account.Referal" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" DataSourceID="SqlDataSource1" > 
        
        <Columns>            
            <asp:TemplateField HeaderText="Customer Name">
                <ItemTemplate>                                                            
                    <asp:LinkButton ID="Customer_View_Button" runat="server" Text='<%# Eval("USER_NAME") %>' CommandArgument='<%# Eval("USER_ID") %>' OnCommand="Customer_View_Button_Command">
                    </asp:LinkButton>                    
                </ItemTemplate>
            </asp:TemplateField>    
            <asp:TemplateField HeaderText="Mobile Number">
                <ItemTemplate>
                    <asp:Label id="Mobile_Number_label" runat="server" Text='<%# Eval("MOBILE_NUMBER") %>'>
                    </asp:Label>
                    <asp:Label ID="Mobile_Number_Validated" runat="server" Text="(Not Validated)" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("MOBILE_VALIDATED").ToString(), "0") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>             
              <asp:TemplateField HeaderText="Email Address">
                <ItemTemplate>
                    <asp:Label id="Email_Address_label" runat="server" Text='<%# Eval("EMAIL_ADDRESS") %>'></asp:Label>
                    <asp:Label ID="Email_Address_Validated" runat="server" Text="(Not Validated)" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("EMAIL_ADDRESS_VALIDATED").ToString(), "0") %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>             
            <asp:BoundField HeaderText="User Type" DataField="USER_TYPE" />
        </Columns>
    </asp:GridView>
</asp:Content>