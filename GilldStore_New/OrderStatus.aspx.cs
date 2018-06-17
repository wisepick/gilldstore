using GilldStore_New.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class OrderStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ORDER_ID"] != null)
            {
                Response.Write(OrderClass.Get_Payment_Status(Request.QueryString["ORDER_ID"].ToString()));
            }

        }
    }
}