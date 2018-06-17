using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Order_Grid : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
            SqlDataSource2.ConnectionString = CommonClass.connectionstring;
        }

        public void Load_Orders(string lOrderStatus, string lOrderStatus1)
        {
            Orders_GridView.DataSourceID = SqlDataSource1.ID;
            SqlDataSource1.SelectParameters["P_STATUS_NAME"].DefaultValue = lOrderStatus;
            SqlDataSource1.SelectParameters["P_STATUS_NAME_1"].DefaultValue = lOrderStatus1;
            SqlDataSource1.DataBind();
        }

        public void Search_Order()
        {
            Orders_GridView.DataSourceID = SqlDataSource2.ID;
            SqlDataSource2.SelectParameters["P_ORDER_ID"].DefaultValue = ORDER_ID.Value;
            SqlDataSource2.DataBind();
            MultiView1.SetActiveView(Order_Summary_View);

        }

        protected void View_Order_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderInfo1.Populate_Orders(dbconn, e.CommandArgument.ToString());

            dbconn.Close();
            MultiView1.SetActiveView(Order_Info_View);
        }

        protected void Orders_GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void Back_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Order_Summary_View);
            Orders_GridView.DataBind();
        }

        public string Order_Id
        {
            set
            {
                ORDER_ID.Value = value;
            }
        }
    }
}