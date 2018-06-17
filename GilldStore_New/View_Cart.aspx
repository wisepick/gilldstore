<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View_Cart.aspx.cs" Inherits="GilldStore_New.View_Cart" %>
<%@ MasterType VirtualPath="~/Site.Master" %>   
<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- MAIN SECTION -->
    <asp:HiddenField ID="Shipping_Charge" runat="server" Value="0.0" />
    <asp:HiddenField ID="Surprise_Discount_Eligible" runat="server" Value="1" />
    <section class="mainContent full-width clearfix">
        <div class="container">
            <div class="row progress-wizard" style="border-bottom:0;">
                <asp:PlaceHolder ID="Order_Info_Active_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Order_Info_Complete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">
                </asp:PlaceHolder>
                        <div class="progress"><div class="progress-bar"></div></div>
                        <a href="~/View_Cart.aspx" runat="server" class="progress-wizard-dot">
                            <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                        </a>
                        <div class="progressInfo">1. Order info</div>
                    </div>             
                <asp:PlaceHolder ID="Personal_Info_Active_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Personal_Info_InComplete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step incomplete">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Personal_Info_Complete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">
                </asp:PlaceHolder>
                        <div class="progress"><div class="progress-bar"></div></div>                    
                        <asp:LinkButton 
                            ID ="Personal_Info_Button" 
                            runat="server" 
                            CssClass="progress-wizard-dot" 
                            Text='<i class="fa fa-map-marker" aria-hidden="true"></i>'
                            OnClick="CheckOut_Button_Click">
                        </asp:LinkButton>                                            
                        <div class="progressInfo">2. Address Info</div>
                    </div>
                 <asp:PlaceHolder ID="Review_Order_Info_Active_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Review_Order_Info_InComplete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step incomplete">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Review_Order_Info_Complete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">
                </asp:PlaceHolder>
                        <div class="progress"><div class="progress-bar"></div></div>
                        <asp:LinkButton 
                            ID ="LinkButton2" 
                            runat="server" 
                            CssClass="progress-wizard-dot" 
                            Text='<i class="fa fa-list" aria-hidden="true"></i>'
                            OnClick="Next_Address_Book_Button_Click">
                        </asp:LinkButton>                           
                        <div class="progressInfo">3. Review Order</div>
                    </div>
                 <asp:PlaceHolder ID="Payment_Info_Active_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Payment_Info_InComplete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step incomplete">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Payment_Info_Complete_PlaceHolder" runat="server" Visible ="false">
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">
                </asp:PlaceHolder>
                        <div class="progress"><div class="progress-bar"></div></div>
                        <asp:LinkButton 
                            ID ="LinkButton1" 
                            runat="server" 
                            CssClass="progress-wizard-dot" 
                            Text='<i class="fa fa-rupee" aria-hidden="true"></i>'
                            OnClick="Next_Review_Order_Button_Click">
                        </asp:LinkButton>                           
                        <div class="progressInfo">4. Payment Info</div>
                    </div>
                <div class="col-sm-2 col-xs-12 progress-wizard-step incomplete">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a href="#" class="progress-wizard-dot">
                        <i class="fa fa-check" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">4. Confirmation</div>
                </div>
            </div>            
        </div>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="OrderView" runat="server">
                <div class="container">
                    <div class="row">
                        <asp:PlaceHolder ID="Checkout_Message_PlaceHolder" runat="server" Visible ="false">
                            <p style="text-align:center">
                                <asp:Label ID="Label1" runat="server" ForeColor="Red"  Text="Alteast one item should be selected">
                                </asp:Label>
                            </p>
                        </asp:PlaceHolder>
                        <div class="col-xs-12">            
                            
                            <div class="cartInfo">
                                <div class="table-responsive">
                                    <table class="table">
                                        <thead>
                                            <tr class="bg-color-1">
                                                <th class="col-lg-2 col-xs-3" style="min-width: 190px; text-indent:-999px;">
                                                empty</th>
                                                <th class="col-lg-4 col-xs-3">Product Name</th>
                                                <th class="col-lg-4 col-xs-3">Size</th>
                                                <th class="col-lg-2 col-xs-2">Price</th>
                                                <th class="col-lg-2 col-xs-2">Quantity</th>
                                                <th class="col-lg-2 col-xs-2">Total</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Cart_Details_Repeater" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>      
                                                            <asp:HiddenField ID="PRODUCT_ID" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />                                                    
                                                            <asp:HiddenField ID="MEASUREMENT_ID" runat="server" Value='<%# Eval("MEASUREMENT_ID") %>' />
                                                            <asp:HiddenField ID="MEASUREMENT_UNIT" runat="server" Value='<%# Eval("MEASUREMENT_UNIT") %>' />
                                                            <asp:HiddenField ID="OLD_QUANTITY" runat="server" Value='<%# Eval("QUANTITY") %>' />                                                    
                                                            <asp:HiddenField ID="PRICE" runat="server" Value='<%# Eval("PRICE") %>' />
                                                            <asp:LinkButton 
                                                                ID="Close_Button" 
                                                                runat="server"  
                                                                CssClass="close" 
                                                                Text='<i class="fa fa-times" aria-hidden="true"></i>' 
                                                                OnCommand="Close_Button_Command"
                                                                CommandArgument='<%# Eval("PRODUCT_ID").ToString() + "," + Eval("MEASUREMENT_UNIT").ToString() + "," + Eval("QUANTITY").ToString() +  "," + (double.Parse(Eval("QUANTITY").ToString()) * double.Parse(Eval("PRICE").ToString())).ToString()%>'
                                                            />                          
                                                            <span class="cartImage">    
                                                                <asp:Image ID="Cart_Image" runat="server" ImageUrl='<%# "~/img/" + Eval("PHOTO_FILE_NAME").ToString() %>' Width="50" CssClass="img-rounded"/>                            
                                                            </span>
                                                        </td>
                                                        <td><%# Eval("PRODUCT_NAME") %></td>
                                                        <td><%# Eval("MEASUREMENT_UNIT").ToString() + " " + Eval("MEASUREMENT_NAME").ToString() %></td>
                                                        <td><i class="fa fa-rupee"></i>&nbsp;<%# Eval("PRICE") %></td>
                                                        <td><asp:TextBox ID="Quantity" runat="server" Text='<%# Eval("QUANTITY") %>' CssClass="form-control border-color-2"></asp:TextBox>    </td>                      
                                                        <td><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Subtotal" runat="server" Text='<%# (double.Parse(Eval("QUANTITY").ToString()) * double.Parse(Eval("PRICE").ToString())).ToString() %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>                                        
                                            <tr>
                                                
                                                <td colspan="3">
                                                    <asp:LinkButton 
                                                        ID="Update_Cart_Button" 
                                                        runat="server" 
                                                        Text="Update Cart" 
                                                        CssClass="btn btn-primary" 
                                                        OnClick="Update_Cart_Button_Click"/>
                                                </td>
                                                <td colspan="3">
                                                    <asp:LinkButton 
                                                        ID="Next_Cart_Button" 
                                                        runat="server" 
                                                        Text='next<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>' 
                                                        CssClass="btn btn-primary" 
                                                        OnClick="CheckOut_Button_Click"/>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                       
            </div>
        </asp:View>
        <asp:View ID="AddressBook_View" runat="server">
            <div class="container">
                <div class="row">   
                    <div class="ol-md-12  col-sm-4 col-xs-12">
                        <asp:LinkButton 
                            id="LinkButton3"
                            runat="server"
                            Text='<i class="fa fa-arrow-circle-left" aria-hidden="true"></i>previous' 
                            CssClass="btn btn-primary"
                            OnClick="Previous_Address_Book_Button_Click"></asp:LinkButton>
                    </div>
                    <div class="ol-md-12 col-sm-4 col-xs-12" style="text-align=center">
                        <h3><asp:Label ID="AddressBook_Message" runat="server" Text="Address should be selected" ForeColor="Red" Visible ="false"></asp:Label></h3> 
                            
                    </div>
                    <div class="ol-md-12  col-sm-4 col-xs-12" style="text-align:right">
                        <asp:LinkButton 
                            id="LinkButton4"
                            runat="server"
                            Text='next<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>' 
                            CssClass="btn btn-primary"
                            OnClick="Next_Address_Book_Button_Click"></asp:LinkButton>
                    </div>   
                    <uc1:AddressBook 
                        ID="AddressBook1" 
                        runat="server"                 
                        Display_DeliveryAddressOption="Y"
                    />
                    <div class="ol-md-12  col-sm-4 col-xs-12">
                        <asp:LinkButton 
                            id="Previous_Address_Book_Button"
                            runat="server"
                            Text='<i class="fa fa-arrow-circle-left" aria-hidden="true"></i>previous' 
                            CssClass="btn btn-primary"
                            OnClick="Previous_Address_Book_Button_Click"></asp:LinkButton>
                    </div>
                    <div class="ol-md-12 col-sm-4 col-xs-12" style="text-align=center">
                        <h3><asp:Label ID="AddressBook_Message1" runat="server" Text="Address should be selected" ForeColor="Red" Visible ="false"></asp:Label></h3> 
                            
                    </div>
                    
                    <div class="ol-md-12  col-sm-4 col-xs-12" style="text-align:right">
                        <asp:LinkButton 
                            id="Next_Address_Book_Button"
                            runat="server"
                            Text='next<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>' 
                            CssClass="btn btn-primary"
                            OnClick="Next_Address_Book_Button_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="Review_Order_View" runat="server">
            <div class="container">
                <div class="row">    
                    <div class="ol-md-12  col-sm-6 col-xs-12" style="text-align:left">
                                                    <asp:LinkButton 
                                                        ID="Previous_Review_Order_Button" 
                                                        runat="server" 
                                                        Text='<i class="fa fa-arrow-circle-left" aria-hidden="true"></i>previous' 
                                                        CssClass="btn btn-primary" 
                                                        OnClick="Previous_Review_Order_Button_Click"/>
                                                </div>  
                    <div class="ol-md-12  col-sm-6 col-xs-12" style="text-align:right">
                                                    <asp:LinkButton 
                                                        ID="Next_Review_Order_Button" 
                                                        runat="server" 
                                                        Text='next<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>' 
                                                        CssClass="btn btn-primary" 
                                                        OnClick="Next_Review_Order_Button_Click"/>
                                                </div>  
                    <asp:PlaceHolder ID="Surprise_Discount_PlaceHolder" runat="server" Visible ="false">
                                <div class="label label-info">
                                    Congratulations, You won a surprise discount of <i class="fa fa-rupee"></i>&nbsp; <asp:Label ID="Surprise_Discount" runat="server"></asp:Label>
                                </div>
                            </asp:PlaceHolder>
                            
                    <div class="col-xs-12">            
                            
                            <div class="cartInfo">
                                <div class="table-responsive">
                                    <table class="table">
                                        <thead>
                                            <tr class="bg-color-1">
                                                <th class="col-lg-2 col-xs-2" style="min-width: 190px; text-indent:-999px;">
                                                empty</th>
                                                <th class="col-lg-4 col-xs-2">Product Name</th>
                                                <th class="col-lg-4 col-xs-2">Size</th>
                                                <th class="col-lg-2 col-xs-2">Price</th>
                                                <th class="col-lg-2 col-xs-2">Quantity</th>
                                                <th class="col-lg-2 col-xs-2">Total</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Review_Order_Repeater" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>      
                                                            <asp:HiddenField ID="PRODUCT_ID" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />                                                    
                                                            <asp:HiddenField ID="MEASUREMENT_ID" runat="server" Value='<%# Eval("MEASUREMENT_ID") %>' />
                                                            <asp:HiddenField ID="MEASUREMENT_UNIT" runat="server" Value='<%# Eval("MEASUREMENT_UNIT") %>' />
                                                            <asp:HiddenField ID="OLD_QUANTITY" runat="server" Value='<%# Eval("QUANTITY") %>' />                                                    
                                                            <asp:HiddenField ID="PRICE" runat="server" Value='<%# Eval("PRICE") %>' />
                                                                                 
                                                            <span class="cartImage">    
                                                                <asp:Image ID="Cart_Image" runat="server" ImageUrl='<%# "~/img/" + Eval("PHOTO_FILE_NAME").ToString() %>' Width="50" CssClass="img-rounded"/>                            
                                                            </span>
                                                        </td>
                                                        <td><%# Eval("PRODUCT_NAME") %></td>
                                                        <td><%# Eval("MEASUREMENT_UNIT").ToString() + " " + Eval("MEASUREMENT_NAME").ToString() %></td>
                                                        <td><i class="fa fa-rupee"></i>&nbsp;<%# Eval("PRICE") %></td>
                                                        <td><%# Eval("QUANTITY") %></td>                      
                                                        <td><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Subtotal" runat="server" Text='<%# (double.Parse(Eval("QUANTITY").ToString()) * double.Parse(Eval("PRICE").ToString())).ToString() %>'></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>                                        
                                            <tr>
                                                 
                                                <td colspan="6">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="Coupon_Code" runat="server" CssClass="form-control border-color-2" placeholder="I have a discount coupon" aria-describedby="basic-addon2" />
                                                        <asp:LinkButton ID="Apply_Coupon_Button" runat="server" CssClass="btn btn-primary input-group-addon" Text="Apply Coupon" OnClick="Apply_Coupon_Button_Click" />                                                
                                                        <asp:Label ID="Promotion_Message" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                
                                                
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                </div>
                <div class="row">                
			                            <div class="col-sm-6 col-sm-offset-6 col-xs-12">
			                                <div class="cartInfo cartTotal">
			                                    <div class="table-responsive">
			                                        <table class="table">
			                                            <thead>
			                                                <tr class="bg-color-2">
			                                                    <th>Cart Total</th>
			                                                    <th></th>
			                                                </tr>
			                                            </thead>
			                                            <tbody>
			                                            <tr>
			                                                <td class="col-xs-6">
			                                                    <strong>Total</strong>
			                                                </td>
			                                                <td class="col-xs-6"><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Total_Amount" runat="server" Text="0.0"></asp:Label></td>
			                                            </tr>
			                                            <tr>
			                                                <td class="col-xs-6">
			                                                    <strong>Shipping Charges</strong>
			                                                </td>
			                                                <td class="col-xs-6"><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Shipping_Charges" runat="server" Text="0.0"></asp:Label></td>
			                                            </tr>
			                                                <tr>
			                                                <td class="col-xs-6">
			                                                    <strong>Shipping Discounts</strong>
			                                                </td>
			                                                <td class="col-xs-6"><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Shipping_Discounts" runat="server" Text="0.0"></asp:Label></td>
			                                            </tr>
			                                                <tr>
			                                                <td class="col-xs-6">
			                                                    <strong>Promotional Discount</strong>
			                                                </td>
			                                                <td class="col-xs-6"><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Promotional_Discounts" runat="server" Text="0.0"></asp:Label></td>
			                                            </tr>
			                                            <tr>
			                                                <td class="col-xs-6">
			                                                    <strong>Grand Total</strong>
			                                                </td>
			                                                <td class="col-xs-6"><i class="fa fa-rupee"></i>&nbsp;<asp:Label ID="Grand_Total" runat="server" Text="0.0"></asp:Label></td>
			                                            </tr>
			                                                <tr>
			                                                    <td class="col-xs-6" colspan="2">
			                                                        <asp:Label ID="Check_Out_Message" runat="server" ForeColor="Red" Visible ="false" Text="Alteast one item should be selected">
			                                                        </asp:Label>
			                                                    </td>
			                                                </tr>
			                                            <tr>
			                                                <td colspan="2" class="btnPart">
			                                                    <asp:LinkButton 
			                                                        ID="CheckOut_Button" 
			                                                        runat="server"
			                                                        CssClass="btn btn-primary pull-right"
			                                                        Text='next<i class="fa fa-arrow-circle-right" aria-hidden="true"></i>'
			                                                        OnClick="Next_Review_Order_Button_Click">                                            
			                                                    </asp:LinkButton>
			                                                </td>
			                                            </tr>
			                                        </tbody>
			                                    </table>
			                                </div>
			                            </div>
			                        </div>
                </div> 
            </div>
        </asp:View>
        <asp:View ID="Payment_View" runat="server">
            <div class="container">
                <div class="row">    
                    <div class="ol-md-12  col-sm-6 col-xs-12">
                        <asp:LinkButton 
                            id="Previous_Payment_View_Button"
                            runat="server"
                            Text='<i class="fa fa-arrow-circle-left" aria-hidden="true"></i>previous' 
                            CssClass="btn btn-primary"
                            OnClick="Previous_Payment_View_Button_Click"></asp:LinkButton>
                    </div>  
                    
                    <div class="col-md-12  col-sm-6 col-xs-12">                    
                        <div class="panel panel-default formPanel">
                            <div class="panel-heading bg-color-4 border-color-4">
                                <h3 class="panel-title">How would you like to pay?</h3>                            
                            </div>
                            <div class="panel-body">      
                                <div class="ol-md-12  col-sm-12 col-xs-12" style="text-align:center">
                                        <asp:LinkButton ID="Cash_On_Delivery_Option" runat="server" Text="Cash On Delivery" CssClass="btn btn-primary" OnClick="Cash_On_Delivery_Option_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="Other_Payment_Types_Option" runat="server" Text="Other Payment Types" CssClass="btn btn-primary" OnClick="Other_Payment_Types_Option_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>        
</section>
</asp:Content>
