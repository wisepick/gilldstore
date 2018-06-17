<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Policy.aspx.cs" Inherits="GilldStore_New.Policy" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <center><h3><asp:Label id="Policy_Header" runat="server"></asp:Label><br /><asp:Label ID="Policy_Update_Date" runat="server"></asp:Label></h3></center>
	    <div class="row ">
            <asp:Repeater ID="Policy_Repeater" runat="server" OnItemDataBound="Policy_Header_OnItemDataBound">
                <HeaderTemplate>
                    <div class="panel panel-primary">                                                                                                           
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="panel-heading"><%# Eval("HEADER_TEXT") %><asp:HiddenField ID="Header_Id" runat="server" Value='<%# Eval("POLICY_HEADER_ID") %>'/></div>
                    <asp:Repeater ID="Policy_Content_Repeater" runat="server">
                        <HeaderTemplate>
                            <div class="panel-body">            
                        </HeaderTemplate>
                        <ItemTemplate>
                            <p>
                                <%# Eval("CONTENT") %>
                            </p>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </ItemTemplate>
                <FooterTemplate>                        
                    </div>
                </FooterTemplate>
            </asp:Repeater>
         </div>            
	</div>
</asp:Content>