using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls.Order
{
    public partial class PreCancellationForm : System.Web.UI.UserControl
    {
        public event EventHandler CancellationSuccess;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            if (Cancellation_Reason_Id.SelectedValue.ToString() == "" &&
                Cancellation_Reason.Text == "")
            {
                Message.Text = "Select the Reason";
            }
            else
            {
                string lCancellationReason = Cancellation_Reason.Text;
                if (lCancellationReason == null || lCancellationReason == "")
                {
                    lCancellationReason = Cancellation_Reason_Id.SelectedItem.Text;
                }
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                if (!Request.ServerVariables["URL"].Contains("orders.aspx"))
                {
                    string lErrorMessage = OrderClass.Cancel_Order(dbconn,
                        Order_Id.Value,
                        Order_Number.Value,
                        Customer_Id.Value,
                        Cancellation_Reason_Id.SelectedValue,
                        Cancellation_Reason.Text,
                        lCancellationReason);
                    if (lErrorMessage != null)
                    {
                        Message.Text = lErrorMessage;
                        return;
                    }
                }
                else
                {
                    string lErrorMessage = OrderClass.Refund_Order(dbconn,
                        Order_Id.Value,
                        Order_Number.Value,
                        Customer_Id.Value,
                        Order_Amount.Value,
                        Transaction_Id.Value,
                        "Cancelled before Delivery - " + lCancellationReason);


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
            if (CancellationSuccess != null)
            {
                CancellationSuccess(this, e);
            }
        }

        public void Load_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.Create_Empty_DropDown_Value(Cancellation_Reason_Id);
            CommonClass.FetchRecordsAndBind("GET_CANCELLATION_REASON_BY_USER_TYPE",
                new string[]
                    {
                        "P_USER_TYPE",
                        "P_SHIPPING_STAGE"
                    },
                new string[]
                    {
                        User_Type.Value,
                        "1"
                    },
                dbconn,
                Cancellation_Reason_Id);

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

        public string UserType
        {
            set
            {
                User_Type.Value = value;
            }
        }

    }
}