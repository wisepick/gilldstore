<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="CustomerView.aspx.cs" Inherits="GilldStore_New.Account.CustomerView" MaintainScrollPositionOnPostback="true"%>
<%@ Register Src="~/Controls/CustomerControl.ascx" TagName="CustmerControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order_Form_Control.ascx" TagName="OrderForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CustomerGrid.ascx" TagName="CustomerGrid" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Payment_Form.ascx" TagName="PaymentForm" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <asp:HiddenField ID="CustomerId" runat="server"/>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View id="Search_Customer_View" runat="server">
            <asp:PlaceHolder ID="Search_Customer_Form" runat="server">
                <table width="40%" align="center">
                    <tr>
                        <td>
                            <div class="panel panel-primary">
                                <div class="panel-heading">Search Customer</div>
                                    <div class="panel-body">
                                        <table class="table table-bordered">
                                            <tr>
                                                <th>
                                                    Customer Name
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="Customer_Name" runat="server" MaxLength="255">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Mobile Number
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="Mobile_Number" runat="server" MaxLength="10">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Email Address
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="Email_Address" runat="server" MaxLength="255">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>                                
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="Search_Customer_Button" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="Search_Customer_Button_Click"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <uc1:CustomerGrid id="CustomerGrid1" runat="server" OnCustomerSelect="CustomerGrid1_OnCustomerSelect"></uc1:CustomerGrid>
        </asp:View>
        <asp:View ID="Customer_View" runat="server">            
            <asp:LinkButton ID="Back_Button" runat="server" Text="Back to Customer List" OnClick="Back_Button_Click"></asp:LinkButton>
             <asp:LinkButton ID="Due_Back_Button" runat="server" Text="Back to Due List" OnClick="Due_Back_Button_Click" Visible ="false"></asp:LinkButton>
            <uc1:CustmerControl 
                ID="CustomerControl1" 
                runat="server"
                OnSuccessfullyUpdated="Customer_View_OnSuccessfullyUpdated"/>
            <p align="center">
                <asp:LinkButton ID="All_Orders_Button" runat="server" Text="All Orders" CssClass="btn btn-primary" OnClick="All_Orders_Button_Click"></asp:LinkButton>
                <asp:LinkButton id="Due_Orders" runat="server" Text="Due Orders" CssClass="btn btn-primary" OnClick="Due_Orders_Click"></asp:LinkButton>
                <asp:LinkButton ID="Create_New_Order_Button" runat="server" Text="Create New Order" CssClass="btn btn-primary" OnClick="Create_New_Order_Button_Click"></asp:LinkButton>
                <asp:LinkButton ID="Address_Book_Button" runat="server" Text="Address Book" CssClass="btn btn-primary" OnClick="Address_Book_Button_Click"></asp:LinkButton>
                <asp:LinkButton ID="Post_Payment_Button" runat="server" Text="Post Payment" CssClass="btn btn-primary" OnClick="Post_Payment_Button_Click"></asp:LinkButton>
                <asp:LinkButton ID="Post_Adjustment_Button" runat="server" Text="Post Adjustment" CssClass="btn btn-primary" OnClick="Post_Adjustment_Button_Click"></asp:LinkButton>

            </p>
            
            <asp:MultiView ID="MultiView2" runat="server">
                <asp:View ID="Address_View" runat="server">                    
                    <uc1:AddressBook 
                        ID="AddressBook1"
                        runat="server" Display_DeliveryAddressOption="N"
                        Set_ListView_Mode ="None"
                    />
                </asp:View>
                <asp:View ID="Order_View" runat="server">
                    <uc1:OrderForm ID="OrderForm1" runat="server" OnOrderLoad="OrderForm1_OnLoad" OnSuccessOrder ="OrderForm1_OnSuccessOrder"/>
                </asp:View>
                <asp:View ID="OrderSummaryView" runat="server">
                    <uc1:OrderInfo ID="OrderInfo1" runat="server" Show_Order_Message="true" Show_Shipping_Order_Button="false"/>
                </asp:View>
                <asp:View ID="AllOrdersView" runat="server">                    
                    <asp:GridView ID="Orders_GridView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="Orders_GridView_PageIndexChanging" DataSourceID="SqlDataSource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Order Number">
                                <ItemTemplate>     
                                    <asp:LinkButton ID="View_Order_Button" runat="server" Text='<%# Eval("ORDER_NUMBER") %>' CommandArgument='<%# Eval("ORDER_ID") %>' OnCommand="View_Order_Button_Command" CommandName="View Order"></asp:LinkButton>                                                                                               
                                </ItemTemplate>
                            </asp:TemplateField>                                  
                            <asp:BoundField DataField="ORDER_DATE" HeaderText="Order Date" />                                
                            <asp:BoundField DataField="ORDER_TOTAL" HeaderText="Order Amount" />
                            <asp:BoundField DataField="ORDER_DUE" HeaderText="Due Amount" />
                            <asp:BoundField DataField="ORDER_STATUS" HeaderText="Order Status" />
                        </Columns>
                    </asp:GridView>     
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
                    SelectCommand="GET_USER_ORDERS_BY_USER_ID"
                    SelectCommandType="StoredProcedure">
                        <SelectParameters>                            
                            <asp:Parameter Name="P_USER_ID"  Direction="Input" ConvertEmptyStringToNull="false"/>                                
                            <asp:Parameter Name="P_DUE_ORDER"  Direction="Input" ConvertEmptyStringToNull="false" DefaultValue="0"/>                                
                        </SelectParameters>
                    </asp:SqlDataSource>                   
                </asp:View>
                <asp:View ID="Order_Details_View" runat="server">
                    <asp:LinkButton ID="Order_Back_Button" runat="server" Text="Back to Order List" OnClick="Order_Back_Button_Click"></asp:LinkButton>
                    <uc1:OrderInfo ID="OrderInfo2" runat="server" />
                </asp:View>  
                <asp:View ID="Payments_View" runat="server">
                    <uc1:PaymentForm ID="PaymentForm1" runat="server" OnSuccessPayment ="Customer_OnSuccessPayment"/>
                </asp:View>
            </asp:MultiView>            
        </asp:View>        
    </asp:MultiView>
    
</asp:Content>
