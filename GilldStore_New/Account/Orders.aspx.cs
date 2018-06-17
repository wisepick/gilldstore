using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != null)
            {
                string lPaymentStatus = "Order Accepted";
                string lPaymentStatus1 = lPaymentStatus;
                if (Request.QueryString["type"].ToString() == "2")
                {
                    lPaymentStatus = "Waiting for Refund Approval";
                    lPaymentStatus1 = lPaymentStatus;
                }
                else if (Request.QueryString["type"].ToString() == "3")
                {
                    lPaymentStatus = "Shipped";
                    lPaymentStatus1 = lPaymentStatus;
                }
                else if (Request.QueryString["type"].ToString() == "4")
                {
                    lPaymentStatus = "Refund Initiated";
                    lPaymentStatus1 = lPaymentStatus;

                }
                else if (Request.QueryString["type"].ToString() == "5")
                {
                    lPaymentStatus = "Payment Failed";
                    lPaymentStatus1 = "Open";
                }
                else if (Request.QueryString["type"].ToString() == "6")
                {
                    lPaymentStatus = "Open";
                    lPaymentStatus1 = lPaymentStatus;
                }
                else if (Request.QueryString["type"].ToString() == "7")
                {
                    lPaymentStatus = "Order Delivered";
                    lPaymentStatus1 = lPaymentStatus;
                }
                OrderGrid1.Load_Orders(lPaymentStatus,
                    lPaymentStatus1);
            }
        }
    }
}