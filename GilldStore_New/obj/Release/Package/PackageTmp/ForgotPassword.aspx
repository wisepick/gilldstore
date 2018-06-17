<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="GilldStore_New.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- MAIN SECTION -->
    <section class="mainContent full-width clearfix">
      <div class="container">
        <div class="row">
          
          <div class="col-md-4 col-md-offset-4 col-sm-6 col-xs-12">
            <div class="panel panel-default formPanel">
              <div class="panel-heading bg-color-1 border-color-1">
                <h3 class="panel-title">Forgot Password?</h3>
              </div>
              <div class="panel-body">
                 <div class="form-group formField">                      
                      <asp:RequiredFieldValidator 
                          ID="Email_Address_Required_Validation" 
                          runat="server"
                          ErrorMessage="Email Address is Mandatory"
                          ControlToValidate="Email_Address"
                          Display="Dynamic"
                          ValidationGroup="VG1"
                          ForeColor="Red">
                      </asp:RequiredFieldValidator>
                      <asp:TextBox ID="Email_Address" runat="server" CssClass="form-control" placeholder="Email_Address" MaxLength="255"></asp:TextBox>                    
                  </div>
                  <div class="form-group formField">
                      <asp:Button ID="Submit_Button" runat="server" CssClass="btn btn-primary btn-block bg-color-3 border-color-3" Text="Reset Password" OnClick="Submit_Button_Click" ValidationGroup="VG1"/>                    
                  </div>
               </div>
            </div>
          </div>
        </div>
      </div>
    </section>
</asp:Content>
