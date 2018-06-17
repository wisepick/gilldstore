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
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (Session["USER_TYPE"] == null)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                if (lUserModel != null)
                {
                    Session["USER_TYPE"] = lUserModel.User_Type_Id.ToString();
                }
                dbconn.Close();
            }
            if (Session["USER_TYPE"].ToString() == "3")
            {
                Response.Redirect("~/");
            }
        }
    }
}