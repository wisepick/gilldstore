<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="GilldStore_New.Account.Invoice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <link rel="stylesheet" href="~/Content/bootstrap.min.css" runat="server">
</head>
<body>
    <table width="90%" align="center" class="table table-bordered">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <th>
                            Tin :
                        </th>
                        <td align="left">
                            33406348085
                        </td>
                    </tr>
                    <tr>
                        <th>
                            CST :
                        </th>
                        <td align="left">
                            1261529
                        </td>

                    </tr>
                </table>
            </td>
            <td>
                <center>
                    <h1>Invoice</h1>
                </center>
            </td>
            <td align="right">
                <table>
                    <tr>
                        <th align="right">Cell :</th>
                        <td align="left">9994543664</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">9994543665</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/navlogo-blue.png" Width="150" />
            </td>
            <td valign="top" align="center" colspan="2">
                <h1>
                    <font color="BLUE">GILLD OIL MILL</font>
                </h1>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <h3>40/22, L.F. Road, Cumbum - 625 516, Theni Dist, Tamil Nadu.</h3>
            </td>
        </tr>
        <tr>
            <td colspan="2">No: <asp:Label ID="Invoice_No" runat="server"></asp:Label></td>
            <td align="right">Date : <asp:Label ID="Invoice_Date" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="3">
                To : <asp:Label ID="User_Name" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <table width="100%" class="table table-bordered">
                                <tr>
                                    <th>
                                        Rate per Unit
                                    </th>
                                    <th>
                                        Particulars
                                    </th>
                                    <th>
                                        Number of Units
                                    </th>
                                    <th>
                                        Amount
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    ₹ <%# Eval("PRICE") %>
                                </td>
                                <td>
                                    <%# Eval("PRODUCT_NAME") %> (<%# Eval("MEASUREMENT_UNIT") %> litre)
                                </td>
                                <td>
                                    <%# Eval("QUANTITY") %>
                                </td>
                                <td align="right">
                                    ₹ <%# Eval("SUBTOTAL") %>
                                </td>
                            </tr>
                        </ItemTemplate>                                                
                    </asp:Repeater>    
                    <tr>
                        <td colspan="3" align="right">
                            Subtotal
                        </td>
                        <td align="right">
                            ₹ <asp:Label ID="Subtotal" runat="server"></asp:Label>
                        </td>
                    </tr>
                <tr>
                        <td colspan="3" align="right">
                            + VAT @5%
                        </td>
                        <td align="right">
                            ₹ <asp:Label ID="VAT" runat="server"></asp:Label>
                        </td>
                    </tr>

                <tr>
                        <td colspan="3" align="right">
                            + Shipping Charges
                        </td>
                        <td align="right">
                            ₹ <asp:Label ID="Shipping_charges" runat="server"></asp:Label>
                        </td>
                    </tr>
                <tr>
                        <td colspan="3" align="right">
                            - Discounts
                        </td>
                        <td align="right">
                            ₹ <asp:Label ID="Discounts" runat="server"></asp:Label>
                        </td>
                    </tr>
                <tr>
                        <td colspan="3" align="right">
                            Grand Total
                        </td>
                        <td align="right">
                            ₹ <asp:Label ID="Grand_Total" runat="server"></asp:Label>
                        </td>
                    </tr>
                <tr>
        <td colspan="4" align="center">
            <h3>Thank you for shopping with us</h3>  
            <h4>Its your Regular Cooking Oil - But Better !</h4>          
        </td>
        </tr>
                </table>            
            </td>
        </tr>
    

    </table>
</body>
</html>
