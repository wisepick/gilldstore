using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using paytm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Order_Form_Control : System.Web.UI.UserControl
    {
        public event EventHandler OrderLoad;
        public event EventHandler SuccessOrder;
        private string WalletPaymentType = "Paytm Wallet";
        private string OthersPaymentType = "Other Payment Types";
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        public void Prepare_Order_Form()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Populate_Products(dbconn);
            Show_Active_Promotion(dbconn);
            AddressBook1.Load_Address(dbconn, true);
            //AddressBook1.Load_Address(dbconn);



            if (Store_Place_Holder.Visible == false)
            {
                PAYMENT_TYPE_ID.Value = WalletPaymentType;
                ORDER_STATUS.Value = "Open";
            }

            Payment_Received_Flag.ClearSelection();
            Payment_Type.ClearSelection();
            Delivery_Method.ClearSelection();
            Calculate_Grand_Total();
            Payment_Received_Information.Visible = false;
            PaymentControl1.Prepare_Payment_Form(dbconn);
            PaymentControl1.Clear_Cheque_Details();


            dbconn.Close();

            if (OrderLoad != null)
            {
                OrderLoad(this, EventArgs.Empty);
            }
        }


        protected void Populate_Products(MySqlConnection dbconn)
        {

            CommonClass.FetchRecordsAndBind("GET_ACIVE_PRODUCTS",
                new string[] { "PHOTO_IND" },
                new string[] { "0" },
                dbconn,
                Product_ListView);

        }

        protected void Show_Active_Promotion(MySqlConnection dbconn)
        {
            string[] lRecords = CommonClass.FetchRecords("GET_ACTIVE_PROMOTIONS",
                new string[]
                {                    
                    "PROMOTION_TYPE"
                },
                new string[]
                {
                    "GLOBAL"
                },
                new string[]
                {
                    "P_PROMOTION_ID",
                    "P_TOTAL_QUANTITY",
                    "P_TOTAL_ORDER"
                },
                dbconn);
            if (lRecords != null)
            {
                Discounted_Quantity.Text = lRecords[1];
                Discounted_Quantity_PlaceHolder.Visible = true;
            }
        }

        protected void Product_ListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem lvdi = (ListViewDataItem)e.Item;
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();

            CommonClass.FetchRecordsAndBind("GET_PRODUCT_PRICE",
                                        new string[] { "P_PRODUCT_ID", "P_CUSTOMER_ID", "P_EXTERNAL_USER_ID" },
                                        new string[] 
                                        { 
                                            Product_ListView.DataKeys[lvdi.DataItemIndex].Value.ToString(), 
                                            USER_ID.Value,
                                            ClaimsPrincipal.Current.FindFirst("user_id").Value
                                        },
                                        dbconn,
                                        e.Item.FindControl("Product_Order_ListView"));
            dbconn.Close();
        }

        protected void Quantity_DropDown_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ListViewItem lvi = (ListViewItem)ddl.Parent;

            ListView lv = (ListView)lvi.Parent.Parent;

            Label lbl = ddl.Parent.FindControl("Subtotal") as Label;
            int lQuantity = int.Parse(ddl.SelectedValue);
            double lUnitPrice = double.Parse(lv.DataKeys[lvi.DataItemIndex].Values[2].ToString());

            lbl.Text = (lQuantity * lUnitPrice).ToString();
            Calculate_Grand_Total();

        }

        protected Dictionary<string, string> Get_Global_Discounts(MySqlConnection dbconn)
        {
            UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
            if (lUserModel == null)
            {
                UserDetails lUserDetails = Messages.Get_Contact_Details(USER_ID.Value, dbconn);
                lUserModel.Email_Address = lUserDetails.Email;
            }
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_GLOBAL_DISCOUNTS",
                new string[]
                {
                    "P_EMAIL_ADDRESSS"
                },
                new string[]
                {
                    lUserModel.Email_Address
                },
                dbconn);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            while (lMySqlDataReader.Read())
            {
                parameters.Add(lMySqlDataReader["PRODUCT_ID"].ToString() + "-" + lMySqlDataReader["MEASUREMENT_UNIT"].ToString(), lMySqlDataReader["DISCOUNT"].ToString());
            }
            lMySqlDataReader.Close();
            return parameters;
        }

        protected void Calculate_Grand_Total()
        {
            GLOBAL_DISCOUNT_APPLIED.Value = "0";
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Dictionary<string, string> parameters = Get_Global_Discounts(dbconn);
            double lGrandTotal = 0.0;
            double lShippingCharges = 0.0;
            double lTotalQuantity = 0.0;
            double lDiscounts = 0.0;
            foreach (ListViewItem item in Product_ListView.Items)
            {
                ListView lv = (ListView)item.FindControl("Product_Order_ListView");
                foreach (ListViewItem item1 in lv.Items)
                {
                    DropDownList ddl = (DropDownList)item1.FindControl("Quantity_Dropdown");
                    double lQuantity = int.Parse(ddl.SelectedValue);
                    double lSize = double.Parse(lv.DataKeys[item1.DataItemIndex].Values[1].ToString());
                    if (lv.DataKeys[item1.DataItemIndex].Values[4].ToString() == "Litre")
                    {
                        lTotalQuantity += (lQuantity * lSize);
                    }

                    lGrandTotal += double.Parse((item1.FindControl("Subtotal") as Label).Text);
                    double lShippingCharge = double.Parse(lv.DataKeys[item1.DataItemIndex].Values[3].ToString());
                    if (Delivery_Method.SelectedValue == "" || Delivery_Method.SelectedValue == "3")
                    {
                        lShippingCharges += (lQuantity * lShippingCharge);
                    }
                    string lstr = lv.DataKeys[item1.DataItemIndex].Values[0].ToString() + "-" + lv.DataKeys[item1.DataItemIndex].Values[1].ToString();
                    //Response.Write(lstr + "<br>");
                    if (parameters.ContainsKey(lstr))
                    {
                        //  Response.Write(lQuantity + "<br>");
                        // Response.Write(parameters[lstr] + "<br>");
                        lDiscounts += (lQuantity * double.Parse(parameters[lstr]));
                        GLOBAL_DISCOUNT_APPLIED.Value = "1";
                    }
                }
            }
            Shipping_Charges.Text = lShippingCharges.ToString();
            Subtotal_Str.Text = lGrandTotal.ToString();

            string[] lRecords = CommonClass.FetchRecords("GET_ACTIVE_PROMOTIONS",
                new string[]
                {
                    "PROMOTION_TYPE"
                },
                new string[]
                {
                    "GLOBAL"
                },
                new string[]
                {
                    "P_PROMOTION_ID",
                    "P_TOTAL_QUANTITY",
                    "P_TOTAL_ORDER"
                },
                dbconn
                );
            dbconn.Close();
            string lPromotionId = lRecords[0];
            int lDiscountedQuantity = int.Parse(lRecords[1]);
            GLOBAL_PROMOTION_ID.Value = "";

            if (USER_TYPE_ID.Value == "0")
            {
                lDiscounts = lShippingCharges;
                Shipping_Discount.Text = lShippingCharges.ToString();
                double lDiscountValue = 0;
                if (DISCOUNT.Value != null && DISCOUNT.Value != "")
                {
                    lDiscountValue = double.Parse(DISCOUNT.Value);
                }
                if (lDiscountValue > 0)
                {
                    if (DISCOUNT_MEASUREMENT_ID.Value == "1")
                    {
                        lDiscounts += lTotalQuantity * lDiscountValue;
                    }
                    else if (DISCOUNT_MEASUREMENT_ID.Value == "2")
                    {
                        lDiscounts += (lGrandTotal * lDiscountValue) / 100;
                    }
                }

            }
            else
            {
                if (lTotalQuantity >= lDiscountedQuantity || SHIPPING_CHARGE_DISCOUNT_FLAG.Value == "Y")
                {
                    GLOBAL_PROMOTION_ID.Value = lPromotionId;
                    lDiscounts += lShippingCharges;
                    Shipping_Discount.Text = lShippingCharges.ToString();
                }
            }

            //Promotion Discount
            if (Promotion_Id.Value != "")
            {
                if (lGrandTotal > 0.0)
                {
                    if (Promotion_Amount.Value != "")
                    {
                        lDiscounts += double.Parse(Promotion_Amount.Value);
                    }
                    else if (Promotion_Percentage.Value != "")
                    {
                        double lPromotionDiscount = (lGrandTotal * double.Parse(Promotion_Percentage.Value)) / 100;
                        double lPromotionMaximum = double.Parse(Promotion_Maximum.Value);
                        if (lPromotionMaximum > 0)
                        {
                            lDiscounts += Math.Min(lPromotionDiscount, lPromotionMaximum);
                        }
                        else
                        {
                            lDiscounts += lPromotionDiscount;
                        }
                    }
                }
            }

            Discounts.Text = lDiscounts.ToString();

            Grand_Total.Text = (lGrandTotal + lShippingCharges - lDiscounts).ToString();
        }

        protected bool Validate_Order()
        {
            bool IsValid = true;
            Message.Text = "";
            if (Grand_Total.Text == "0.0" || Grand_Total.Text == "0")
            {
                Message.Text += "Atleast one product should be ordered<br>";
                IsValid = false;
            }


            if (Store_Place_Holder.Visible == true)
            {
                if (Payment_Received_Flag.SelectedValue == "")
                {
                    Message.Text += "Select the Payment Information<br>";
                    IsValid = false;
                }
                if (Payment_Received_Information.Visible == true)
                {
                    if (Payment_Type.SelectedValue == "")
                    {
                        Message.Text += "Select the Payment Type<br>";
                        IsValid = false;
                    }
                    else if (Payment_Type.SelectedItem.Text == "Cheque")
                    {
                        string lPaymentMessage = PaymentControl1.Validate_Cheque_Details();
                        if (lPaymentMessage != null)
                        {
                            Message.Text += lPaymentMessage + "<br>";
                            IsValid = false;
                        }
                    }
                }

                if (Delivery_Method.SelectedValue == "")
                {
                    Message.Text += "Select the Delivery Information<br>";
                    IsValid = false;
                }
            }

            if (Online_Payment_PlaceHolder.Visible == true)
            {
                if (Online_Payment_Type.SelectedValue == "")
                {
                    Message.Text += "Select the Payment Type<br>";
                    IsValid = false;
                }
            }

            if (AddressBook1.GetDeliveryAddressId == "" && AddressBook1.Visible == true)
            {
                Message.Text += "Select the Delivery Address<br>";
                IsValid = false;
            }



            if (IsValid && Surprise_Gift_Percentage.Text == "")
            {
                //Surprise_Gift_PlaceHolder.Visible = true;
                if (Surprise_Gift_Percentage.Text == "")
                {
                    Random rand = new Random();
                    int numIterations = rand.Next(1, 6);
                    Surprise_Gift_Percentage.Text = numIterations.ToString();
                }
            }

            if (Surprise_Gift_Percentage.Text != "" && IsValid && GLOBAL_DISCOUNT_APPLIED.Value == "0")
            {
                double lExistingSurpriseGift = double.Parse(Surprise_Gift.Text);
                double discountpercent = double.Parse(Surprise_Gift_Percentage.Text);
                //double lOrderAmount = double.Parse(Grand_Total.Text) + double.Parse(Shipping_Discount.Text) - double.Parse(Shipping_Charges.Text) + lExistingSurpriseGift;
                double lOrderAmount = double.Parse(Subtotal_Str.Text);
                double lSurpriseDiscount = Math.Round((lOrderAmount * discountpercent) / 100);
                // Grand_Total.Text = (double.Parse(Grand_Total.Text) - lSurpriseDiscount).ToString();
                // Discounts.Text = (double.Parse(Discounts.Text) + lSurpriseDiscount).ToString();
                bool lSurprise_Gift_Applied = false;
                double lPromotionalDiscount = double.Parse(Promotional_Discounts.Text);
                if (Surprise_Gift.Text == "")
                {

                    Surprise_Gift.Text = lSurpriseDiscount.ToString();
                    lPromotionalDiscount += lSurpriseDiscount;
                    lSurprise_Gift_Applied = true;
                }
                else
                {
                    //double lExistingSurpriseGift = double.Parse(Surprise_Gift.Text);
                    if (lExistingSurpriseGift != lSurpriseDiscount)
                    {
                        lPromotionalDiscount -= lExistingSurpriseGift;
                        Surprise_Gift.Text = lSurpriseDiscount.ToString();
                        lPromotionalDiscount += lSurpriseDiscount;
                        lSurprise_Gift_Applied = true;
                    }
                }
                if (lSurprise_Gift_Applied == true)
                {

                    string message = "Congratulations, you have won a surprise gift of INR " + lSurpriseDiscount.ToString() + ", Press Place Order Again to continue";

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                    IsValid = false;
                }
                Promotional_Discounts.Text = lPromotionalDiscount.ToString();
                Discounts.Text = (double.Parse(Shipping_Discount.Text) + lPromotionalDiscount).ToString();
                Grand_Total.Text = (lOrderAmount + double.Parse(Shipping_Charges.Text) - double.Parse(Shipping_Discount.Text) - lPromotionalDiscount).ToString();
            }
            return IsValid;

        }

        protected void Order_Form_OnCommand(object sender, CommandEventArgs e)
        {
            Message.Text = "";
            if (e.CommandName == "Place Order")
            {

                if (Page.IsValid && Validate_Order())
                {
                    MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                    dbconn.Open();
                    string lDeliveryMethod = "3"; // To be couriered
                    if (Delivery_Method.SelectedValue != "")
                    {
                        lDeliveryMethod = Delivery_Method.SelectedValue;
                    }
                    string[] lRecords = CommonClass.ExecuteQuery("ADD_ORDER1",
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
                            "P_STATUS",
                            "P_DELIVERY_METHOD"
                        },
                        new string[]
                        {
                            USER_ID.Value,
                            ClaimsPrincipal.Current.FindFirst("user_id").Value,
                            Grand_Total.Text,
                            Shipping_Charges.Text,
                            Discounts.Text,
                            AddressBook1.GetDeliveryAddressId,
                            Promotion_Id.Value,
                            PAYMENT_TYPE_ID.Value,
                            ORDER_STATUS.Value,
                            lDeliveryMethod
                        },
                        new string[]
                        {
                            "P_ORDER_ID"
                        },
                        dbconn);
                    foreach (ListViewItem item in Product_ListView.Items)
                    {
                        ListView lv = (ListView)item.FindControl("Product_Order_ListView");
                        foreach (ListViewItem item1 in lv.Items)
                        {
                            Label lSubtotalLabel = item1.FindControl("Subtotal") as Label;
                            if (lSubtotalLabel.Text != "0" && lSubtotalLabel.Text != "0.0")
                            {
                                CommonClass.ExecuteQuery("ADD_ORDER_DETAILS",
                                    new string[]
                                    {
                                        "P_ORDER_ID",
                                        "P_PRODUCT_ID",
                                        "P_MEASUREMENT_UNIT",
                                        "P_QUANTITY",
                                        "P_PRICE"
                                    },
                                    new string[]
                                    {
                                        lRecords[0],
                                        lv.DataKeys[item1.DataItemIndex].Values[0].ToString(),
                                        lv.DataKeys[item1.DataItemIndex].Values[1].ToString(),
                                        (item1.FindControl("Quantity_DropDown") as DropDownList).SelectedValue.ToString(),
                                        lv.DataKeys[item1.DataItemIndex].Values[2].ToString()
                                    },
                                    dbconn
                                    );
                            }
                        }
                    }
                    ORDER_ID.Value = lRecords[0];
                    if (PAYMENT_TYPE_ID.Value == "Cash")
                    {
                        PaymentClass.Create_Cash_Payment(dbconn,
                            Grand_Total.Text,
                            USER_ID.Value);
                    }


                    //string lPaymentType = "";
                    //if (PAYMENT_TYPE_ID.Value != "" && PAYMENT_TYPE_ID.Value != null)
                    //{
                    //    MySqlCommand dbcomm = new MySqlCommand("SELECT GET_ATTRIBUTE_VALUE(GET_PAYMENT_TYPE_MASTER_TYPE(), " + PAYMENT_TYPE_ID.Value + ")", dbconn);
                    //   MySqlDataReader reader = dbcomm.ExecuteReader();

                    //                        while (reader.Read())
                    //                      {
                    //                        lPaymentType = reader[0].ToString();
                    //                  }
                    //                reader.Close();
                    //          }

                    if (PAYMENT_TYPE_ID.Value == WalletPaymentType)
                    {
                        UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);

                        PaymentClass.Initiate_PayTm_Order(ORDER_ID.Value,
                            Grand_Total.Text,
                            lUserModel.Email_Address,
                            lUserModel.Mobile_Number);
                        dbconn.Close();
                    }
                    else if (PAYMENT_TYPE_ID.Value == OthersPaymentType)
                    {
                        UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);

                        PaymentClass.Initiate_CCAvenue_Order(ORDER_ID.Value,
                            Grand_Total.Text,
                            lUserModel.Email_Address,
                            lUserModel.Mobile_Number,
                            AddressBook1.GetDeliveryAddressId,
                            dbconn);
                        dbconn.Close();
                    }
                    else
                    {
                        OrderClass.Confirm_Order(dbconn, ORDER_ID.Value);
                        dbconn.Close();

                        if (SuccessOrder != null)
                        {
                            SuccessOrder(this, EventArgs.Empty);
                        }
                    }

                }
            }
        }

        public Boolean Show_Address_Book
        {
            set
            {
                AddressBook1.Visible = value;
            }
        }

        public string OrderId
        {
            get
            {
                return ORDER_ID.Value;
            }
        }


        public Boolean Show_Store_PlaceHolder
        {
            set
            {
                Store_Place_Holder.Visible = value;
            }
        }

        public string Set_Customer_Id
        {
            set
            {
                USER_ID.Value = value;
                AddressBook1.Set_User_Id = value;
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                AddressBook1.Load_Address(dbconn);
                MySqlDataReader reader = CommonClass.FetchRecords("GET_CUSTOMER_BY_ID",
                    new string[] { "P_CUSTOMER_ID" },
                    new string[] { USER_ID.Value },
                dbconn);
                while (reader.Read())
                {
                    USER_TYPE_ID.Value = reader["ONLINE_USER_FLAG"].ToString();
                    DISCOUNT.Value = reader["DISCOUNT"].ToString();
                    DISCOUNT_MEASUREMENT_ID.Value = reader["DISCOUNT_MEASUREMENT_ID"].ToString();
                }
                dbconn.Close();
                Calculate_Grand_Total();
            }
        }

        protected void Payment_Received_Flag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Payment_Received_Flag.SelectedValue == "1")
            {
                Payment_Received_Information.Visible = true;
                ORDER_STATUS.Value = "Order Accepted";
            }
            else
            {
                PAYMENT_TYPE_ID.Value = "Cash on Delivery";
                ORDER_STATUS.Value = "Order Accepted";
                Payment_Received_Information.Visible = false;
            }
        }

        protected void Payment_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Payment_Type.SelectedValue == "2")
            {
                Cheque_Payment_Details.Visible = true;
            }
            else
            {
                Cheque_Payment_Details.Visible = false;
            }
            PAYMENT_TYPE_ID.Value = Payment_Type.SelectedItem.Text;
            ORDER_STATUS.Value = "Order Accepted";
        }

        protected void Apply_Promotion_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string[] lRecords = CommonClass.FetchRecords("VALIDATE_PROMOTION",
                new string[] { "P_PROMOTION_CODE" },
                new string[] { Server.HtmlEncode(Promotion_Code.Text) },
                new string[] 
                {
                    "P_PROMOTION_ID",
                    "P_MAXIMUM_AMOUNT",
                    "P_PROMOTION_AMOUNT",
                    "P_PERCENTAGE_DISCOUNT",
                    "P_ERROR_STRING"
                },
                dbconn);
            if (lRecords[4] != null && lRecords[4] != "")
            {
                Promotion_Message.ForeColor = Color.Red;
                Promotion_Message.Text = lRecords[4];
                Promotion_Id.Value = "";
                Promotion_Maximum.Value = "";
                Promotion_Amount.Value = "";
                Promotion_Percentage.Value = "";
            }
            else
            {
                Promotion_Id.Value = lRecords[0];
                Promotion_Maximum.Value = lRecords[1];
                Promotion_Amount.Value = lRecords[2];
                Promotion_Percentage.Value = lRecords[3];
                Promotion_Message.ForeColor = Color.Green;
                Promotion_Message.Text = "Successfully Applied";
                if (lRecords[2] != null && lRecords[2] != "" && lRecords[2] != "0")
                {
                    Promotion_Message.Text += ",Discount of ₹ " + lRecords[2];
                }
                else
                {
                    Promotion_Message.Text += "," + lRecords[3] + "%";
                    if (lRecords[1] != null && lRecords[1] != "" && lRecords[1] != "0")
                    {
                        Promotion_Message.Text += " upto Maximum of ₹ " + lRecords[1];
                    }
                }
            }
            dbconn.Close();
            Calculate_Grand_Total();
        }

        protected void AddressBook1_OnAddressSelected(object sender, EventArgs e)
        {
            if (!Request.ServerVariables["URL"].Contains("CustomerView.aspx"))
            {
                SHIPPING_CHARGE_DISCOUNT_FLAG.Value = "N";
                if (AddressBook1.GetDeliveryPinCode == "")
                {
                    PAYMENT_TYPE_ID.Value = WalletPaymentType;
                    ORDER_STATUS.Value = "Open";
                    Online_Payment_PlaceHolder.Visible = false;
                }
                else
                {
                    MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                    dbconn.Open();
                    CommonClass.FetchRecordsAndBind("GET_PAYMENT_OPTIONS",
                        new string[]
                    {
                        "P_PIN_CODE"
                    },
                        new string[]    
                    {
                        AddressBook1.GetDeliveryPinCode
                    },
                        dbconn,
                        Online_Payment_Type);
                    dbconn.Close();


                    if (Online_Payment_Type.Items.Count == 2)
                    {
                        Online_Payment_Type.Items[0].Selected = true;
                        Online_Payment_Type_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        SHIPPING_CHARGE_DISCOUNT_FLAG.Value = "Y";
                    }
                    Online_Payment_PlaceHolder.Visible = true;
                }
                Calculate_Grand_Total();
            }
        }

        protected void Online_Payment_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            PAYMENT_TYPE_ID.Value = Online_Payment_Type.SelectedItem.Text;
            if (PAYMENT_TYPE_ID.Value == WalletPaymentType || PAYMENT_TYPE_ID.Value == OthersPaymentType)
            {
                ORDER_STATUS.Value = "Open";
            }
            else
            {
                ORDER_STATUS.Value = "Order Accepted";
            }
        }

        protected void Delivery_Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Delivery_Method.SelectedValue != "1")
            {
                AddressBook1.Visible = true;
            }
            else
            {
                ORDER_STATUS.Value = "Order Delivered";
                AddressBook1.Visible = false;
            }
            Calculate_Grand_Total();
        }





    }
}