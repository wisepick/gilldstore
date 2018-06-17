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
    public partial class UnDeliveredForm : System.Web.UI.UserControl
    {
        public event EventHandler UnDelivered;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            if (UnDelivered_Reason_Id.SelectedValue.ToString() == "" &&
                UnDelivered_Reason.Text == "")
            {
                Message.Text = "Select the Reason";
            }
            else
            {
                string lUnDeliveredReason = UnDelivered_Reason.Text;
                if (lUnDeliveredReason == null || lUnDeliveredReason == "")
                {
                    lUnDeliveredReason = UnDelivered_Reason_Id.SelectedItem.Text;
                }
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                if (Request.ServerVariables["URL"].Contains("orders.aspx"))
                {
                    string lErrorMessage = OrderClass.Undeliver_Order(dbconn,
                        Order_Id.Value,
                        Order_Number.Value,
                        Customer_Id.Value,
                        UnDelivered_Reason_Id.SelectedValue,
                        UnDelivered_Reason.Text,
                        lUnDeliveredReason);
                    if (lErrorMessage != null)
                    {
                        Message.Text = lErrorMessage;
                        return;
                    }
                    lErrorMessage = OrderClass.Refund_Order(dbconn,
                        Order_Id.Value,
                        Order_Number.Value,
                        Customer_Id.Value,
                        Order_Amount.Value,
                        Transaction_Id.Value,
                        "Could not be delivered - " + lUnDeliveredReason);


                    if (lErrorMessage != null) // some error happened
                    {
                        Message.Text = lErrorMessage;
                        return;
                    }
                }
                dbconn.Close();
                Cancel_Button_Click(sender, e);
            }
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (UnDelivered != null)
            {
                UnDelivered(this, e);
            }
        }

        public void Load_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.Create_Empty_DropDown_Value(UnDelivered_Reason_Id);
            CommonClass.Load_Attributes(dbconn, "Undelivered Reason", UnDelivered_Reason_Id);

            dbconn.Close();
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

        public string CustomerId
        {
            set
            {
                Customer_Id.Value = value;
            }
        }

        public string OrderAmount
        {
            set
            {
                Order_Amount.Value = value;
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