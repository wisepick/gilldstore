using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Open_Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                CommonClass.FetchRecordsAndBind("GET_OPEN_SHOPPING_CART",
                    dbconn,
                    GridView1);
                dbconn.Close();
                
            }
        }

        protected void View_Detail_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.FetchRecordsAndBind("GET_SHOPPING_CART_DETAILS",
                new string[]
                {
                    "P_SHOPPING_CART_ID",
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    e.CommandArgument.ToString(),
                    null,
                },
                dbconn,
                Shopping_Cart_Details_View);
            dbconn.Close();
        }

        protected void Send_Email_Command(object sender, CommandEventArgs e)
        {
            string[] lArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Messages.Send_Missed_Order_Message(lArgs[0],
                lArgs[1],
                dbconn);
            dbconn.Close();
        }
    }
}