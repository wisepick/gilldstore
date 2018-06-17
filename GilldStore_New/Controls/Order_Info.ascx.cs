using GilldStore_New.App_Start;
using GilldStore_New.Controls.Order;
using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Order_Info : System.Web.UI.UserControl
    {
        string[] lForms = new string[]
        {
            "PreCancellationForm1",
            "ShippingInformationForm1",
            "CancellationApprovalForm1",
            "CancellationRejectionForm1",
            "OrderDeliveryForm1",
            "UnDeliveredForm1"
        };

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        public void Populate_Orders(MySqlConnection dbconn,
            string lOrderId)
        {

            CommonClass.FetchRecordsAndBind("GET_ORDER_BY_ID",
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
                                dbconn,
                                Order_Summary_FormView);
            CommonClass.FetchRecordsAndBind("GET_ORDER_DETAILS_BY_ID",
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
                dbconn,
                Order_Detail_Summary);

            CommonClass.FetchRecordsAndBind("GET_PAYMENT_BY_ORDER_ID",
                new string[]
                {
                    "ORDER_ID"
                },
                new string[]
                {
                    lOrderId
                },
                dbconn,
                Payment_Repeater);
            CommonClass.FetchRecordsAndBind("GET_SHIPPING_INFO",
                new string[]
                {
                    "P_ORDER_ID"
                },
                new string[]
                {
                    lOrderId
                },
                dbconn,
                Shipping_Repeater);

            Shipping_Info.Visible = true;
            if (Shipping_Repeater.Items.Count == 0)
            {
                Shipping_Info.Visible = false;
            }

            CommonClass.FetchRecordsAndBind("GET_REFUND",
                new string[]
                {
                    "P_ORDER_ID"
                },
                new string[]
                {
                    lOrderId
                },
                dbconn,
                Refund_Repeater);

            Refund_Info_PlaceHolder.Visible = true;
            if (Refund_Repeater.Items.Count == 0)
            {
                Refund_Info_PlaceHolder.Visible = false;
            }


            if (Order_Summary_FormView.DataKey.Values[2].ToString() != "Order Accepted")
            {
                Thank_You_Message_PlaceHolder.Visible = false;
            }

        }

        public bool Show_Order_Message
        {
            get
            {
                return Thank_You_Message_PlaceHolder.Visible;
            }
            set
            {
                Thank_You_Message_PlaceHolder.Visible = value;
            }
        }

        protected void Show_PreCancellationForm(object sender, EventArgs e)
        {
            PreCancellationForm PreCancellationForm1 = (PreCancellationForm)Order_Summary_FormView.FindControl("PreCancellationForm1");
            PreCancellationForm1.UserType = "2";
            if (Order_Summary_FormView.DataKey.Values[3].ToString() == "1" && Request.ServerVariables["URL"].Contains("orders.aspx")) //Staff
            {
                PreCancellationForm1.UserType = "1"; //Seller
            }
            PreCancellationForm1.Load_Form();
            Show_Form("PreCancellationForm1");
        }

        protected void Show_ShippingInformationForm(object sender, EventArgs e)
        {
            ShippingInformationForm ShippingInformationForm1 = (ShippingInformationForm)Order_Summary_FormView.FindControl("ShippingInformationForm1");
            ShippingInformationForm1.Load_Form();
            Show_Form("ShippingInformationForm1");

        }

        protected void Show_CancellationApprovalForm(object sender, EventArgs e)
        {
            Show_Form("CancellationApprovalForm1");
        }

        protected void Show_OrderDeliveryForm(object sender, EventArgs e)
        {
            Show_Form("OrderDeliveryForm1");
        }

        protected void Show_UndeliveredForm(object sender, EventArgs e)
        {
            UnDeliveredForm UnDeliveredForm1 = (UnDeliveredForm)Order_Summary_FormView.FindControl("UnDeliveredForm1");
            UnDeliveredForm1.Load_Form();
            Show_Form("UnDeliveredForm1");
        }

        protected void Show_CancellationRejectionForm(object sender, EventArgs e)
        {
            CancellationRejectionForm CancellationRejectionForm1 = (CancellationRejectionForm)Order_Summary_FormView.FindControl("CancellationRejectionForm1");
            CancellationRejectionForm1.Load_Form();
            Show_Form("CancellationRejectionForm1");
        }

        protected void Refresh_Order_Info(object sender, EventArgs e)
        {
            UserControl lUserControl = (UserControl)sender;
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Populate_Orders(dbconn, Order_Summary_FormView.DataKey[0].ToString());
            dbconn.Close();
            lUserControl.Visible = false;
        }


        protected void Show_Form(string pFormName)
        {
            foreach (string lFormName in lForms)
            {
                Order_Summary_FormView.FindControl(lFormName).Visible = false;
                if (lFormName == pFormName)
                {
                    Order_Summary_FormView.FindControl(lFormName).Visible = true;
                }
            }
        }



        protected void Return_Order_Button_Click(object sender, EventArgs e)
        {

        }

        protected void Failed_Mail_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Messages.Send_Missed_Order_Message(Order_Summary_FormView.DataKey[1].ToString(),
                Order_Summary_FormView.DataKey[7].ToString(),
                dbconn);
            dbconn.Close();
        }

        protected void Void_Order_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.ExecuteQuery("VOID_ORDER",
                new string[] 
                {
                    "P_ORDER_ID",
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    Order_Summary_FormView.DataKey[0].ToString(),
                    ClaimsPrincipal.Current.FindFirst("user_id").Value
                },
                dbconn);
            Populate_Orders(dbconn, Order_Summary_FormView.DataKey[0].ToString());
            dbconn.Close();
        }

        protected void Resend_To_Seller_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderClass.Resend_Order_Information(dbconn,
                Order_Summary_FormView.DataKey[0].ToString());
            dbconn.Close();
        }

        protected void Confirm_Button_Click(object sender, EventArgs e)
        {
            PaymentClass.CCAvenue_Confirm_Order(Order_Summary_FormView.DataKey[0].ToString(),
                Order_Summary_FormView.DataKey[4].ToString());
        }

        protected void View_Button_Click(object sender, EventArgs e)
        {
            string lJsonData = "{";
            lJsonData += "\"username\": \"gilldstore\",";
            lJsonData += "\"password\": \"" + "61e98e24e7042aad480165b3fe29426c" + "\",";
            lJsonData += "\"order_id\": \"" + Order_Summary_FormView.DataKey[6].ToString() + "\"";            
            lJsonData += "}";

            Response.Write(lJsonData);


            RestClient lRestClient = new RestClient("https://shipway.in/api/getOrderShipmentDetails");
            RestRequest lRestRequest = new RestRequest(Method.POST);
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);            
        }

        //protected void Print_Button_Click(object sender, EventArgs e)
        //{

        //}

    }
}