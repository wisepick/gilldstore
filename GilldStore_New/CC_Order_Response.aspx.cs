using CCA.Util;
using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class CC_Order_Response : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CCAvanueConfiguration lCCAvanueConfiguration = PaymentClass.Get_CCAvenue_Configuration();
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], lCCAvanueConfiguration.Working_Key);
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            //NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    parameters.Add(Key, Value);
                }
            }



            Create_CC_Payment(parameters);
        }

        protected void Create_CC_Payment(Dictionary<string, string> pParameters)
        {

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();

            string lTxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string lTxnAmount = "";
            string lTxnId = "";
            string lOrderId = pParameters["order_id"];
            if (pParameters.ContainsKey("trans_date"))
            {
                lTxnDate = pParameters["trans_date"];
            }
            if (pParameters.ContainsKey("amount"))
            {
                lTxnAmount = pParameters["amount"];
            }
            if (pParameters.ContainsKey("tracking_id"))
            {
                lTxnId = pParameters["tracking_id"];
            }
            string[] lRecords = CommonClass.FetchRecords("ADD_CC_AVANUE_PAYMENT",
                new string[] 
                        {
                            "P_ORDER_ID",
                            "P_PAYMENT_DATE",
                            "P_PAYMENT_AMOUNT",
                            "P_TRACKING_ID",
                            "P_PAYMENT_MODE",
                            "P_TRANSACTION_STATUS",                            
                            "P_FAILURE_MESSAGE",
                            "P_EXTERNAL_USER_ID",
                            "P_USER_ID"
                        },
                new string[]
                        {
                            lOrderId,
                            lTxnDate,
                            lTxnAmount,
                            lTxnId,
                            pParameters["payment_mode"],
                            pParameters["order_status"],                            
                            pParameters["failure_message"],
                            ClaimsPrincipal.Current.FindFirst("user_id").Value,
                            null
                        },
                new string[]
                        {
                            "P_ERROR_STRING"
                        },
                dbconn);

            if ((!(lRecords[0] != null && lRecords[0] != "")) && pParameters["order_status"] == "Success")
            {
                OrderClass.Confirm_Order(dbconn, pParameters["order_id"]);
            }


            dbconn.Close();
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/OrderSummary.aspx?Order_Id=" + lOrderId);
        }




    }
}