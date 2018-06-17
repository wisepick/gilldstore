using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using RestSharp;

namespace GilldStore_New.App_Start
{
    public class Messages
    {

        public static void Generate_GUID(MySqlConnection dbconn,
           string pEmail,
           string pUserName)
        {
            Guid g = Guid.NewGuid();
            Guid lUserGuid = Guid.NewGuid();
            CommonClass.ExecuteQuery("ADD_GUID",
                new string[]
                    {
                        "P_EXTERNAL_USER_ID",
                        "P_EMAIL",
                        "P_GUID",
                        "P_USER_GUID"
                    },
                new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,                   
                        pEmail,
                        g.ToString(),
                        lUserGuid.ToString()
                    },
                dbconn);


            Send_Welcome_Email_Validate_Message(pEmail, pUserName, g.ToString(), lUserGuid.ToString(), dbconn);

        }

        public static void Generate_OTP(MySqlConnection dbconn,
            string pMobileNumber,
            string pUserName,
            Label pLabel)
        {
            string[] lRecords = CommonClass.FetchRecords("GENERATE_MOBILE_ACTIVATION_CODE",
                new string[] 
                { 
                    "P_USER_ID",
                    "P_MOBILE_NUMBER"
                },
                new string[] 
                {
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    pMobileNumber
                },
                new string[] 
                {
                    "P_ACTIVATION_CODE"
                },
                dbconn);

            if (lRecords[0] != null)
            {
                Send_Mobile_Validate_Message(pMobileNumber,
                    pUserName,
                    lRecords[0],
                    dbconn);
                pLabel.Text = "OTP Successfully Generated";
                pLabel.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                pLabel.ForeColor = System.Drawing.Color.Red;
                pLabel.Text = "Unable to Generate OTP";
            }
        }

        public static void Validate_OTP(MySqlConnection dbconn,
            string pMobileNumber,
            string pOTP,
            Label pLabel)
        {
            string[] lRecords = CommonClass.FetchRecords("VALIDATE_MOBILE",
                new string[]
                    {
                        "P_USER_ID",
                        "P_MOBILE_NUMBER",
                        "P_VALIDATION_CODE"
                    },
                new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        HttpContext.Current.Server.HtmlEncode(pMobileNumber),
                        HttpContext.Current.Server.HtmlEncode(pOTP)
                    },
                new string[]
                    {
                        "P_STATUS"
                    },
                dbconn);

