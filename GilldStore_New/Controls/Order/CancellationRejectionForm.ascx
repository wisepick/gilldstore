<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancellationRejectionForm.ascx.cs" Inherits="GilldStore_New.Controls.Order.CancellationRejectionForm" %>
<asp:HiddenField ID="Order_Id" runat="server" />
<asp:HiddenField ID="Order_Number" runat="server" />
<asp:HiddenField ID="Customer_Id" runat="server" />
<tr>
    <td colspan="4"">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Reject Refund</strong></div>
            <div class="panel-body">
                <table style="width:100%" class="table table-bordered">
                    <tr>
                        <th>Reason for Rejection</th>
                        <td>
                            <asp:DropDownList ID="Rejection_Reason_Id" runat="server" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" AppendDataBoundItems="true" CssClass="form-control">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>If Others</th>
                        <td>
                            <asp:TextBox ID="Rejection_Reason" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
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