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
    public partial class ShippingInformationForm : System.Web.UI.UserControl
    {
        public event EventHandler ShippingSuccess;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            Message.Text = "<ul>";
            bool lValidationSuccessFlag = true;
            if (Calendar1.SelectedDate.ToString() == "")
            {
                lValidationSuccessFlag = false;
                Message.Text += "<li>Select the Shipping Date</li>";
            }
            if (Courier_Agency_Id.SelectedValue != "")
            {
                if (Reference_Number.Text == "")
                {
                    lValidationSuccessFlag = false;
                    Message.Text += "<li>Enter the Reference Number</li>";
                }
            }
            Message.Text += "</ul>";
            if (lValidationSuccessFlag == true)//Inseert Shipping Information
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                OrderClass.Ship_Order(dbconn,
                    Order_Id.Value,
                    Order_Number.Value,
                    Customer_Id.Value,
                    Courier_Agency_Id.SelectedValue,
                    Courier_Agency_Id.SelectedItem.Text,
                    Calendar1.SelectedDate.ToString(),
                    Server.HtmlEncode(Reference_Number.Text)
                    );
                dbconn.Close();
                Cancel_Button_Click(sender, e);
            }
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (ShippingSuccess != null)
            {
                ShippingSuccess(this, e);
            }
        }

        

        public void Load_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.Create_Empty_DropDown_Value(Courier_Agency_Id);
            CommonClass.Load_Attributes(dbconn,
                "Courier Agencies",
                Courier_Agency_Id);
            dbconn.Close();
            Calendar1.SelectedDate = DateTime.Now;
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