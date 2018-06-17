using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Product_Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Product_Id"] != null)
                {
                    MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                    dbconn.Open();
                    ProductDetailControl1.Refresh_Product_Details(dbconn,
                        Request.QueryString["Product_Id"].ToString());
                    dbconn.Close();
                }
            }
        }

        protected void Cart_Modified(object sender, CommandEventArgs e)
        {
            string[] lCommandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

            this.Master.Shopping_Cart_Amount = ShoppingCartClass.Add_To_Shopping_Cart(int.Parse(lCommandArgs[0]),
                        double.Parse(lCommandArgs[1]),
                        int.Parse(lCommandArgs[2]),
                        double.Parse(lCommandArgs[3]));
        }

    }
}