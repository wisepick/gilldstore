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
    public partial class OrderSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Order_Id"] != null)
                {
                    MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                    dbconn.Open();
                    OrderInfo1.Populate_Orders(dbconn, Request.QueryString["Order_Id"].ToString());
                    dbconn.Close();
                }
            }

        }
    }
}