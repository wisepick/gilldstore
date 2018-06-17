<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="GilldStore_New._404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <!-- MAIN SECTION -->
    <section class="mainContent full-width clearfix">
      <div class="container">
        <div class="row">
          <div class="col-sm-4 col-sm-offset-1 col-xs-12">            
          </div>
          <div class="col-sm-6 col-xs-12">
            <div class="errorInfo">
              <h3>Oops!</h3>
              <p>We are sorry, This page could not found!</p>
              
                <div class="formBtnArea pull-left">
                  <a href="~/" runat="server" class="btn btn-primary bg-color-3">Return to home page</a>
                </div>
              
            </div>
          </div>
        </div>

      </div>
    </section>
</asp:Content>
