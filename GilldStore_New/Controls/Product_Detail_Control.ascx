<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Product_Detail_Control.ascx.cs" Inherits="GilldStore_New.Controls.Product_Detail_Control" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    
        <asp:HiddenField ID="Product_Id" runat="server" />
        <!-- MAIN SECTION -->
        <style type="text/css">
            .ratingStar
            {
                font-size: 0pt;
                width: 12px;
                height: 12px;
                cursor: pointer;
                background-repeat: no-repeat;
                display: block;
            }

            .filledRatingStar
            {
                background-image: url(img/ratingStarFilled.png);
            }
        
            .emptyRatingStar
            {
                background-image: url(img/ratingStarEmpty.png);
            }

            .savedRatingStar
            {
                /*change this to your image name*/
                background-image: url(img/ratingStarSaved.png);
            }
        </style>
        
                <div class="col-md-6 col-sm-7 col-xs-12">
                    <div class="product-gallery">
                        <asp:Repeater ID="Product_Gallery_Content_Repeater" runat="server">
                            <HeaderTemplate>
                                <div class="product-gallery-content">
                                    <ul class="product-gallery-preview">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li id='preview<%# Eval("RANK") %>' <%# Check_Rank(Eval("RANK").ToString(), "current") %>>
                                    <asp:Image ID="Preview_Image" runat="server" CssClass="img-responsive" ImageUrl='<%# "~/img/" + Eval("ATTRIBUTE_NAME").ToString() %>' />
                                </li>                            
                            </ItemTemplate>
                            <FooterTemplate>
                                    </ul>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Repeater id="Product_Gallery_Thumblist_Repeater" runat="server">
                            <HeaderTemplate>
                                <ul class="product-gallery-thumblist">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li <%# Check_Rank(Eval("RANK").ToString(), "Active") %>>
                                    <a href='#preview<%# Eval("RANK").ToString() %>'>
                                        <asp:Image ID="Thumbnail_Image" runat="server" CssClass="img-thumbnail" ImageUrl='<%# "~/img/" + Eval("ATTRIBUTE_NAME").ToString() %>' />
                                    </a>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>                        
                    </div>
                </div>
                <div class="col-md-6 col-sm-5 col-xs-12">
                    <div class="product-info">
                        <h1 class="product-title"><asp:Label ID="Product_Name" runat="server"></asp:Label></h1>                      
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
                        <p><asp:Label ID="Highlight_Text" runat="server"></asp:Label></p>
                        <div class="add-cart">
                            <asp:LinkButton 
                                ID="Add_To_Cart_Button" 
                                runat="server" 
                                Text='<i class="fa fa-shopping-basket " aria-hidden="true"></i>Add to Cart'
                                CssClass="add-to-cart btn btn-primary" 
                                OnClick="Add_To_Cart_Button_Click">
                            </asp:LinkButton>                
                        </div>
                        <div class="product-meta">
                            <div class="product-category">
                                Category:
                                <asp:Label ID="Product_Category_Name" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>                 
            
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>


        <div class="tabCommon">
            <ul class="nav nav-tabs">
                <asp:PlaceHolder ID="Details_Button_Active_PlaceHolder" runat="server" Visible="true">
                    <li class="active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Details_Button_InActive_PlaceHolder" runat="server" Visible="false">
                    <li>
                </asp:PlaceHolder>
                    <asp:LinkButton ID="Details_Button" runat="server"  Text="Details" OnClick="Details_Button_Click">

                    </asp:LinkButton>
                </li>
                <asp:PlaceHolder ID="Review_Button_Active_PlaceHolder" runat="server" Visible="false">
                    <li class="active">
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="Review_Button_InActive_PlaceHolder" runat="server" Visible="true">
                    <li>
                </asp:PlaceHolder>
                    <asp:LinkButton ID="Review_Button" runat="server" Text="Reviews" OnClick="Review_Button_Click"></asp:LinkButton>
                </li>
            </ul>
            <div class="tab-content">
                
                <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">            
                    <asp:View ID="Product_Description_View" runat="server">                   
                        <div class="panel panel-primary">
                            <div class="panel-heading ">
                                <h3 class="panel-title">Product Description</h3>                            
                            </div>
                            <div class="panel-body">                            
                                <h3>Product Description</h3>
                                <p>
                                    <asp:Label ID="Highlight_Text_1" runat="server"></asp:Label>
                                </p>
                                <p>
                                    <asp:Label ID="Summary" runat="server"></asp:Label>
                                </p>              
                            </div>                  
                        </div>  
                    </asp:View>
                    <asp:View ID="Review_View" runat="server">      
                                <div class="panel panel-primary">
                                        <div class="panel-body">                                    
                            <asp:ListView ID="Review_List" runat="server" InsertItemPosition="LastItem">
                                <LayoutTemplate>
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <InsertItemTemplate>         
                                    <div class="panel panel-primary">
                                        <div class="panel-body">   
                                            <asp:PlaceHolder ID="Add_Comment_PlaceHolder" runat="server">
                                                <div class="col-sm-12 col-xs-12">   
                                                    <div class="form-group formField">
                                                        <label for="Headline">Review Headline</label>							                
							                            <asp:TextBox ID="Headline" runat="server" MaxLength="255" CssClass="form-control border-color-2">
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator 
                                                            id="Headline_Required_Validation"
                                                            runat="server"
                                                            Text="Enter the Headline"
                                                            ForeColor="Red"
                                                            Display="Dynamic"
                                                            ValidationGroup="VG1"
                                                            ControlToValidate="Headline">
                                                        </asp:RequiredFieldValidator>
						                            </div>	
                                                </div>
                                                <div class="col-sm-12 col-xs-12">   
                                                    <div class="form-group formField">
                                                        <label for="Rating1">Rating</label>							                
                                                        <asp:Rating 
                                                            ID="Rating1" 
                                                            runat="server"
                                                            StarCssClass="ratingStar"
                                                            FilledStarCssClass="filledRatingStar"
                                                            EmptyStarCssClass="emptyRatingStar"
                                                            WaitingStarCssClass="savedRatingStar" 
                                                            MaxRating="5" 
                                                            CurrentRating="0"></asp:Rating>                                                        
                                                        <asp:Label ID="Rating_Message" runat="server" ForeColor="Red"></asp:Label>                                                         
						                            </div>	

                                                </div>
                                                <div class="col-sm-12 col-xs-12">   
                                                    <div class="form-group">
                                                        <label for="Comment">Comment</label>							                
							                            <asp:TextBox ID="Comment" runat="server" Rows="5" TextMode="MultiLine" CssClass="form-control border-color-2">
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator 
                                                            id="Comment_Required_Validation"
                                                            runat="server"
                                                            Text="Enter the Comments"
                                                            ForeColor="Red"
                                                            Display="Dynamic"
                                                            ValidationGroup="VG1"
                                                            ControlToValidate="Comment">
                                                        </asp:RequiredFieldValidator>
						                            </div>	
                                                </div>  
                                                <div class="col-sm-12 col-xs-12">   
                                                       <asp:Label ID="Completion_Message" runat="server" Text="Thank you for sharing your feedback, Will be posted after reviewing the content" Visible="false" ForeColor="green"></asp:Label>
                                                </div>
                                                <div class="col-sm-6 col-xs-12">   
                                                    <div class="form-group formField">
                                                        <asp:Button ID="Submit_Button" 
                                                            runat="Server" 
                                                            Text="Submit Message" 
                                                            CssClass="btn btn-primary" 
                                                            OnCommand="Submit_Button_Command" 
                                                            ValidationGroup="VG1"/>                
                                                    </div>
                                                </div> 
                                            </asp:PlaceHolder>       
                                            <asp:PlaceHolder ID="Review_Login_PlaceHolder" runat="server">
                                                <div class="col-sm-12 col-xs-12">   
                                                       <a href="~/Account/Login.aspx" runat="server">Login & Comment</a>
                                                </div>
                                            </asp:PlaceHolder>
                                        </div>                                          
                                    </div>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <div class="panel panel-primary">
                                        <div class="panel-heading ">
                                            <h3 class="panel-title"><%# Eval("HEAD_LINE") %> <%# check_reviewed(Eval("STATUS").ToString()) %> </h3>                            
                                        </div>
                                        <div class="panel-body">                                                           
                                             <h4>                                                      
                                                <asp:Rating 
                                                ID="Rating1" 
                                                runat="server"
                                                StarCssClass="ratingStar"
                                                FilledStarCssClass="filledRatingStar"
                                                EmptyStarCssClass="emptyRatingStar"
                                                WaitingStarCssClass="savedRatingStar" 
                                                MaxRating="5"                                                             
                                                CurrentRating='<%# Eval("RATING") %>' ReadOnly="true"></asp:Rating>
                                            </h4>                                           
                                            <p><%# Eval("REVIEW_COMMENT") %></p>
                                            <cite><%# Eval("USER_NAME") %></cite>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>              
                                            </div>
                                    </div>                                                  
                    </asp:View>
                </asp:MultiView>                
            </div>
        </div>
                                            </ContentTemplate>
                </asp:UpdatePanel>
    </div>         
</section>


                  


