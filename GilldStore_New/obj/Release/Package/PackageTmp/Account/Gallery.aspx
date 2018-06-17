<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="GilldStore_New.Account.Gallery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="Product_Id" runat="server" DataTextField="PRODUCT_NAME" DataValueField="PRODUCT_ID" AppendDataBoundItems="true">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" CssClass="btn btn-primary" accept="image/gif, image/jpeg, image/png"/>
            </td>
            <td>
                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click"  Text="Upload" CssClass="btn btn-primary"/>
            </td>
        </tr>
    </table>
    <asp:ListView ID="Gallery_ListView" runat="server" GroupItemCount="6"> 
        <LayoutTemplate>
            <div class="container">
                <asp:PlaceHolder ID="groupPlaceholder" runat="server">
                </asp:PlaceHolder>                                                        
            </div>
        </LayoutTemplate>
        <GroupTemplate>
            <div id="pro5ects-container" class="row">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </div>
        </GroupTemplate>
	    <ItemTemplate>   
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="hover-content">
                    <asp:Image ID="Image1" runat="server" AlternateText='<%# Eval("ATTRIBUTE_NAME")  %>' class="img-responsive animation" ImageUrl='<%# "~/img/" + Eval("ATTRIBUTE_NAME").ToString()%>' />				                    
                    <center>
                    <asp:LinkButton ID="Delete_Button" runat="server" Text="Delete" OnCommand="Delete_Button_Command" CommandArgument='<%# Eval("FILE_ID") %>'>

                    </asp:LinkButton>
                        </center>
 
			    </div>
		    </div>
	    </ItemTemplate>
    </asp:ListView>					
    
</asp:Content>
