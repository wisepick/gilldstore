using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using paytm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Order_Response : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();


            if (Request.Form.AllKeys.Length > 0)
            {
                string paytmChecksum = "";
                foreach (string key in Request.Form.Keys)
                {
                    parameters.Add(key.Trim(), Request.Form[key].Trim());
                }


                if (parameters.ContainsKey("CHECKSUMHASH"))
                {
                    paytmChecksum = parameters["CHECKSUMHASH"];
                    parameters.Remove("CHECKSUMHASH");

                }
                PayTmConfiguration lPayTmConfiguration = PaymentClass.Get_Paytm_Configuration();
                if (CheckSum.verifyCheckSum(lPayTmConfiguration.Merchant_Key, parameters, paytmChecksum))
                {
                    Create_Paytm_Payment(parameters);

                }
                else
                {
                    Response.Write("Invalid Request");
                }

            }

        }

        protected void Create_Paytm_Payment(Dictionary<string, string> pParameters)
        {

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();

            string lTxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string lTxnAmount = "";
            string lTxnId = "";
            string lOrderId = pParameters["ORDERID"];
            if (pParameters.ContainsKey("TXNDATE"))
            {
                lTxnDate = pParameters["TXNDATE"];
            }
            if (pParameters.ContainsKey("TXNAMOUNT"))
            {
                lTxnAmount = pParameters["TXNAMOUNT"];
            }
            if (pParameters.ContainsKey("TXNID"))
            {
                lTxnId = pParameters["TXNID"];
            }
            string[] lRecords = CommonClass.FetchRecords("ADD_PAYTM_PAYMENT",
                new string[] 
                        {
                            "P_ORDER_ID",
                            "P_PAYMENT_DATE",
                            "P_PAYMENT_AMOUNT",
                            "P_PAYTM_TXN_ID",
                            "P_PAYTM_TXN_STATUS",
                            "P_PAYTM_TXN_RESPCODE",
                            "P_PAYTM_TXN_RESPMSG",
                            "P_EXTERNAL_USER_ID",
                            "P_USER_ID"
                        },
                new string[]
                        {
                            lOrderId,
                            lTxnDate,
                            lTxnAmount,
                            lTxnId,
                            pParameters["STATUS"],
                            pParameters["RESPCODE"],
                            pParameters["RESPMSG"],
                            ClaimsPrincipal.Current.FindFirst("user_id").Value,
                            null
                        },
                new string[]
                        {
                            "P_ERROR_STRING"
                        },
                dbconn);
            if ((!(lRecords[0] != null && lRecords[0] != "")) && pParameters["RESPCODE"] == "01")
            {
                OrderClass.Confirm_Order(dbconn, pParameters["ORDERID"]);
            }

            //    //)
            //    //(lEmail,
            //    //lUserName,
            //    //lMobileNumber,
            //    //lOrderId,
            //    //lPaymentGatewayResponse.TXNAMOUNT.ToString(),
            //    //dbconn);

            dbconn.Close();
            Response.Redirect("~/OrderSummary.aspx?Order_Id=" + lOrderId);
        }


        //else if (Request.QueryString["Order_Id"] != null)
        //{
        //    string lOrderId = Request.QueryString["Order_Id"].ToString();
        //    string lResponseFromSender = "";
        //    WebRequest lWebRequest = WebRequest.Create("https://pguat.paytm.com/oltp/HANDLER_INTERNAL/TXNSTATUS?JsonData={\"MID\":\"GILLDO95102413872758\",\"ORDERID\":\"" + lOrderId + "\"}");
        //    HttpWebResponse lHttpWebResponse = (HttpWebResponse)lWebRequest.GetResponse();
        //    Stream lStream = lHttpWebResponse.GetResponseStream();
        //    StreamReader lStreamReader = new StreamReader(lStream);                
        //    lResponseFromSender = lStreamReader.ReadToEnd();
        //    JavaScriptSerializer lJavaScriptSerializer = new JavaScriptSerializer();
        //    PaymentGatewayResponse lPaymentGatewayResponse = lJavaScriptSerializer.Deserialize<PaymentGatewayResponse>(lResponseFromSender);
        //    lStreamReader.Close();
        //    lStream.Close();
        //    lHttpWebResponse.Close();

        //    if (lPaymentGatewayResponse != null)
        //    {
        //        MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
        //        dbconn.Open();
        //        string[] lRecords = CommonClass.FetchRecords("ADD_PAYTM_PAYMENT",
        //            new string[] 
        //            {
        //                "P_ORDER_ID",
        //                "P_PAYMENT_DATE",
        //                "P_PAYMENT_AMOUNT",
        //                "P_PAYTM_TXN_ID",
        //                "P_PAYTM_TXN_STATUS",
        //                "P_PAYTM_TXN_RESPCODE",
        //                "P_PAYTM_TXN_RESPMSG",
        //                "P_EXTERNAL_USER_ID",
        //                "P_USER_ID"
        //            },
        //            new string[]
        //            {
        //                lOrderId,
        //                lPaymentGatewayResponse.TXNDATE,
        //                lPaymentGatewayResponse.TXNAMOUNT.ToString(),
        //                lPaymentGatewayResponse.TXNID.ToString(),
        //                lPaymentGatewayResponse.STATUS,
        //                lPaymentGatewayResponse.RESPCODE,
        //                lPaymentGatewayResponse.RESPMSG,
        //                ClaimsPrincipal.Current.FindFirst("user_id").Value,
        //                null
        //            },
        //            new string[]
        //            {
        //                "P_ERROR_STRING"
        //            },
        //            dbconn);
        //        if (!(lRecords[0] != null && lRecords[0] != ""))
        //        {
        //            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_USER_BY_ID",
        //                new string[] { "P_EXTERNAL_USER_ID" },
        //                new string[] { ClaimsPrincipal.Current.FindFirst("user_id").Value },
        //                dbconn);
        //            string lEmail = "";
        //            string lMobileNumber = "";
        //            string lUserName = "";
        //            if (lMySqlDataReader.Read())
        //            {
        //                lUserName = lMySqlDataReader["USER_NAME"].ToString();
        //                if (lMySqlDataReader["EMAIL_ADDRESS_VALIDATED"].ToString() == "1")
        //                {
        //                    lEmail = lMySqlDataReader["EMAIL_ADDRESS"].ToString();
        //                }
        //                if (lMySqlDataReader["MOBILE_VALIDATED"].ToString() == "1")
        //                {
        //                    lMobileNumber = lMySqlDataReader["MOBILE_NUMBER"].ToString();
        //                }
        //            }
        //            lMySqlDataReader.Close();


        //            lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_BY_ID",
        //                new string[]
        //                {
        //                    "P_ORDER_ID",
        //                    "P_EXTERNAL_USER_ID"
        //                },
        //                new string[]
        //                {
        //                    lOrderId,
        //                    ClaimsPrincipal.Current.FindFirst("user_id").Value
        //                },
        //                dbconn
        //                );
        //            string lAddress = "";
        //            string lOrderDate = "";
        //            string lOrderAmount = "";
        //            string lSubtotal = "";
        //            string lShippingCharges = "";
        //            string lDiscount = "";
        //            string lGrandTotal = "";
        //            if (lMySqlDataReader.Read())
        //            {
        //                lAddress = CommonClass.Format_Address(lMySqlDataReader["SHIPPING_ADDRESS"].ToString(), lMySqlDataReader["CITY_NAME"].ToString(), lMySqlDataReader["STATE_NAME"].ToString(), lMySqlDataReader["PIN_CODE"].ToString());
        //                lOrderDate = lMySqlDataReader["ORDER_DATE"].ToString();
        //                lOrderAmount = lMySqlDataReader["ORDER_TOTAL"].ToString();                            
        //                lShippingCharges = lMySqlDataReader["SHIPPING_CHARGE"].ToString();
        //                lDiscount = lMySqlDataReader["DISCOUNTS"].ToString();
        //                lSubtotal = (double.Parse(lOrderAmount) - double.Parse(lShippingCharges) + double.Parse(lDiscount)).ToString();                            
        //                lGrandTotal = lOrderAmount;
        //            }

        //            lMySqlDataReader.Close();
        //            lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_DETAILS_BY_ID",
        //                new string[]
        //                {
        //                    "P_ORDER_ID",
        //                    "P_EXTERNAL_ID"
        //                },
        //                new string[]
        //                {
        //                    lOrderId,
        //                    ClaimsPrincipal.Current.FindFirst("user_id").Value
        //                },
        //                dbconn);
        //            string lOrderSummary = "";
        //            while (lMySqlDataReader.Read())
        //            {
        //                lOrderSummary += "<tr>";
        //                lOrderSummary += "<td>" + lMySqlDataReader["PRODUCT_NAME"].ToString() + " </td>";
        //                lOrderSummary += "<td>" + lMySqlDataReader["MEASUREMENT_UNIT"].ToString() + " </td>";
        //                lOrderSummary += "<td>" + lMySqlDataReader["QUANTITY"].ToString() + " </td>";
        //                lOrderSummary += "<td>₹ " + lMySqlDataReader["PRICE"].ToString() + " </td>";
        //                lOrderSummary += "<td>₹ " + lMySqlDataReader["SUBTOTAL"].ToString() + " </td>";
        //                lOrderSummary += "</tr>";
        //            }
        //            lMySqlDataReader.Close();

        //            Messages.Send_Order_Confirmation_Message(lEmail,
        //                lUserName,
        //                lAddress,
        //                lMobileNumber,
        //                lOrderId,
        //                lOrderDate,
        //                lOrderAmount,
        //                lOrderSummary,
        //                lSubtotal,
        //                lShippingCharges,
        //                lGrandTotal,
        //                lDiscount,
        //                dbconn);
        //        }

        //            //    //)
        //            //    //(lEmail,
        //            //    //lUserName,
        //            //    //lMobileNumber,
        //            //    //lOrderId,
        //            //    //lPaymentGatewayResponse.TXNAMOUNT.ToString(),
        //            //    //dbconn);

        //        dbconn.Close();
        //        Response.Redirect("~/OrderSummary.aspx?Order_Id=" + lOrderId);
        //    }                

        // }
    }
}