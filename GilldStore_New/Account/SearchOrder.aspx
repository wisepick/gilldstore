<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="SearchOrder.aspx.cs" Inherits="GilldStore_New.Account.SearchOrder" %>
<%@ Register Src="~/Controls/Order_Grid.ascx" TagName="OrderGrid" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <table width="40%" align="center">
                    <tr>
                         <td>
                             <div class="panel panel-primary">
                                <div class="panel-heading">Search Order</div>
                                    <div class="panel-body">
                                        <table width="100%" class="table table-bordered" align="center">
                                            <tr>
                                                <th>
                                                    Enter the Order Number
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="Order_Id" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="Search_Order_Button" runat="server" Text="Search" CssClass="btn-primary" OnClick="Search_Order_Button_Click"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                 <uc1:OrderGrid id ="OrderGrid1" runat="server">

                </uc1:OrderGrid>
            </td>
        </tr>
    </table>
   
</asp:Content>
