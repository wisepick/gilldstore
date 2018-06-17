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
    public partial class OrderDeliveryForm : System.Web.UI.UserControl
    {
        public event EventHandler DeliverySuccess;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Calendar1.SelectedDate = DateTime.Now;
            }
        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate.ToString() == "")
            {
                Message.Text = "Select the Delivery Date";
                return;
            }
         
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderClass.Add_Delivery_Information(dbconn,
                Order_Id.Value,
                Order_Number.Value,
                Customer_Id.Value,
                Order_Amount.Value,
                Calendar1.SelectedDate.ToString().Substring(0,10),
                Payment_Ref.Value
                );
            dbconn.Close();
            Cancel_Button_Click(sender, e);
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (DeliverySuccess != null)
            {
                DeliverySuccess(this, e);
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

        public string OrderAmount
        {
            set
            {
                Order_Amount.Value = value;
            }
        }

        public string PaymentRef
        {
            set
            {
                Payment_Ref.Value = value;
            }
        }
    }
}