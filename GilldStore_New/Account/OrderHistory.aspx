<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="GilldStore_New.Account.OrderHistory" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- WHITE SECTION -->
    <section class="full-width clearfix eventSection" id="ourEvents">
        <div class="container">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <div class="row">
                        <asp:ListView ID="Order_ListView" runat="server" DataKeyNames="ORDER_ID" GroupItemCount="2" OnPagePropertiesChanging="Order_ListView_PagePropertiesChanging" DataSourceID="SqlDataSource1"> 
                            <LayoutTemplate>    
                                <div class="pagerArea text-center">
                                    <ul class="pager">
                                        <asp:DataPager ID="DataPager2" runat="server" PagedControlID="Order_ListView" PageSize="12" >                                    
                                            <Fields>   
                                                <asp:NumericPagerField ButtonType="Link" ButtonCount="10" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-dribbble"/>                                        
                                            </Fields>
                                        </asp:DataPager>                      
                                    </ul>             
                                </div>                   
                                <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>       
                                <div class="pagerArea text-center">
                                    <ul class="pager">
                                        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="Order_ListView" PageSize="12" >                                    
                                            <Fields>                        
                                        
                                                <asp:NumericPagerField ButtonType="Link" ButtonCount="10" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-dribbble"/>
                                        
                                            </Fields>
                                        </asp:DataPager>                      
                                    </ul>             
                                </div>                                                       
                            </LayoutTemplate>  
                            <GroupTemplate>   
                                <div class="row">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                
                                </div>
                            </GroupTemplate>          
                            <ItemTemplate>                      
                                <div class="col-sm-6 col-xs-12 block">
                                    <div class='media eventContent bg-color-4'>
                                        <a class="media-left" href="order_detail.aspx">
                                            <span class="sticker-round"><%# Eval("ORDER_DAY") %> <br><%# Eval("ORDER_MONTH") %><br /><%# Eval("ORDER_YEAR") %></span>
                                            &nbsp;
                                        </a>
                                        <div class="media-body">
                                            <h3>Order Number : <%# Eval("ORDER_NUMBER") %></h3>
                                            <ul class="list-unstyled">
                                                <li>Order Amount <i class="fa fa-rupee" aria-hidden="true"></i><%# Eval("ORDER_TOTAL") %></li>
                                                <li>Order Status <i class='fa <%# Eval("ICON_NAME") %>' aria-hidden="true"></i><%# Eval("ORDER_STATUS") %></li>                                        
                                            </ul>
                                            <p>Products Ordered : <%# Eval("ORDERED_PRODUCTS") %> </p>
                                            <ul class="list-inline btn-yellow">
                                                <li><asp:Button ID="View_Details_Button" runat="server" OnCommand="View_Details_Button_Command" Text="View Detail" CssClass="btn btn-primary" CommandArgument='<%# Eval("ORDER_ID") %>'/>
                                                    </li>
                                                <%--<li><a href="single-event-left-sidebar.html" class="btn btn-primary">Cancel</a></li>
                                                <li><a href="single-event-left-sidebar.html" class="btn btn-primary">Buy Again</a></li>--%>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <div class="row">                        
                        <p style="text-align:right">
                            <asp:Button ID="Back_Button" runat="server" Text="Back" OnClick="Back_Button_Click" CssClass="btn btn-primary"/>
                        </p>
                        <uc1:OrderInfo ID="OrderInfo1" runat="server" />
                        <p style="text-align:right">
                            <asp:Button ID="Back_Button2" runat="server" Text="Back" OnClick="Back_Button_Click" CssClass="btn btn-primary"/>
                        </p>                        
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </section>
                            
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
    SelectCommand="GET_USER_ORDERS"
    SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="P_EXTERNAL_USER_ID"  Direction="Input" ConvertEmptyStringToNull="false"/>                                                                
        </SelectParameters>
    </asp:SqlDataSource>   
</asp:Content>
