using CCA.Util;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using paytm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Script.Serialization;

namespace GilldStore_New.App_Start
{
    public class OrderClass
    {

        public static void Create_Order(MySqlConnection dbconn,
            string CustomerId,
            string GrandTotal,
            string ShippingCharges,
            string Discounts,
            string AddressId,
            string PromotionId,
            string PaymentType,
            string Status,
            string DeliveryMethod)
        {
            string[] lRecords = CommonClass.ExecuteQuery("ADD_ORDER",
                        new string[]
                        {
                            "P_USER_ID",
                            "P_EXTERNAL_USER_ID",
                            "P_ORDER_TOTAL",
                            "P_SHIPPING_CHARGE",
                            "P_DISCOUNTS",
                            "P_ADDRESS_ID",
                            "P_PROMOTION_ID",
                            "P_PAYMENT_TYPE",
                            "P_ORDER_STATUS",                            
                            "P_DELIVERY_METHOD",
                            "P_SHOPPING_CART_ID"
                        },
                        new string[]
                        {
                            CustomerId,
                            ClaimsPrincipal.Current.FindFirst("user_id").Value,
                            GrandTotal,
                            ShippingCharges,
                            Discounts,
                            AddressId,
                            PromotionId,
                            PaymentType,
                            Status,
                            DeliveryMethod,
                            HttpContext.Current.Session["SHOPPING_CART_ID"].ToString()
                        },
                        new string[]
                        {
                            "P_ORDER_ID"
                        },
                        dbconn);
            string lOrderid = lRecords[0];
            if (PaymentType == "Others")
            {
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);

                PaymentClass.Initiate_CCAvenue_Order(lOrderid,
                    GrandTotal,
                    lUserModel.Email_Address,
                    lUserModel.Mobile_Number,
                    AddressId,
                    dbconn);
                dbconn.Close();
            }
            else
            {
                Confirm_Order(dbconn,
                    lOrderid);
                dbconn.Close();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Response.Redirect("~/OrderSummary.aspx?Order_Id=" + lOrderid);
            }

        }

        public static void Confirm_Order(MySqlConnection dbconn,
            string lOrderId)
        {

            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_BY_ID",
                new string[]
                            {
                                "P_ORDER_ID",
                                "P_EXTERNAL_USER_ID"
                            },
                new string[]
                            {
                                lOrderId,                                
                                ClaimsPrincipal.Current.FindFirst("user_id").Value
                            },
                dbconn
                );
            string lAddress = "";
            string lOrderDate = "";
            string lOrderAmount = "";
            string lSubtotal = "";
            string lShippingCharges = "";
            string lDiscount = "";
            string lGrandTotal = "";
            string lOrderNumber = "";
            string lPaymentType = "";
            string lUserId = null;
            string lDeliveryMethod = null;
            string[] lCodEligible = null;
            string lPinCode = null;
            string lPromotionApplied = "N";

            if (lMySqlDataReader.Read())
            {
                lOrderNumber = lMySqlDataReader["ORDER_NUMBER"].ToString();
                if (lMySqlDataReader["ADDRESS_ID"] != null)
                {

                    string[] lAddressArray = CommonClass.Get_Address_By_Id(lMySqlDataReader["ADDRESS_ID"].ToString());
                    lAddress = CommonClass.Format_Address(lAddressArray[0], lAddressArray[1], lAddressArray[2], lAddressArray[3], lAddressArray[4], lAddressArray[5]);
                    lPinCode = lAddressArray[5];
                }
                lOrderDate = lMySqlDataReader["ORDER_DATE"].ToString();
                lOrderAmount = lMySqlDataReader["ORDER_TOTAL"].ToString();
                lShippingCharges = lMySqlDataReader["SHIPPING_CHARGE"].ToString();
                lDiscount = lMySqlDataReader["DISCOUNTS"].ToString();
                lSubtotal = (double.Parse(lOrderAmount) - double.Parse(lShippingCharges) + double.Parse(lDiscount)).ToString();
                lPaymentType = lMySqlDataReader["PAYMENT_TYPE"].ToString();
                lGrandTotal = lOrderAmount;
                lUserId = lMySqlDataReader["USER_ID"].ToString();
                lDeliveryMethod = lMySqlDataReader["DELIVERY_METHOD_ID"].ToString();
                lPromotionApplied = lMySqlDataReader["PROMOTION_APPLIED"].ToString();
            }

