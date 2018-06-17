<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancellationApprovalForm.ascx.cs" Inherits="GilldStore_New.Controls.Order.CancellationApprovalForm" %>
<asp:HiddenField ID="Order_Id" runat="server" />
<asp:HiddenField ID="Order_Number" runat="server" />
<asp:HiddenField ID="Order_Amount" runat="server" />
<asp:HiddenField ID="Shipping_Charge" runat="server" />
<asp:HiddenField ID="Customer_Id" runat="server" />
<asp:HiddenField ID="Transaction_Id" runat="server" />

<tr>
    <td colspan="4"">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Approval Information</strong></div>
            <div class="panel-body">
                <table style="width:100%" class="table table-bordered">
                    <tr>
                        <th>Percentage of Refund</th>
                        <td><asp:TextBox ID="Refund_Percentage" runat="server" CssClass="form-control" OnTextChanged="Refund_Percentage_TextChanged" AutoPostBack="true" Text="95">

                            </asp:TextBox>                                                    
                        </td>
                    </tr>
                    <tr>
                        <th>Refund Amount</th>
                        <td>
                            <asp:TextBox ID="Refund_Amount" runat="server" Text='<%# (((double.Parse(Eval("ORDER_TOTAL").ToString()) - double.Parse(Eval("SHIPPING_CHARGE").ToString())) * 95))/100 %>'></asp:TextBox>
                        </td>
                    </tr>
                                            
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Save_Button" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="Save_Button_Click" />&nbsp;
                            <asp:Button ID="Canel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Button_Click" />
                        </td>
                    </tr>
                </table>        
            </div>
        </div>
    </td>
</tr>
<tr>
    <td colspan="4">
        &nbsp;
    </td>
</tr>