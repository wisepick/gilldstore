<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="GilldStore_New.Contact" %>
<%@ Register Src="~/Controls/Contact.ascx" TagName="ContactInfo" TagPrefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
        <!-- MAIN SECTION -->
    <asp:TextBox ID="txtBox" runat="server" Rows="10" CssClass="form-control-feedback" TextMode="MultiLine"> </asp:TextBox>
    <section class="mainContent full-width clearfix aboutSection">
      <div class="container">
        <div class="row">
            <uc1:ContactInfo id="ContactInfo1" runat="server">
            </uc1:ContactInfo>
        </div>
          </div
    </section>
</asp:Content>