<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="GilldStore_New.Stats"  MasterPageFile="~/Site.Master"%>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:Repeater ID="Session_Repeater" runat="server">
            <ItemTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong><%# Eval("SESSION_DATE1") %></strong></div>
                    <div class="panel-body">
                        <div class="row">
				            <div class="col-md-4 col-sm-4 news wow fadeInUp" data-wow-delay="0.2s" data-wow-offset="10">					
                                <%# Eval("SESSION_COUNT") %>
                            </div>                            
                        </div>                        
                    </div>
                </div>
            </ItemTemplate>            
        </asp:Repeater>
    </div>
</asp:Content>

