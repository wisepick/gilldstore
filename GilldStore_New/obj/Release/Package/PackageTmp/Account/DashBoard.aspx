<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="GilldStore_New.Account.DashBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  DataSourceID="SqlDataSource1" Caption="<h3>Customer Statistics</h3>" >         
        <Columns>            
            <asp:BoundField DataField="TOTAL_CUSTOMERS" HeaderText="No. of Customers" />
            <asp:BoundField DataField="ONLINE_ACTIVE_CUSTOMERS" HeaderText="Active Online Customers" />
            <asp:BoundField DataField="STORE_ACTIVE_CUSTOMERS" HeaderText="Store Customers" />
            <asp:BoundField DataField="SHOPPED_CUSTOMERS" HeaderText="No. Of Customers Bought" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="CUSTOMER_STATS"
        SelectCommandType="StoredProcedure">        
    </asp:SqlDataSource>
    <asp:GridView ID="GridView2" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  DataSourceID="SqlDataSource2" Caption="<h3>Order Statistics</h3>" >         
        <Columns>            
            <asp:BoundField DataField="ATTRIBUTE_NAME" HeaderText="Order Status" />
            <asp:BoundField DataField="STATS" HeaderText="No. of Orders" />            
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="ORDER_STATS"
        SelectCommandType="StoredProcedure">        
    </asp:SqlDataSource>
    <asp:GridView ID="GridView3" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"  DataSourceID="SqlDataSource3" Caption="<h3>Order Statistics</h3>" >         
        <Columns>            
            <asp:BoundField DataField="TOTAL_SALES" HeaderText="Total Sales" DataFormatString="{0:C2}" HtmlEncode="false"/>
            <asp:BoundField DataField="CONFIRMED_SALES" HeaderText="Confirmed Sales" DataFormatString="{0:C2}" HtmlEncode="false"/>            
            <asp:BoundField DataField="SALES_PENDING" HeaderText="Pending Sales" DataFormatString="{0:c2}" HtmlEncode="false"/>            
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="SALES_STATS"
        SelectCommandType="StoredProcedure">        
    </asp:SqlDataSource>
</asp:Content>
