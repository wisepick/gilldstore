<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="GilldStore_New.Account.Manage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Order_Info.ascx" TagName="OrderInfo" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:ScriptManager ID="scriptmanager1" runat="server" ></asp:ScriptManager>--%>
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></cc1:ToolkitScriptManager>
<%--    <asp:RequiredFieldValidator runat="server" ControlToValidate="dummyTextBox" ValidationGroup="dummy" CssClass="hidden"></asp:RequiredFieldValidator>
<asp:TextBox runat="server" ID="dummyTextBox" CssClass="hidden"></asp:TextBox>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>

        
    <section id="Manage_Profile" class="gray-bg padding-top-bottom">
        <div class="container">
            <div class="row">
                <asp:Button id="Profile_Link" runat="server" Text="Profile" CssClass="btn btn-primary" OnCommand="Link_Command" CommandArgument="0">
                </asp:Button>
                <asp:Button id="Address_Book_Link" runat="server" Text="Address Book" CssClass="btn btn-primary" OnCommand="Link_Command" CommandArgument="1">
                </asp:Button>
                <asp:Button id="Order_History_Link" runat="server" Text="Order History" CssClass="btn btn-primary" OnCommand="Link_Command" CommandArgument="2">
                </asp:Button><br /><br />
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="Profile_View" runat="server">
                        <div class="panel panel-primary">
                            <div class="panel-heading">Profile</div>
                            <div class="panel-body">
                                <asp:FormView ID="User_FormView" runat="server" DefaultMode="Edit" Width="75%" DataKeyNames="USER_ID, EXTERNAL_USER_ID, MOBILE_NUMBER, MOBILE_VALIDATED, EMAIL_ADDRESS_VALIDATED" >
                                    <EditItemTemplate>
                                        <asp:Label ID="Mobile_Email_Validation" runat="server" Text="Mobile Number and Email Address are mandatory and must be verified to process any orders" ForeColor="Red" Font-Size="Large" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("MOBILE_VALIDATED").ToString(), "0") ||  GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("EMAIL_ADDRESS_VALIDATED").ToString(), "0")%>'></asp:Label>
                                        <table class="table table-bordered" width="100%">
                                            <tr>
                                                <th>
                                                    Name
                                                </th>
                                                <td>
                                                    <asp:TextBox id="User_Name" runat="Server" MaxLength="255" CssClass="form-control" Text='<%# Eval("USER_NAME") %>'></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:RequiredFieldValidator ID="User_Name_Validation" runat="server" Text="User Name is mandatory" Display="Dynamic"  ControlToValidate="User_Name" ForeColor="Red">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Mobile Number
                                                </th>
                                                <td>
                                                    <asp:TextBox id="Mobile_Number" runat="Server" MaxLength="10" CssClass="form-control" Text='<%# Eval("MOBILE_NUMBER") %>' OnTextChanged="Contact_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="Mobile_Delivery_Option" runat="server" Text="Want to receive order information through mobile" Checked='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("MOBILE_DELIVERY_OPTION").ToString(), "1") %>'/>
                                                </td>
                                            </tr>
                                            <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="OTP_Message" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                            <asp:PlaceHolder ID="Mobile_PlaceHolder" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) && Display_Validation(Eval("MOBILE_NUMBER").ToString()) %>'>
                                                <asp:Repeater ID="Mobile_Number_Repeater" runat="server">
                                                    <ItemTemplate>                                                                                                        
                                                        <tr>                
                                                            <th>
                                                                <asp:Label ID="OTP_Label" runat="server" Text="Enter the OTP" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>'></asp:Label>
                                                            </th>                                        
                                                            <td >                                                            
                                                                <asp:TextBox ID="Mobile_OTP" runat="server" CssClass="form-control" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>'></asp:TextBox><br /><br />                                                            
                                                                <asp:Button ID="Validate_OTP_Button" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' Text="Validate OTP" CssClass="btn btn-primary"  CommandName="Validate OTP" OnCommand="Manage_User"/>
                                                                <asp:Button ID="Resend_OTP_Button" runat="server" Visible='<%# Display_Validation(Eval("MOBILE_VALIDATED").ToString()) %>' Text="Resend OTP" CssClass="btn btn-primary"  CommandName="Regenerate OTP" OnCommand="Manage_User"/>
                                                            </td>
                                                            <td>
                                                    &nbsp;
                                                </td>
                                                        </tr>                                            
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </asp:PlaceHolder>
                                             <tr>
                                                    <th>
                                                        Email Address
                                                    </th>
                                                    <td>
                                                        <asp:TextBox id="Email_Address" runat="Server" MaxLength="255" CssClass="form-control" Text='<%# Eval("EMAIL_ADDRESS") %>' AutoPostBack="true" OnTextChanged="Contact_TextChanged"></asp:TextBox>                                                                                                                                                            
                                                    </td>
                                                 <td>
                                                    <asp:CheckBox ID="Email_Delivery_Option" runat="server" Text="Want to receive order information through Email" Checked='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("EMAIL_DELIVERY_OPTION").ToString(), "1") %>'/>
                                                </td>
                                                </tr>
                                               <tr>
                                                    <td colspan="3">
                                                        <asp:RequiredFieldValidator ID="Email_Address_Validation" runat="server" ErrorMessage="Email Address is mandatory" Display="Dynamic"  ControlToValidate="Email_Address" ForeColor="Red" >
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label id="Email_Validation_Message" runat="server" ForeColor="Red" Visible='<%# Display_Validation(Eval("EMAIL_ADDRESS_VALIDATED").ToString()) %>'></asp:Label>
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td colspan="3">                                                    
                                                    <asp:Button ID="Submit_Button" runat="server" Text="Save" CssClass="btn btn-primary"   CausesValidation="true" OnCommand="Update_User_Details"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </asp:FormView>
                            </div>
                        </div>    
                    </asp:View>
                    <asp:View ID="Address_View" runat="server">
                        <uc1:AddressBook 
                            ID="AddressBook1" 
                            runat="server"                 
                            Display_DeliveryAddressOption="N"
                        />
                    </asp:View>
                    <asp:View ID="Orders_View" runat="server">
                         <!-- WHITE SECTION -->
    <section class="whiteSection full-width clearfix eventSection" id="ourEvents">
      <div class="container">
     

        <div class="row">
            
        
              <asp:ListView ID="Order_ListView" runat="server" DataKeyNames="ORDER_ID" GroupItemCount="2" OnPagePropertiesChanging="Order_ListView_PagePropertiesChanging" DataSourceID="SqlDataSource1"> 
                            <LayoutTemplate>              
                               
                                    <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>       
                                <div class="pagerArea text-center">
                                    <ul class="pager">
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="Order_ListView" PageSize="12" >
                                    
                    <Fields>
                        
                        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                            ShowNextPageButton="false" ButtonCssClass="prev"/>
                        <asp:NumericPagerField ButtonType="Link" />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false" ShowPreviousPageButton = "false"  ButtonCssClass="next"/>
                    </Fields>
                </asp:DataPager>                      
                                        </ul>             
                                    </div>                                                       
                            </LayoutTemplate>  
                            <GroupTemplate>   
             <div class="row">
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                
             </div>
                            </GroupTemplate>          
                            <ItemTemplate>  
                    
                               <div class="col-sm-6 col-xs-12 block">
            <div class='media eventContent bg-color-<%# Eval("RANK") %>'>
              <a class="media-left" href="single-event-left-sidebar.html">
                &nbsp;<%# Eval("ORDER_STATUS") %>
                  <asp:PlaceHolder ID="Order_Accepted_PlaceHolder" runat="server" Visible='<%# GilldStore_New.App_Start.CommonClass.Display_Attributes_IfMathes(Eval("ORDER_STATUS").ToString(), "Payment Failed") %>'>
                      <h1><i class="fa fa-times" aria-hidden="true"></i></h1>
                  </asp:PlaceHolder>
                  
                  <img class="media-object" src="img/home/event/event-1.jpg" alt="Image">
                <span class="sticker-round"><%# Eval("ORDER_DAY") %> <br><%# Eval("ORDER_MONTH") %><br /><%# Eval("ORDER_YEAR") %></span>
              </a>
              <div class="media-body">
                <h3 class="media-heading"><%# Eval("ORDER_NUMBER") %></h3>
                <ul class="list-unstyled">
                  <li><i class="fa fa-calendar-o" aria-hidden="true"></i>Age 2 to 4 Years</li>
                  <li><i class="fa fa-clock-o" aria-hidden="true"></i>9.00AM-11.00AM</li>
                </ul>
                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor </p>
                <ul class="list-inline btn-yellow">
                  <li><a href="single-event-left-sidebar.html" class="btn btn-primary">read more</a></li>
                </ul>
              </div>
            </div>
          </div>
          </ItemTemplate>
                  </asp:ListView>
        </div>
      </div>
    </section>

                            
                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" 
                            SelectCommand="GET_USER_ORDERS"
                            SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="P_EXTERNAL_USER_ID"  Direction="Input" ConvertEmptyStringToNull="false"/>                                                                
                            </SelectParameters>
                        </asp:SqlDataSource>                   
                    </asp:View>
                    <asp:View ID="Order_Details_View" runat="server">
                        <uc1:OrderInfo ID="OrderInfo1" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </section>
<%--</ContentTemplate>
    </asp:UpdatePanel>  --%>  
</asp:Content>

