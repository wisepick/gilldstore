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
    public partial class Contact : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                if (lUserModel != null)
                {
                    User_Name.Text = lUserModel.User_Name;
                    Email_Address.Text = lUserModel.Email_Address;
                }
                dbconn.Close();
            }
        }

        protected void Contact_OnCommand(object sender, CommandEventArgs e)
        {

            Messages.Send_Feedback_Message(Server.HtmlEncode(Email_Address.Text),
                Server.HtmlEncode(User_Name.Text),
                Server.HtmlEncode(Message.Text).Replace(Environment.NewLine, "<br>"));
            Messages.send_Email(Application["ADMIN_ADDRESS"].ToString(), Application["ADMIN_ADDRESS"].ToString(),
                "Feedback Recieved from " + Server.HtmlEncode(User_Name.Text),
                "Feedback Received from " + Server.HtmlEncode(User_Name.Text) + "<br><br>" +
                "Email Address : " + Server.HtmlEncode(Email_Address.Text) + "<br><br>" +
                "Feedback Details : <br><br>" + Server.HtmlEncode(Message.Text).Replace(Environment.NewLine, "<br>") + "<br><br>");
            Email_Address.Text = "";
            User_Name.Text = "";
            Message.Text = "";
            Final_Message.Text = "Thank you for sharing your feedback, We shall come back to you shortly";
        }
    }
}