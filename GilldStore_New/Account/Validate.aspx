<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Validate.aspx.cs" Inherits="GilldStore_New.Account.Validate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="Referer" runat="server" />
    <div class="container">
        <center>
            <h6><asp:Label ID="Header_Label" runat="server">
            </asp:Label></h6>
        </center>
        <asp:PlaceHolder ID="Email_Placeholder" runat="server" Visible="false">
            <div class="panel panel-primary">
            <div class="panel-heading"><h6><asp:Label ID="Step1" runat="server"></asp:Label>Validate Your Email Address (Check your mail : <asp:Label ID="Email_Address" runat="server"></asp:Label>) <asp:LinkButton ID="Regenerate_Guid_Button" runat="server" Text="Resend Validation Mail" OnClick="Regenerate_Guid_Button_Click" CssClass="btn btn-primary"></asp:LinkButton> </h6></div>            
            <div class="panel-body">
                <center>
                    <asp:LinkButton id="Change_Email_Adress_Button" runat="server" Text="Change Email Address" OnClick="Change_Email_Adress_Button_Click"></asp:LinkButton>
                    <asp:Label id="Validation_Email_Message" runat="server" ForeColor="Green">
                    </asp:Label>
                </center>
                <asp:PlaceHolder ID="New_Email_Address_Placeholder" runat="server" Visible="false">
                    <table width="50%">
                    <tr>
                        <th>
                            Email Address
                        </th>
                        <td>
                            <asp:TextBox ID="New_Email_Address" runat="server" MaxLength="255" CssClass="form-control">
                            </asp:TextBox>
                        </td>
                    </tr>                                        
                    <tr>                        
                        <td colspan="2">
                            <asp:Button ID="Submit_Button" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Submit_Button_Click"/>
                        </td>
                    </tr>
                </table>
                </asp:PlaceHolder>
                <asp:Button ID="Complete_Button" runat="server" Text="Complete" CssClass="btn btn-primary" OnClick="Complete_Button_Click" />
            </div>
        </div>
        </asp:PlaceHolder>
    <asp:PlaceHolder ID="Mobile_Placeholder" runat="server" Visible="false">
        <div class="panel panel-primary">
            <div class="panel-heading"><h6><asp:Label ID="Step2" runat="server"></asp:Label>Validate Your Mobile</h6></div>
            <div class="panel-body">
                <table width="50%">
                    <tr>
                        <th>
                            Mobile Number
                        </th>
                        <td>
                            <asp:TextBox ID="Mobile_Number" runat="server" MaxLength="10" CssClass="form-control" OnTextChanged="Mobile_Number_TextChanged" AutoPostBack="true">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Enter the OTP
                        </th>
                        <td>
                            <asp:TextBox ID="OTP" runat="server" MaxLength="6" CssClass="form-control" >
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Mobile_Validation_Message" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="Validate_OTP_Button" runat="server" Text="Validate OTP" CssClass="btn btn-primary" OnClick="Validate_OTP_Button_Click"/>
                        </td>
                        <td>
                            <asp:Button ID="Regenerate_OTP_Button" runat="server" Text="Regenerate OTP" CssClass="btn btn-primary" OnClick="Mobile_Number_TextChanged"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:PlaceHolder>
    </div>
</asp:Content>
