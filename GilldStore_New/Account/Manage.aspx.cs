using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Bind_User_Profile(dbconn);
                dbconn.Close();
            }
            
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
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
                    Generate_GUID();
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


        protected void Generate_GUID()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            MySqlTransaction Transaction = dbconn.BeginTransaction();
            //            Repeater lEmailRepeater = User_FormView.FindControl("Email_Repeater") as Repeater;
            Label lEmailMessageLabel = User_FormView.FindControl("Email_Validation_Message") as Label;
            //lEmailMessageLabel.Visible = true;
            try
            {
                string lEmail = Server.HtmlEncode((User_FormView.FindControl("Email_Address") as TextBox).Text);
                string lUserName = Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text);
                Guid g = Guid.NewGuid();
                Guid lUserGuid = Guid.NewGuid();
                CommonClass.ExecuteQuery("ADD_GUID",
                    new string[]
                    {
                        "P_EXTERNAL_USER_ID",
                        "P_EMAIL",
                        "P_GUID",
                        "P_USER_GUID"
                    },
                    new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,                   
                        lEmail,
                        g.ToString(),
                        lUserGuid.ToString()
                    },
                    dbconn);


                Messages.Send_Welcome_Email_Validate_Message(lEmail, lUserName, g.ToString(), lUserGuid.ToString(), dbconn);

                // Bind_Contacts("2", dbconn, lEmailRepeater);
                Transaction.Commit();

                lEmailMessageLabel.Text = "Validation Email Sent";
                //User_FormView.FindControl("Email_PlaceHolder").Visible = true;
                lEmailMessageLabel.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                lEmailMessageLabel.Text = ex.Message;
                lEmailMessageLabel.Visible = true;
                lEmailMessageLabel.ForeColor = Color.Red;
            }
            finally
            {
                dbconn.Close();
            }
        }

        protected void Generate_OTP()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            MySqlTransaction Transaction = dbconn.BeginTransaction();
            Repeater lMobileNumberRepeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            Label lOTPMessageLabel = User_FormView.FindControl("OTP_Message") as Label;
            lOTPMessageLabel.Visible = true;
            try
            {
                string lMobileNumber = Server.HtmlEncode((User_FormView.FindControl("Mobile_Number") as TextBox).Text);
                string lUserName = Server.HtmlEncode((User_FormView.FindControl("User_Name") as TextBox).Text);
                string[] lRecords = CommonClass.FetchRecords("GENERATE_MOBILE_ACTIVATION_CODE",
                    new string[] 
                    { 
                        "P_USER_ID",
                        "P_MOBILE_NUMBER"
                    },
                    new string[] 
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        lMobileNumber
                    },
                    new string[] 
                    {
                        "P_ACTIVATION_CODE"
                    },
                    dbconn);

                if (lRecords[0] != null)
                {
                    Messages.Send_Mobile_Validate_Message(lMobileNumber, lUserName, lRecords[0], dbconn);
                    Bind_Contacts("1", dbconn, lMobileNumberRepeater);
                    User_FormView.FindControl("Mobile_PlaceHolder").Visible = true;
                    lOTPMessageLabel.Text = "OTP Successfully Generated";
                    lOTPMessageLabel.ForeColor = Color.Green;
                }
                else
                {
                    lOTPMessageLabel.ForeColor = Color.Red;
                    lOTPMessageLabel.Text = "Unable to Generate OTP";
                }
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                lOTPMessageLabel.Text = "Server Error, Try after sometime";
                lOTPMessageLabel.ForeColor = Color.Red;
            }
            finally
            {
                dbconn.Close();
            }
        }

        protected void Manage_User(object sender, CommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Regenerate OTP")
            {
                Generate_OTP();
            }
            else if (e.CommandName.ToString() == "Resend GUID")
            {
                Generate_GUID();
            }
            else if (e.CommandName.ToString() == "Validate OTP")
            {
                Validate_Mobile();
            }
            else if (e.CommandName.ToString() == "View Order")
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                OrderInfo1.Populate_Orders(dbconn, e.CommandArgument.ToString());
                dbconn.Close();
                MultiView1.SetActiveView(Order_Details_View);
            }
            else //Validate Email
            {
                Validate_Email();
            }
        }

        protected void Validate_Mobile()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            TextBox lMobile_Number_TextBox = User_FormView.FindControl("Mobile_Number") as TextBox;
            Repeater lMobile_Number_Repeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            RepeaterItem lRepeaterItem1 = lMobile_Number_Repeater.Items[0];
            string[] lRecords = CommonClass.FetchRecords("VALIDATE_MOBILE",
                new string[]
                    {
                        "P_USER_ID",
                        "P_MOBILE_NUMBER",
                        "P_VALIDATION_CODE"
                    },
                new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        Server.HtmlEncode(lMobile_Number_TextBox.Text),
                        Server.HtmlEncode((lRepeaterItem1.FindControl("Mobile_OTP") as TextBox).Text)
                    },
                new string[]
                    {
                        "P_STATUS"
                    },
                dbconn);

            Bind_Contacts("1", dbconn, lMobile_Number_Repeater);
            dbconn.Close();

            Label OTP_Message_Label = User_FormView.FindControl("OTP_Message") as Label;
            OTP_Message_Label.Visible = true;
            if (lRecords[0] != null)
            {
                OTP_Message_Label.Text = lRecords[0];
                OTP_Message_Label.ForeColor = Color.Red;
            }
            else
            {
                User_FormView.FindControl("Mobile_PlaceHolder").Visible = false;
                OTP_Message_Label.Text = "OTP Successfully Validated";
                OTP_Message_Label.ForeColor = Color.Green;
            }
        }

        protected void Validate_Email()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            TextBox lEmail_Address_TextBox = User_FormView.FindControl("Email_Address") as TextBox;
            Repeater lEmail_Address_Repeater = User_FormView.FindControl("Email_Repeater") as Repeater;
            RepeaterItem lRepeaterItem1 = lEmail_Address_Repeater.Items[0];
            string[] lRecords = CommonClass.FetchRecords("VALIDATE_EMAIL",
                new string[]
                    {
                        "P_USER_ID",
                        "P_EMAIL",
                        "P_EMAIL_VALIDATION_CODE"
                    },
                new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        Server.HtmlEncode(lEmail_Address_TextBox.Text),
                        Server.HtmlEncode((lRepeaterItem1.FindControl("Email_Guid") as TextBox).Text)
                    },
                new string[]
                    {
                        "P_STATUS"
                    },
                dbconn);

            Bind_Contacts("2", dbconn, lEmail_Address_Repeater);
            dbconn.Close();

            Label GUID_Message_Label = User_FormView.FindControl("Email_Validation_Message") as Label;
            GUID_Message_Label.Visible = true;
            if (lRecords[0] != null)
            {
                GUID_Message_Label.Text = lRecords[0];
                GUID_Message_Label.ForeColor = Color.Red;
            }
            else
            {
                User_FormView.FindControl("Email_PlaceHolder").Visible = false;
                GUID_Message_Label.Text = "Successfully Validated";
                GUID_Message_Label.ForeColor = Color.Green;
            }
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
            Bind_User_Profile(dbconn);
            dbconn.Close();
        }

        protected void Link_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            if (e.CommandArgument.ToString() == "0")
            {
                Bind_User_Profile(dbconn);
                MultiView1.SetActiveView(Profile_View);
            }
            else if (e.CommandArgument.ToString() == "1")
            {

                AddressBook1.Load_Address(dbconn);

                MultiView1.SetActiveView(Address_View);
            }
            else if (e.CommandArgument.ToString() == "2")
            {
                SqlDataSource1.EnableCaching = false;
                SqlDataSource1.SelectParameters["P_EXTERNAL_USER_ID"].DefaultValue = ClaimsPrincipal.Current.FindFirst("user_id").Value;
                SqlDataSource1.DataBind();

                Order_ListView.DataBind();
                MultiView1.SetActiveView(Orders_View);
            }
            dbconn.Close();

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

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

        protected void Order_ListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

    }
}