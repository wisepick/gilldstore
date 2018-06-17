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

namespace GilldStore_New
{
    public partial class CheckOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserModel lUserModel;
                lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                bool lRedirect = false;
                if (lUserModel.Email_Address_Validated == 0 || lUserModel.Mobile_Validated == 0)
                {
                    lRedirect = true;
                }
                dbconn.Close();
                if (lRedirect)
                {
                    Response.Redirect("~/Account/Validate.aspx");
                }

            }
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();

                AddressBook1.Load_Address(dbconn, true);
                dbconn.Close();
            }
        }
    }
}