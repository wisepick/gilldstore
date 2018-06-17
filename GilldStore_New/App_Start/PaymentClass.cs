using CCA.Util;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using paytm;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace GilldStore_New.App_Start
{
    public class PaymentClass
    {
        public static PayTmConfiguration Get_Paytm_Configuration()
        {
            PayTmConfiguration lPayTmConfiguration = new PayTmConfiguration();
            if (CommonClass.Is_Production())
            {

                //lPayTmConfiguration.Website_Name = "gilldstore";
                //lPayTmConfiguration.MID = "GILLDO95102413872758";
                //lPayTmConfiguration.Merchant_Key = "7HWZdpS66qDTqnFh";
                //lPayTmConfiguration.Industry_Type = "Retail";
                //lPayTmConfiguration.Channel_Id = "WEB";
                //lPayTmConfiguration.Payment_Url = "https://pguat.paytm.com/oltp-web/processTransaction";
                //lPayTmConfiguration.Mobile_Number = "7777777777";
                //lPayTmConfiguration.Refund_Url = "https://pguat.paytm.com/oltp/HANDLER_INTERNAL/REFUND";
                lPayTmConfiguration.Website_Name = "GILLDOweb";
                lPayTmConfiguration.MID = "GILLDO65732891881355";
                lPayTmConfiguration.Merchant_Key = "42d2UDzdHqRKnmC@";
                lPayTmConfiguration.Industry_Type = "Retail120";
                lPayTmConfiguration.Channel_Id = "WEB";
                lPayTmConfiguration.Payment_Url = "https://secure.paytm.in/oltp-web/processTransaction";
            }
            else
            {
                lPayTmConfiguration.Website_Name = HttpContext.Current.Application["COMPANY_DISPLAY_NAME"].ToString();
                lPayTmConfiguration.MID = "GILLDO95102413872758";
                lPayTmConfiguration.Merchant_Key = "7HWZdpS66qDTqnFh";
                lPayTmConfiguration.Industry_Type = "Retail";
                lPayTmConfiguration.Channel_Id = "WEB";
                lPayTmConfiguration.Payment_Url = "https://pguat.paytm.com/oltp-web/processTransaction";
                lPayTmConfiguration.Mobile_Number = "7777777777";
                lPayTmConfiguration.Refund_Url = "https://pguat.paytm.com/oltp/HANDLER_INTERNAL/REFUND";

            }
            return lPayTmConfiguration;
        }

        public static CCAvanueConfiguration Get_CCAvenue_Configuration()
        {
            CCAvanueConfiguration lCCAvanueConfiguration = new CCAvanueConfiguration();

            if (CommonClass.Is_Production())
            {
                lCCAvanueConfiguration.Merchant_Key = "124136";
                lCCAvanueConfiguration.Access_Code = "AVLP69EB20AX46PLXA";
                lCCAvanueConfiguration.Working_Key = "1FD75F126B9B2DBC9594C9D66E742B22";
                lCCAvanueConfiguration.Payment_Url = "https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction";
            }
            else
            {
                lCCAvanueConfiguration.Merchant_Key = "124136";
                lCCAvanueConfiguration.Access_Code = "AVZW00EB88BF97WZFB";
                lCCAvanueConfiguration.Working_Key = "A7B7791C9F1A2686E47F8C71DB2FF9E6";
                lCCAvanueConfiguration.Payment_Url = "https://test.ccavenue.com/transaction/transaction.do?command=initiateTransaction";

            }
            return lCCAvanueConfiguration;
        }
        public static void Allocate_Order(
            MySqlConnection dbconn,
            string p_UserId)
        {
            MySqlConnection dbconn1 = new MySqlConnection(CommonClass.connectionstring);
            MySqlConnection dbconn2 = new MySqlConnection(CommonClass.connectionstring);
            dbconn1.Open();
            dbconn2.Open();
            MySqlDataReader lMySqlDataReader1 = CommonClass.FetchRecords("GET_UNALLOCATED_PAYMENT_BY_USER_ID",
                new string[]
                {
                    "P_USER_ID"
                },
                new string[]
                {
                    p_UserId
                },
                dbconn1);

            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_DUE_ORDER_LIST_BY_USER_ID",
                new string[]
                {
                    "P_USER_ID"
                },
                new string[]
                {
                    p_UserId
                },
                dbconn);
            double lUnallocated_Amount = 0;
            string lPaymentId = null;
            while (lMySqlDataReader.Read())
            {
                string lOrderId = lMySqlDataReader["ORDER_ID"].ToString();
                double lOrderDue = double.Parse(lMySqlDataReader["ORDER_DUE"].ToString());
                while (lOrderDue != 0)
                {
                    double lAllocatedAmount = 0;
                    if (lUnallocated_Amount == 0)
                    {
                        if (lMySqlDataReader1.Read())
                        {
                            lUnallocated_Amount = double.Parse(lMySqlDataReader1["UNALLOCATED_AMOUNT"].ToString());
                            lPaymentId = lMySqlDataReader1["PAYMENT_ID"].ToString();
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (lOrderDue >= lUnallocated_Amount)
                    {
                        lAllocatedAmount = lUnallocated_Amount;
                        lOrderDue -= lUnallocated_Amount;
                        lUnallocated_Amount = 0;
                    }
                    else
                    {
                        lUnallocated_Amount -= lOrderDue;
                        lAllocatedAmount = lOrderDue;
                        lOrderDue = 0;
                    }

                    CommonClass.ExecuteQuery("ADD_PAYMENT_ORDER",
                        new string[] 
                        {
                            "P_PAYMENT_ID",
                            "P_ORDER_ID",
                            "P_AMOUNT"
                        },
                        new string[]
                        {
                            lPaymentId,
                            lOrderId,
                            lAllocatedAmount.ToString()
                        },
                        dbconn2);
                }
            }
            lMySqlDataReader.Close();
            lMySqlDataReader1.Close();
            dbconn1.Close();
            dbconn2.Close();
        }

        public static void Create_Cash_Payment(MySqlConnection dbconn,
            string lOrderAmount,
            string lCustomerId)
        {
            string[] lPaymentRecords = CommonClass.FetchRecords("ADD_CASH_PAYMENT",
                       new string[] 
                        {                                                    
                            "P_PAYMENT_AMOUNT",
                            "P_EXTERNAL_USER_ID",
                            "P_CUSTOMER_ID",                            
                        },
                       new string[]
                        {                         
                            lOrderAmount,                            
                            ClaimsPrincipal.Current.FindFirst("user_id").Value,
                            lCustomerId
                        },
                       new string[]
                        {
                            "P_ERROR_STRING"
                        },
                       dbconn);
            Allocate_Order(dbconn, lCustomerId);
        }

        public static void Initiate_CCAvenue_Order(string pOrderId,
           string pTransactionAmount,
           string pEmail,
           string pMobileNumber,
           string pAddressId,
           MySqlConnection dbconn)
        {
            UserAddress lUserAddress = UserClass.Get_User_Address(pAddressId,
                dbconn);

            CCAvanueConfiguration lCCAvanueConfiguration = PaymentClass.Get_CCAvenue_Configuration();
            string lMobileNumber = pMobileNumber;
            CCACrypto ccaCrypto = new CCACrypto();
            string ccaRequest = "";
            var lCallBackUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/CC_Order_Response.aspx"; //This parameter is not mandatory. Use this to pass the callback url dynamically.
            if (CommonClass.Is_Production())
            {
                lCallBackUrl = lCallBackUrl.Replace("http://", "https://");
            }
            ccaRequest += "merchant_id=" + HttpUtility.UrlEncode(lCCAvanueConfiguration.Merchant_Key) + "&";
            ccaRequest += "order_id=" + HttpUtility.UrlEncode(pOrderId) + "&";
            ccaRequest += "currency=" + HttpUtility.UrlEncode("INR") + "&";
            ccaRequest += "amount=" + HttpUtility.UrlEncode(pTransactionAmount) + "&";
            ccaRequest += "redirect_url=" + HttpUtility.UrlEncode(lCallBackUrl) + "&";
            ccaRequest += "cancel_url=" + HttpUtility.UrlEncode(lCallBackUrl) + "&";
            ccaRequest += "language=" + HttpUtility.UrlEncode("en") + "&";
            ccaRequest += "billing_name=" + HttpUtility.UrlEncode(lUserAddress.User_Name) + "&";
            ccaRequest += "billing_address=" + HttpUtility.UrlEncode(lUserAddress.Address) + "&";
            ccaRequest += "billing_city=" + HttpUtility.UrlEncode(lUserAddress.City) + "&";
            ccaRequest += "billing_state=" + HttpUtility.UrlEncode(lUserAddress.State) + "&";
            ccaRequest += "billing_zip=" + HttpUtility.UrlEncode(lUserAddress.Pin_Code) + "&";
            ccaRequest += "billing_country=" + HttpUtility.UrlEncode("India") + "&";
            ccaRequest += "billing_tel=" + HttpUtility.UrlEncode(lUserAddress.Mobile_Number) + "&";
            ccaRequest += "billing_email=" + HttpUtility.UrlEncode(pEmail) + "&";
            ccaRequest += "delivery_name=" + HttpUtility.UrlEncode(lUserAddress.User_Name) + "&";
            ccaRequest += "delivery_address=" + HttpUtility.UrlEncode(lUserAddress.Address) + "&";
            ccaRequest += "delivery_city=" + HttpUtility.UrlEncode(lUserAddress.City) + "&";
            ccaRequest += "delivery_state=" + HttpUtility.UrlEncode(lUserAddress.State) + "&";
            ccaRequest += "delivery_zip=" + HttpUtility.UrlEncode(lUserAddress.Pin_Code) + "&";
            ccaRequest += "delivery_country=" + HttpUtility.UrlEncode("India") + "&";
            ccaRequest += "delivery_tel=" + HttpUtility.UrlEncode(lUserAddress.Mobile_Number) + "&";

            string strEncRequest = ccaCrypto.Encrypt(ccaRequest, lCCAvanueConfiguration.Working_Key);

            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            ////string lOrderId = lRecords[0];
            //parameters.Add("REQUEST_TYPE", "DEFAULT");
            //parameters.Add("MID", lPayTmConfiguration.MID);
            //parameters.Add("CHANNEL_ID", lPayTmConfiguration.Channel_Id);
            //parameters.Add("INDUSTRY_TYPE_ID", lPayTmConfiguration.Industry_Type);
            //parameters.Add("WEBSITE", lPayTmConfiguration.Website_Name);
            //parameters.Add("EMAIL", pEmail);
            //parameters.Add("MOBILE_NO", lMobileNumber);
            //parameters.Add("CUST_ID", ClaimsPrincipal.Current.FindFirst("user_id").Value);
            //parameters.Add("ORDER_ID", pOrderId);
            //parameters.Add("TXN_AMOUNT", pTransactionAmount);
            //var lCallBackUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Order_Response.aspx"; //This parameter is not mandatory. Use this to pass the callback url dynamically.
            //if (CommonClass.Is_Production())
            //{
            //    lCallBackUrl = lCallBackUrl.Replace("http://", "https://");
            //}
            //parameters.Add("CALLBACK_URL", lCallBackUrl);
            //string paytmURL = lPayTmConfiguration.Payment_Url;

            //string checksum = CheckSum.generateCheckSum(lPayTmConfiguration.Merchant_Key, parameters);
            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>CCAvenue Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + lCCAvanueConfiguration.Payment_Url + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            outputHTML += "<input type='hidden' name='encRequest' value='" + strEncRequest + "'>";
            outputHTML += "<input type='hidden' name='access_code' value='" + lCCAvanueConfiguration.Access_Code + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            HttpContext.Current.Response.Write(outputHTML);
        }

        public static void Initiate_PayTm_Order(string pOrderId,
           string pTransactionAmount,
           string pEmail,
           string pMobileNumber)
        {
            PayTmConfiguration lPayTmConfiguration = PaymentClass.Get_Paytm_Configuration();
            string lMobileNumber = pMobileNumber;
            if (lPayTmConfiguration.Mobile_Number != null)
            {
                lMobileNumber = lPayTmConfiguration.Mobile_Number;
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //string lOrderId = lRecords[0];
            parameters.Add("REQUEST_TYPE", "DEFAULT");
            parameters.Add("MID", lPayTmConfiguration.MID);
            parameters.Add("CHANNEL_ID", lPayTmConfiguration.Channel_Id);
            parameters.Add("INDUSTRY_TYPE_ID", lPayTmConfiguration.Industry_Type);
            parameters.Add("WEBSITE", lPayTmConfiguration.Website_Name);
            parameters.Add("EMAIL", pEmail);
            parameters.Add("MOBILE_NO", lMobileNumber);
            parameters.Add("CUST_ID", ClaimsPrincipal.Current.FindFirst("user_id").Value);
            parameters.Add("ORDER_ID", pOrderId);
            parameters.Add("TXN_AMOUNT", pTransactionAmount);
            var lCallBackUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Order_Response.aspx"; //This parameter is not mandatory. Use this to pass the callback url dynamically.
            if (CommonClass.Is_Production())
            {
                lCallBackUrl = lCallBackUrl.Replace("http://", "https://");
            }
            parameters.Add("CALLBACK_URL", lCallBackUrl);
            string paytmURL = lPayTmConfiguration.Payment_Url;

            string checksum = CheckSum.generateCheckSum(lPayTmConfiguration.Merchant_Key, parameters);
            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Paytm Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            HttpContext.Current.Response.Write(outputHTML);
        }

        public static void CCAvenue_Confirm_Order(string pPaymentRef,
          string pAmount)
        {
            CCAvanueConfiguration lCCAvanueConfiguration = PaymentClass.Get_CCAvenue_Configuration();
            CCACrypto ccaCrypto = new CCACrypto();
            string lJsonData = "{\"order_List\": [ {\"reference_no\":\"" + pPaymentRef + "\",\"amount\":\"" + pAmount + "\"} ] }";
            string strEncRequest = ccaCrypto.Encrypt(lJsonData, lCCAvanueConfiguration.Working_Key);

            RestClient lRestClient = new RestClient("https://login.ccavenue.com/apis/servlet/DoWebTrans");

            lJsonData = "{\"request_type\":\"JSON\",\"Command\":\"confirmOrder\",\"access_code\":\"" + lCCAvanueConfiguration.Access_Code + "\",\"response_type\":\"JSON\"}";

            RestRequest lRestRequest = new RestRequest(Method.POST);
            lRestRequest.AddQueryParameter("command", "confirmOrder");
            lRestRequest.AddQueryParameter("request_type", "JSON");
            lRestRequest.AddQueryParameter("access_code", lCCAvanueConfiguration.Access_Code);
            lRestRequest.AddQueryParameter("response_type", "JSON");
            lRestRequest.AddQueryParameter("enc_request", strEncRequest);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);

            //HttpContext.Current.Response.Write(lRestResponse.Content);            

        }



    }
}