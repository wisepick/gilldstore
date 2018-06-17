<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSummary.aspx.cs" Inherits="GilldStore_New.OrderSummary" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="mainContent full-width clearfix">
        <div class="container">
            <div class="row">
                <div class="row progress-wizard" style="border-bottom:0;">                
                
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">                
                        <div class="progress"><div class="progress-bar"></div></div>
                        <a href="#" runat="server" class="progress-wizard-dot">
                            <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                        </a>
                        <div class="progressInfo">1. Order info</div>
                    </div>             
                
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">                
                        <div class="progress"><div class="progress-bar"></div></div>                    
                        <a href="#" runat="server" class="progress-wizard-dot">
                            <i class="fa fa-map-marker" aria-hidden="true"></i>
                        </a>                        
                        <div class="progressInfo">2. Address Info</div>
                    </div>
                 
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">                
                        <div class="progress"><div class="progress-bar"></div></div>
                        <a href="#" runat="server" class="progress-wizard-dot">
                            <i class="fa fa-list" aria-hidden="true"></i>
                        </a>
                        <div class="progressInfo">3. Review Order</div>
                    </div>
                    <div class="col-sm-2 col-xs-12 progress-wizard-step complete">                
                        <div class="progress"><div class="progress-bar"></div></div>
                        <a href="#" runat="server" class="progress-wizard-dot">
                            <i class="fa fa-rupee" aria-hidden="true"></i>
                        </a>                                           
                        <div class="progressInfo">4. Payment Info</div>
                    </div>
                <div class="col-sm-2 col-xs-12 progress-wizard-step complete">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a href="#" class="progress-wizard-dot">
                        <i class="fa fa-check" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">4. Confirmation</div>
                </div>
            </div>            
        </div>
    <uc1:OrderInfo ID="OrderInfo1" runat="server" Show_Order_Message="true" Show_Shipping_Order_Button="false"/>
                </div>
</asp:Content>