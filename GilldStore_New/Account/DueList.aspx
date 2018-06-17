<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="DueList.aspx.cs" Inherits="GilldStore_New.Account.DueList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <asp:GridView ID="DueListView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" OnPageIndexChanging="DueListView_PageIndexChanging" DataSourceID="SqlDataSource1" AllowSorting="true">         
        <Columns>            
            <asp:TemplateField HeaderText="Customer Name">
                <ItemTemplate>                                                            
                    <asp:LinkButton ID="Customer_View_Button" runat="server" Text='<%# Eval("USER_NAME") %>' CommandArgument='<%# Eval("USER_ID") %>' OnCommand="Customer_View_Button_Command" >

                    </asp:LinkButton>                    
                </ItemTemplate>
            </asp:TemplateField>    
            <asp:TemplateField HeaderText="Mobile Number">
                <ItemTemplate>
                    <asp:Label id="Mobile_Number_label" runat="server" Text='<%# Eval("MOBILE_NUMBER") %>'>
                    </asp:Label>                    
                </ItemTemplate>
            </asp:TemplateField>             
            <asp:TemplateField HeaderText="Email Address">
                <ItemTemplate>
                    <asp:Label id="Email_Address_label" runat="server" Text='<%# Eval("EMAIL_ADDRESS") %>'></asp:Label>                    
                    
                </ItemTemplate>
            </asp:TemplateField>             
            <asp:TemplateField HeaderText="Amount Due" SortExpression="AMOUNT_DUE">
                <ItemTemplate>
                    <asp:Label id="Amount_Due_Label" runat="server" Text='<%# "₹ " + Eval("AMOUNT_DUE").ToString() %>'></asp:Label>                    
                    
                </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="Due Since" SortExpression="PENDING_SINCE">
                <ItemTemplate>
                    <asp:Label id="Due_Since_Label" runat="server" Text='<%# Eval("PENDING_SINCE").ToString() + " Days" %>'></asp:Label>                   
                    
                </ItemTemplate>
            </asp:TemplateField>    
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="Send_Remainder_Button" runat="server" Text="Send Remainder" CssClass="btn btn-primary" OnCommand="Send_Remainder_Button_Command" CommandArgument='<%# Eval("USER_ID") %>'/>
                    <asp:Label ID="Remainder_Message" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>          
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
        SelectCommand="GET_DUE_LIST"
        SelectCommandType="StoredProcedure">        
    </asp:SqlDataSource>
        </asp:MultiView>
</asp:Content>
