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
    public partial class DueList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
        }

        protected void DueListView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void Send_Remainder_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Messages.Send_Due_Remainder(dbconn,
                e.CommandArgument.ToString());
            dbconn.Close();
        }

        protected void Customer_View_Button_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("CustomerView.aspx?ref=dl&Customer_Id=" + e.CommandArgument.ToString());
        }
    }
}