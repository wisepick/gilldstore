<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="GilldStore_New.Account.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="User_FormView" runat="server" DefaultMode="Edit" Width="75%" DataKeyNames="USER_ID, EXTERNAL_USER_ID, MOBILE_NUMBER, MOBILE_VALIDATED, EMAIL_ADDRESS_VALIDATED" >
        <EditItemTemplate>
            <!-- MAIN SECTION -->
            <section class="mainContent  clearfix">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12 col-md-offset-2 col-sm-6 col-xs-12">
                            <div class="panel panel-default formPanel">
                                <div class="panel-heading bg-color-4 border-color-4">
                                    <h3 class="panel-title">Profile</h3>
                                </div>
                                <div class="panel-body">   
                                    <div class="col-sm-12 col-xs-12">             
                                        <div class="form-group formField">
                                            <asp:Label ID="Mobile_Email_Validation" runat="server" CssClass="form-control border-color-2" Text="Mobile Number & Email are mandatory and must be verified to process any orders" ForeColor="Red" Font-Size="Large" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("MOBILE_VALIDATED").ToString(), "0") ||  GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("EMAIL_ADDRESS_VALIDATED").ToString(), "0")%>'></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12">             
                                        <div class="form-group formField">
                                            <asp:RequiredFieldValidator ID="User_Name_Validation" runat="server" Text="User Name is mandatory" Display="Dynamic"  ControlToValidate="User_Name" ForeColor="Red">
                                            </asp:RequiredFieldValidator>
                                            <label for="User_Name">Name</label>
                                            <asp:TextBox id="User_Name" runat="Server" MaxLength="255" CssClass="form-control border-color-2" Text='<%# Eval("USER_NAME") %>'></asp:TextBox>
                                        </div>                  
                                    </div>
                                    <div class="col-sm-6 col-xs-12">             
                                        <div class="form-group formField">
                                            <label for="Mobile_Number">Mobile Number</label>
                                            <asp:TextBox id="Mobile_Number" runat="Server" MaxLength="10" CssClass="form-control border-color-2" Text='<%# Eval("MOBILE_NUMBER") %>' OnTextChanged="Contact_TextChanged" AutoPostBack="true"></asp:TextBox>                     
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12">             
                                        <div class="form-group formField">
                                            <asp:Label ID="OTP_Message" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' ForeColor="Red" CssClass="alert-info"></asp:Label>
                                        </div>
                                    </div>
                                    <asp:PlaceHolder ID="Mobile_PlaceHolder" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) && Display_Validation(Eval("MOBILE_NUMBER").ToString()) %>'>
                                        <asp:Repeater ID="Mobile_Number_Repeater" runat="server">
                                            <ItemTemplate>                                                                                                                                        
                                                <div class="col-sm-6  col-xs-12">             
                                                    <div class="form-group formField">
                                                        <label for="Mobile_OTP"><asp:Label ID="OTP_Label" runat="server" Text="Enter the OTP" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>'></asp:Label></label>
                                                        <asp:TextBox ID="Mobile_OTP" runat="server" CssClass="form-control border-color-2" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>'></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-offset-6 col-xs-6">             
                                                    <div class="form-group formField">
                                                        <asp:Button ID="Validate_OTP_Button" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' Text="Validate OTP" CssClass="btn btn-primary btn-block bg-color-3 border-color-3"  CommandName="Validate OTP" OnCommand="Validate_OTP_Button_Command"/>
                                                        <asp:Button ID="Resend_OTP_Button" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' Text="Resend OTP" CssClass="btn btn-primary btn-block bg-color-3 border-color-3"  CommandName="Regenerate OTP" OnCommand="Resend_OTP_Button_Command"/>                                    
                                                    </div>                                
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:PlaceHolder>
                                    <div class="col-sm-6 col-md-offset-6 col-xs-6">             
                                        <div class="form-group formField">
                                            <asp:CheckBox ID="Mobile_Delivery_Option" runat="server" Text="Want to receive order information through mobile" Checked='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("MOBILE_DELIVERY_OPTION").ToString(), "1") %>' CssClass="checkbox-inline"/>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-6">             
                                        <div class="form-group formField">
                                            <label for="Email_Address">Email Address</label>
                                            <asp:TextBox id="Email_Address" runat="Server" MaxLength="255" CssClass="form-control border-color-2" Text='<%# Eval("EMAIL_ADDRESS") %>' AutoPostBack="true" OnTextChanged="Contact_TextChanged"></asp:TextBox>                                                                                                                                                            
                                            <asp:CheckBox ID="Email_Delivery_Option" runat="server" Text="Want to receive order information through Email" Checked='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("EMAIL_DELIVERY_OPTION").ToString(), "1") %>' CssClass="checkbox-inline"/>
                                            <asp:RequiredFieldValidator ID="Email_Address_Validation" runat="server" ErrorMessage="Email Address is mandatory" Display="Dynamic"  ControlToValidate="Email_Address" ForeColor="Red" >
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-6">             
                                        <div class="form-group formField">
                                            <label><asp:Label id="Email_Validation_Message" runat="server" ForeColor="Red" Visible='<%# Display_Validation(Eval("EMAIL_ADDRESS_VALIDATED").ToString()) %>'></asp:Label></label>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-offset-6 col-xs-6">             
                                        <div class="form-group formField">
                                            <asp:Button ID="Submit_Button" runat="server" Text="Save" CssClass="btn btn-primary btn-block bg-color-3 border-color-3"   CausesValidation="true" OnCommand="Update_User_Details"/>
                                        </div>               
                                    </div>   
                                </div>
                            </div>
                        </div>         
                    </div>
                </div>
            </section>           
        </EditItemTemplate>
    </asp:FormView>
</asp:Content>
