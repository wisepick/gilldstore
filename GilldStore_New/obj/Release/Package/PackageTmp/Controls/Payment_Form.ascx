<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Payment_Form.ascx.cs" Inherits="GilldStore_New.Controls.Payment_Form" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:HiddenField ID="Customer_Id" runat="server" />
<center><h3><asp:Label ID="Payment_Success_message" runat="server" BackColor="Yellow"></asp:Label></h3></center>
    <div class="panel panel-primary">
        <div class="panel-heading"><strong>Payment Information</strong></div>
        <div class="panel-body">
            <table width="100%" class="table table-bordered">
                <tr>
                    <th>
                        Select the Payment Date
                    </th>
                    <td>
                        <asp:TextBox ID="Payment_Date" runat="server" CssClass="form-control"></asp:TextBox>
                        <obout:Calendar ID="Calendar1" runat="server" DateFormat="dd-MM-yyyy" DatePickerMode="true" ShowTimeSelector="false"  TextBoxId="Payment_Date" DatePickerSynchronize="true"></obout:Calendar>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RequiredFieldValidator ID="Payment_Date_Required" runat="server" ErrorMessage="Payment Date is Mandatory" Display="Dynamic" ControlToValidate="Payment_Date" ValidationGroup ="Payment_Validation" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        Select the Payment Type
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
                <tr>
                    <td colspan="2">
                        <asp:RequiredFieldValidator ID="Payment_Type_Validation" runat="server" ErrorMessage="Payment type is mandatory" Display="Dynamic" ControlToValidate="Payment_Type" ValidationGroup ="Payment_Validation" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>Payment Amount</th>
                    <td>
                        <asp:TextBox ID="Payment_Amount" runat="server" CssClass="form-control">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RequiredFieldValidator ID="Payment_Amount_Validation" runat="server" ErrorMessage="Payment amount is mandatory" Display="Dynamic" ControlToValidate="Payment_Amount" ValidationGroup ="Payment_Validation" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="Save_Payment_Button" runat="server" Text="Save" OnClick="Save_Payment_Button_Click" CssClass="btn btn-primary" ValidationGroup="Payment_Validation" CausesValidation="true"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>