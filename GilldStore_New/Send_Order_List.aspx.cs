using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Send_Order_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MySqlConnection dbcon = new MySqlConnection(CommonClass.connectionstring);
            dbcon.Open();
            string[] lRecords = CommonClass.FetchRecords("GET_ORDERS_PENDING_FOR_SHIPPING",
                new string[]
                {
                },
                new string[]
                {
                },
                new string[]
                {
                    "P_NUMBER_OF_ORDER"
                },
                dbcon);
            if (lRecords != null)
            {
                if (lRecords[0] != "0")
                {
                    Messages.send_sms("9980075754",
                        "Dear Karpaga Kumar, You have total " + lRecords[0] + " orders to be shipped today-Thanks");
                    Messages.send_sms("9994543664",
                        "Dear Kasi Rajan, You have total " + lRecords[0] + " orders to be shipped today-Thanks");
                }
            }
            dbcon.Close();
        }
    }
}