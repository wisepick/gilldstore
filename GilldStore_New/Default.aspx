<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GilldStore_New.Default" %>
<%@ Register Src="~/Controls/Contact.ascx" TagName="ContactInfo" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Site.Master" %>   
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- BANNER -->
    <section class="bannercontainer bannercontainerV2">
        <div class="fullscreenbanner-container">
            <div class="fullscreenbanner">
                <ul>
                    <li data-transition="fade" data-slotamount="5" data-masterspeed="300" data-title="banner_1">
                        <img runat="server" src="~/img/slider-bg-1.png"  alt="slidebg" data-bgfit="cover" data-bgposition="center center" data-bgrepeat="repeat">
                        <div class="slider-caption container">
                            <!-- LAYER NR. 1 -->
                            <div class="tp-caption rs-caption-1 sft text-center"
                                data-hoffset="0"
                                data-x="right"
                                data-y="130"
                                data-speed="1200"
                                data-start="1600"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                style="z-index: 5;">
                                Healthy<br>
                                <span>Life Begins Here</span>
                            </div>
                            <!-- LAYER NR. 2 -->
                            <div class="tp-caption rs-caption-2 sft text-center"
                                data-hoffset="0"
                                data-x="right"
                                data-y="245"
                                data-speed="1800"
                                data-start="2000"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">                 
                            </div>
                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption rs-caption-3 sft text-center"
                                data-hoffset="0"
                                data-x="right"
                                data-y="350"
                                data-speed="2000"
                                data-start="3000"
                                data-easing="Power4.easeOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">
                                <a runat="server" href="~/Products.aspx" class="btn btn-default">purchase</a>
                            </div>
                            <!-- LAYER NR. 4 -->
                            <div class="tp-caption tp-resizeme sfT stt "
                                data-x="-50"
                                data-y="130"
                                data-speed="500"
                                data-easing="Power1.easeIn"
                                data-endeasing="Power1.easeInOut"
                                data-endspeed="200"
                                style="z-index: 2;">
                                <img runat="server" src="~/img/caption-img1.png" alt="caption" width="758" height="409">
                            </div>
                        </div>
                    </li>
                    <li data-transition="fade" data-slotamount="5" data-masterspeed="1000" data-title="banner_2">
                        <img runat="server" src="~/img/slider-bg-1.png"  alt="slidebg" data-bgfit="cover" data-bgposition="center center" data-bgrepeat="repeat">
                        <div class="slider-caption container">
                            <!-- LAYER NR. 1 -->
                            <div class="tp-caption rs-caption-1 sft text-center"
                                data-hoffset="0"
                                data-y="130"
                                data-speed="1200"
                                data-start="1600"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                style="z-index: 5;">
				                100% Pure & Sure <br>
                                <span>Traditional Gilld Edible Oil</span>                
                            </div>
                                
                            <!-- LAYER NR. 2 -->
                            <div class="tp-caption rs-caption-2 sft"
                                data-hoffset="0"
                                data-y="245"
                                data-speed="1800"
                                data-start="2000"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">                 
                            </div>
                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption rs-caption-3 sft"
                                data-hoffset="0"
                                data-y="350"
                                data-speed="2000"
                                data-start="3000"
                                data-easing="Power4.easeOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">
                                <a runat="server" href="~/Products.aspx" class="btn btn-default">purchase</a>
                            </div>
                            <!-- LAYER NR. 4 -->
                            <div class="tp-caption tp-resizeme sft stt "
                                data-x="right"
                                data-hoffset="-100"
                                data-y="100"
                                data-speed="500"
                                data-easing="Power1.easeIn"
                                data-endeasing="Power1.easeInOut"
                                data-endspeed="200"
                                style="z-index: 2;">
                                <img runat="server" src="~/img/caption-img2.png" alt="caption" width="405" height="417">
                            </div>
                        </div>
                    </li>
                    <li data-transition="fade" data-slotamount="5" data-masterspeed="1000" data-title="banner_3">
                        <img runat="server" src="~/img/slider-bg-1.png"  alt="slidebg" data-bgfit="cover" data-bgposition="center center" data-bgrepeat="repeat">
                        <div class="slider-caption container">
                            <!-- LAYER NR. 1 -->
                            <div class="tp-caption rs-caption-1 sft"
                                data-hoffset="0"
                                data-y="130"
                                data-speed="1200"
                                data-start="1600"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                style="z-index: 5;">
                                No <br>
                                <span>Chemical Added for Colour, Taste & Aroma</span>
                            </div>
                            <!-- LAYER NR. 2 -->
                            <div class="tp-caption rs-caption-2 sft"
                                data-hoffset="0"
                                data-y="245"
                                data-speed="1800"
                                data-start="2000"
                                data-easing="Back.easeInOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">                                        
                            </div>
                            <!-- LAYER NR. 3 -->    
                            <div class="tp-caption rs-caption-3 sft"
                                data-hoffset="0"
                                data-y="350"
                                data-speed="2000"
                                data-start="3000"
                                data-easing="Power4.easeOut"
                                data-endspeed="300"
                                data-endeasing="Power1.easeIn"
                                data-captionhidden="off"
                                style="z-index: 5;">
                                <a runat="server" href="~/Products.aspx"  class="btn btn-default">purchase</a>
                            </div>
                            <!-- LAYER NR. 4 -->
                            <div class="tp-caption tp-resizeme sft stt "
                                data-x="right"
                                data-hoffset="-100"
                                data-y="100"
                                data-speed="500"
                                data-easing="Power1.easeIn"
                                data-endeasing="Power1.easeInOut"
                                data-endspeed="200"
                                style="z-index: 2;">
                                <img runat="server" src="~/img/caption-img3.png" alt="caption" width="405" height="417">
                            </div>
                        </div>
                    </li>                             
                </ul>
            </div>   
        </div>        
    </section>

    <!-- BANNER BOTTOM -->
    <section class="banner-bottom hidden-xs">
      <div class="container">
        <div class="banner-bottom-inner">
          <div class="row">
            <div class="col-sm-4">
              <div class="colContent">    
                <div class="colContent-info text-center">
                    <font size="10"><i class="fa fa-thumbs-up"></i></font>              
                  <h3 class="bg-color-6">Quality Guaranteed</h3>
                </div>
              </div>
            </div>
            <div class="col-sm-4">
              <div class="colContent">
                
                 <div class="colContent-info text-center">
                     <font size="10"><i class="fa fa-truck"></i></font>              
                  <h3 class="bg-color-5">Delivered to Home</h3>
                </div>
              </div>
            </div>
            <div class="col-sm-4">
              <div class="colContent">
                
                 <div class="colContent-info text-center">
                     <font size="10"><i class="fa fa-undo"></i></font>
                  <h3 class="bg-color-1">100% Return Policy</h3>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

      <!-- PRODUCT SECTION -->

            <section class="mainContent full-width clearfix productSection">
                <div class="container">
                    <div class="sectionTitle text-center">
                        <h2>
                            <span class="shape shape-left bg-color-4"></span>
                            <span>Recent Products</span>
                            <span class="shape shape-right bg-color-4"></span>
                        </h2>
                    </div>
            
                    <div class="tabCommon">
                        <ul class="nav nav-tabs">
                            <li class="active"><a data-toggle="tab" href="#menu1">New Arrivals</a></li>
                            <li><a data-toggle="tab" href="#menu2">Popular</a></li>            
                        </ul>
                
                        <div class="tab-content">
                            <div id="menu1" class="tab-pane fade in active">
                                <asp:ListView ID="New_Products" runat="server" DataKeyNames="PRODUCT_ID" OnItemDataBound="Product_ListView_OnItemDataBound"> 
                                    <LayoutTemplate>               
                                        <div class="row">
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                                                    
                                        </div>                
                                    </LayoutTemplate>            
                                    <ItemTemplate>  
                                        <div class="col-md-3 col-xs-6">
                                            <div class='box bg-color-<%# Eval("RANK") %>'>
                                                <div class='box-img border-color-<%# Eval("RANK")%> text-center'>
                                                    <a href='<%# "product_detail.aspx?product_id=" + Eval("PRODUCT_ID").ToString() %>' runat="server">
                                                        <asp:Image ID="Image3" runat="server" ImageUrl='<%# "~/img/" +  Eval("PHOTO_FILE_NAME").ToString() %>' AlternateText='<%# Eval("PRODUCT_NAME") %>' ToolTip='<%# Eval("PRODUCT_NAME") %>' CssClass="img-responsive" />							                                
                                                    </a>                                                                                                
                                                </div>
                                                <div class="box-info">
                                                    <h4><%# Eval("PRODUCT_NAME") %></h4>
                                                    <asp:Repeater ID="Product_Price_List" runat="server" OnItemDataBound="Product_Price_List_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="formField">
                                                                <table class="table table-responsive">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:HiddenField ID="Measurement_Unit" runat="server" Value='<%# Eval("MEASUREMENT_UNIT") %>' />
                                                                            <asp:HiddenField ID="Product_Id" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />
                                                                            <asp:HiddenField ID="Price" runat="server" Value='<%# Eval("PRICE") %>' />                                                                
                                                                            <%# Eval("MEASUREMENT_UNIT") %>&nbsp;<%# Eval("MEASUREMENT_NAME") %>                                                                                                    
                                                                        </td>
                                                                        <td style="text-align:right">
                                                                            <i class="fa fa-rupee"></i>&nbsp;<%# Eval("PRICE") %>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList 
                                                                                ID="Quantity" 
                                                                                runat="server" 
                                                                                CssClass="form-control border-color-2" 
                                                                                AppendDataBoundItems="true" 
                                                                                DataValueField="SR_NO"
                                                                                DataTextField="SR_NO">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater> 
                                                    <div class="add-cart">
                                                        <asp:LinkButton 
                                                            ID="Add_To_Cart_Button" 
                                                            runat="server" 
                                                            Text='<i class="fa fa-shopping-basket " aria-hidden="true"></i>Add to Cart'
                                                            CssClass="add-to-cart btn btn-primary" 
                                                            OnClick="Add_To_Cart_Button_Click">
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>		        
                            </div>      
                            <div id="menu2" class="tab-pane fade">
                                <asp:ListView ID="Popular_Products" runat="server" DataKeyNames="PRODUCT_ID" OnItemDataBound="Product_ListView_OnItemDataBound"> 
                                    <LayoutTemplate>               
                                        <div class="row">
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                                                    
                                        </div>                
                                    </LayoutTemplate>            
                                    <ItemTemplate>  
                                        <div class="col-md-3 col-xs-6">
                                            <div class='box bg-color-<%# Eval("RANK") %>'>
                                                <div class='box-img border-color-<%# Eval("RANK")%> text-center'>
                                                    <a href='<%# "product_detail.aspx?product_id=" + Eval("PRODUCT_ID").ToString() %>' runat="server">
                                                        <asp:Image ID="Image3" runat="server" ImageUrl='<%# "~/img/" +  Eval("PHOTO_FILE_NAME").ToString() %>' AlternateText='<%# Eval("PRODUCT_NAME") %>' ToolTip='<%# Eval("PRODUCT_NAME") %>' CssClass="img-responsive" />							                                
                                                    </a>
                                                    <span class='sticker-round bg-color-<%# Eval("RANK") %>'>Popular</span>                                                    
                                                </div>
                                                <div class="box-info">
                                                    <h4><%# Eval("PRODUCT_NAME") %></h4>
                                                    <asp:Repeater ID="Product_Price_List" runat="server" OnItemDataBound="Product_Price_List_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="formField">
                                                                <table class="table table-responsive">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:HiddenField ID="Measurement_Unit" runat="server" Value='<%# Eval("MEASUREMENT_UNIT") %>' />
                                                                            <asp:HiddenField ID="Product_Id" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />
                                                                            <asp:HiddenField ID="Price" runat="server" Value='<%# Eval("PRICE") %>' />                                                                
                                                                            <%# Eval("MEASUREMENT_UNIT") %>&nbsp;<%# Eval("MEASUREMENT_NAME") %>                                                                                                    
                                                                        </td>
                                                                        <td style="text-align:right">
                                                                            <i clas s="fa fa-rupee"></i>&nbsp;<%# Eval("PRICE") %>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList 
                                                                                ID="Quantity" 
                                                                                runat="server" 
                                                                                CssClass="form-control border-color-2" 
                                                                                AppendDataBoundItems="true" 
                                                                                DataValueField="SR_NO"
                                                                                DataTextField="SR_NO">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater> 
                                                    <div class="add-cart">
                                                        <asp:LinkButton 
                                                            ID="Add_To_Cart_Button" 
                                                            runat="server" 
                                                            Text='<i class="fa fa-shopping-basket " aria-hidden="true"></i>Add to Cart'
                                                            CssClass="add-to-cart btn btn-primary" 
                                                            OnClick="Add_To_Cart_Button_Click">
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>		              
                            </div>                
                        </div>
                    </div>
                </div>
            </section>            
        

    <!-- PROMOTION SECTION-->
    <section class="promotionWrapper" style="background-image: url(img/header.jpg);">
      <div class="container">
        <div class="promotionInfo">
          
        </div>
      </div>
    </section>

  

    <!-- FEATURE SECTION -->
    <section class="mainContent full-width clearfix featureSection">
      <div class="container">
        <div class="sectionTitle text-center">
          <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span>Our Features</span>
            <span class="shape shape-right bg-color-4"></span>
          </h2>
        </div>

        <div class="row">          
          
          <div class="col-sm-4 col-xs-12">
            <div class="media featuresContent">
              <span class="media-left bg-color-4">
                <i class="fa fa-truck bg-color-4" aria-hidden="true"></i>
              </span>
              <div class="media-body">
                <h3 class="media-heading color-4">Home Delivery</h3>               
              </div>
            </div>
          </div>
		  
          <div class="col-sm-4 col-xs-12">
            <div class="media featuresContent">
              <span class="media-left bg-color-5">
                <i class="fa fa-thumbs-up bg-color-5" aria-hidden="true"></i>
              </span>
              <div class="media-body">
                <h3 class="media-heading color-5">Quality Guranteed</h3>                
              </div>
            </div>
          </div>
		   <div class="col-sm-4 col-xs-12">
            <div class="media featuresContent">
              <span class="media-left bg-color-6">
                <i class="fa fa-undo bg-color-6" aria-hidden="true"></i>
              </span>
              <div class="media-body">
                <h3 class="media-heading color-6">100% Return Policy</h3>                
              </div>
            </div>
          </div>
		 </div>
		 <div class="row">
		 <div class="col-sm-12 col-xs-12">
            <div class="media featuresContent">
              <span class="media-left bg-color-3">
                <i class="fa fa-inr bg-color-3" aria-hidden="true"></i>
              </span>
              <div class="media-body">
                <h3 class="media-heading color-3">Pay with All Major Instruments</h3>                
              </div>
            </div>
          </div>
         
        </div>
      </div>
    </section>

    <!-- COLOR SECTION -->
    <section class="colorSection full-width clearfix bg-color-6 commentSection">
      <div class="container">
        <div class="row">
          <div class="col-xs-12">
            <div class="owl-carousel commentSlider">
              <asp:Repeater ID="Review_Repeater" runat="server">
                  <ItemTemplate>
                      <div class="slide">
                        <div class="commentContent text-center">
                            <i class="fa fa-comments-o" aria-hidden="true"></i>
                            <h2><%# Eval("REVIEW_COMMENT") %></h2>
                            <h3><%# Eval("USER_NAME") %></h3>  
                            <p>for product <%# Eval("PRODUCT_NAME") %></p>                          
                        </div>
                    </div>
                  </ItemTemplate>
              </asp:Repeater>              
              
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- WHITE SECTION -->
    <section class="whiteSection full-width clearfix newsSection" id="latestNews">
      <div class="container">
        <div class="sectionTitle text-center">
          <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span>Certified By</span>
            <span class="shape shape-right bg-color-4"></span>
          </h2>
        </div>

        <div class="row">
          <div class="col-sm-4 col-xs-12 block">
            <div class="thumbnail thumbnailContent">
              <img runat="server" src="~/img/certification1.jpg" alt="image" class="img-responsive">                     
            </div>
          </div>
          <div class="col-sm-4 col-xs-12 block">
            <div class="thumbnail thumbnailContent">
              <img runat="server" src="~/img/certification2.jpg" alt="image" class="img-responsive">
              
            </div>
          </div>         
        </div>

        

      </div>
    </section>
    
    <!-- BRAND SECTION -->
    
    <section class="brandSection clearfix">
      <div class="container">
        <div class="sectionTitle text-center">
          <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span>Product Gallery</span>
            <span class="shape shape-right bg-color-4"></span>
          </h2>
        </div>
          </div>

          <asp:ListView ID="Brand_ListView" runat="server" DataKeyNames="PRODUCT_ID" > 
            <LayoutTemplate>
                <div class="container">
                    <div class="owl-carousel partnersLogoSlider">
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                                                    
                    </div>
                </div>
            </LayoutTemplate>            
            <ItemTemplate>  
                <div class="slide">
                    <div class="partnersLogo clearfix">
                        <asp:Image ID="Image2" 
                            runat="server" 
                            ImageUrl='<%# "~/img/" +  Eval("ATTRIBUTE_NAME").ToString() %>' 
                            AlternateText='<%# Eval("PRODUCT_NAME") %>' 
                            ToolTip='<%# Eval("PRODUCT_NAME") %>' 
                            CssClass="img-responsive" Height="250"/>							                                
                    </div>
                </div>                         
            </ItemTemplate>
        </asp:ListView>	
    </section>
</asp:Content>
