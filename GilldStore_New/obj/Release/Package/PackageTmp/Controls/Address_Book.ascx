<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address_Book.ascx.cs" Inherits="GilldStore_New.Controls.Address_Book" %>
<asp:HiddenField ID="Display_AddressView_Flag" runat="server" />
<asp:HiddenField ID="Selected_AddressId_Value" runat="server" />
<asp:HiddenField ID="Selected_Address_PinCode" runat="server" />
<asp:HiddenField ID="Selected_Address_State" runat="server" />
<asp:HiddenField ID="Default_Mode" runat="server" />
<asp:HiddenField ID="USER_ID" runat="server" />

<!-- ADDRESSBOOK SECTION -->
          
                <div class="col-sm-12 col-xs-12">   
                    <asp:Button ID="Add_New_Button" runat="server" Text="Add New" OnClick="Add_New_Button_Click" CssClass="btn btn-primary"/>
                </div>      
                <div class="col-md-12  col-sm-6 col-xs-12">                    
                    <div class="panel panel-default formPanel">
                        <div class="panel-heading bg-color-4 border-color-4">
                            <h3 class="panel-title">Address</h3>                            
                        </div>
                        <div class="panel-body">                
                            <asp:ListView 
                                ID="User_Address_ListView" 
                                runat="server" 
                                InsertItemPosition="FirstItem" 
                                OnItemEditing="User_Address_ListView_ItemEditing" 
                                DataKeyNames="ADDRESS_ID, STATE_ID, CITY_ID, PIN_CODE" 
                                OnItemCanceling="User_Address_ListView_ItemCanceling"  >            
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>                
                                </LayoutTemplate>            
                                <InsertItemTemplate>                                    
                                    <div class="col-sm-6 col-xs-12">   
                                         <div class="form-group formField">
                                            <label for="User_Name">Name</label>							                
							                <asp:TextBox ID="User_Name" runat="server" MaxLength="40" CssClass="form-control border-color-2">
                                            </asp:TextBox>
						                </div>	
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
                                            <label for="Mobile_Number">Mobile Number</label>							                
							                <asp:TextBox ID="Mobile_Number" runat="server" MaxLength="10" CssClass="form-control border-color-2">
                                            </asp:TextBox>
						                </div>	                       
                                    </div>     	    
						            <div class="col-sm-12  col-xs-12">   
                                        <div class="form-group formField">
    							            <label for="Shipping_Address">Address</label>
	    						            <asp:TextBox ID="Shipping_Address" runat="server" MaxLength="120" Columns="40" Rows="4" TextMode="MultiLine" CssClass="form-control border-color-2">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Adress_Custom_Validator" runat="server" Text="Address is mandatory" ControlToValidate="Shipping_Address" ValidationGroup="Address_Book" Display="Dynamic" ForeColor="Red">

                                    </asp:RequiredFieldValidator>							    
						                </div>
                                    </div>	
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">                                            
							                <label for="State_Id">State</label>
							                <asp:DropDownList ID="State_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" OnSelectedIndexChanged="State_Id_OnSelectedIndexChanged" AutoPostBack="true" CssClass="form-control border-color-2">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="State_Validation" runat="server" Text="State is mandatory" ControlToValidate="State_Id" ValidateEmptyText="true" ValidationGroup="Address_Book"  Display="Dynamic" ForeColor="Red">
                                            </asp:RequiredFieldValidator>
						                </div>	
                                    </div>                                    
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
							                <label for="City_Id">City</label>
							                <asp:DropDownList ID="City_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" CssClass="form-control border-color-2">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
						                </div>	
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
							                <label for="City_Name">City (If Others)</label>
							                <asp:TextBox ID="City_Name" runat="server" MaxLength="255" CssClass="form-control border-color-2">
                                            </asp:TextBox>
                                            <asp:CustomValidator ID="City_Validation" runat="server" Text="City is mandatory" ControlToValidate="City_Id" ValidateEmptyText="true" ValidationGroup="Address_Book" OnServerValidate="Validate_City" Display="Dynamic" ForeColor="Red">
                                            </asp:CustomValidator>
						                </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">                                        
							                <label for="Pin_Code">Pin Code</label>
							                <asp:TextBox ID="Pin_Code" runat="server" MaxLength="6" CssClass="form-control border-color-2">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PinCode_Validation" runat="server" Text="Pin Code is mandatory" ControlToValidate="Pin_Code" ValidateEmptyText="true" ValidationGroup="Address_Book" OnServerValidate="Validate_City" Display="Dynamic" ForeColor="Red">
                                            </asp:RequiredFieldValidator>
						                </div>        
                                    </div>
                                    <div class="col-sm-12 col-xs-12">   
                                        <div class="form-group formField">   
							                <asp:Button ID="Save_Button" runat="server" Text="Save" ValidationGroup="Address_Book" CssClass="btn btn-primary" OnCommand="Address_Book_On_Command" CommandName="Save"/>
                                            <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" CommandName="Cancel"/>
						                </div>            
                                    </div>                                                                      
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <div class="col-sm-6 col-xs-12">   
                                         <div class="form-group formField">
                                            <label for="User_Name">Name</label>							                
							                <asp:TextBox ID="User_Name" runat="server" MaxLength="40" CssClass="form-control border-color-2" Text='<%# Eval("USER_NAME") %>'>
                                            </asp:TextBox>
						                </div>	
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
                                            <label for="Mobile_Number">Mobile Number</label>							                
							                <asp:TextBox ID="Mobile_Number" runat="server" MaxLength="10" CssClass="form-control border-color-2" Text='<%# Eval("MOBILE_NUMBER") %>'>
                                            </asp:TextBox>
						                </div>	                       
                                    </div>     	    
						            <div class="col-sm-12  col-xs-12">   
                                        <div class="form-group formField">
    							            <label for="Shipping_Address">Address</label>
	    						            <asp:TextBox ID="Shipping_Address" runat="server" MaxLength="120" Columns="40" Rows="4" TextMode="MultiLine" CssClass="form-control border-color-2" Text='<%# Eval("SHIPPING_ADDRESS").ToString().Replace("<br>", Environment.NewLine) %>'>
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Text="Address is mandatory" ControlToValidate="Shipping_Address" ValidationGroup="Address_Book" Display="Dynamic" ForeColor="Red">

                                    </asp:RequiredFieldValidator>							    
						                </div>
                                    </div>	
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">                                            
							                <label for="State_Id">State</label>
							                <asp:DropDownList ID="State_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" OnSelectedIndexChanged="State_Id_OnSelectedIndexChanged" AutoPostBack="true" CssClass="form-control border-color-2">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="State_Validation" runat="server" Text="State is mandatory" ControlToValidate="State_Id" ValidateEmptyText="true" ValidationGroup="Address_Book"  Display="Dynamic" ForeColor="Red">
                                            </asp:RequiredFieldValidator>
						                </div>	
                                    </div>                                    
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
							                <label for="City_Id">City</label>
							                <asp:DropDownList ID="City_Id" runat="server" AppendDataBoundItems="true" DataTextField="ATTRIBUTE_NAME" DataValueField="ATTRIBUTE_ID" CssClass="form-control border-color-2">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
						                </div>	
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">
							                <label for="City_Name">City (If Others)</label>
							                <asp:TextBox ID="City_Name" runat="server" MaxLength="255" CssClass="form-control border-color-2">
                                            </asp:TextBox>
                                            <asp:CustomValidator ID="City_Validation" runat="server" Text="City is mandatory" ControlToValidate="City_Id" ValidateEmptyText="true" ValidationGroup="Address_Book" OnServerValidate="Validate_City" Display="Dynamic" ForeColor="Red">
                                            </asp:CustomValidator>
						                </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12">   
                                        <div class="form-group formField">                                        
							                <label for="Pin_Code">Pin Code</label>
							                <asp:TextBox ID="Pin_Code" runat="server" MaxLength="6" CssClass="form-control border-color-2" Text='<%# Eval("PIN_CODE") %>'>
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PinCode_Validation" runat="server" Text="Pin Code is mandatory" ControlToValidate="Pin_Code" ValidateEmptyText="true" ValidationGroup="Address_Book" OnServerValidate="Validate_City" Display="Dynamic" ForeColor="Red" >
                                            </asp:RequiredFieldValidator>
						                </div>        
                                    </div>
                                    <div class="col-sm-12 col-xs-12">   
                                        <div class="form-group formField">   
							                <asp:Button ID="Save_Button" runat="server" Text="Save" ValidationGroup="Address_Book" CssClass="btn btn-primary" OnCommand="Address_Book_On_Command" CommandName="Save"/>
                                            <asp:Button ID="Cancel_Button" runat="server" Text="Cancel" CssClass="btn btn-primary" CommandName="Cancel"/>
						                </div>            
                                    </div>           
                                </EditItemTemplate>
                                <ItemTemplate>       
                                    <div class="col-sm-12 col-xs-12">   
                                        <div class="form-group formField">  
                                            <asp:RadioButton ID="Address_RadioButton" runat="server" ValidationGroup="ADDRESS_RADIO" OnCheckedChanged="Address_RadioButton_CheckedChanged" AutoPostBack="true" Text="Deliver to this Address" Visible='<%# Check_Eligiblity() %>'/>                                            
                                        </div>
                                    </div>
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="form-group">
                                            <%# GilldStore_New.App_Start.CommonClass.Format_Address(Eval("USER_NAME").ToString(), Eval("MOBILE_NUMBER").ToString(), Eval("SHIPPING_ADDRESS").ToString(), Eval("CITY_NAME").ToString(), Eval("STATE_NAME").ToString(), Eval("PIN_CODE").ToString()) %>
                                        </div>
                                    </div>
                                    <div class="col-sm-12 col-xs-12">   
                                        <div class="form-group formField">  
                                            <asp:ImageButton ID="Edit_Button" runat="server" AlternateText="Edit" ToolTip="Edit" ImageUrl="~/img/EDIT.png" CommandName="Edit"/>
                                            <asp:ImageButton ID="Delete_Button" runat="server" AlternateText="Delete" ToolTip="Delete" ImageUrl="~/img/cross.png" CommandName="Delete_Address" OnCommand="Address_Book_On_Command" CommandArgument='<%# Eval("ADDRESS_ID") %>'/>
                                            <hr>   
                                        </div>
                                    </div>            
                                         
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            