            pLabel.Visible = true;
            if (lRecords[0] != null)
            {
                pLabel.Text = lRecords[0];
                pLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                pLabel.Text = "OTP Successfully Validated";
                pLabel.ForeColor = System.Drawing.Color.Green;
            }
        }

        protected static string[] Get_Message(MySqlConnection dbconn,
            string p_TemplateType)
        {
            string[] lRecords = CommonClass.FetchRecords("GET_TEMPLATE",
                 new string[] 
                 {
                     "P_TEMPLATE_TYPE",                     
                 },
                 new string[]
                 {
                     p_TemplateType
                 },
                new string[]
                {
                    "P_SMS_TEMPLATE_MESSAGE",
                     "P_EMAIL_TEMPLATE_MESSAGE",
                     "P_EMAIL_SUBJECT"
                },
                dbconn);
            return lRecords;
        }

        public static void Send_Order_Cancellation_Request_Message(
            string pOrderNumber,
            string pUserId,
            MySqlConnection dbconn)
        {

            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);

            string[] lRecords = Get_Message(dbconn, "CANCELLATION_REQUEST");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;

            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }

        public static void Send_Order_Undelivered_Message(
            string pOrderNumber,
            string pUnDeliveredReason,
            string pUserId,
            MySqlConnection dbconn)
        {

            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);

            string[] lRecords = Get_Message(dbconn, "ORDER_UNDELIVERED");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;
            lVariableModel.Undelivered_Reason = pUnDeliveredReason;

            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }

        public static void Send_Refund_Request_Message(
            string pOrderNumber,
            string pRefundAmount,
            string pUserId,
            MySqlConnection dbconn)
        {

            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);

            string[] lRecords = Get_Message(dbconn, "REFUND_REQUEST");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;
            lVariableModel.Refund_Amount = pRefundAmount;

            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }

        public static void Send_Reject_Refund_Request_Message(
            string pOrderNumber,
            string pRejectionReason,
            string pUserId,
            MySqlConnection dbconn)
        {
            UserDetails lUserDetails = Get_Contact_Details(pUserId,
               dbconn);

            string[] lRecords = Get_Message(dbconn, "REJECT_REFUND_REQUEST");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;
            lVariableModel.Rejection_Reason = pRejectionReason;

            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }



        public static void Send_Message(string[] lMessageDetails, UserDetails pUserDetails)
        {
            if (lMessageDetails[0] != null && lMessageDetails[0] != "")
            {
                send_sms(
                    pUserDetails.Mobile_Number,
                    lMessageDetails[0]);
            }

            if (lMessageDetails[1] != null && lMessageDetails[1] != "")
            {
                send_Email(pUserDetails.Email,
                    pUserDetails.User_Name,
                    lMessageDetails[2],
                    lMessageDetails[1]);
            }
        }

        public static void Send_Feedback_Message(string pEmail,
            string pFullName,
            string pMessage)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string[] lRecords = Get_Message(dbconn, "FEEDBACK_REPLY");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = pFullName;
            lVariableModel.Email = pEmail;
            lVariableModel.Feedback_Message = pMessage;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[1] != null && lRecords[1] != "")
            {
                send_Email(pEmail,
                    pFullName,
                    lRecords[2],
                    lRecords[1]);
            }

            dbconn.Close();
        }
        public static void Send_Mobile_Validate_Message(string pMobileNumber,
            string pUserName,
            string pActivationCode,
            MySqlConnection dbconn)
        {
            string[] lRecords = Get_Message(dbconn, "MOBILE_VALIDATE");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = pUserName;
            lVariableModel.Activation_Code = pActivationCode;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[0] != null && lRecords[0] != "")
            {
                string lSmsMessage = lRecords[0];
                send_sms(pMobileNumber,
                    lSmsMessage);
            }

        }

        public static void Send_Order_Delivered_Message(
            string pOrderNumber,
            string pUserId,
            string pDeliveredDate,
            MySqlConnection dbconn)
        {

            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);

            string[] lRecords = Get_Message(dbconn, "ORDER_DELIVERED");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;
            lVariableModel.Delivered_Date = pDeliveredDate;

            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }

        public static void Send_Due_Remainder(MySqlConnection dbconn,
            string pUserId)
        {
            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);
            string[] lRecords = Get_Message(dbconn, "DUE_REMAINDER");
            string lSMSTemplate = lRecords[0];
            string lEmailTemplate = lRecords[1];
            string lEmailSubject = lRecords[2];
            MySqlDataReader reader = CommonClass.FetchRecords("GET_DUE_ORDER_LIST_BY_USER_ID",
                new string[]
                {
                    "P_USER_ID"
                },
                new string[]
                {
                    pUserId
                },
                dbconn);
            string lOrderList = "<TABLE WIDTH=100% ALIGN=CENTER BORDER=1>";
            lOrderList += "<TR>";
            lOrderList += "<TH>Order Number</TH>";
            lOrderList += "<TH>Order Date</TH>";
            lOrderList += "<TH>Due Amount</TH>";
            lOrderList += "</TR>";
            double lTotal = 0;
            while (reader.Read())
            {
                VariableModel lVariableModel = new VariableModel();
                lVariableModel.User_Name = lUserDetails.User_Name;
                lVariableModel.Order_Number = reader["ORDER_NUMBER"].ToString();
                lVariableModel.Order_Amount = reader["ORDER_DUE"].ToString();
                lTotal += double.Parse(reader["ORDER_DUE"].ToString());
                string[] lRecords1 = Format_Messages(new string[] { lSMSTemplate, lEmailTemplate, lEmailSubject },
                                            lVariableModel);
                send_sms(lUserDetails.Mobile_Number,
                    lRecords1[0]);
                lOrderList += "<TR>";
                lOrderList += "<TD>" + reader["ORDER_NUMBER"].ToString() + "</TD>";
                lOrderList += "<TD>" + reader["ORDER_DATE"].ToString() + "</TD>";
                lOrderList += "<TD ALIGN=CENTER>₹ " + reader["ORDER_DUE"].ToString() + "</TD>";
                lOrderList += "</TR>";

                //Send_Message(lRecords, lUserDetails);
            }

            lOrderList += "<TR>";
            lOrderList += "<TH COLSPAN=2>Total Due</TH>";
            lOrderList += "<TH align=right>₹ " + lTotal.ToString() + "</TH>";
            lOrderList += "</TR>";
            lOrderList += "<TR>";
            lOrderList += "<Th COLSPAN=3 ALIGN=CENTER>" + CommonClass.NumberToWords(int.Parse(lTotal.ToString())).ToString() + "</TD>";
            lOrderList += "</TR>";
            lOrderList += "</TABLE>";
            reader.Close();
            VariableModel lVariableModel1 = new VariableModel();
            lVariableModel1.User_Name = lUserDetails.User_Name;
            lVariableModel1.Order_Summary = lOrderList;
            string[] lMessages = Format_Messages(new string[] { lSMSTemplate, lEmailTemplate, lEmailSubject },
                                        lVariableModel1);

            send_Email(lUserDetails.Email,
                lUserDetails.User_Name,
                lMessages[2],
                lMessages[1]);
        }

        public static void Send_Order_Confirmation_Message(string pEmail,
            string pUserName,
            string pUserAddress,
            string pMobileNumber,
            string pOrderNo,
            string pOrderDate,
            string pOrderAmount,
            string pOrderSummary,
            string pSubtotal,
            string pShippingCharge,
            string pGrandTotal,
            string pDiscount,
            string pPaymentType,
            MySqlConnection dbconn)
        {
            string[] lRecords = null;
            if (pPaymentType == "Cash")
            {
                lRecords = Get_Message(dbconn, "CASH_ORDER");
            }
            else
            {
                lRecords = Get_Message(dbconn, "SUCESSFULL_ORDER");
            }
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = pUserName;
            lVariableModel.Email = pEmail;
            lVariableModel.User_Address = pUserAddress;
            lVariableModel.Order_Date = pOrderDate;
            lVariableModel.Order_Number = pOrderNo;
            lVariableModel.Order_Amount = pOrderAmount;
            lVariableModel.Order_Summary = pOrderSummary;
            lVariableModel.Subtotal_Amount = pSubtotal;
            lVariableModel.Shipping_Charges = pShippingCharge;
            lVariableModel.Grand_Total = pGrandTotal;
            lVariableModel.Discount = pDiscount;
            lVariableModel.Payment_Type = pPaymentType;
            lVariableModel.Delivered_Date = pOrderDate;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[0] != null && lRecords[0] != "")
            {
                string lSmsMessage = lRecords[0];
                send_sms(pMobileNumber,
                    lSmsMessage);
                send_Email(pEmail,
                    pUserName,
                    lRecords[2],
                    lRecords[1]);
                if (CommonClass.Is_Production())
                {
                    send_sms("9980075754",
                        "Dear Karpaga Kumar, New order has been registered for INR " + pOrderAmount + " -Thanks");
                }
            }
        }

        public static void Send_Order_Confirmation_Message_To_Seller(
           string pUserAddress,
           string pOrderNo,
           string pOrderDate,
           string pOrderAmount,
           string pOrderSummary,
           string pSubtotal,
           string pShippingCharge,
           string pGrandTotal,
           string pDiscount,
           string pPaymentType,
           MySqlConnection dbconn)
        {
            string[] lRecords = Get_Message(dbconn, "NEW_ORDER");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = HttpContext.Current.Application["COMPANY_NAME"].ToString();
            lVariableModel.Email = HttpContext.Current.Application["EMAIL_ADDRESS"].ToString();
            lVariableModel.User_Address = pUserAddress;
            lVariableModel.Order_Date = pOrderDate;
            lVariableModel.Order_Number = pOrderNo;
            lVariableModel.Order_Amount = pOrderAmount;
            lVariableModel.Order_Summary = pOrderSummary;
            lVariableModel.Subtotal_Amount = pSubtotal;
            lVariableModel.Shipping_Charges = pShippingCharge;
            lVariableModel.Grand_Total = pGrandTotal;
            lVariableModel.Discount = pDiscount;
            lVariableModel.Payment_Type = pPaymentType;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[1] != null && lRecords[1] != "")
            {
                //string lSmsMessage = lRecords[0];
                //send_sms(pMobileNumber,
                //lSmsMessage);
                if (CommonClass.Is_Production())
                {
                    send_Email("helpdesk.gilld@gmail.com",
                        HttpContext.Current.Application["COMPANY_NAME"].ToString(),
                        lRecords[2],
                        lRecords[1]);
                }
                send_Email("karpaga.kumar@gmail.com",
                   HttpContext.Current.Application["COMPANY_NAME"].ToString(),
                   lRecords[2],
                   lRecords[1]);
            }
            //Generate_Bill(lVariableModel);
        }

        private static void AddCell(string lText, PdfPTable table)
        {
            AddCell(lText, 1, table, 0, 10);
        }

        private static void AddCell(string lText,
            int colspan,
            PdfPTable table,
            int Alignment,
            int pFontSize)
        {
            AddCell(lText, colspan, table, Alignment, pFontSize, iTextSharp.text.BaseColor.BLACK);
        }

        private static void AddCell(string lText,
            int colspan,
            PdfPTable table,
            int Alignment,
            int pFontSize,
            iTextSharp.text.BaseColor color)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            cell.Colspan = colspan;
            cell.HorizontalAlignment = Alignment;
            Font f = new Font(Font.FontFamily.TIMES_ROMAN);
            f.Color = color;
            f.Size = pFontSize;
            cell.Phrase = new Phrase(lText, f);
            table.AddCell(cell);
        }

        private static void AddCell(PdfPTable table)
        {
            AddCell(" ", 5, table, 0, 10);
        }

        private static void AddCellWithLine(PdfPTable table)
        {
            AddCell("\n", 5, table, 0, 10);
        }


        public static void Generate_Bill(VariableModel pVariableModel)
        {
            //double amount = double.Parse(reader["PAYMENT_AMOUNT"].ToString());
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            string lPath = HttpContext.Current.Server.MapPath("Receipts/");
            PdfAWriter.GetInstance(doc, new FileStream(lPath + pVariableModel.Order_Number + ".pdf", FileMode.Create));
            doc.Open();
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.DefaultCell.Border = 1;

            string lImagePath = HttpContext.Current.Server.MapPath("img/") + "navlogo-blue.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(lImagePath);
            img.ScaleToFit(100, 25);
            img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(img, true);
            cell.Border = 0;
            cell.Colspan = 1;
            table.AddCell(cell);
            AddCell("GILLD OIL MILL", 4, table, 1, 24, iTextSharp.text.BaseColor.BLUE);
            AddCell("Door No. 40/22, L.F. Road, Cumbum, Theni District, Tamil Nadu - 625 516", 5, table, 0, 10);
            AddCell("Tin :", table);
            AddCell("33406348085", table);
            AddCell("", table);
            AddCell("CST :", table);
            AddCell("1261529", table);
            AddCell("Order No : ", table);
            AddCell(pVariableModel.Order_Number, table);
            AddCell("", table);
            AddCell("Order Date : ", table);
            AddCell(pVariableModel.Order_Date, table);
            AddCell("To :", table);
            AddCell(pVariableModel.User_Address.Replace("<br>", "\n"), 4, table, 0, 10);
            AddCell("Payment Type : ", table);
            AddCell(pVariableModel.Payment_Type, table);
            AddCell("", 3, table, 0, 10);
            AddCell("Product", table);
            AddCell("Size", table);
            AddCell("Quantity", table);
            AddCell("Price", table);
            AddCell("Total", table);

            for (int lCounter = 0; lCounter < 3; lCounter++)
            {
                AddCellWithLine(table);
            }

            for (int lCounter = 0; lCounter < 25; lCounter++)
            {
                AddCell(table);
            }


            AddCell("Note : ", 5, table, 0, 10);
            AddCell("This is the Computer Generated Receipt on behalf of " + HttpContext.Current.Application["COMPANY_NAME"].ToString(), 5, table, 0, 10);
            AddCell("Hence No Signature is required.", 5, table, 0, 10);

            doc.Add(table);

            doc.Close();
        }

        public static void Send_Welcome_Email_Validate_Message(string pEmail,
            string pUserName,
            string pGuid,
            string pUserGuid,
            MySqlConnection dbconn)
        {
            string[] lRecords = Get_Message(dbconn, "WELCOME_EMAIL_VALIDATE");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = pUserName;
            lVariableModel.Guid = pGuid;
            lVariableModel.UserGuid = pUserGuid;
            lVariableModel.Email = pEmail;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[1] != null && lRecords[1] != "")
            {
                send_Email(pEmail,
                    pUserName,
                    lRecords[2],
                    lRecords[1]);
            }
        }

        public static void Send_Welcome_Message(string pEmail,
            string pUserName,
            MySqlConnection dbconn)
        {
            string[] lRecords = Get_Message(dbconn, "WELCOME_EMAIL");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = pUserName;
            lVariableModel.Email = pEmail;
            lRecords = Format_Messages(lRecords,
                lVariableModel);
            if (lRecords[1] != null && lRecords[1] != "")
            {
                send_Email(pEmail,
                    pUserName,
                    lRecords[2],
                    lRecords[1]);
            }
        }


        public static void Send_Missed_Order_Message(string pUserId,
            string pOrderDate,
            MySqlConnection dbconn)
        {
            UserDetails lUserDetails = Get_Contact_Details(pUserId,
                dbconn);

            string[] lRecords = Get_Message(dbconn, "MISSED_ORDER");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Date = pOrderDate;


            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);
        }

        private static string[] Format_Messages(string[] pMessages,
            VariableModel lVariableModel)
        {

            for (int lCounter = 0; lCounter < 3; lCounter++)
            {
                if (pMessages[lCounter] != null)
                {
                    pMessages[lCounter] = pMessages[lCounter].Replace("%User_Name%", lVariableModel.User_Name);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Website_Address%", "www." + HttpContext.Current.Application["WEB_ADDRESS"].ToString());
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Activation_Code%", lVariableModel.Activation_Code);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%WebMaster_Email_Address%", HttpContext.Current.Application["WEB_MASTER_ADDRESS"].ToString());
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Guid%", lVariableModel.Guid);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Guid1%", lVariableModel.UserGuid);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Email%", lVariableModel.Email);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Feedback_Message%", lVariableModel.Feedback_Message);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Order_Number%", lVariableModel.Order_Number);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Order_Amount%", lVariableModel.Order_Amount);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Subtotal_Amount%", lVariableModel.Subtotal_Amount);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Currency_Symbol%", "₹ ");
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Shipping_Charges%", lVariableModel.Shipping_Charges);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Grand_Total%", lVariableModel.Grand_Total);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Order_Summary%", lVariableModel.Order_Summary);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%User_Address%", lVariableModel.User_Address);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Order_Date%", lVariableModel.Order_Date);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Corporate_Mobile%", HttpContext.Current.Application["CONTACT_MOBILE_NUMBER"].ToString());
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Corporate_Email%", HttpContext.Current.Application["EMAIL_ADDRESS"].ToString());
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Discount%", lVariableModel.Discount);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Currency_Symbol%", "₹ ");
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Shipping_Details%", lVariableModel.Shipping_Details);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Refund_Amount%", lVariableModel.Refund_Amount);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Rejection_Reason%", lVariableModel.Rejection_Reason);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Delivered_Date%", lVariableModel.Delivered_Date);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Undelivered_Reason%", lVariableModel.Undelivered_Reason);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Payment_Type%", lVariableModel.Payment_Type);
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Company_Name%", HttpContext.Current.Application["COMPANY_NAME"].ToString());
                    pMessages[lCounter] = pMessages[lCounter].Replace("%Host%", HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
                }
            }
            return pMessages;
        }





        public static string send_sms(string pMobileNo,
                                  string pMessage)
        {
            string responseFromServer = "";
            if (ConfigurationManager.AppSettings["SMSOnline"].ToString() != "No" || CommonClass.Is_Production())
            {
                if (pMobileNo != "" && pMobileNo != null)
                {
                    pMessage = HttpContext.Current.Server.UrlEncode(pMessage);

                    WebRequest request = WebRequest.Create("http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=" + CommonClass.SMS_User_Id + "&password=" + CommonClass.SMS_Pasword + "&sendername=" + HttpContext.Current.Application["SMS_SENDER_ID"].ToString() + "&mobileno=" + pMobileNo + "&message=" + pMessage);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    if (responseFromServer != "" && responseFromServer != null && responseFromServer.Contains("Your message is successfully") == false)
                    {
                        throw new Exception(responseFromServer);
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Write("");
            }

            return responseFromServer;
        }

        public static void send_Email(string pEmail,
                                    string pFullName,
                                    string pSubject,
                                    string pMessage)
        {
            if (ConfigurationManager.AppSettings["EmailOnline"].ToString() != "No" || CommonClass.Is_Production())
            {
                if (pEmail != "" && pEmail != null)
                {

                    pMessage = "<strong>Dear " + pFullName + "</strong>,<br><br>" + pMessage;

                    LinkedResource Logo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/img/logo.png"), "image/png");
                    //            LinkedResource Logo = new LinkedResource(pLogoPath, "iamge/png");
                    Logo.ContentId = "logo";
                    Logo.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                    LinkedResource facebook = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/img/facebook.png"), "image/png");
                    //            LinkedResource Logo = new LinkedResource(pLogoPath, "iamge/png");
                    facebook.ContentId = "facebook";
                    facebook.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    
                    LinkedResource thickdivider = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/img/thickdivider.jpg"), "image/jpg");
                    LinkedResource divider = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/img/divider.jpg"), "image/jpg");
                    
                    thickdivider.ContentId = "thickdivider";
                    thickdivider.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    divider.ContentId = "divider";
                    divider.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                    string lHtmlTemplate = HttpContext.Current.Server.HtmlDecode(HtmlTemplate.Replace("%Message%", pMessage));
                    AlternateView AV1 = AlternateView.CreateAlternateViewFromString(lHtmlTemplate, null, MediaTypeNames.Text.Html);
                    AV1.LinkedResources.Add(Logo);                    
                    AV1.LinkedResources.Add(thickdivider);
                    AV1.LinkedResources.Add(divider);
                    AV1.LinkedResources.Add(facebook);
                    MailAddress from = new MailAddress(HttpContext.Current.Application["WEB_MASTER_ADDRESS"].ToString(), HttpContext.Current.Application["COMPANY_NAME"].ToString());
                    MailAddress to = new MailAddress(pEmail, pFullName);
                    MailMessage message = new MailMessage(from, to);
                    message.ReplyToList.Add(new MailAddress(HttpContext.Current.Application["WEB_MASTER_ADDRESS"].ToString(), HttpContext.Current.Application["COMPANY_NAME"].ToString()));
                    message.Subject = pSubject;
                    message.AlternateViews.Add(AV1);
                    //message.Body = pMessage;
                    //message.IsBodyHtml = true;
                    message.Priority = MailPriority.High;

                    SmtpClient client = new SmtpClient();
                    client.Port = 8889;
                    client.Host = "mail." + HttpContext.Current.Application["WEB_ADDRESS"].ToString().Replace("http://", "").Replace("https://", "").Replace("www.", "");
                    client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Application["WEB_MASTER_ADDRESS"].ToString(), CommonClass.SMTP_Password);
                    client.Send(message);
                    client.Dispose();
                    message = null;
                    Logo.Dispose();                    
                    thickdivider.Dispose();
                    divider.Dispose();
                    facebook.Dispose();
                }
            }

            return;
        }

       

        public static UserDetails Get_Contact_Details(string pUserId,
            MySqlConnection dbconn)
        {
            UserDetails lUserDetails = new UserDetails();
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_USER_BY_ID",
                                new string[] { "P_USER_ID" },
                                new string[] { pUserId },
                                dbconn);

            if (lMySqlDataReader.Read())
            {
                lUserDetails.User_Name = lMySqlDataReader["USER_NAME"].ToString();
                if (lMySqlDataReader["EMAIL_ADDRESS_VALIDATED"].ToString() == "1" && lMySqlDataReader["EMAIL_DELIVERY_OPTION"].ToString() == "1")
                {
                    lUserDetails.Email = lMySqlDataReader["EMAIL_ADDRESS"].ToString();
                }
                if (lMySqlDataReader["MOBILE_VALIDATED"].ToString() == "1" && lMySqlDataReader["MOBILE_DELIVERY_OPTION"].ToString() == "1")
                {
                    lUserDetails.Mobile_Number = lMySqlDataReader["MOBILE_NUMBER"].ToString();
                }
                lUserDetails.PUblic_Mobile_Number = lMySqlDataReader["MOBILE_NUMBER"].ToString();
            }

            lMySqlDataReader.Close();
            return lUserDetails;
        }

        

        protected static string HtmlTemplate = "<!DOCTYPE html> " + "\r\n" +
