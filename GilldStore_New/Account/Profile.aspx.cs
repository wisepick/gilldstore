using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Bind_User_Profile(dbconn);
                dbconn.Close();
            }
        }

        protected bool Display_Validation(string lValidationIndicator)
        {
            if ((lValidationIndicator == "0" || lValidationIndicator != "") && lValidationIndicator != "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void Contact_TextChanged(object sender, EventArgs e)
        {

            TextBox lContactTextBox = (TextBox)sender;
            if (Server.HtmlEncode(lContactTextBox.Text).Trim() != "")
            {
                if (lContactTextBox.ID == "Mobile_Number" && Request.Params["__EVENTTARGET"].ToString().Contains(User_FormView.FindControl("Mobile_Number").ID))
                {
                    Generate_OTP();
                }
                else if (Request.Params["__EVENTTARGET"].ToString().Contains(User_FormView.FindControl("Email_Address").ID))
                {
                    MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                    dbconn.Open();
                    UserClass.Generate_GUID(Server.HtmlEncode((User_FormView.FindControl("Email_Address") as TextBox).Text),
                        Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text),
                        User_FormView.FindControl("Email_Validation_Message") as Label,
                        dbconn);
                    Repeater lEmailRepeater = User_FormView.FindControl("Email_Repeater") as Repeater;
                    Bind_Contacts("2", 
                        dbconn, 
                        lEmailRepeater);
                    dbconn.Close();
                        
                }
            }
            else
            {
                if (lContactTextBox.ID == "Mobile_Number")
                {
                    User_FormView.FindControl("OTP_Message").Visible = false;
                    User_FormView.FindControl("Mobile_PlaceHolder").Visible = false;
                }
                else
                {
                    User_FormView.FindControl("Email_Validation_Message").Visible = false;
                    User_FormView.FindControl("Email_PlaceHolder").Visible = false;
                }
            }
        }

        protected void Validate_OTP_Button_Command(object sender, CommandEventArgs e)
        {
            Repeater lMobile_Number_Repeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            RepeaterItem lRepeaterItem1 = lMobile_Number_Repeater.Items[0];
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            UserClass.Validate_Mobile(Server.HtmlEncode((User_FormView.FindControl("Mobile_Number") as TextBox).Text),
                Server.HtmlEncode((lRepeaterItem1.FindControl("Mobile_OTP") as TextBox).Text),
                User_FormView.FindControl("OTP_Message") as Label,
                User_FormView.FindControl("Mobile_PlaceHolder") as PlaceHolder,
                dbconn);
            Bind_Contacts("1", dbconn, lMobile_Number_Repeater);
            dbconn.Close();
        }

        protected void Update_User_Details(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string lMobileDeliveryOption = "0";
            string lEmailDeliveryOption = "0";
            if ((User_FormView.FindControl("Email_Delivery_Option") as CheckBox).Checked)
            {
                lEmailDeliveryOption = "1";
            }
            if ((User_FormView.FindControl("Mobile_Delivery_Option") as CheckBox).Checked)
            {
                lMobileDeliveryOption = "1";
            }
            CommonClass.ExecuteQuery("UPDATE_USER_DETAILS",
                new string[] {
                    "P_USER_ID",
                    "P_USER_NAME",
                    "P_MOBILE_NUMBER",
                    "P_EMAIL_DELIVERY_OPTION",
                    "P_MOBILE_DELIVERY_OPTION"
                },
                new string[] {
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text),
                    Server.HtmlEncode((User_FormView.FindControl("Mobile_Number") as TextBox).Text),
                    lEmailDeliveryOption,
                    lMobileDeliveryOption
                },
                dbconn
                );

            if (lEmailDeliveryOption == "1")
            {
                UserClass.Add_To_MailChimp((User_FormView.FindControl("Email_Address") as TextBox).Text,
                    Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text));
            }
            Bind_User_Profile(dbconn);
            dbconn.Close();
        }

        protected void Bind_User_Profile(MySqlConnection dbconn)
        {
            CommonClass.FetchRecordsAndBind("GET_USER_BY_EXTERNAL_ID",
                new string[] 
                {
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    ClaimsPrincipal.Current.FindFirst("user_id").Value
                },
                dbconn,
                User_FormView);
            Bind_Contacts("1",
                   dbconn,
                   User_FormView.FindControl("Mobile_Number_Repeater"));
            Bind_Contacts("2",
                dbconn,
                User_FormView.FindControl("Email_Repeater"));

        }

        protected void Bind_Contacts(string lContactType,
            MySqlConnection dbconn,
            Object obj)
        {
            CommonClass.FetchRecordsAndBind("GET_CONTACT_DETAILS",
                new string[]
                {                    
                    "P_CONTACT_ID_TYPE",
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    lContactType,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value
                },
                dbconn,
                obj);
        }

        protected void Resend_OTP_Button_Command(object sender, CommandEventArgs e)
        {
            Generate_OTP();
        }

        protected void Generate_OTP()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Repeater lMobileNumberRepeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            UserClass.Generate_OTP(Server.HtmlEncode((User_FormView.FindControl("Mobile_Number") as TextBox).Text),
                Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text),
                User_FormView.FindControl("OTP_Message") as Label,
                User_FormView.FindControl("Mobile_PlaceHolder") as PlaceHolder,
                dbconn);
            Bind_Contacts("1", dbconn, lMobileNumberRepeater);
            dbconn.Close();
        }
    }
}