using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Validate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();

                string[] lRecords = CommonClass.FetchRecords("VALIDATE_EMAIL",
               new string[]
                    {
                        "P_USER_ID_VALIDATION_CODE",                        
                        "P_EMAIL_VALIDATION_CODE"
                    },
               new string[]
                    {
                        Request.QueryString["uid"].ToString(),
                        Request.QueryString["guid"].ToString()                        
                    },
               new string[]
                    {
                        "P_STATUS"
                    },
               dbconn);
                if (lRecords[0] != null)
                {
                    Message.Text = lRecords[0];
                }
                else
                {
                    Message.Text = "Successfully Validated.. Proceed on to <a href=Account/Login.aspx>Login</a>";
                }
                dbconn.Close();
            }
        }
    }
}