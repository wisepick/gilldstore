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
    public partial class ReturnedOrderForm : System.Web.UI.UserControl
    {
        public event EventHandler ReturnedOderSuccess;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Load_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.Create_Empty_DropDown_Value(Return_Reason_Id);
            CommonClass.Load_Attributes(dbconn,
                "Returned Order Reason",
                Return_Reason_Id);
            dbconn.Close();
        }

        protected void Save_Button_Click(object sender, EventArgs e)
        {
            if (Return_Reason.Text == "" && Return_Reason_Id.SelectedValue == "")
            {
                Message.Text = "Return reason is mandatory";
                return;
            }
            Cancel_Button_Click(sender, e);
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (ReturnedOderSuccess != null)
            {
                ReturnedOderSuccess(this, e);
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