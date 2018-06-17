<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddressBook.aspx.cs" Inherits="GilldStore_New.Account.AddressBook" %>
<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
         <section class="mainContent clearfix">
        <div class="container">
            <div class="row">    

                <uc1:AddressBook 
                    ID="AddressBook1" 
                    runat="server"                 
                    Display_DeliveryAddressOption="N"
                />
            </div>
        </div>
    </section>
    
</asp:Content>
