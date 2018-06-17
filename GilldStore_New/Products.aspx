<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="GilldStore_New.Products" %>
<%@ Register Src="~/Controls/Product_Detail_Control.ascx" TagName="ProductDetailControl" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Site.Master" %>   
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="Products_View" runat="server">
            <!-- PRODUCT SECTION -->
            <section class="mainContent full-width clearfix productSection">
                <div class="container">
                    <div class="row">
                        <div class="col-md-9 col-sm-7 col-xs-12 pull-right">
                            <asp:ListView 
                                ID="Product_ListView" 
                                runat="server" 
                                DataKeyNames="PRODUCT_ID" 
                                GroupItemCount="3" 
                                OnPagePropertiesChanging="OnPagePropertiesChanging"
                                OnItemDataBound="Product_Repeater_ItemDataBound"> 
                                <LayoutTemplate>    
                                    <div class="pagerArea text-center">
                                        <ul class="pager">
                                            <asp:DataPager ID="DataPager2" runat="server" PagedControlID="Product_ListView" PageSize="12" >                                    
                                                <Fields>                        
                                                    <asp:NumericPagerField ButtonType="Link" ButtonCount="10" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-dribbble"/>
                                                </Fields>
                                            </asp:DataPager>                      
                                        </ul>             
                                    </div>                                            
                                    <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>       
                                    <div class="pagerArea text-center">
                                        <ul class="pager">
                                            <asp:DataPager ID="DataPager1" runat="server" PagedControlID="Product_ListView" PageSize="12" >                                    
                                                <Fields>                        
                                                    <asp:NumericPagerField ButtonType="Link" ButtonCount="10" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-dribbble"/>
                                                </Fields>
                                            </asp:DataPager>                      
                                        </ul>             
                                    </div>                                                       
                                </LayoutTemplate>  
                                <GroupTemplate>   
                                    <div class="row" style="height:100%">                         
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                
                                    </div>
                                </GroupTemplate>          
                                <ItemTemplate>  
                                    <div class="col-md-4 col-xs-6" style="height:auto">
                                        <div class='box bg-color-<%# Eval("RANK") %>'>
                                            <div class="box-img border-color-<%# Eval("RANK")%> text-center">
                                                <asp:ImageButton id="Product_Detail_Button" runat="server" 
                                                    ImageUrl='<%# "~/img/" +  Eval("PHOTO_FILE_NAME").ToString() %>' 
                                                    AlternateText='<%# Eval("PRODUCT_NAME") %>' 
                                                    ToolTip='<%# Eval("PRODUCT_NAME") %>' 
                                                    CssClass="img-responsive" 
                                                    CommandArgument='<%# Eval("PRODUCT_ID") %>' OnCommand="Product_Detail_Button_Command"/>
                                                
                                            </div>
                                            <div class="box-info">
                                                <h4><%# Eval("PRODUCT_NAME") %></h4>
                                                <asp:Repeater ID="Product_Price_List" runat="server" OnItemDataBound="Product_Price_List_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="formField">
                                                            <table class="table table-responsive">
                                                                <tr>
                                                                    <td>
                                                                        <asp:HiddenField ID="Measurement_Unit" runat="server" Value='<%# Eval("MEASUREMENT_UNIT") %>' />
                                                                        <asp:HiddenField ID="Product_Id" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />
                                                                        <asp:HiddenField ID="Price" runat="server" Value='<%# Eval("PRICE") %>' />                                                                
                                                                        <%# Eval("MEASUREMENT_UNIT") %>&nbsp;<%# Eval("MEASUREMENT_NAME") %>                                                                                                    
                                                                    </td>
                                                                    <td style="text-align:right">
                                                                        <i class="fa fa-rupee"></i>&nbsp;<%# Eval("PRICE") %>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList 
                                                                            ID="Quantity" 
                                                                            runat="server" 
                                                                            CssClass="form-control border-color-2" 
                                                                            AppendDataBoundItems="true" 
                                                                            DataValueField="SR_NO"
                                                                            DataTextField="SR_NO">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>                 
                                                <asp:LinkButton 
                                                    ID="Add_To_Cart_Button" 
                                                    runat="server" 
                                                    Text='<i class="fa fa-shopping-basket " aria-hidden="true"></i>Add to Cart'
                                                    CssClass="btn btn-primary" 
                                                    OnClick="Add_To_Cart_Button_Click">
                                                </asp:LinkButton>
                                        
                                        
                                            </div>
                                        </div>
                                    </div>                             
                                </ItemTemplate>
                            </asp:ListView>		                         
                        </div>
                        <div class="col-md-3 col-sm-5 col-xs-12 pull-left">
                            <aside>
                                <div class="panel panel-default courseSidebar">
                                    <div class="panel-heading bg-color-5 border-color-5">
                                        <h3 class="panel-title">Categories</h3>
                                    </div>
                                    <div class="panel-body">
                                        <ul class="list-unstyled categoryItem">
                                            <asp:Repeater ID="Product_Category_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <li><asp:LinkButton id="Product_Category_Id" runat="server" Text='All' OnCommand="Product_Category_Id_Command" CommandArgument='All' CssClass="btn-primary"> </asp:LinkButton></li>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li><asp:LinkButton id="Product_Category_Id" runat="server" Text='<%# Eval("PRODUCT_CATEGORY_NAME") %>' OnCommand="Product_Category_Id_Command" CommandArgument='<%# Eval("PRODUCT_CATEGORY_ID") %>'> </asp:LinkButton></li>
                                                </ItemTemplate>
                                        
                                            </asp:Repeater>                                    
                                        </ul>
                                    </div>
                                </div>
                            </aside>
                        </div>
                    </div>
                </div>
            </section>            
        </asp:View>
        <asp:View ID="Product_Detail_View" runat="server">
            <section class="mainContent ">
            <div class="container">
            <p style="text-align:left">
                <asp:Button ID="Back_Button" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="Back_Button_Click"/>
            </p>
                
            <uc1:ProductDetailControl id="ProductDetailControl1" runat="server" OnCartModified ="Cart_Modified">

            </uc1:ProductDetailControl>
                </div>
                </section>
        </asp:View>
    </asp:MultiView>
</asp:Content>
