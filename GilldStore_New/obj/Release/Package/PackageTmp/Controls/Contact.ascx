<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contact.ascx.cs" Inherits="GilldStore_New.Controls.Contact" %>
    <div class="col-md-12  col-sm-6 col-xs-12">                    
        <div class="panel panel-default formPanel">
            <div class="panel-heading bg-color-4 border-color-4">
                <h3 class="panel-title">Do you have some kind of problem with our products?</h3>                            
            </div>
            <div class="panel-body">
                <div class="col-sm-6 col-xs-12">   
                    <div class="form-group formField">
                        <asp:TextBox ID="User_Name" runat="Server" CssClass="form-control border-color-2" placeHolder ="Your Name"></asp:TextBox>                
                        <asp:RequiredFieldValidator ID="User_Name_RequiredValidation" runat="server" ErrorMessage="Name is Mandatory" ValidationGroup="VG1" Display="Dynamic" ControlToValidate="User_Name" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">   
                    <div class="form-group formField">
                        <asp:TextBox ID="Email_Address" runat="Server" CssClass="form-control border-color-2" placeHolder ="Your Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="Email_Address_RequiredValidation" runat="server" ErrorMessage="Email is Mandatory" ValidationGroup="VG1" Display="Dynamic" ControlToValidate="Email_Address" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-12 col-xs-12">   
                    <div class="form-group">
                        <asp:TextBox ID="Message" runat="Server" CssClass="form-control border-color-2" placeHolder ="Your Message" TextMode="MultiLine" Rows="10" Columns="100" ></asp:TextBox>                
                        <asp:RequiredFieldValidator ID="Message_RequiredValidation" runat="server" ErrorMessage="Message is Mandatory" ValidationGroup="VG1" Display="Dynamic" ControlToValidate="Message" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">   
                    <div class="form-group formField">
                        <asp:Button ID="Submit_Button" runat="Server" Text="Send Message" CssClass="btn btn-primary" OnCommand="Contact_OnCommand" ValidationGroup="VG1"/>                
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">   
                    <div class="form-group formField">
                        <asp:Label ID="Final_Message" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
