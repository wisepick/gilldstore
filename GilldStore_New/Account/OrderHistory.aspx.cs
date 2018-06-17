using GilldStore_New.App_Start;
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
    public partial class OrderHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            Refresh_Data();
        }
        protected void Refresh_Data()
        {
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
            SqlDataSource1.EnableCaching = false;
            SqlDataSource1.SelectParameters["P_EXTERNAL_USER_ID"].DefaultValue = ClaimsPrincipal.Current.FindFirst("user_id").Value;
            SqlDataSource1.DataBind();

            Order_ListView.DataBind();
        }

        protected void Order_ListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (Order_ListView.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            (Order_ListView.FindControl("DataPager2") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            Refresh_Data();
        }

       
        protected void View_Details_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderInfo1.Populate_Orders(dbconn, e.CommandArgument.ToString());
            dbconn.Close();
            MultiView1.SetActiveView(View2);
        }

        protected void Back_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View1);
        }
    }
}