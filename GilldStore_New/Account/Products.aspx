<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="GilldStore_New.Account.Products" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
<%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>       --%>
            <asp:Button ID="Add_Button" runat="server" Text="Add New" OnClick="Add_Button_Click"  CssClass="btn btn-primary"/>

            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Product_All_View" runat="server">
                    <asp:ListView ID="Product_View" runat="server" DataKeyNames="PRODUCT_ID, PRODUCT_CATEGORY_ID">
                        <LayoutTemplate>
                            <table class="table table-bordered">
                                <tr>
                                    <th>Product Name</th>
                                    <th>Product Category</th>                                       
                                    <th>Measurement Unit</th>
                                    <th>Higlight Text</th>                                    
                                    <th>Summary</th>
                                    <th>Image</th>
                                    <th>Action</th>                                    
                                </tr>
                                <tr>
                                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                                </tr>
                            </table>            
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("PRODUCT_NAME") %>                                    
                                </td>
                                <td>
                                    <%# Eval("PRODUCT_CATEGORY_NAME") %>
                                </td>          
                                <td>
                                    <%# Eval("MEASUREMENT_NAME") %>
                                </td>      
                                <td>
                                    <%# Server.HtmlDecode(Eval("HIGHLIGHT_TEXT").ToString()) %>
                                </td>
                                <td>
                                    <%# Server.HtmlDecode(Eval("SUMMARY").ToString()) %>
                                </td>                                
                                <td>
                                    <asp:Image id="Product_Image" runat="server" ImageUrl='<%# "~/img/" + Eval("PHOTO_FILE_NAME").ToString() %>' Width="200"/>
                                </td>
                                <td>
                                    <asp:Button ID="Edit_Button" runat="server" Text="Edit" CssClass="btn btn-primary" OnCommand="Edit_Image_Button_Command"  CommandArgument='<%# Eval("PRODUCT_ID") %>' />                                    
                                </td>
                            </tr>
                        </ItemTemplate>        
                
                    </asp:ListView>
                </asp:View>
                <asp:View ID="Product_Edit_View" runat="server">
                    <asp:FormView ID="Product_Form_View" runat="server" Width="100%" DataKeyNames="PRODUCT_ID, PRODUCT_CATEGORY_ID, MEASUREMENT_ID, SUMMARY">
                        <InsertItemTemplate>
                            <table class="table table-bordered" width="100%">
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="TextBox2" runat="server" Visible="false"></asp:TextBox>
                                        <asp:Button ID="Button3" runat="server" Text="Save"  CssClass="btn btn-primary" ValidationGroup="VG1" CausesValidation="true" OnCommand="Add_Save_Button_Command"/>
                                        <asp:Button ID="Button4" runat="server" Text="Cancel" OnClick="Cancel_Button_Click" CssClass="btn btn-primary" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Product Name</th>
                                    <td>
                                        <asp:TextBox ID="Product_Name" runat="server" MaxLength="255" CssClass="form-control">
                                        </asp:TextBox>
                                    </td>
                                    <th>Product Category</th>
                                    <td>
                                        <asp:DropDownList ID="Product_Category_Id" runat="server" AppendDataBoundItems="true" DataTextField="PRODUCT_CATEGORY_NAME" DataValueField="PRODUCT_CATEGORY_ID" CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="Product_Name_Required" runat="server" ErrorMessage="Enter the Product Name" Display="Dynamic" ControlToValidate="Product_Name" ValidationGroup="VG1" ForeColor="Red">
                                        </asp:RequiredFieldValidator>                
                                    </td>            
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="Product_Category_Required" runat="server" ErrorMessage="Select the Product Category" Display="Dynamic" ControlToValidate="Product_Category_Id" ValidationGroup="VG1" ForeColor="Red">
                                        </asp:RequiredFieldValidator>                
                                    </td>
                                </tr>
                                <tr>
                                    <th>Measurement Unit</th>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Measurement_Unit" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Highlight Text</th>
                                    <td colspan="3">
                                        <asp:TextBox ID="Highlight_Text" runat="server" MaxLength="255" CssClass="form-control" TextMode="MultiLine" Rows="4">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Summary
                                    </th>
                                    <td colspan="3">                                        
                                        <obout:Editor ID="Summary" runat="server" Height="600px" Width="100%" BottomToolbar-ShowPreviewButton="false" BottomToolbar-ShowHtmlTextButton="false" ></obout:Editor>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Image</th>
                                    <td colspan="3">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="Add_Save_Button" runat="server" Text="Save"  CssClass="btn btn-primary" ValidationGroup="VG1" CausesValidation="true" OnCommand="Add_Save_Button_Command"/>
                                        <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" OnClick="Cancel_Button_Click" CssClass="btn btn-primary" />
                                    </td>
                                </tr>
                            </table>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <table class="table table-bordered" width="100%">
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" Text="Save"  CssClass="btn btn-primary" ValidationGroup="VG1" CausesValidation="true" OnCommand="Add_Save_Button_Command"/>
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="Cancel_Button_Click" CssClass="btn btn-primary" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Product Name</th>
                                    <td>
                                        <asp:TextBox ID="Product_Name" runat="server" MaxLength="255" CssClass="form-control" Text='<%# Eval("PRODUCT_NAME") %>'>
                                        </asp:TextBox>
                                    </td>
                                    <th>Product Category</th>
                                    <td>
                                        <asp:DropDownList ID="Product_Category_Id" runat="server" AppendDataBoundItems="true" DataTextField="PRODUCT_CATEGORY_NAME" DataValueField="PRODUCT_CATEGORY_ID" CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="Product_Name_Required" runat="server" ErrorMessage="Enter the Product Name" Display="Dynamic" ControlToValidate="Product_Name" ValidationGroup="VG1" ForeColor="Red">
                                        </asp:RequiredFieldValidator>                
                                    </td>            
                                    <td colspan="2">
                                        <asp:RequiredFieldValidator ID="Product_Category_Required" runat="server" ErrorMessage="Select the Product Category" Display="Dynamic" ControlToValidate="Product_Category_Id" ValidationGroup="VG1" ForeColor="Red">
                                        </asp:RequiredFieldValidator>                
                                    </td>
                                </tr>
                                
                                <tr>
                                    <th>Measurement Unit</th>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Measurement_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Highlight Text</th>
                                    <td colspan="3">
                                        <asp:TextBox ID="Highlight_Text" runat="server" MaxLength="255" CssClass="form-control" Text='<%# Server.HtmlDecode(Eval("HIGHLIGHT_TEXT").ToString()) %>' TextMode="MultiLine" Rows="4" ValidateRequestMode="Disabled">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Summary
                                    </th>
                                    <td colspan="3">
                                        <cc1:Editor ID="Summary" runat="server" Content='<%# Server.HtmlDecode(Eval("SUMMARY").ToString()) %>'/>                                        
                                    </td>
                                </tr>                        
                                
                                <tr>
                                    <th>Image</th>
                                    <td colspan="3">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:Image ID="Product_Image" runat="server" AlternateText='<%# Eval("PRODUCT_NAME") %>' ImageUrl='<%# "~/img/" + Eval("PHOTO_FILE_NAME").ToString() %>' Width="100" />
	                                    <asp:Button ID="Remove_Image_Button" runat="server" Text="Remove Image"  CssClass="btn btn-primary"  OnClick="Remove_Image_Button_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="Options" runat="server" Visible="false"></asp:TextBox>
                                        <asp:Button ID="Add_Save_Button" runat="server" Text="Save"  CssClass="btn btn-primary" ValidationGroup="VG1" CausesValidation="true" OnCommand="Add_Save_Button_Command"/>
                                        <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" OnClick="Cancel_Button_Click" CssClass="btn btn-primary" />
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </asp:FormView>
                </asp:View>
            </asp:MultiView>        
 <%--       </ContentTemplate>
    </asp:UpdatePanel>--%>

</asp:Content>
