<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_Grid.ascx.cs" Inherits="GilldStore_New.Controls.Order_Grid" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="Order_Summary_View" runat="server">
        <asp:HiddenField ID="ORDER_ID" runat="server" />
        <asp:GridView ID="Orders_GridView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="Orders_GridView_PageIndexChanging" AllowSorting="true" EmptyDataText="No Orders Found" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:TemplateField HeaderText="Order Number" SortExpression="ORDER_NUMBER">
                    <ItemTemplate>     
                        <asp:LinkButton ID="View_Order_Button" runat="server" Text='<%# Eval("ORDER_NUMBER") %>' CommandArgument='<%# Eval("ORDER_ID") %>' OnCommand="View_Order_Button_Command" CommandName="View Order"></asp:LinkButton>                                                                                               
                    </ItemTemplate>
                </asp:TemplateField>                                  
                <asp:BoundField DataField="USER_NAME" HeaderText="Customer Name" SortExpression="USER_NAME"/>                                
                <asp:BoundField DataField="ORDER_DATE" HeaderText="Order Date" SortExpression="ORDER_DATE"/>                                
                <asp:BoundField DataField="ORDER_TOTAL" HeaderText="Order Amount" SortExpression="ORDER_TOTAL"/>
                <asp:BoundField DataField="ORDER_STATUS" HeaderText="Order Status" />
            </Columns>
        </asp:GridView>     
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="GET_ORDERS_BY_STATUS"
        SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="P_STATUS_NAME"  Direction="Input" ConvertEmptyStringToNull="false"/>                                
                <asp:Parameter Name="P_STATUS_NAME_1"  Direction="Input" ConvertEmptyStringToNull="false"/>                                
            </SelectParameters>
        </asp:SqlDataSource>    
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="SEARCH_ORDER"
        SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Name="P_ORDER_ID"  Direction="Input" ConvertEmptyStringToNull="false"/>                                                
            </SelectParameters>
        </asp:SqlDataSource>                 
    </asp:View>
    <asp:View ID="Order_Info_View" runat="server">
        <asp:Button ID="Back_Button" runat="server" Text="Back" OnClick="Back_Button_Click" CssClass="btn btn-primary" /><br /><br />   
        <uc1:OrderInfo id="OrderInfo1" runat="server"></uc1:OrderInfo>
    </asp:View>
</asp:MultiView>
