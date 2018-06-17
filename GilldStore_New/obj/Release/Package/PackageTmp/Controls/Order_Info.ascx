<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_Info.ascx.cs" Inherits="GilldStore_New.Controls.Order_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Order/PreCancellationForm.ascx" TagName="PreCancellationForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order/ShippingInformationForm.ascx" TagName="ShippingInformationForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order/CancellationApprovalForm.ascx" TagName="CancellationApprovalForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order/CancellationRejectionForm.ascx" TagName="CancellationRejectionForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order/OrderDeliveryForm.ascx" TagName="OrderDeliveryForm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order/UnDeliveredForm.ascx" TagName="UnDeliveredForm" TagPrefix="uc1" %>

<script type="text/javascript">
    function openWin(lOrderId) {
        window.open('Invoice.aspx?oid=' + lOrderId, 'Invoice', 'width=' + screen.width, 'height=' + screen.height);
    }
</script>
<asp:PlaceHolder ID="Thank_You_Message_PlaceHolder" runat="server" Visible="false">
    <p style="text-align:center">
        <strong>
            Your Order has been successfully Registered. We will sent you the delivery details soon.<br /><br />
            Thank you for shopping with us.<br /><br /> 
        </strong>
    </p>
</asp:PlaceHolder>

<div class="panel panel-primary">
    <div class="panel-heading"><strong>Order Summary</strong></div>
        <div class="panel-body">        
            <asp:FormView 
                ID="Order_Summary_FormView" 
                runat="server" 
                DataKeyNames="ORDER_ID, USER_ID, STATUS, STAFF_FLAG, ORDER_TOTAL, SHIPPING_CHARGE, ORDER_NUMBER, ORDER_DATE" 
                Width="100%">
                <HeaderTemplate>                
                    <table style="width:80%" class="table table-condensed">                                              
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <th>
                            Order Number
                        </th>
                        <td style="vertical-align:top">                            
                            <%# Eval("ORDER_NUMBER") %>
                        </td>
                        <th>
                            Order Date
                        </th>
                        <td style="vertical-align:top">
                            <%# Eval("ORDER_DATE") %>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Subtotal
                        </th>
                        <td>
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp; <%# double.Parse(Eval("ORDER_TOTAL").ToString()) + double.Parse(Eval("DISCOUNTS").ToString()) - double.Parse(Eval("SHIPPING_CHARGE").ToString()) %>
                        </td>
                        <th>
                            Shipping Charge
                        </th>
                        <td style="vertical-align:top">
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp; <%# Eval("SHIPPING_CHARGE") %>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Discounts
                        </th>
                        <td style="vertical-align:top">
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;  <%# Eval("DISCOUNTS") %>
                        </td>
                        <th>
                            Order Total
                        </th>
                        <td style="vertical-align:top">
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;  <%# Eval("ORDER_TOTAL") %>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Due Amount
                        </th>
                        <td>
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp; <%# Eval("ORDER_DUE") %>
                        </td>
                        <th>
                            Payment Option
                        </th>
                        <td>
                            <%# Eval("PAYMENT_TYPE") %>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Order Status
                        </th>
                        <td style="vertical-align:top" colspan="3">                            
                            <i class='fa <%# Eval("STATUS_ICON") %>'></i>&nbsp;<%# Eval("STATUS") %>                            
                            <asp:Label ID="Cancellation_Reason_Label" runat="server" Text='<%# "<br> Cancellation Reason: " + Eval("CANCELLATION_REASON").ToString() %>' Visible='<%# !GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("CANCELLATION_REASON").ToString(), "") %>' ForeColor="Red">
                            </asp:Label>
                            <asp:Label ID="Rejection_Reason_Label" runat="server" Text='<%# "<br> Refund Rejection Reason: " + Eval("REJECTION_REASON").ToString() %>' Visible='<%# !GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("REJECTION_REASON").ToString(), "") %>' ForeColor="Red">
                            </asp:Label>
                            <asp:Label id="Failure_Reason" runat="server" Text='<%# "<br> Failure Reason: " + Eval("FAILURE_REASON").ToString() %>' Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Payment Failed") %>' ForeColor="Red">

                            </asp:Label> 
                        </td>                               
                    </tr>
                    <tr>
                        <th>
                            Delivery Address
                        </th>
                        <td style="vertical-align:top" colspan="3">                            
                            <%# GilldStore_New.App_Start.CommonClass.Format_Address(GilldStore_New.App_Start.CommonClass.Get_Address_By_Id(Eval("ADDRESS_ID").ToString()))%><br />                            
                            <asp:Label ID="Email_Address_Label" runat="server" Text='<%# Eval("EMAIL_ADDRESS") %>' Visible='<%# Request.ServerVariables["URL"].Contains("orders.aspx")%>' ></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4">                            
                            <asp:Button ID="Show_PreCancellationForm_Button" runat="server" Text="Cancel Order" CssClass="btn btn-primary" OnClick="Show_PreCancellationForm" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Order Accepted")  %>'/>                        
                            <asp:Button ID="Show_ShippingInformationForm_Button" runat="server" Text="Ship Order" CssClass="btn btn-primary" OnClick="Show_ShippingInformationForm" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Order Accepted") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Show_CancellationApprovalForm_Button" runat="server" Text="Approve Refund" CssClass="btn btn-primary" OnClick="Show_CancellationApprovalForm" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Waiting for Refund Approval") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>                            
                            <asp:Button ID="Show_CancellationRejectionForm_Button" runat="server" Text="Reject Refund" CssClass="btn btn-primary" OnClick="Show_CancellationRejectionForm" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Waiting for Refund Approval") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Show_OrderDeliveryForm_Button" runat="server" Text="Confirm Delivery" CssClass="btn btn-primary" OnClick="Show_OrderDeliveryForm"  Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && (GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Shipped") || GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Order Accepted") ) && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Show_UndeliveredForm_Button" runat="server" Text="Delivery Returned" CssClass="btn btn-primary" OnClick ="Show_UndeliveredForm" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Shipped") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Show_ReturnedOrderForm_Button" runat="server" Text="Return Complete Order" CssClass="btn btn-primary"  Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Shipped") && !Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Button1" runat="server" Text="Return Partial Order" CssClass="btn btn-primary" OnClick="Return_Order_Button_Click" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Shipped") && !Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Void_Order_Button" runat="server" Text="Void Order" CssClass="btn btn-primary" OnClick="Void_Order_Button_Click" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && (GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Open") || GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Payment Failed")) && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Failed_Mail" runat="server" Text="Send Email" CssClass="btn btn-primary" OnClick="Failed_Mail_Click" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && (GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Open") || GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Payment Failed")) && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Print_Button" runat="server" Text="Print Invoice" CssClass="btn btn-primary" OnClientClick='<%# String.Format("javascript:return openWin({0})", Eval("ORDER_ID")) %>' Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Order Accepted") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Resend_To_Seller" runat="server" Text="Resend Mail to Seller" CssClass="btn btn-primary" OnClick="Resend_To_Seller_Click" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Order Accepted") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <asp:Button ID="Confirm_Button" runat="server" Text="Confirm Order" CssClass="btn btn-primary" OnClick="Confirm_Button_Click" Visible ='<%# Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>
                            <%--<asp:Button ID="Cancel_Delivery_Button" runat="server" Text="Cancel Delivery" CssClass="btn btn-primary" OnClick="Reject_Refund_Button_Click" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STAFF_FLAG").ToString(), "1") && GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("STATUS").ToString(), "Shipped") && Request.ServerVariables["URL"].Contains("orders.aspx")%>'/>--%>
                            <asp:Label ID="Message" runat="server"></asp:Label><br /><br />
        <%--                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure want to Cancel?" TargetControlID="Cancel_Button"></cc1:ConfirmButtonExtender>--%>
                        </td>
                    </tr>

                    <uc1:PreCancellationForm 
                        id="PreCancellationForm1" 
                        runat="server" 
                        visible="false" 
                        OnCancellationSuccess="Refresh_Order_Info"
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'
                        CustomerId = '<%# Eval("USER_ID") %>'
                        OrderAmount = '<%# Eval("ORDER_TOTAL") %>'                        
                        UserType = '' />

                    <uc1:ShippingInformationForm
                        id="ShippingInformationForm1"
                        runat="server"
                        Visible ="false"
                        OnShippingSuccess="Refresh_Order_Info"
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'
                        CustomerId = '<%# Eval("USER_ID") %>' />

                     <uc1:CancellationApprovalForm
                        id="CancellationApprovalForm1"
                        runat="server"
                        Visible ="false"
                        OnApprovalSuccess="Refresh_Order_Info"
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'
                        OrderAmount = '<%# Eval("ORDER_TOTAL") %>'
                        ShippingCharge = '<%# Eval("SHIPPING_CHARGE") %>'
                        CustomerId = '<%# Eval("USER_ID") %>' />
               
                    <uc1:CancellationRejectionForm
                        id="CancellationRejectionForm1"
                        runat="server"
                        Visible ="false"
                        OnRejectionSuccess="Refresh_Order_Info"
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'                        
                        CustomerId = '<%# Eval("USER_ID") %>' />                    

                    <uc1:OrderDeliveryForm
                        id="OrderDeliveryForm1"
                        runat="server"
                        Visible ="false"
                        OnDeliverySuccess="Refresh_Order_Info"
                        OrderAmount = '<%# Eval("ORDER_TOTAL") %>'
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'                        
                        CustomerId = '<%# Eval("USER_ID") %>' 
                        PaymentRef = '<%# Eval("TXN_REFERENCE") %>'
                        />                    

                    <uc1:UnDeliveredForm
                        id="UnDeliveredForm1"
                        runat="server"
                        Visible ="false"
                        OnUnDelivered="Refresh_Order_Info"
                        OrderId = '<%# Eval("ORDER_ID") %>'
                        OrderNumber = '<%# Eval("ORDER_NUMBER") %>'
                        CustomerId = '<%# Eval("USER_ID") %>'
                        OrderAmount = '<%# Eval("ORDER_TOTAL") %>'
                        />                    
                 </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:FormView>                      
        </div>
    </div>  

    <div class="panel panel-primary">
        <div class="panel-heading"><strong>Payment Information</strong></div>            
        <div class="panel-body">
            <asp:Repeater ID="Payment_Repeater" runat="server">
                <HeaderTemplate>
                    <table style="width:80%" class="table table-condensed">
                        <tr>
                            <th>
                                Payment Date
                            </th>
                            <th>
                                Payment Type
                            </th>
                            <th>
                                Payment Status
                            </th>
                            <th>
                                Total Payment
                            </th>
                            <th>
                                This Order Payment
                            </th>                                                            
                        </tr>                      
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("PAYMENT_DATE") %>
                        </td>
                        <td>
                            <%# Eval("PAYMENT_TYPE") %>
                        </td>
                        <td>
                            <%# Eval("PAYMENT_STATUS") %>                              
                        </td>
                        <td>
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;<%# Eval("TOTAL_PAYMENT") %>
                        </td>
                        <td>
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;<%# Eval("ORDER_PAYMENT") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:PlaceHolder ID="Shipping_Info" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Shipping Information</strong></div>            
            <div class="panel-body">
                <asp:Repeater ID="Shipping_Repeater" runat="server">
                    <HeaderTemplate>
                        <table style="width:80%" class="table table-condensed">
                            <tr>
                                <th>
                                    Shipping Date
                                </th>
                                <th>
                                    Agency Name
                                </th>
                                <th>
                                    Reference Number
                                </th>
                            </tr>                      
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("SHIPPING_DATE") %>
                            </td>
                            <td>
                                <%# Eval("AGENCY_NAME") %>
                            </td>
                            <td>
                                <asp:HyperLink ID="hyperlink1" runat="server" NavigateUrl='<%# "http://www.tpcindia.com/Tracking2014.aspx?id=" + Eval("SHIPPING_REFERENCE").ToString() + "&type=0&service=0" %>' Text='<%# Eval("SHIPPING_REFERENCE") %>' Visible='<%# !GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("SHIPPING_REFERENCE").ToString(), "") %>' Target="_blank">

                                </asp:HyperLink>
                              
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="Refund_Info_PlaceHolder" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Refund Information</strong></div>            
            <div class="panel-body">
                <asp:Repeater ID="Refund_Repeater" runat="server">
                    <HeaderTemplate>
                        <table style="width:80%" class="table table-condensed">
                            <tr>
                                <th>
                                    Refund Reference
                                </th>
                                <th>
                                    Refund Date
                                </th>
                                <th>
                                    Refund Amount
                                </th>
                                <th>
                                    Refund Status
                                </th>
                            </tr>                      
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("REFUND_ID") %>
                            </td>
                            <td>
                                <%# Eval("REFUND_DATE") %>
                            </td>
                            <td>
                                <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp;<%# Eval("REFUND_AMOUNT") %>
                            </td>
                            <td>
                                <%# Eval("REFUND_STATUS") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:PlaceHolder>
    
    <div class="panel panel-primary">
        <div class="panel-heading"><strong>Details</strong></div>
        <div class="panel-body">
            <asp:Repeater ID="Order_Detail_Summary" runat="server">
                <HeaderTemplate>
                    <table style="width:80%" class="table table-condensed">
                        <tr>
                            <th>
                                Product
                            </th>
                            <th>
                                Size
                            </th>
                            <th>
                                Quantity
                            </th>
                            <th>
                                Price
                            </th>                 
                            <th>
                                Subtotal
                            </th>                   
                        </tr>                            
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="vertical-align:top">
                            <%# Eval("PRODUCT_NAME") %>
                        </td>
                        <td style="vertical-align:top">
                            <%# Eval("MEASUREMENT_UNIT") %>
                        </td>
                        <td style="vertical-align:top">
                            <%# Eval("QUANTITY") %>
                        </td>
                        <td style="vertical-align:top">
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp; <%# Eval("PRICE") %>
                        </td>           
                        <td style="vertical-align:top">
                            <i class="fa fa-rupee" aria-hidden="true"></i>&nbsp; <%# Eval("SUBTOTAL") %>
                        </td>                     
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>  
        </div>
    </div>            