"<html > " + "\r\n" +
"<head> " + "\r\n" +
"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> " + "\r\n" +
"<title>" + HttpContext.Current.Application["COMPANY_NAME"].ToString() + "</title> " + "\r\n" +
"</head> " + "\r\n" +
"<body bgcolor=\"#f7f7f7\"> " + "\r\n" +
"<table width=100% border=0 cellspacing=0 cellpadding=0 bgcolor=\"#8d8e90\"> " + "\r\n" +
  "<tr> " + "\r\n" +
    "<td><table width=600 border=0 cellspacing=0 cellpadding=0 bgcolor=\"#FFFFFF\" align=\"center\"> " + "\r\n" +
        "<tr> " + "\r\n" +
          "<td><table width=100% border=0 cellspacing=0 cellpadding=0> " + "\r\n" +
          "    <tr> " + "\r\n" +           
            "    <td align=left><a href=\"https://www." + HttpContext.Current.Application["WEB_ADDRESS"].ToString() + "\"><img src='cid:logo' width=\"137\" border=0/></a></td> " + "\r\n" +
            "    <td align=right  valign=top><a href=\"" + HttpContext.Current.Application["FACEBOOK_LINK"].ToString() + "\"><img src='cid:facebook' width=\"38\" border=0/></a></td> " + "\r\n" +             
             " </tr> " + "\r\n" +
            "</table></td> " + "\r\n" +
        "</tr> " + "\r\n" +
         "<tr> " + "\r\n" +
          "<td><img src='cid:divider' width=598  style=\"display:block\" border=0 /></td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td>&nbsp;</td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td><table width=100% border=0 cellspacing=0 cellpadding=0> " + "\r\n" +
          "    <tr> " + "\r\n" +
           "     <td width=10%>&nbsp;</td> " + "\r\n" +
            "    <td width=80% align=left valign=top> " + "\r\n" +
             "     <font style=\"font-family: Verdana, Geneva, sans-serif; color:#666766; font-size:13px; line-height:21px\"> " + "\r\n" +
                "%Message%</font></td> " + "\r\n" +
                "<td width=10%>&nbsp;</td> " + "\r\n" +
              "</tr>               " + "\r\n" +
            "</table></td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td>&nbsp;</td> " + "\r\n" +
        "</tr>         " + "\r\n" +
        "<tr> " + "\r\n" +
         "<td><img src='cid:divider' width=598 style=\"display:block\" border=0 /></td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td>&nbsp;</td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td align=center><font style=\"font-family:'Myriad Pro', Helvetica, Arial, sans-serif; color:#231f20; font-size:8px\"><strong>&copy; " + HttpContext.Current.Application["COMPANY_NAME"].ToString() + " 2016.</strong></font></td> " + "\r\n" +
        "</tr> " + "\r\n" +
        "<tr> " + "\r\n" +
         " <td>&nbsp;</td> " + "\r\n" +
        "</tr> " + "\r\n" +
      "</table></td> " + "\r\n" +
  "</tr> " + "\r\n" +
