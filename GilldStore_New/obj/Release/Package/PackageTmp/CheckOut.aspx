<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="GilldStore_New.CheckOut" %>
<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="mainContent clearfix">
        <div class="container">
            <div class="row progress-wizard" style="border-bottom:0;">
                <div class="col-sm-3 col-xs-12 progress-wizard-step complete">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a href="~/View_Cart.aspx" runat="server" class="progress-wizard-dot">
                        <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">1. Order info</div>
                </div>
                <div class="col-sm-3 col-xs-12 progress-wizard-step active">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a runat="server" href="~/Checkout.aspx" class="progress-wizard-dot">
                        <i class="fa fa-map-marker" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">2. Address Info</div>
                </div>
                <div class="col-sm-3 col-xs-12 progress-wizard-step incomplete">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a runat="server" href="~/Checkout.aspx" class="progress-wizard-dot">
                        <i class="fa fa-rupee" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">3. Payment Info</div>
                </div>
                <div class="col-sm-3 col-xs-12 progress-wizard-step incomplete">
                    <div class="progress"><div class="progress-bar"></div></div>
                    <a href="#" class="progress-wizard-dot">
                        <i class="fa fa-check" aria-hidden="true"></i>
                    </a>
                    <div class="progressInfo">4. Confirmation</div>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="row">      
                 <uc1:AddressBook 
                    ID="AddressBook1" 
                    runat="server"                 
                    Display_DeliveryAddressOption="Y"
                />
            </div>
            <div class="row">
                 
            </div>
        </div>
    </section>
</asp:Content>
