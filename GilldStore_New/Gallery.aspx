<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="GilldStore_New.Gallery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <section class="mainContent full-width clearfix homeGallerySection">
                <div class="container">
                    
                        <div class="row isotopeContainer" id="container">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-3 col-sm-6 col-xs-12 isotopeSelector">
                <article class="">
                    <figure>
                        <asp:Image ID="image1" runat="server" ImageUrl='<%# "~/img/" + Eval("ATTRIBUTE_NAME").ToString() %>' CssClass="img-rounded"/>                
                        <div class="overlay-background">
                            <div class="inner"></div>
                        </div>
                        <div class="overlay">
                            <a class="fancybox-pop" rel="portfolio-1" href='<%# "~/img/" + Eval("ATTRIBUTE_NAME").ToString() %>' runat="server">
                                <i class="fa fa-search-plus" aria-hidden="true"></i>
                            </a>
                        </div>
                    </figure>
                </article>
            </div>            
        </ItemTemplate>
        <FooterTemplate>
                        </div>
                    
                </div>
            </section>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
