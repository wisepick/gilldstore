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

namespace GilldStore_New.Account
{
    public partial class Validate : System.Web.UI.Page
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
                Refresh_Validation_Page(dbconn);
                dbconn.Close();
            }
        }

        protected void Refresh_Validation_Page(MySqlConnection dbconn)
        {
            UserModel lUserModel;
            lUserModel = CommonClass.Get_External_User_Profile(dbconn);

            Header_Label.Text = "You are ";
            int lSteps = 0;
            Email_Placeholder.Visible = false;
            Mobile_Placeholder.Visible = false;
            if (lUserModel.Email_Address_Validated == 0)
            {
                lSteps++;
                Step1.Text = lSteps.ToString() + ". ";
                Email_Placeholder.Visible = true;
                Email_Address.Text = lUserModel.Email_Address;
            }
            else
            {
                if (lUserModel.Email_Delivery_Option == 1)
                {
                    UserClass.Add_To_MailChimp(lUserModel.Email_Address,
                        lUserModel.User_Name);
                }
            }

            if (lUserModel.Mobile_Validated == 0)
            {
                lSteps++;
                Step2.Text = lSteps.ToString() + ". ";
                Mobile_Placeholder.Visible = true;
                Mobile_Number.Text = lUserModel.Mobile_Number;

            }
            if (lSteps != 0)
            {
                Header_Label.Text = "You are " + lSteps.ToString() + " step ahead.";
                if (lUserModel.Email_Address_Validated == 0)
                {
                    Header_Label.Text += " After validation, press on Complete";
                }
            }
            else
            {
                string r_url = "~/";
             
                if (lUserModel.User_Type_Id == 3)
                {
                    r_url += "view_cart.aspx";
                }
                else
                {
                    r_url += "Account/Dashboard.aspx";
                }
             
                Response.Redirect(r_url);
            }

        }

        protected void Regenerate_Guid_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
            Messages.Generate_GUID(dbconn,
                lUserModel.Email_Address,
                lUserModel.User_Name);
            Validation_Email_Message.Text = "Validation Email sent";
            dbconn.Close();
        }



        protected void Mobile_Number_TextChanged(object sender, EventArgs e)
        {
            if (Mobile_Number.Text != "")
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                Messages.Generate_OTP(dbconn,
                    Server.HtmlEncode(Mobile_Number.Text),
                    lUserModel.User_Name,
                    Mobile_Validation_Message);
                dbconn.Close();
            }
            else
            {
                Mobile_Validation_Message.Text = "Mobile Number is mandatory";
                Mobile_Validation_Message.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Validate_OTP_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Messages.Validate_OTP(dbconn,
                Mobile_Number.Text,
                OTP.Text,
                Mobile_Validation_Message);
            Refresh_Validation_Page(dbconn);

            dbconn.Close();


        }

        protected void Change_Email_Adress_Button_Click(object sender, EventArgs e)
        {
            New_Email_Address_Placeholder.Visible = true;
        }

        protected void Submit_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
            Messages.Generate_GUID(dbconn,
                Server.HtmlEncode(New_Email_Address.Text),
                lUserModel.User_Name);
            Refresh_Validation_Page(dbconn);
            New_Email_Address_Placeholder.Visible = false;
            dbconn.Close();
        }

        protected void Complete_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Refresh_Validation_Page(dbconn);
            dbconn.Close();
        }
    }
}