<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Templates.aspx.cs" Inherits="GilldStore_New.Account.Templates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="Add_New_Button" runat="server" CssClass="btn btn-primary" Text="Add New" OnClick="Add_New_Button_Click"/>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="Templates_View" runat="server">
            <asp:ListView ID="Templates_ListView" runat="server" OnItemEditing="Templates_ListView_ItemEditing" DataKeyNames="TEMPLATE_ID">
                <LayoutTemplate>
                    <table width="100%" class="table table-bordered">
                        <tr>
                            <th width="10%">Template Type</th>                            
                            <th width="10%">&nbsp;</th>
                        </tr>
                       
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                       
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="10%">
                            <%# Eval("TEMPLATE_TYPE") %>
                        </td>                        
                        <td width="10%">
                            <asp:Button ID="Edit_Button" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-primary" />
                        </td>
                    </tr>
                </ItemTemplate>                                
            </asp:ListView>
        </asp:View>
        <asp:View ID="Detail_View" runat="server">
            <asp:FormView ID="Detail_Form_View" runat="server" DataKeyNames="TEMPLATE_ID">                
                <InsertItemTemplate>
                    <table>
                     <tr>
                         <th>
                             Template Type
                         </th>
                        <td>
                            <asp:TextBox ID="Template_Type" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Template_Type_Required" runat="server" ErrorMessage="<br>Template Type is required" Display="Dynamic" ControlToValidate="Template_Type" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Sms Message</th>
                        <td>                            
                            <asp:TextBox ID="Sms_Template_Message" runat="server" MaxLength="255" CssClass="form-control" TextMode="MultiLine" Columns="10" Rows="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Sms_Message_Required" runat="server" ErrorMessage="<br>Sms Message is required" Display="Dynamic" ControlToValidate="Sms_Template_Message" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Email Subject</th>
                        <td>
                            <asp:TextBox ID="Email_Subject" runat="server" MaxLength="255" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Email_Subject_Required" runat="server" ErrorMessage="<br>Email Subject is required" Display="Dynamic" ControlToValidate="Email_Subject" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>                       
                        </td>
                    </tr>
                    <tr>
                        <th>Email Message</th>
                        <td>
                            <cc1:Editor ID="Email_Template_Message" runat="server" />     
                            <asp:RequiredFieldValidator ID="Email_Message_Required" runat="server" ErrorMessage="<br>Email Message is required" Display="Dynamic" ControlToValidate="Email_Template_Message" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                    <Tr>
                        <td colspan="2">
                            <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </Tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Insert_Button" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Insert_Button_Click" CausesValidation="true" ValidationGroup="VG1"/>
                            <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Button_Click"/>
                        </td>
                    </tr>
                        </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <table>
                    <tr>
                         <th>
                             Template Type
                         </th>
                        <td>
                            <asp:TextBox ID="Template_Type" runat="server" MaxLength="255" CssClass="form-control" Text='<%# Eval("TEMPLATE_TYPE") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Template_Type_Required" runat="server" ErrorMessage="<br>Template Type is required" Display="Dynamic" ControlToValidate="Template_Type" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Sms Message</th>
                        <td>                            
                            <asp:TextBox ID="Sms_Template_Message" runat="server" MaxLength="255" CssClass="form-control" TextMode="MultiLine" Columns="10" Rows="4" Text='<%# Eval("SMS_TEMPLATE_MESSAGE") %>'></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <th>Email Subject</th>
                        <td>
                            <asp:TextBox ID="Email_Subject" runat="server" MaxLength="255" CssClass="form-control" Text='<%# Eval("EMAIL_SUBJECT") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Email_Subject_Required" runat="server" ErrorMessage="<br>Email Subject is required" Display="Dynamic" ControlToValidate="Email_Subject" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>                       
                        </td>
                    </tr>
                    <tr>
                        <th>Email Message</th>
                        <td>
                            <cc1:Editor ID="Email_Template_Message" runat="server" Content='<%# Server.HtmlDecode(Eval("EMAIL_TEMPLATE_MESSAGE").ToString()) %>' Height="2000" DesignPanelCssPath="~/plugins/bootstrap/css/bootstrap.min.css"/>     
                            <asp:RequiredFieldValidator ID="Email_Message_Required" runat="server" ErrorMessage="<br>Email Message is required" Display="Dynamic" ControlToValidate="Email_Template_Message" ValidationGroup="VG1" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                    <Tr>
                        <td colspan="2">
                            <asp:Label ID="Message" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </Tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Update_Button" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Update_Button_Click" CausesValidation="true" ValidationGroup="VG1"/>
                            <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Button_Click"/>
                        </td>
                    </tr>
                        </table>
                </EditItemTemplate>
            </asp:FormView>
        </asp:View>
    </asp:MultiView>
    
</asp:Content>
