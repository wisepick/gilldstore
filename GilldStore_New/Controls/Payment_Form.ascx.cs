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
    public partial class Payment_Form : System.Web.UI.UserControl
    {
        public event EventHandler SuccessPayment;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Calendar1.SelectedDate = DateTime.UtcNow;
            }
        }

        protected void Payment_Type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Save_Payment_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            PaymentClass.Create_Cash_Payment(dbconn,
                Payment_Amount.Text,
                Customer_Id.Value
                );
            dbconn.Close();
            Clear_Payment_Form();
            Payment_Success_message.Text = "Payment Successfully Posted";
            if (SuccessPayment != null)
            {
                SuccessPayment(this, EventArgs.Empty);
            }
        }

        public void Clear_Payment_Form()
        {
            Calendar1.SelectedDate = DateTime.UtcNow;
            Payment_Type.ClearSelection();
            Payment_Amount.Text = "";
            Payment_Success_message.Text = "";
        }

        public string Set_Payment_Amount
        {
            set
            {
                Payment_Amount.Text = value;
            }
        }

        public string Set_Customer_Id
        {
            set
            {
                Customer_Id.Value = value;
            }
        }
    }
}