"</table> " + "\r\n" +
"</body> " + "\r\n" +
"</html> " + "\r\n";


        protected static string HtmlTemplate2 = "<html> " + "\r\n" +
"<head> " + "\r\n" +
"<meta http-equiv=Content-Type content=text/html; charset=UTF-8> " + "\r\n" +
"</head> " + "\r\n" +
"<body> " + "\r\n" +
"<div style=\"border-color: #428bca;\"> " + "\r\n" +
    "<div style=\"color: #ffffff;background-color: #428bca;border-color: #428bca;border-top-color: #428bca;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-right-radius: 3px;border-top-left-radius: 3px;\"> " + "\r\n" +
    "<img src='cid:logo' width=150 />" + "\r\n" +
    "</div> " + "\r\n" +
    "<div style=\"padding: 15px;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-right-radius: 3px;border-top-left-radius: 3px;\"> " + "\r\n" +
"   %Message%" + "\r\n" +
"	</div> " + "\r\n" +
"	<div style=\"border-bottom-color: #428bca;padding: 10px 15px;background-color: #f5f5f5;border-top: 1px solid #dddddd;border-bottom-right-radius: 3px;border-bottom-left-radius: 3px;\"> " + "\r\n" +
"   <p align=\"center\">&copy; " + HttpContext.Current.Application["COMPANY_NAME"].ToString() + " 2016.</p>" + "\r\n" +
"	</div> " + "\r\n" +
"</div> " + "\r\n" +
"</body>";

        protected static string HtmlTemplate1 = "<!DOCTYPE html> " + "\r\n" +
       "<head>" + "\r\n" +
           "<meta http-equiv=Content-Type content=text/html; charset=utf-8>" + "\r\n" +
           "<title>" + HttpContext.Current.Application["COMPANY_NAME"].ToString() + "</title>" + "\r\n" +
           "<style type=\"text/css\" >" + "\r\n" +
               ".panel {" + "\r\n" +
                   "margin-bottom: 20px;" + "\r\n" +
                   "background-color: #ffffff;" + "\r\n" +
                   "border: 1px solid transparent;" + "\r\n" +
                   "border-radius: 4px;" + "\r\n" +
                   "-webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.05);" + "\r\n" +
                   "box-shadow: 0 1px 1px rgba(0, 0, 0, 0.05);" + "\r\n" +
               "}" + "\r\n" +
               ".panel-body {" + "\r\n" +
               "   padding: 15px;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-body:before," + "\r\n" +
               ".panel-body:after {" + "\r\n" +
               "   display: table;" + "\r\n" +
               "   content: \" \";" + "\r\n" +
               "}" + "\r\n" +
               ".panel-body:after {" + "\r\n" +
               "   clear: both;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-body:before," + "\r\n" +
               ".panel-body:after {" + "\r\n" +
                   "display: table;" + "\r\n" +
                   "content: \" \";" + "\r\n" +
               "}" + "\r\n" +
               ".panel-body:after {" + "\r\n" +
               "  clear: both;" + "\r\n" +
               "}" + "\r\n" +
               ".panel > .list-group {" + "\r\n" +
               "   margin-bottom: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel > .list-group .list-group-item {" + "\r\n" +
               "     border-width: 1px 0;" + "\r\n" +
               "   }" + "\r\n" +
               ".panel > .list-group .list-group-item:first-child {" + "\r\n" +
               "    border-top-right-radius: 0;" + "\r\n" +
               "    border-top-left-radius: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel > .list-group .list-group-item:last-child {" + "\r\n" +
               "    border-bottom: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-heading + .list-group .list-group-item:first-child {" + "\r\n" +
               "   border-top-width: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel > .table {" + "\r\n" +
               "   margin-bottom: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel > .panel-body + .table {" + "\r\n" +
               "   border-top: 1px solid #dddddd;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-heading {" + "\r\n" +
               "   padding: 10px 15px;" + "\r\n" +
               "   border-bottom: 1px solid transparent;" + "\r\n" +
               "   border-top-right-radius: 3px;" + "\r\n" +
               "   border-top-left-radius: 3px;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-title {" + "\r\n" +
               "   margin-top: 0;" + "\r\n" +
               "   margin-bottom: 0;" + "\r\n" +
               "   font-size: 16px;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-title > a {" + "\r\n" +
               "   color: inherit;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-footer {" + "\r\n" +
               "   padding: 10px 15px;" + "\r\n" +
               "   background-color: #f5f5f5;" + "\r\n" +
               "   border-top: 1px solid #dddddd;" + "\r\n" +
               "   border-bottom-right-radius: 3px;" + "\r\n" +
               "   border-bottom-left-radius: 3px;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-group .panel {" + "\r\n" +
               "   margin-bottom: 0;" + "\r\n" +
               "   overflow: hidden;" + "\r\n" +
               "   border-radius: 4px;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-group .panel + .panel {" + "\r\n" +
               "   margin-top: 5px;" + "\r\n" +
               " }" + "\r\n" +
               ".panel-group .panel-heading {" + "\r\n" +
               "   border-bottom: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-group .panel-heading + .panel-collapse .panel-body {" + "\r\n" +
               "   border-top: 1px solid #dddddd;" + "\r\n" +
               " }" + "\r\n" +
               ".panel-group .panel-footer {" + "\r\n" +
               "   border-top: 0;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-group .panel-footer + .panel-collapse .panel-body {" + "\r\n" +
               "   border-bottom: 1px solid #dddddd;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-primary {" + "\r\n" +
               "   border-color: #428bca;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-primary > .panel-heading {" + "\r\n" +
               "   color: #ffffff;" + "\r\n" +
               "   background-color: #428bca;" + "\r\n" +
               "   border-color: #428bca;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-primary > .panel-heading + .panel-collapse .panel-body {" + "\r\n" +
               "   border-top-color: #428bca;" + "\r\n" +
               "}" + "\r\n" +
               ".panel-primary > .panel-footer + .panel-collapse .panel-body {" + "\r\n" +
               "   border-bottom-color: #428bca;" + "\r\n" +
               "}" + "\r\n" +
           "</style>" + "\r\n" +
       "</head>" + "\r\n" +
       "<body>" + "\r\n" +
       "   <div class=\"panel-primary\">" + "\r\n" +
       "      <div class=\"panel-heading\"><img src='cid:logo' width=150 /></div>" + "\r\n" +
       "      <div class=\"panel-body\">" + "\r\n" +
       "   %Message%" + "\r\n" +
       "       </div>" + "\r\n" +
       "       <div class=\"panel-footer\">" + "\r\n" +
       "   <p align=\"center\">&copy; " + HttpContext.Current.Application["COMPANY_NAME"].ToString() + " 2016.</p>" + "\r\n" +
       "       </div>" + "\r\n" +
       "   </div>" + "\r\n" +
       "</body>" + "\r\n" +
   "</html>";


        public static void Send_Shipping_Confirmation(
            string pOrderNumber,
            string pUserId,
            string pShippingDetails,
            string pCourierAgencyId,
            string pReferenceNumber,
            MySqlConnection dbconn)
        {
            UserDetails lUserDetails = Get_Contact_Details(pUserId,
               dbconn);

            string[] lRecords = Get_Message(dbconn, "ORDER_SHIPPED");
            VariableModel lVariableModel = new VariableModel();
            lVariableModel.User_Name = lUserDetails.User_Name;
            lVariableModel.Order_Number = pOrderNumber;
            lVariableModel.Shipping_Details = pShippingDetails;
            lRecords = Format_Messages(lRecords,
                lVariableModel);

            Send_Message(lRecords, lUserDetails);

            string[] lCourierDetails = CommonClass.FetchRecords("GET_ATTRIBUTE_EXTERNALVALUE",
                new string[] 
                {
                    "P_ATTRIBUTE_ID"
                },
                new string[]
                {
                    pCourierAgencyId
                },
                new string[]
                {
                    "P_EXTERNAL_KEY_VALUE"
                },
                dbconn);

            string lJsonData = "{";
            lJsonData += "\"username\": \"gilldstore\",";
            lJsonData += "\"password\": \"" + "61e98e24e7042aad480165b3fe29426c" + "\",";
            lJsonData += "\"carrier_id\": \"" + lCourierDetails[0] + "\",";
            lJsonData += "\"awb\": \"" + pReferenceNumber + "\",";
            lJsonData += "\"order_id\": \"" + pOrderNumber + "\",";
            lJsonData += "\"email\": \"" + lUserDetails.Email + "\",";
            lJsonData += "\"phone\": \"" + lUserDetails.Mobile_Number + "\",";
            lJsonData += "\"first_name\": \"" + lUserDetails.User_Name + "\",";
            lJsonData += "\"last_name\": \"\",";
            lJsonData += "\"products\": \"N/A\",";
            lJsonData += "\"Company\": \"Gilld Store\"";
            lJsonData += "}";


            RestClient lRestClient = new RestClient("https://shipway.in/api/pushOrderData");
            RestRequest lRestRequest = new RestRequest(Method.POST);            
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
            HttpContext.Current.Response.Write(lRestResponse.Content.ToString());
        }

        internal static void send_Email(string p1, string p2, string p3)
        {
            throw new NotImplementedException();
        }
    }
}