            lMySqlDataReader.Close();


            if (lPinCode != null && lPromotionApplied == "N")
            {
                lCodEligible = CommonClass.FetchRecords("IS_COD",
                            new string[] 
                        {
                            "P_PIN_CODE"
                        },
                            new string[]
                        {
                            lPinCode
                        },
                            new string[]
                        {
                            "P_ELIGIBLE"
                        },
                            dbconn);
            }

            PaymentClass.Allocate_Order(dbconn, lUserId);

            UserDetails lUserDetails = Messages.Get_Contact_Details(lUserId, dbconn);
            lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_DETAILS_BY_ID",
                new string[]
                            {
                                "P_ORDER_ID",
                                "P_EXTERNAL_ID"
                            },
                new string[]
                            {
                                lOrderId,
                                ClaimsPrincipal.Current.FindFirst("user_id").Value
                            },
                dbconn);
            string lOrderSummary = "";
            while (lMySqlDataReader.Read())
            {
                lOrderSummary += "<tr>";
                lOrderSummary += "<td>" + lMySqlDataReader["PRODUCT_NAME"].ToString() + " </td>";
                lOrderSummary += "<td>" + lMySqlDataReader["MEASUREMENT_UNIT"].ToString() + " </td>";
                lOrderSummary += "<td>" + lMySqlDataReader["QUANTITY"].ToString() + " </td>";
                lOrderSummary += "<td>₹ " + lMySqlDataReader["PRICE"].ToString() + " </td>";
                lOrderSummary += "<td>₹ " + lMySqlDataReader["SUBTOTAL"].ToString() + " </td>";
                lOrderSummary += "</tr>";
            }
            lMySqlDataReader.Close();

            Messages.Send_Order_Confirmation_Message(lUserDetails.Email,
                lUserDetails.User_Name,
                lAddress,
                lUserDetails.Mobile_Number,
                lOrderNumber,
                lOrderDate,
                lOrderAmount,
                lOrderSummary,
                lSubtotal,
                lShippingCharges,
                lGrandTotal,
                lDiscount,
                lPaymentType,
                dbconn);


            if (lDeliveryMethod == "3" && lCodEligible[0] != "1")
            {
                Messages.Send_Order_Confirmation_Message_To_Seller(
                    lAddress,
                    lOrderNumber,
                    lOrderDate,
                    lOrderAmount,
                    lOrderSummary,
                    lSubtotal,
                    lShippingCharges,
                    lGrandTotal,
                    lDiscount,
                    lPaymentType,
                    dbconn);
            }
        }

        public static void Resend_Order_Information(MySqlConnection dbconn,
            string lOrderId)
        {

            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_BY_ID",
                new string[]
                            {
                                "P_ORDER_ID",
                                "P_EXTERNAL_USER_ID"
                            },
                new string[]
                            {
                                lOrderId,                                
                                ClaimsPrincipal.Current.FindFirst("user_id").Value
                            },
                dbconn
                );
            string lAddress = "";
            string lOrderDate = "";
            string lOrderAmount = "";
            string lSubtotal = "";
            string lShippingCharges = "";
            string lDiscount = "";
            string lGrandTotal = "";
            string lOrderNumber = "";
            string lPaymentType = "";
            string lUserId = null;
            string lDeliveryMethod = null;

            string lPinCode = null;
            string lPromotionApplied = "N";

            if (lMySqlDataReader.Read())
            {
                lOrderNumber = lMySqlDataReader["ORDER_NUMBER"].ToString();
                if (lMySqlDataReader["ADDRESS_ID"] != null)
                {

                    string[] lAddressArray = CommonClass.Get_Address_By_Id(lMySqlDataReader["ADDRESS_ID"].ToString());
                    lAddress = CommonClass.Format_Address(lAddressArray[0], lAddressArray[1], lAddressArray[2], lAddressArray[3], lAddressArray[4], lAddressArray[5]);
                    lPinCode = lAddressArray[5];
                }
                lOrderDate = lMySqlDataReader["ORDER_DATE"].ToString();
                lOrderAmount = lMySqlDataReader["ORDER_TOTAL"].ToString();
                lShippingCharges = lMySqlDataReader["SHIPPING_CHARGE"].ToString();
                lDiscount = lMySqlDataReader["DISCOUNTS"].ToString();
                lSubtotal = (double.Parse(lOrderAmount) - double.Parse(lShippingCharges) + double.Parse(lDiscount)).ToString();
                lPaymentType = lMySqlDataReader["PAYMENT_TYPE"].ToString();
                lGrandTotal = lOrderAmount;
                lUserId = lMySqlDataReader["USER_ID"].ToString();
                lDeliveryMethod = lMySqlDataReader["DELIVERY_METHOD_ID"].ToString();
                lPromotionApplied = lMySqlDataReader["PROMOTION_APPLIED"].ToString();
            }

            lMySqlDataReader.Close();


            UserDetails lUserDetails = Messages.Get_Contact_Details(lUserId, dbconn);
            lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_DETAILS_BY_ID",
                new string[]
                            {
                                "P_ORDER_ID",
                                "P_EXTERNAL_ID"
                            },
                new string[]
                            {
                                lOrderId,
                                ClaimsPrincipal.Current.FindFirst("user_id").Value
                            },
                dbconn);
            string lOrderSummary = "";
            while (lMySqlDataReader.Read())
            {
                lOrderSummary += "<tr>";
                lOrderSummary += "<td>" + lMySqlDataReader["PRODUCT_NAME"].ToString() + " </td>";
                lOrderSummary += "<td>" + lMySqlDataReader["MEASUREMENT_UNIT"].ToString() + " </td>";
                lOrderSummary += "<td>" + lMySqlDataReader["QUANTITY"].ToString() + " </td>";
                lOrderSummary += "<td>₹ " + lMySqlDataReader["PRICE"].ToString() + " </td>";
                lOrderSummary += "<td>₹ " + lMySqlDataReader["SUBTOTAL"].ToString() + " </td>";
                lOrderSummary += "</tr>";
            }
            lMySqlDataReader.Close();

            Messages.Send_Order_Confirmation_Message_To_Seller(
                    lAddress,
                    lOrderNumber,
                    lOrderDate,
                    lOrderAmount,
                    lOrderSummary,
                    lSubtotal,
                    lShippingCharges,
                    lGrandTotal,
                    lDiscount,
                    lPaymentType,
                    dbconn);

        }


        public static string Cancel_Order(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pCustomerId,
            string pCancellationReasonId,
            string pCancellationReason,
            string pCancellationReasonDisplay)
        {
            string lErrorMessage = null;
            string[] lRecords = CommonClass.FetchRecords("CANCEL_ORDER",
                new string[]                 
                {
                    "P_CUSTOMER_ID", 
                    "P_EXTERNAL_USER_ID",
                    "P_ORDER_ID",
                    "P_CANCELLATION_REASON_ID",
                    "P_CANCELLATION_REASON"
                },
                new string[]
                {
                    pCustomerId,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    pOrderId,
                    pCancellationReasonId,                    
                    HttpContext.Current.Server.HtmlEncode(pCancellationReason),
                },
                new string[] 
                {
                    "P_ERROR_STRING"
                },
                dbconn);

            if (lRecords[0] != null && lRecords[0] != "")
            {
                lErrorMessage = lRecords[0];
            }
            else
            {
                Messages.Send_Order_Cancellation_Request_Message(pOrderNumber,
                            pCustomerId,
                            dbconn);
            }
            return lErrorMessage;

        }

        public static string Undeliver_Order(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pCustomerId,
            string pUnDeliveredReasonId,
            string pUnDeliveredReason,
            string pUnDeliveredReasonDisplay)
        {
            string lErrorMessage = null;
            string[] lRecords = CommonClass.FetchRecords("ADD_DELIVERY_RETURN",
                new string[]                 
                {
                    "P_CUSTOMER_ID", 
                    "P_EXTERNAL_USER_ID",
                    "P_ORDER_ID",
                    "P_UNDELIVERED_REASON_ID",
                    "P_UNDELIVERED_REASON"
                },
                new string[]
                {
                    pCustomerId,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    pOrderId,
                    pUnDeliveredReasonId,                    
                    HttpContext.Current.Server.HtmlEncode(pUnDeliveredReason),
                },
                new string[] 
                {
                    "P_ERROR_STRING"
                },
                dbconn);

            if (lRecords[0] != null && lRecords[0] != "")
            {
                lErrorMessage = lRecords[0];
            }
            else
            {
                Messages.Send_Order_Undelivered_Message(pOrderNumber,
                            pUnDeliveredReasonDisplay,
                            pCustomerId,
                            dbconn);
            }
            return lErrorMessage;

        }

        public static string Refund_Order(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pCustomerId,
            string pRefundAmount,
            string pOriginalTransactionId,
            string pCancellationReason)
        {
            string lErrorMessage = null;

            string[] lRecords = CommonClass.FetchRecords("ADD_REFUND",
                    new string[] 
                {
                    "P_ORDER_ID",   
                    "P_REFUND_AMOUNT",
                    "P_EXTERNAL_USER_ID"
                },
                    new string[]
                {
                    pOrderId,
                    pRefundAmount,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value
                },
                    new string[]
                {
                    "P_ERROR_STRING",
                    "P_REFUND_ID"
                },
                    dbconn);

            if (lRecords[0] != null && lRecords[0] != "") // some error happened
            {
                lErrorMessage = lRecords[0];
            }
            else
            {
                Messages.Send_Refund_Request_Message(pOrderNumber,
                    pRefundAmount,
                    pCustomerId,
                    dbconn);
                Refund_PayTm_Order(pOrderId,
                    pOriginalTransactionId,
                    pRefundAmount,
                    pCancellationReason,
                    lRecords[1]);
            }
            return lErrorMessage;
        }

        public static string Reject_Refund_Order(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pRejectionReasonId,
            string pRejectionReason,
            string pCustomerId,
            string pRejectionReasonDisplay)
        {
            string lErrorMessage = null;
            string[] lRecords = CommonClass.FetchRecords("REJECT_REUND",
                new string[]
                {
                    "P_CUSTOMER_ID",
                    "P_EXTERNAL_USER_ID",
                    "P_ORDER_ID",
                    "P_REJECTION_REASON_ID",
                    "P_REJECTION_REASON"
                },
                new string[]
                {
                    pCustomerId,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    pOrderId,
                    pRejectionReasonId,
                    pRejectionReason
                },
                new string[]
                {
                    "P_ERROR_STRING"
                },
                dbconn
                );

            if (lRecords[0] != null && lRecords[0] != "")
            {
                lErrorMessage = lRecords[0];
            }
            else
            {
                Messages.Send_Reject_Refund_Request_Message(pOrderNumber,
                    pRejectionReasonDisplay,
                    pCustomerId,
                    dbconn);
            }
            return lErrorMessage;
        }

        public static void Ship_Order(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pCustomerId,
            string pCourierAgencyId,
            string pCourierAgencyName,
            string pShippingDate,
            string pReferenceNumber)
        {
            CommonClass.ExecuteQuery("ADD_SHIPPING_INFO",
                    new string[] 
                    {
                        "P_ORDER_ID",
                        "P_AGENCY_ID",
                        "P_SHIPPING_DATE",
                        "P_SHIPPING_REFERENCE",
                        "P_EXTERNAL_USER_ID"
                    },
                    new string[]
                    {
                        pOrderId,
                        pCourierAgencyId,
                        pShippingDate,
                        pReferenceNumber,
                        ClaimsPrincipal.Current.FindFirst("user_id").Value
                    },
                    dbconn);
            string lShippingDetails = "";
            if (pCourierAgencyName != "")
            {
                lShippingDetails = "through " + pCourierAgencyName + ", Ref No : " + pReferenceNumber;
            }

            Messages.Send_Shipping_Confirmation(pOrderNumber,
                pCustomerId,
                lShippingDetails,
                pCourierAgencyId,
                pReferenceNumber,
                dbconn);

           
           
        
        }

        public static string Add_Delivery_Information(MySqlConnection dbconn,
            string pOrderId,
            string pOrderNumber,
            string pCustomerId,
            string pOrderAmount,
            string pDeliveryDate,
            string pPaymentRef)
        {
            PaymentClass.CCAvenue_Confirm_Order(pPaymentRef, pOrderAmount);
            string lErrorMessage = null;
            string[] lRecords = CommonClass.FetchRecords("ADD_DELIVERY_INFO",
                new string[]
                {
                    "P_ORDER_ID",
                    "P_CUSTOMER_ID",
                    "P_EXTERNAL_USER_ID",
                    "P_DELIVERY_DATE"
                },
                new string[]
                {
                    pOrderId,
                    pCustomerId,
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    pDeliveryDate
                },
                new string[]
                {
                    "P_ERROR_STRING"
                },
                dbconn
                );
            if (lRecords[0] != null && lRecords[0] != "")
            {
                lErrorMessage = lRecords[0];
            }
            else
            {
                Messages.Send_Order_Delivered_Message(pOrderNumber,
                    pCustomerId,
                    pDeliveryDate,
                    dbconn);
            }
            return lErrorMessage;
        }

        public static string Get_Payment_Status(string pOrderId)
        {
            PayTmConfiguration lPayTmConfiguration = PaymentClass.Get_Paytm_Configuration();
            WebRequest lWebRequest = WebRequest.Create("https://secure.paytm.in/oltp/HANDLER_INTERNAL/TXNSTATUS?JsonData={\"ORDERID\":\"" + pOrderId + "\",\"MID\":\"" + lPayTmConfiguration.MID + "\"}");
            HttpWebResponse lHttpWebResponse = (HttpWebResponse)lWebRequest.GetResponse();
            Stream lStream = lHttpWebResponse.GetResponseStream();
            StreamReader lStreamReader = new StreamReader(lStream);
            string lResponseFromSender = lStreamReader.ReadToEnd();


            lStreamReader.Close();
            lStream.Close();
            lHttpWebResponse.Close();
            return lResponseFromSender;
        }



        public static void Refund_PayTm_Order(string pOrderId,
            string pTxnId,
            string pRefundAmount,
            string pComments,
            string pReferenceId)
        {
            PayTmConfiguration lPayTmConfiguration = PaymentClass.Get_Paytm_Configuration();
            PayTmPaymentGatewayRefundRequest lPaymentGatewayRefundRequest = new PayTmPaymentGatewayRefundRequest();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            lPaymentGatewayRefundRequest.MID = lPayTmConfiguration.MID;
            lPaymentGatewayRefundRequest.TXNID = pTxnId;
            lPaymentGatewayRefundRequest.ORDERID = pOrderId;
            lPaymentGatewayRefundRequest.REFUNDAMOUNT = pRefundAmount;
            lPaymentGatewayRefundRequest.TXNTYPE = "REFUND";
            lPaymentGatewayRefundRequest.COMMENTS = pComments;
            lPaymentGatewayRefundRequest.REFID = pReferenceId;
            parameters.Add("MID", lPayTmConfiguration.MID);
            parameters.Add("TXNID", pTxnId);
            parameters.Add("ORDERID", pOrderId);
            parameters.Add("REFUNDAMOUNT", pRefundAmount);
            parameters.Add("TXNTYPE", "REFUND");
            parameters.Add("COMMENTS", pComments);
            parameters.Add("REFID", pReferenceId);

            string checksum = CheckSum.generateCheckSum(lPayTmConfiguration.Merchant_Key, parameters);
            lPaymentGatewayRefundRequest.CHECKSUM = checksum;

            JavaScriptSerializer lJavaScriptSerializer = new JavaScriptSerializer();
            var lJson = lJavaScriptSerializer.Serialize(lPaymentGatewayRefundRequest);

            WebRequest lWebRequest = WebRequest.Create(lPayTmConfiguration.Refund_Url + "?JsonData=" + lJson);
            HttpWebResponse lHttpWebResponse = (HttpWebResponse)lWebRequest.GetResponse();
            Stream lStream = lHttpWebResponse.GetResponseStream();
            StreamReader lStreamReader = new StreamReader(lStream);
            string lResponseFromSender = lStreamReader.ReadToEnd();

            //    PaymentGatewayResponse lPaymentGatewayResponse = lJavaScriptSerializer.Deserialize<PaymentGatewayResponse>(lResponseFromSender);
            lStreamReader.Close();
            lStream.Close();
            lHttpWebResponse.Close();

            //    if (lPaymentGatewayResponse != null)
            //    {

        }
    }


}