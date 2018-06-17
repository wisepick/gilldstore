<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_Form_Control.ascx.cs" Inherits="GilldStore_New.Controls.Order_Form_Control" %>
<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Payment_Control.ascx" TagName="PaymentControl" TagPrefix="uc1" %>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="Submit_Button" />
        
    </Triggers>
    <ContentTemplate>
        <asp:HiddenField ID="USER_ID" runat="server" />
<asp:HiddenField ID="USER_TYPE_ID" runat="server" />
<asp:HiddenField ID="DISCOUNT" runat="server"/>
<asp:HiddenField ID="DISCOUNT_MEASUREMENT_ID" runat="server"/>
<asp:HiddenField ID="PAYMENT_TYPE_ID" runat="server"/>
<asp:HiddenField ID="ORDER_STATUS" runat="server" />
        <asp:HiddenField ID="GLOBAL_DISCOUNT_APPLIED" runat="server" />
        <asp:HiddenField ID="ORDER_ID" runat="server" />
        <asp:HiddenField ID="SHIPPING_CHARGE_DISCOUNT_FLAG" runat="server" Value="N"/>
        <div class="container">            
      
            <asp:PlaceHolder ID="Discounted_Quantity_PlaceHolder" runat="server" Visible="false">
                <div class="alert alert-info" role="alert">
                <center>Order for <asp:Label ID="Discounted_Quantity" runat="server"></asp:Label> litre of any oil and get shipping charges FREE</center>
                    </div>
            </asp:PlaceHolder>
                
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Order_Form_view" runat="server">
                    <div class="panel panel-primary">
                        <div class="panel-heading"><strong>Order Form</strong></div>
                        <div class="panel-body">
                            <asp:ListView ID="Product_ListView" runat="Server" DataKeyNames="PRODUCT_ID" OnItemDataBound="Product_ListView_OnItemDataBound">
                                <LayoutTemplate>
                                    <table width="100%" class="table table-bordered">						                
							            <tr>
							                <th>Product</th>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <th width="20%">Size</th>
							                            <th width="20%">Unit Cost</th>                                                        
							                            <th width="30%">Quantity</th>
							                            <th width="30%" class="text-right">Total</th>
                                                    </tr>
                                                </table>
                                            </td>                                            
							            </tr>
                                        <tr id="itemPlaceholder" runat="server"></tr>                                        
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
							            <td valign="top" width="20%"><strong><%# Eval("PRODUCT_NAME") %></strong></td>
                                        <asp:ListView ID="Product_Order_ListView" runat="server" DataKeyNames="PRODUCT_ID, MEASUREMENT_UNIT,  PRICE, SHIPPING_CHARGE, MEASUREMENT_NAME">
                                            <LayoutTemplate>
                                                <td width="80%">
                                                    <table width="100%">
                                                        <tr id="itemPlaceholder" runat="server"></tr>
                                                    </table>
                                                </td>
                                            </LayoutTemplate>
                                            <ItemTemplate>                                            
                                                <tr>
                                                    <td class="vert-align" width="20%"><%# Eval("MEASUREMENT_UNIT").ToString() + " " + Eval("MEASUREMENT_NAME").ToString() %></td>
                                                    <td class="vert-align" width="20%">₹ <%# Eval("PRICE") %></td>                                                    
							                        <td width="30%">
                                                        <asp:DropDownList 
                                                            ID="Quantity_DropDown" 
                                                            runat="server" 
                                                            CssClass="form-control"
                                                            OnSelectedIndexChanged="Quantity_DropDown_Changed" 
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>  
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>  
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>  
                                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>  
                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>  
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>    
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>                                                        
                                                        </asp:DropDownList>								        
							                        </td>
							                        <td class="text-right vert-align" width="30%">₹ <asp:Label ID="Subtotal" runat="server" Text="0"></asp:Label></td>
							                    </tr>							
                                            </ItemTemplate>
                                        </asp:ListView>
							        </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    
                    <asp:PlaceHolder ID="Store_Place_Holder" runat="server" Visible="false">
                        <div class="panel panel-primary">
                            <div class="panel-heading"><strong>Payment Information</strong></div>
                            <div class="panel-body">
                                <table width="100%" class="table table-bordered">
                                    <tr>
                                        <th>Payment Received ?</th>
                                        <td>
                                            <asp:RadioButtonList ID="Payment_Received_Flag" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Payment_Received_Flag_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="Payment_Received_Information" Visible="false" runat="server">
                                        <tr>
                                            <th>
                                                Received By
                                            </th>
                                            <td>
                                                <asp:RadioButtonList ID="Payment_Type" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Payment_Type_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Cash" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Cheque" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Online Transfer" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Credit Card" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Demand Draft" Value="5"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="Cheque_Payment_Details" runat="server" Visible="false">
                                        <tr>
                                            <td colspan="2">
                                                <uc1:PaymentControl id="PaymentControl1" runat="server"></uc1:PaymentControl>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </table>
                            </div>
                        </div>
                        <div class="panel panel-primary">
                            <div class="panel-heading"><strong>Delivery Information</strong></div>
                            <div class="panel-body">
                                <table width="100%" class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="Delivery_Method" runat="server" RepeatDirection="Horizontal" CssClass="form-control" OnSelectedIndexChanged="Delivery_Method_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Delivered" Value="1">
                                                </asp:ListItem>
                                                <asp:ListItem Text="To be Delivered On Hand" Value="2">
                                                </asp:ListItem>
                                                <asp:ListItem Text="To be Delivered By Courier" Value="3">
                                                </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <uc1:AddressBook 
                            ID="AddressBook1" 
                            runat="server"   
                            OnAddressSelected ="AddressBook1_OnAddressSelected"                                                            
                            />
                    <div class="panel panel-primary">
                        <div class="panel-heading"><strong>Order Summary</strong></div>
                        <div class="panel-body">
                            <table class="table">						
                                  <tr>
							        <td><strong>Subtotal:</strong></td>
							        <td></td>
							        <td class="text-right">
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        ₹ <asp:Label ID="Subtotal_Str" runat="server" Text="0.0"></asp:Label>
							        </td>
						        </tr>
						        <tr>
							        <td><strong>Shipping cost:</strong></td>
							        <td></td>
							        <td class="text-right">
                                        <asp:HiddenField ID="GLOBAL_PROMOTION_ID" runat="server" />
                                        ₹ <asp:Label ID="Shipping_Charges" runat="server" Text="0.0"></asp:Label>
							        </td>
						        </tr>
                                <tr>
							        <td><strong>Shipping Discount:</strong></td>
							        <td></td>
							        <td class="text-right">                                        
                                        ₹ <asp:Label ID="Shipping_Discount" runat="server" Text="0.0"></asp:Label>
							        </td>
						        </tr>
                                <tr>
                                    <td><strong>Promotional Discount</strong></td>
                                    <td></td>
                                    <td class="text-right">
                                        ₹ <asp:Label ID="Promotional_Discounts" runat="server" Text="0.0"></asp:Label>
                                        <asp:Label ID="Discounts" runat="server" Visible ="false"></asp:Label>
							        </td>
                                </tr>
                                <asp:PlaceHolder ID="Surprise_Gift_PlaceHolder" runat="server" Visible="false">
                                    <tr>
                                        <td><strong>Suprise Discount</strong></td>
                                        <td></td>
                                        <td class="text-right">                                            
                                            <asp:Label ID="Surprise_Gift_Percentage" runat="server" Visible="false"></asp:Label>
                                            ₹ <asp:Label ID="Surprise_Gift" runat="server" Text="0"></asp:Label>
							            </td>    
                                    </tr>
                                </asp:PlaceHolder>
						        <tr>
							        <td><strong>Total:</strong></td>
							        <td></td>
							        <td id="total" class="text-right">
                                        ₹ <asp:Label ID="Grand_Total" runat="server" Text="0.0"></asp:Label></td>
						        </tr>						
					        </table>
                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <div class="panel-heading"><strong>Promotion Discount</strong></div>
                        <div class="panel-body">
                            <table class="table">						
						        <tr>
							        <td><strong>Enter Promotion Code:</strong></td>
							        <td></td>
							        <td class="text-right">
                                        <asp:HiddenField ID="Promotion_Id" runat="server" />
                                        <asp:HiddenField ID="Promotion_Amount" runat="server" />
                                        <asp:HiddenField ID="Promotion_Percentage" runat="server" />
                                        <asp:HiddenField ID="Promotion_Maximum" runat="server" />
                                        <asp:TextBox ID="Promotion_Code" runat="server" MaxLength="20" CssClass="form-control" ValidationGroup="PROMOTION_VALIDATION">
                                        </asp:TextBox>
							        </td>
                                    <td>
                                        <asp:Button ID="Apply_Promotion_Button" runat="server" Text="Apply Promotion" CssClass="btn btn-primary" ValidationGroup="PROMOTION_VALIDATION" CausesValidation="true" OnClick="Apply_Promotion_Button_Click"/>
                                    </td>
						        </tr>   
                                <tr>
                                    <asp:Label ID="Promotion_Message" runat="server">
                                    </asp:Label>
                                </tr>                             
					        </table>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="Online_Payment_PlaceHolder" runat="server" Visible="false">
                        <div class="panel panel-primary">
                            <div class="panel-heading"><strong>Payment</strong></div>
                            <div class="panel-body">
                                <table width="100%" class="table table-bordered">
                                    <tr>
                                        <td>
                                             Payment Type
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="Online_Payment_Type" runat="server" DataTextField="PAYMENT_TYPE_NAME" DataValueField="PAYMENT_TYPE_ID"  OnSelectedIndexChanged="Online_Payment_Type_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">                                                
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    
                    <center>
                        <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label><br />
                        <asp:Button ID="Submit_Button" runat="server" Text="Place Order" CssClass="btn btn-primary" OnCommand="Order_Form_OnCommand" CommandName="Place Order" ValidationGroup="VG1"/>
                    </center>			    

                </asp:View>
                <asp:View ID="Order_Summary_View" runat="server">
                    <uc1:OrderInfo id="Order_Info" runat="server" Show_Order_Message="true">
                    </uc1:OrderInfo>
                </asp:View>
            
            </asp:MultiView>            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
    