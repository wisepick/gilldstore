<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingInformationForm.ascx.cs" Inherits="GilldStore_New.Controls.Order.ShippingInformationForm" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<asp:HiddenField ID="Order_Id" runat="server" />
<asp:HiddenField ID="Order_Number" runat="server" />
<asp:HiddenField ID="Customer_Id" runat="server" />
<tr>
    <td colspan="4"">
        <div class="panel panel-primary">
            <div class="panel-heading"><strong>Shipping Information</strong></div>
            <div class="panel-body">
                <table style="width:100%" class="table table-bordered">
                    <tr>
                        <th>Shipping Date</th>
                        <td><asp:TextBox ID="Shipping_Date" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            <obout:Calendar ID="Calendar1" runat="server" DateFormat="dd-MM-yyyy hh:mm:ss" DatePickerMode="true" ShowTimeSelector="true" TimeSelectorType="DropDownList" TextBoxId="Shipping_Date" DatePickerSynchronize="true"></obout:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <th>Agency Name</th>
                        <td>
                            <asp:DropDownList 
                                ID="Courier_Agency_Id" 
                                runat="server" 
                                DataTextField="ATTRIBUTE_NAME" 
                                DataValueField="ATTRIBUTE_ID" 
                                AppendDataBoundItems="true"
                                CssClass="form-control">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>                                                    
                        </td>
                    </tr>
                    <tr>
                        <th>Reference Number</th>
                        <td><asp:TextBox ID="Reference_Number" runat="server" CssClass="form-control"></asp:TextBox></td>
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