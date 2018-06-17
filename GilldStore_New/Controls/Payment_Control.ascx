<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Payment_Control.ascx.cs" Inherits="GilldStore_New.Controls.Payment_Control" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
        <table width="100%" class="table table-bordered">
            <tr>
                <th>
                    Cheque Date
                </th>
                <td>               
                    <asp:TextBox ID="Cheque_Date" runat="server" ReadOnly="true"></asp:TextBox>     
                        <obout:Calendar ID="Calendar1" runat="server" DatePickerMode="true" TextBoxId="Cheque_Date" DatePickerSynchronize="true"></obout:Calendar>
                </td>
                <th>
                    Cheque Number
                </th>
                <td>
                    <asp:TextBox ID="Cheque_Number" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>                    
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="Cheque_Number"></cc1:FilteredTextBoxExtender>
                </td>                
            </tr>
            <tr>
                <th>MICR Code</th>
                <td>
                    <asp:TextBox ID="MICR_Code" runat="server" CssClass="form-control" MaxLength="9" OnTextChanged="MICR_Code_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="MICR_Code"></cc1:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <th>
                    Bank Name
                </th>             
                <td>
                    <asp:TextBox ID="Bank_Name" runat="server" CssClass="form-control"> 
                    </asp:TextBox>
                </td>   
                <th>
                    Branch Name
                </th>
                <td>
                    <asp:TextBox ID="Branch_Name" runat="server" CssClass="form-control">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
    