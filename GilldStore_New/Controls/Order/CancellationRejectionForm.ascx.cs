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
    public partial class CancellationRejectionForm : System.Web.UI.UserControl
    {
        public event EventHandler RejectionSuccess;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Load_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.Create_Empty_DropDown_Value(Rejection_Reason_Id);
            CommonClass.Load_Attributes(dbconn,
                "Rejection Reason",
                Rejection_Reason_Id);
            dbconn.Close();
        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            Message.Text = "";
            if (Rejection_Reason_Id.SelectedValue == "" && Rejection_Reason.Text == "")
            {
                Message.Text = "Rejection Reason is mandatory";
                return;
            }
            else
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                string lRejectionReasonDisplay = Server.HtmlEncode(Rejection_Reason.Text);
                if (lRejectionReasonDisplay == "")
                {
                    lRejectionReasonDisplay = Rejection_Reason_Id.SelectedItem.Text;
                }
                string lErrorMessage = OrderClass.Reject_Refund_Order(dbconn,
                    Order_Id.Value,
                    Order_Number.Value,
                    Rejection_Reason_Id.SelectedValue,
                    Server.HtmlEncode(Rejection_Reason.Text),
                    Customer_Id.Value,
                    lRejectionReasonDisplay);
                if (lErrorMessage != null)
                {
                    Message.Text = lErrorMessage;
                    return;
                }

                dbconn.Close();
            }
            Cancel_Button_Click(sender, e);

        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (RejectionSuccess != null)
            {
                RejectionSuccess(this, e);
            }
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
    }
}