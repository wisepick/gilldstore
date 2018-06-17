using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class approve_comments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["approval_id"] != null)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                CommonClass.ExecuteQuery("APPROVE_COMMENT",
                    new string[] { "P_APPROVAL_CODE" },
                    new string[] { Request.QueryString["approval_id"].ToString() },
                    dbconn);
                dbconn.Close();
            }
            Response.Redirect("~/");
        }
    }
}