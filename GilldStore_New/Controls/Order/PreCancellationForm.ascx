<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreCancellationForm.ascx.cs" Inherits="GilldStore_New.Controls.Order.PreCancellationForm" %>
<asp:HiddenField ID="Order_Id" runat="server" />
<asp:HiddenField ID="Order_Number" runat="server" />
<asp:HiddenField ID="Customer_Id" runat="server" />
<asp:HiddenField ID="Order_Amount" runat="server" />
<asp:HiddenField ID="Transaction_Id" runat="server" />
<asp:HiddenField ID="User_Type" runat="server" />
<tr>
    <td colspan="4">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <strong>Cancellation Details</strong>
            </div>
            <div class="panel-body">
                <table style="width:100%" class="table table-bordered">
                    <tr>
                        <th>
                            Cancellation Reason
                        </th>
                        <td>
                            <asp:DropDownList ID="Cancellation_Reason_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" CssClass="form-control">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>                                                    
                        </td>                                                
                    </tr>
                    <tr>
                        <th>
                            If Others
                        </th>
                        <td>
                            <asp:TextBox ID="Cancellation_Reason" runat="server" MaxLength="255">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Save_Button" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="Save_Button_Click" />&nbsp;
                            <asp:Button ID="Cancel_Button" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Button_Click" />
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