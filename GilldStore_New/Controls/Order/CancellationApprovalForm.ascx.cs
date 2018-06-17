using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls.Order
{
    public partial class CancellationApprovalForm : System.Web.UI.UserControl
    {
        public event EventHandler ApprovalSuccess;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            if (Refund_Amount.Text == "")
            {
                Message.Text = "Refund Amount is Mandatory";
                return;
            }
            else
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                string lErrorString = OrderClass.Refund_Order(dbconn,
                    Order_Id.Value,
                    Order_Number.Value,
                    Customer_Id.Value,
                    Server.HtmlEncode(Refund_Amount.Text),
                    Transaction_Id.Value,//Original Transaction Id
                    "Cancelled Before Delivery");

                if (lErrorString != null)
                {
                    Message.Text = lErrorString;
                    return;
                }
                dbconn.Close();
            }

            Cancel_Button_Click(sender, e);
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (ApprovalSuccess != null)
            {
                ApprovalSuccess(this, e);
            }
        }

        protected void Refund_Percentage_TextChanged(object sender, EventArgs e)
        {
            double lEligibleRefund = double.Parse(Order_Amount.Value) - double.Parse(Shipping_Charge.Value);

            lEligibleRefund = (lEligibleRefund * double.Parse(Refund_Percentage.Text)) / 100;
            Refund_Amount.Text = lEligibleRefund.ToString();
        }

        public string OrderId
        {
            set
            {
                Order_Id.Value = value;
            }
        }

        public string OrderNumber
        {
            set
            {
                Order_Number.Value = value;
            }
        }

        public string OrderAmount
        {
            set
            {
                Order_Amount.Value = value;
            }
        }

        public string ShippingCharge
        {
            set
            {
                Shipping_Charge.Value = value;
            }
        }

        public string CustomerId
        {
            set
            {
                Customer_Id.Value = value;
            }
        }

        public string TransactionId
        {
            set
            {
                Transaction_Id.Value = value;
            }
        }


    }
}