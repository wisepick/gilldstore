<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerControl.ascx.cs" Inherits="GilldStore_New.Controls.CustomerControl" %>
<div class="panel panel-primary">
        <div class="panel-heading">Customer Details</div>
        <div class="panel-body">
            <asp:FormView ID="Customer_FormView" runat="server"                 
                Width="100%" OnModeChanging="Customer_FormView_ModeChanging" DataKeyNames="USER_ID, USER_TYPE, DISCOUNT_MEASUREMENT_ID, EMAIL_ADDRESS, USER_NAME">
                <InsertItemTemplate>
                    <table class="table table-bordered" width="100%">
                        <tr>
                            <th>
                                Name
                            </th>
                            <td>
                                <asp:TextBox id="User_Name" runat="Server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ID="User_Name_Validation" runat="server" ControlToValidate="User_Name" ErrorMessage="Name is Mandatory" Display="Dynamic" ValidationGroup="Manage_Customer_VG" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Email Address
                            </th>
                            <td>
                                <asp:TextBox id="Email_Address" runat="Server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Mobile Number
                            </th>
                            <td>
                                <asp:TextBox id="Mobile_Number" runat="Server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Disount
                            </th>
                            <td>
                                <asp:TextBox ID="Discount" runat="server" MaxLength="2" CssClass="form-control"></asp:TextBox>
                                <asp:RadioButtonList ID="Discount_Measurement_Id" runat="server">
                                    <asp:ListItem Text="In Amount" Value ="1"></asp:ListItem>
                                    <asp:ListItem Text="In Percentage" Value ="2"></asp:ListItem>
                                </asp:RadioButtonList>

                             </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ID="Mobile_Number_Validation" runat="server" ControlToValidate="Mobile_Number" ErrorMessage="Mobile Number is Mandatory" Display="Dynamic" ValidationGroup="Manage_Customer_VG" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">                                                    
                                <asp:Button ID="Submit_Button" runat="server" Text="Save" CssClass="btn btn-primary"   CausesValidation="true" ValidationGroup="Manage_Customer_VG" OnCommand="Submit_Button_Command" CommandName="Add_New"/>
                            </td>
                        </tr>                        
                    </table>
                </InsertItemTemplate>                
                <EditItemTemplate>
                    <table class="table table-bordered" width="100%">
                        <tr>
                            <th>
                                Name
                            </th>
                            <td>
                                <asp:TextBox id="User_Name" runat="Server" MaxLength="255" CssClass="form-control" Text='<%# Eval("USER_NAME") %>'></asp:TextBox>
                            </td>
                            <th>
                                Email Address
                            </th>
                            <td>
                                <asp:TextBox id="Email_Address" runat="Server" MaxLength="255" CssClass="form-control" Text='<%# Eval("EMAIL_ADDRESS") %>'></asp:TextBox>
                            </td>                            
                            <th>
                                Mobile Number
                            </th>
                            <td>
                                <asp:TextBox id="Mobile_Number" runat="Server" MaxLength="10" CssClass="form-control" Text='<%# Eval("MOBILE_NUMBER") %>'></asp:TextBox>
                            </td>                        
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ID="User_Name_Validation" runat="server" ControlToValidate="User_Name" ErrorMessage="Name is Mandatory" Display="Dynamic" ValidationGroup="Manage_Customer_VG" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:RequiredFieldValidator ID="Mobile_Number_Validation" runat="server" ControlToValidate="Mobile_Number" ErrorMessage="Mobile Number is Mandatory" Display="Dynamic" ValidationGroup="Manage_Customer_VG" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        <tr>
                            <th>
                                Disount
                            </th>
                            <td>
                                <asp:TextBox ID="Discount" runat="server" MaxLength="2" CssClass="form-control" Text='<%# Eval("DISCOUNT") %>'></asp:TextBox>
                                <asp:RadioButtonList ID="Discount_Measurement_Id" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="In Amount" Value ="1"></asp:ListItem>
                                    <asp:ListItem Text="In Percentage" Value ="2"></asp:ListItem>
                                </asp:RadioButtonList>

                             </td>
                            <td colspan="4"> 
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">                                                    
                                <asp:Button ID="Submit_Button1" runat="server" Text="Save" CssClass="btn btn-primary"   CausesValidation="true" ValidationGroup="Manage_Customer_VG" OnCommand="Submit_Button_Command" CommandName="Update_Customer"/>
                                <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary"   CausesValidation="false" ValidationGroup="Manage_Customer_VG" OnCommand="Submit_Button_Command" CommandName="Cancel_Form"/>
                            </td>
                        </tr>                        
                    </table>
                </EditItemTemplate>
                <ItemTemplate>
                    <table class="table table-bordered" width="100%">
                        <tr>
                            <td colspan="6" align="right">
                                <asp:ImageButton ID="Edit_Button" runat="server" AlternateText="Edit" ToolTip="Edit" ImageUrl="~/img/EDIT.png" CommandName="Edit"/>
                            </td>
                        </tr>
                        <tr>
                            <th>Customer Name</th>
                            <td><%# Eval("USER_NAME") %></td>
                            <th>Email Address</th>
                            <td><%# Eval("EMAIL_ADDRESS") %></td>
                            <th>Mobile Number</th>
                            <td><%# Eval("MOBILE_NUMBER") %></td>
                        </tr>                        
                        <tr>                            
                            <th>Discount</th>
                            <td><%# Eval("DISCOUNT") + " " + Eval("DISCOUNT_MEASUREMENT") %></td>                        
                            <th>User Type</th>
                            <td colspan="3"><%# Eval("USER_TYPE") %></td>
                        </tr>         
                        <tr>
                            <th>Amount Due</th>
                            <td>₹ <asp:Label id="Due_Amount" runat="server" Text='<%# Eval("AMOUNT_DUE") %>'>

                                  </asp:Label></td>
                            <td colspan="2">
                                <asp:Button ID="Allocate_Payment_Button" runat="server" Text="Allocate Pending Payments" OnClick="Allocate_Payment_Button_Click" />
                                <asp:Button ID="Add_To_MailChimp_Button" runat="server" Text="Add To Mail Chimp" OnClick="Add_To_MailChimp_Button_Click" />
                            </td>
                        </tr>               
                    </table>
                </ItemTemplate>
            </asp:FormView>
        </div>
    </div>
