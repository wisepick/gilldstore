using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Address_Book : System.Web.UI.UserControl
    {
        public event EventHandler AddressSelected;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Load_Address(MySqlConnection dbconn,
            bool SetInsertPosition)
        {
            if (SetInsertPosition == true)
            {
                string lUserid = null;
                if (ClaimsPrincipal.Current.FindFirst("user_id") != null)
                {
                    lUserid = ClaimsPrincipal.Current.FindFirst("user_id").Value;
                }
                MySqlDataReader reader = CommonClass.FetchRecords("GET_USER_ADDRESS",
                    new string[]
                {
                    "P_USER_ID",
                    "P_EXTERNAL_USER_ID"
                },
                    new string[]
                {
                    USER_ID.Value,
                    lUserid
                },
                    dbconn);

                if (reader.Read())
                {
                    User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
                }
                else
                {
                    User_Address_ListView.InsertItemPosition = InsertItemPosition.FirstItem;
                }

                reader.Close();
            }

            Load_Address(dbconn);



        }

        public void Load_Address(MySqlConnection dbconn)
        {
            string lUserid = null;
            if (ClaimsPrincipal.Current.FindFirst("user_id") != null)
            {
                lUserid = ClaimsPrincipal.Current.FindFirst("user_id").Value;
            }
            CommonClass.FetchRecordsAndBind("GET_USER_ADDRESS",
                new string[]
                {
                    "P_USER_ID",
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    USER_ID.Value,
                    lUserid
                },
                dbconn,
                User_Address_ListView);
            if (User_Address_ListView.InsertItemPosition == InsertItemPosition.FirstItem)
            {
                CommonClass.Load_Attributes(dbconn, "States", User_Address_ListView.InsertItem.FindControl("STATE_ID"));
                UserDetails lUserDetails;
                string lUserId = USER_ID.Value;
                if (USER_ID.Value == "")
                {
                    UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                    lUserId = lUserModel.User_Id.ToString();
                }

                lUserDetails = Messages.Get_Contact_Details(lUserId, dbconn);

                if (lUserDetails != null)
                {
                    (User_Address_ListView.InsertItem.FindControl("USER_NAME") as TextBox).Text = lUserDetails.User_Name;
                    (User_Address_ListView.InsertItem.FindControl("MOBILE_NUMBER") as TextBox).Text = lUserDetails.PUblic_Mobile_Number;
                }
            }
            else if (User_Address_ListView.EditIndex != -1)
            {
                DropDownList lStateIdDropDownList = User_Address_ListView.EditItem.FindControl("STATE_ID") as DropDownList;
                CommonClass.Load_Attributes(dbconn, "States", lStateIdDropDownList);
                lStateIdDropDownList.SelectedValue = User_Address_ListView.DataKeys[User_Address_ListView.EditIndex].Values[1].ToString();
                State_Id_OnSelectedIndexChanged(lStateIdDropDownList, EventArgs.Empty);
                (User_Address_ListView.EditItem.FindControl("CITY_ID") as DropDownList).SelectedValue = User_Address_ListView.DataKeys[User_Address_ListView.EditIndex].Values[2].ToString();
            }
            Selected_AddressId_Value.Value = "";
            Selected_Address_PinCode.Value = "";
            Selected_Address_State.Value = "";
            if (Session["DELIVERY_ADDRESS_ID"] != null)
            {
                foreach(ListViewItem lListViewItem in User_Address_ListView.Items)
                {
                    if (User_Address_ListView.DataKeys[lListViewItem.DataItemIndex].Values[0].ToString() == Session["DELIVERY_ADDRESS_ID"].ToString())
                    {
                        DataKey lDataKey = User_Address_ListView.DataKeys[lListViewItem.DataItemIndex];
                        RadioButton lRadioButton = lListViewItem.FindControl("Address_RadioButton") as RadioButton;
                        if(lRadioButton != null)
                        {
                            lRadioButton.Checked = true;
                        }
                            
                        Selected_AddressId_Value.Value = lDataKey.Values[0].ToString();
                        Selected_Address_PinCode.Value = lDataKey.Values[3].ToString();
                        Selected_Address_State.Value = lDataKey.Values[1].ToString();
                        Session["PIN_CODE"] = Selected_Address_PinCode.Value;
                    }
                }
            }
            else if (User_Address_ListView.Items.Count == 1)
            {
                if (User_Address_ListView.EditIndex == -1)
                {
                    (User_Address_ListView.Items[0].FindControl("Address_RadioButton") as RadioButton).Checked = true;
                    Selected_AddressId_Value.Value = User_Address_ListView.DataKeys[0].Values[0].ToString();
                    Selected_Address_PinCode.Value = User_Address_ListView.DataKeys[0].Values[3].ToString();
                    Selected_Address_State.Value = User_Address_ListView.DataKeys[0].Values[1].ToString();
                    Session["PIN_CODE"] = Selected_Address_PinCode.Value;
                }
            }
            else
            {
                Session["PIN_CODE"] = null;
            }

            if (AddressSelected != null)
            {
                AddressSelected(this, EventArgs.Empty);
            }
        }



        protected void Validate_City(object sender, ServerValidateEventArgs e)
        {
            CustomValidator lCustomValidator = (CustomValidator)sender;
            if ((lCustomValidator.Parent.FindControl("CITY_ID") as DropDownList).SelectedValue == "" &&
                (lCustomValidator.Parent.FindControl("CITY_NAME") as TextBox).Text == "")
            {
                e.IsValid = false;
            }
        }

        protected void State_Id_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList lStateId_DDL = (DropDownList)sender;
            DropDownList lCityId_DDL = (DropDownList)lStateId_DDL.Parent.FindControl("CITY_ID");

            CommonClass.Create_Empty_DropDown_Value(lCityId_DDL);

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();

            CommonClass.FetchRecordsAndBind("GET_CITY_BY_STATE_ID",
                new string[]
                {
                    "P_STATE_ID"
                },
                new string[]
                {
                    lStateId_DDL.SelectedValue.ToString()
                },
                dbconn,
                lCityId_DDL
                );

            dbconn.Close();
            lCityId_DDL.Focus();
        }

        protected void Address_Book_On_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            if (e.CommandName == "Save" && Page.IsValid == true)
            {
                ListViewItem item;
                bool lEditFlag = false;


                if (User_Address_ListView.EditIndex != -1)
                {
                    lEditFlag = true;
                    item = User_Address_ListView.EditItem;
                }
                else
                {
                    item = User_Address_ListView.InsertItem;
                }

                if (lEditFlag == false)
                {
                    CommonClass.ExecuteQuery("ADD_ADDRESS",
                        new string[]
                   {
                       "P_USER_NAME",
                       "P_MOBILE_NUMBER",
                       "P_SHIPPING_ADDRESS",                       
                       "P_CITY_ID",
                       "P_CITY_NAME",
                       "P_STATE_ID",                       
                       "P_PIN_CODE",
                       "P_USER_ID",
                       "P_EXTERNAL_USER_ID"
                   },
                        new string[]
                   {
                       Server.HtmlEncode((item.FindControl("User_Name") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("Mobile_Number") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("Shipping_Address") as TextBox).Text).Replace(Environment.NewLine, "<br>"),                       
                       Server.HtmlEncode((item.FindControl("City_Id") as DropDownList).SelectedValue),
                       Server.HtmlEncode((item.FindControl("City_Name") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("State_Id") as DropDownList).SelectedValue),
                       Server.HtmlEncode((item.FindControl("PIn_Code") as TextBox).Text),
                       USER_ID.Value,
                       ClaimsPrincipal.Current.FindFirst("user_id").Value

                   },
                        dbconn
                        );
                    User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
                    Load_Address(dbconn);
                    User_Address_ListView.Focus();
                }
                else
                {
                    CommonClass.ExecuteQuery("UPDATE_ADDRESS",
                        new string[]
                   {
                       "P_USER_ID",
                       "P_EXTERNAL_USER_ID",
                       "P_ADDRESS_ID",
                       "P_USER_NAME",
                       "P_MOBILE_NUMBER",
                       "P_SHIPPING_ADDRESS",                       
                       "P_CITY_ID",
                       "P_CITY_NAME",
                       "P_STATE_ID",                       
                       "P_PIN_CODE"                       
                   },
                        new string[]
                   {
                       USER_ID.Value,
                       ClaimsPrincipal.Current.FindFirst("user_id").Value,
                       User_Address_ListView.DataKeys[item.DataItemIndex].Values[0].ToString(),
                       Server.HtmlEncode((item.FindControl("User_Name") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("Mobile_Number") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("Shipping_Address") as TextBox).Text).Replace(Environment.NewLine,"<br>"),
                       Server.HtmlEncode((item.FindControl("City_Id") as DropDownList).SelectedValue),
                       Server.HtmlEncode((item.FindControl("City_Name") as TextBox).Text),
                       Server.HtmlEncode((item.FindControl("State_Id") as DropDownList).SelectedValue),
                       Server.HtmlEncode((item.FindControl("PIn_Code") as TextBox).Text),
                       

                   },
                        dbconn
                        );
                }
                User_Address_ListView.EditIndex = -1;
                //if (Default_Mode.Value == "None")
                // {
                User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
                //}
                //else
                //{
                //     User_Address_ListView.InsertItemPosition = InsertItemPosition.FirstItem;
                // }
                Load_Address(dbconn);
                User_Address_ListView.Focus();

            }
            else if (e.CommandName == "Delete_Address")
            {
                CommonClass.ExecuteQuery("DELETE_ADDRESS",
                    new string[]
                   {
                       "P_USER_ID",
                       "P_EXTERNAL_USER_ID",
                       "P_ADDRESS_ID"
                   },
                    new string[]
                   {
                       USER_ID.Value,
                       ClaimsPrincipal.Current.FindFirst("user_id").Value,
                       e.CommandArgument.ToString()
                   },
                    dbconn);
                User_Address_ListView.EditIndex = -1;
                //if (Default_Mode.Value == "None")
                //{
                //  User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
                // }
                //else
                //{
                //     User_Address_ListView.InsertItemPosition = InsertItemPosition.FirstItem;
                // }
                Load_Address(dbconn, true);
                User_Address_ListView.Focus();


            }
            dbconn.Close();
        }


        protected void User_Address_ListView_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            User_Address_ListView.EditIndex = e.NewEditIndex;
            User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
            Load_Address(dbconn);
            dbconn.Close();

        }

        protected void User_Address_ListView_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            User_Address_ListView.EditIndex = -1;
            //if (Default_Mode.Value == "None")
            //{
            // User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
            //}
            //else
            //{
            //   User_Address_ListView.InsertItemPosition = InsertItemPosition.FirstItem;
            //}
            Load_Address(dbconn, true);
            dbconn.Close();
        }

        protected void Address_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton RadioButton1 = (RadioButton)sender;
            foreach (ListViewItem item in User_Address_ListView.Items)
            {
                RadioButton lAddress_RadioButton = item.FindControl("Address_RadioButton") as RadioButton;
                if (lAddress_RadioButton.ClientID != RadioButton1.ClientID)
                {
                    (item.FindControl("Address_RadioButton") as RadioButton).Checked = false;
                }
                else
                {
                    Selected_AddressId_Value.Value = User_Address_ListView.DataKeys[item.DataItemIndex].Values[0].ToString();
                    Selected_Address_PinCode.Value = User_Address_ListView.DataKeys[item.DataItemIndex].Values[3].ToString();
                    Selected_Address_State.Value = User_Address_ListView.DataKeys[item.DataItemIndex].Values[1].ToString();
                    if (AddressSelected != null)
                    {
                        AddressSelected(this, EventArgs.Empty);
                    }
                }
            }
        }

        public string Display_DeliveryAddressOption
        {
            get
            {
                return Display_AddressView_Flag.Value;
            }
            set
            {
                Display_AddressView_Flag.Value = value;
            }
        }

        public string GetDeliveryAddressId
        {
            get
            {
                return Selected_AddressId_Value.Value;
            }
        }

        public string GetDeliveryState
        {
            get
            {
                return Selected_Address_State.Value;
            }
        }

        public string GetDeliveryPinCode
        {
            get
            {
                return Selected_Address_PinCode.Value;
            }
        }
        protected bool Check_Eligiblity()
        {
            if (Display_AddressView_Flag.Value == "N")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string Set_User_Id
        {
            set
            {
                USER_ID.Value = value;
            }
        }

        public string Set_ListView_Mode
        {
            set
            {
                Default_Mode.Value = value;
                if (value == "None")
                {
                    User_Address_ListView.InsertItemPosition = InsertItemPosition.None;
                }
            }
        }

        protected void Add_New_Button_Click(object sender, EventArgs e)
        {

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            User_Address_ListView.EditIndex = -1;
            User_Address_ListView.InsertItemPosition = InsertItemPosition.FirstItem;
            Load_Address(dbconn);
            dbconn.Close();
        }
    }
}