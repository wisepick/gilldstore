<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderDeliveryForm.ascx.cs" Inherits="GilldStore_New.Controls.Order.OrderDeliveryForm" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:HiddenField ID="Order_Id" runat="server" />
<asp:HiddenField ID="Order_Number" runat="server" />
<asp:HiddenField ID="Customer_Id" runat="server" />
<asp:HiddenField ID="Order_Amount" runat="server" />
<asp:HiddenField ID="Payment_Ref" runat="server" />
<tr>
    <td colspan="4"">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Delivery Information</strong></div>
            <div class="panel-body">
                <table style="width:100%" class="table table-bordered">
                    <tr>
                        <th>Delivery Date</th>
                        <td><asp:TextBox ID="Delivery_Date" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            <obout:Calendar ID="Calendar1" runat="server" DateFormat="dd-MM-yyyy" DatePickerMode="true"  TextBoxId="Delivery_Date" DatePickerSynchronize="true"></obout:Calendar>
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
                            <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Button_Click" />
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