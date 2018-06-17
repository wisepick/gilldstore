using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class View_Cart : System.Web.UI.Page
    {
       Dictionary<string, double> gQuantity = new Dictionary<string, double>();
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                ShoppingCartClass.Create_Shopping_Cart(dbconn);
                //Load_States(dbconn);
                Load_Shopping_Cart(dbconn, 
                    Cart_Details_Repeater);
                AddressBook1.Load_Address(dbconn, true);
                dbconn.Close();
                Change_View("Order_Info");
            }
        }

        
        protected void Load_Shopping_Cart(MySqlConnection dbconn, 
            Repeater lRepeater)
        {
            ShoppingCartClass.Create_Shopping_Cart(dbconn);
            gQuantity.Clear();
            string lUserId = null;
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == true)
            {
                lUserId = ClaimsPrincipal.Current.FindFirst("user_id").Value;
            }
                
            CommonClass.FetchRecordsAndBind("GET_SHOPPING_CART_DETAILS",
                new string[] 
            {
                "P_SHOPPING_CART_ID",
                "P_EXTERNAL_USER_ID"
            },
                new string[]
            {
                Session["SHOPPING_CART_ID"].ToString(),
                lUserId
            },
                dbconn,
                lRepeater);
            double lTotal = 0.0;
            double lTotalShippingCharges = 0.0;                
            double lShippingDiscounts = 0.0;
            double lPromotionalDiscounts = 0.0;
            foreach(RepeaterItem lRepeaterItem in lRepeater.Items)
            {
               double lQuantity = double.Parse((lRepeaterItem.FindControl("OLD_QUANTITY") as HiddenField).Value.ToString());
               double lOrderAmount = double.Parse((lRepeaterItem.FindControl("PRICE") as HiddenField).Value.ToString());               
               double lShippingCharges = double.Parse(Shipping_Charge.Value);
               double lSize = double.Parse((lRepeaterItem.FindControl("MEASUREMENT_UNIT") as HiddenField).Value.ToString());
               lTotal +=  lOrderAmount * lQuantity;
               lTotalShippingCharges += lShippingCharges * lQuantity;                   
               string lMeasurementId = (lRepeaterItem.FindControl("MEASUREMENT_ID") as HiddenField).Value;
               if (gQuantity.ContainsKey(lMeasurementId))
               {
                    gQuantity[lMeasurementId] = gQuantity[lMeasurementId] + (lQuantity * lSize);
               }
               else
               {
                   gQuantity[lMeasurementId] = lQuantity * lSize;
               }
           }

            ViewState["COUNTER"] = gQuantity;
            Shipping_Charges.Text = lTotalShippingCharges.ToString();
            Shipping_Discounts.Text = lShippingDiscounts.ToString();
            Promotional_Discounts.Text = lPromotionalDiscounts.ToString();                
            Total_Amount.Text = lTotal.ToString();
            if (Session["PROMOTION_CODE"] != null)
            {
                Coupon_Code.Text = Session["PROMOTION_CODE"].ToString();
            }
            Apply_Coupon_Button_Click(new object { }, EventArgs.Empty);
                            
        }

       

      

        protected void Close_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string[] lArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

           
            this.Master.Shopping_Cart_Amount = ShoppingCartClass.Remove_From_Shopping_Cart(int.Parse(lArgs[0]),
                double.Parse(lArgs[1]),
                int.Parse(lArgs[2]),
                double.Parse(lArgs[3]));
            Response.Redirect("~/View_Cart.aspx");
            dbconn.Close();
        }

        protected void Update_Cart_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            foreach (RepeaterItem lRepeaterItem in Cart_Details_Repeater.Items)
            {
                ShoppingCartClass.Update_Shopping_Cart(dbconn,
                    int.Parse((lRepeaterItem.FindControl("PRODUCT_ID") as HiddenField).Value),
                    double.Parse((lRepeaterItem.FindControl("MEASUREMENT_UNIT") as HiddenField).Value),
                    int.Parse((lRepeaterItem.FindControl("OLD_QUANTITY") as HiddenField).Value),
                    int.Parse((lRepeaterItem.FindControl("Quantity") as TextBox).Text),
                    double.Parse((lRepeaterItem.FindControl("PRICE") as HiddenField).Value));
            }
            
            dbconn.Close();
            Response.Redirect("~/View_Cart.aspx");
        }

       

       

       

        protected void CheckOut_Button_Click(object sender, EventArgs e)
        {
            //Check_Out_Message.Visible = false;
            Checkout_Message_PlaceHolder.Visible = false;
            if(Cart_Details_Repeater.Items.Count != 0)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
                {
                    dbconn.Close();
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {   
                    UserModel lUserModel;
                    lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                    bool lRedirect = false;
                    if (lUserModel.Email_Address_Validated == 0 || lUserModel.Mobile_Validated == 0)
                    {
                        lRedirect = true;
                    }                    
                    if (lRedirect)
                    {
                        dbconn.Close();
                        Response.Redirect("~/Account/Validate.aspx");
                    }
                }                
                foreach (RepeaterItem lRepeaterItem in Cart_Details_Repeater.Items)
                {
                    ShoppingCartClass.Update_Shopping_Cart(dbconn,
                        int.Parse((lRepeaterItem.FindControl("PRODUCT_ID") as HiddenField).Value),
                        double.Parse((lRepeaterItem.FindControl("MEASUREMENT_UNIT") as HiddenField).Value),
                        int.Parse((lRepeaterItem.FindControl("OLD_QUANTITY") as HiddenField).Value),
                        int.Parse((lRepeaterItem.FindControl("Quantity") as TextBox).Text),
                        double.Parse((lRepeaterItem.FindControl("PRICE") as HiddenField).Value));
                }


                AddressBook1.Load_Address(dbconn, true);
                dbconn.Close();
                Change_View("Personal_Info");
                MultiView1.SetActiveView(AddressBook_View);

            }
            else
            {
                Checkout_Message_PlaceHolder.Visible = true;
                //Check_Out_Message.Visible = true;
            }
        }

        protected void Next_Address_Book_Button_Click(object sender, EventArgs e)
        {
            AddressBook_Message.Visible = false;
            AddressBook_Message1.Visible = false;
            if (AddressBook1.GetDeliveryAddressId != "")
            {                
                Change_View("Review_Order_Info");
                MultiView1.SetActiveView(Review_Order_View);
                Session["DELIVERY_ADDRESS_ID"] = AddressBook1.GetDeliveryAddressId.ToString();
                Session["SHIPPING_STATE"] = AddressBook1.GetDeliveryState.ToString();
                Session["PIN_CODE"] = AddressBook1.GetDeliveryPinCode.ToString();
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                ShoppingCartClass.Create_Shopping_Cart(dbconn);
                CommonClass.ExecuteQuery("UPDATE_SHOPPING_DELIVERY_ADDRESS",
                    new string[]
                    {
                        "P_SHOPPING_CART_ID",
                        "P_DELIVERY_ADDRESS_ID",
                        "P_PIN_CODE"
                    },
                    new string[]
                    {
                        Session["SHOPPING_CART_ID"].ToString(),
                        AddressBook1.GetDeliveryAddressId.ToString(),
                        AddressBook1.GetDeliveryPinCode.ToString()
                    },
                    dbconn);
                Get_Shipping_Charge(dbconn);
                Load_Shopping_Cart(dbconn, 
                    Review_Order_Repeater);
                
                dbconn.Close();
                
            }
            else
            {
                AddressBook_Message.Visible = true;
                AddressBook_Message1.Visible = true;
                Session["SHIPPING_STATE"] = null;
                Session["DELIVERY_ADDRESS_ID"] = null;
                Session["PIN_CODE"] = null;
            }
        }

        protected void Next_Review_Order_Button_Click(object sender, EventArgs e)
        {
            Change_View("Payment_Info");

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            if(Is_COD(dbconn) == true)
            {
                MultiView1.SetActiveView(Payment_View);
            }
            else
            {
                Place_Online_Order(dbconn,
                    "Others",
                    "Open");
                //Place Order
            }
            
        }

        protected void Place_Online_Order(MySqlConnection dbconn,
            string PaymentType,
            string PaymentStatus)
        {
            OrderClass.Create_Order(dbconn,
                    "",
                    Grand_Total.Text,
                    Shipping_Charges.Text,
                    (double.Parse(Shipping_Discounts.Text) + double.Parse(Promotional_Discounts.Text)).ToString(),
                    Session["DELIVERY_ADDRESS_ID"].ToString(),
                    Coupon_Code.Text,
                    PaymentType,
                    PaymentStatus,
                    "3");
        }

        protected bool Is_COD(MySqlConnection dbconn)
        {
            if (Session["PIN_CODE"] != null)
            {
                string[] lRecords = CommonClass.FetchRecords("IS_COD",
                    new string[]
                {
                    "P_PIN_CODE"                    
                },
                    new string[]
                {
                    Session["PIN_CODE"].ToString()
                },
                    new string[]
                {
                    "P_ELIGIBLE"
                },
                    dbconn);
                if (lRecords[0] != null && lRecords[0] != "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
            //dbconn.Close();
        }

        protected void Apply_Coupon_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            double lPromotionDiscount = 0.0;
            Promotion_Message.Text = "";
            if (Coupon_Code.Text != "")
            {
                string[] lPromotionDetails = CommonClass.FetchRecords("VALIDATE_PROMOTION",
                    new string[] 
	                {
	                    "P_PROMOTION_CODE"
	                },
                    new string[]
	                {
	                    Server.HtmlEncode(Coupon_Code.Text)
	                },
                    new string[]
	                {
	                    "P_PROMOTION_ID",
	                    "P_MAXIMUM_AMOUNT",
	                    "P_PROMOTION_AMOUNT",
	                    "P_PERCENTAGE_DISCOUNT",
                        "P_SURPRISE_DISCOUNT_ELIGIBLE",
	                    "P_ERROR_STRING"
	                },
                    dbconn);

                if (lPromotionDetails[5] != null && lPromotionDetails[5] != "")
                {
                    Promotion_Message.ForeColor = Color.Red;
                    Promotion_Message.Text = lPromotionDetails[5];
                }
                else
                {
                    Surprise_Discount_Eligible.Value = lPromotionDetails[4];
                    
                    CommonClass.ExecuteQuery("UPDATE_SHOPPING_PROMO",
                        new string[]
	                    {
	                        "P_SHOPPING_CART_ID",
	                        "P_PROMOTION_CODE"
	                    },
                        new string[]
	                    {
	                        Session["SHOPPING_CART_ID"].ToString(),
	                        Server.HtmlEncode(Coupon_Code.Text)
	                    },
                        dbconn);
                    Session["PROMOTION_CODE"] = Server.HtmlEncode(Coupon_Code.Text);
                    Promotion_Message.ForeColor = Color.Green;
                    Promotion_Message.Text = "Successfully Applied";

                    if (lPromotionDetails[2] != null && lPromotionDetails[2] != "" && lPromotionDetails[2] != "0")
                    {

                        lPromotionDiscount = double.Parse(lPromotionDetails[2]);
                        if (double.Parse(Total_Amount.Text) < lPromotionDiscount)
                        {
                            lPromotionDiscount = double.Parse(Total_Amount.Text);
                        }
                        Promotion_Message.Text += ",Discount of ? " + lPromotionDiscount;
                    }
                    else
                    {
                        Promotion_Message.Text += "," + lPromotionDetails[3] + "%";
                        lPromotionDiscount = (double.Parse(Total_Amount.Text) * double.Parse(lPromotionDetails[3])) / 100;
                        if (lPromotionDetails[1] != null && lPromotionDetails[1] != "" && lPromotionDetails[1] != "0")
                        {
                            Promotion_Message.Text += " upto Maximum of ? " + lPromotionDetails[1];
                            if (lPromotionDiscount >= double.Parse(lPromotionDetails[1]))
                            {
                                lPromotionDiscount = double.Parse(lPromotionDetails[1]);
                            }
                        }
                    }
                }
            }
            else
            {
                Surprise_Discount_Eligible.Value = "1";
                CommonClass.ExecuteQuery("UPDATE_SHOPPING_PROMO",
                        new string[]
	                    {
	                        "P_SHOPPING_CART_ID",
	                        "P_PROMOTION_CODE"
	                    },
                        new string[]
	                    {
	                        Session["SHOPPING_CART_ID"].ToString(),
	                        null
	                    },
                        dbconn);
                Session["PROMOTION_CODE"] = null;
            }

            Promotional_Discounts.Text = lPromotionDiscount.ToString();
            Apply_Promotions(dbconn);


            dbconn.Close();
        }

        protected void Get_Shipping_Charge(MySqlConnection dbconn)
        {
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_SHIPPING_CHARGE",
                new string[]
		                {
		                    "P_STATE_ID"
		                },
                new string[]
		                {
		                    AddressBook1.GetDeliveryState.ToString()
		                },
                dbconn);
            if (lMySqlDataReader.Read())
            {
                Shipping_Charge.Value = lMySqlDataReader["SHIPPING_CHARGE"].ToString();
            }
            lMySqlDataReader.Close();
        }

        protected void Apply_Promotions(MySqlConnection dbconn)
        {
            ShoppingCartClass.Create_Shopping_Cart(dbconn);
            if (Is_COD(dbconn))
            {
                Shipping_Discounts.Text = Shipping_Charges.Text;
            }
            else
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
                    gQuantity = ViewState["COUNTER"] as Dictionary<string, double>;
                    if (gQuantity.ContainsKey("4"))
                    {
                        if (gQuantity["4"] >= double.Parse(lRecords[1]))
                        {
                            Shipping_Discounts.Text = Shipping_Charges.Text;
                        }
                    }
                }
            }
            Surprise_Discount_PlaceHolder.Visible = false;

            if (Session["SURPRISE_DISCOUNT_PERCENTAGE"] != null && Surprise_Discount_Eligible.Value == "1")
            {
                double lSurpriseDiscount = Math.Round(((double.Parse(Total_Amount.Text) * double.Parse(Session["SURPRISE_DISCOUNT_PERCENTAGE"].ToString())) / 100));
                Promotional_Discounts.Text = (double.Parse(Promotional_Discounts.Text) +
                                              lSurpriseDiscount).ToString();
                Surprise_Discount.Text = lSurpriseDiscount.ToString();
                if (lSurpriseDiscount != 0)
                {
                    Surprise_Discount_PlaceHolder.Visible = true;
                }
            }

            Compute_Grand_Total();
        }

        protected void Compute_Grand_Total()
        {
            Grand_Total.Text = (double.Parse(Total_Amount.Text) + double.Parse(Shipping_Charges.Text) - double.Parse(Shipping_Discounts.Text) - double.Parse(Promotional_Discounts.Text)).ToString();
        }

        protected void Previous_Address_Book_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(OrderView);
            Change_View("Order_Info");
        }

        protected void Previous_Review_Order_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(AddressBook_View);
            Change_View("Personal_Info");
        }
        
        protected void Change_View(string ViewName)
        {
            if (ViewName == "Order_Info")
            {
                Order_Info_Active_PlaceHolder.Visible = true;
                Order_Info_Complete_PlaceHolder.Visible = false;
                Personal_Info_Active_PlaceHolder.Visible = false;                
                Personal_Info_Complete_PlaceHolder.Visible = false;
                Personal_Info_InComplete_PlaceHolder.Visible = true;
                Review_Order_Info_Active_PlaceHolder.Visible = false;
                Review_Order_Info_Complete_PlaceHolder.Visible = false;
                Review_Order_Info_InComplete_PlaceHolder.Visible = true;
                Payment_Info_Active_PlaceHolder.Visible = false;                
                Payment_Info_Complete_PlaceHolder.Visible = false;
                Payment_Info_InComplete_PlaceHolder.Visible = true;
            }
            else if (ViewName == "Personal_Info")
            {
                Order_Info_Active_PlaceHolder.Visible = false;
                Order_Info_Complete_PlaceHolder.Visible = true;
                Personal_Info_Active_PlaceHolder.Visible = true;
                Personal_Info_Complete_PlaceHolder.Visible = false;
                Personal_Info_InComplete_PlaceHolder.Visible = false;
                Review_Order_Info_Active_PlaceHolder.Visible = false;
                Review_Order_Info_Complete_PlaceHolder.Visible = false;
                Review_Order_Info_InComplete_PlaceHolder.Visible = true;
                Payment_Info_Active_PlaceHolder.Visible = false;
                Payment_Info_Complete_PlaceHolder.Visible = false;
                Payment_Info_InComplete_PlaceHolder.Visible = true;
            }
            else if (ViewName == "Review_Order_Info")
            {
                Order_Info_Active_PlaceHolder.Visible = false;
                Order_Info_Complete_PlaceHolder.Visible = true;
                Personal_Info_Active_PlaceHolder.Visible = false;
                Personal_Info_Complete_PlaceHolder.Visible = true;
                Personal_Info_InComplete_PlaceHolder.Visible = false;
                Review_Order_Info_Active_PlaceHolder.Visible = true;
                Review_Order_Info_Complete_PlaceHolder.Visible = false;
                Review_Order_Info_InComplete_PlaceHolder.Visible = false;
                Payment_Info_Active_PlaceHolder.Visible = false;
                Payment_Info_Complete_PlaceHolder.Visible = false;
                Payment_Info_InComplete_PlaceHolder.Visible = true;
            }
            else if (ViewName == "Payment_Info")
            {
                Order_Info_Active_PlaceHolder.Visible = false;
                Order_Info_Complete_PlaceHolder.Visible = true;
                Personal_Info_Active_PlaceHolder.Visible = false;
                Personal_Info_Complete_PlaceHolder.Visible = true;
                Personal_Info_InComplete_PlaceHolder.Visible = false;
                Review_Order_Info_Active_PlaceHolder.Visible = false;
                Review_Order_Info_Complete_PlaceHolder.Visible = true;
                Review_Order_Info_InComplete_PlaceHolder.Visible = false;
                Payment_Info_Active_PlaceHolder.Visible = true;
                Payment_Info_Complete_PlaceHolder.Visible = false;
                Payment_Info_InComplete_PlaceHolder.Visible = false;
            }
            
            
            
        }

        protected void Previous_Payment_View_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Review_Order_View);
            Change_View("Review_Order_Info");
        }

        protected void Next_Payment_View_Button_Click(object sender, EventArgs e)
        {
            //Place Order
        }

        protected void Other_Payment_Types_Option_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Place_Online_Order(dbconn, 
                "Others",
                "Open");
            dbconn.Close();
        }

        protected void Cash_On_Delivery_Option_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Place_Online_Order(dbconn,
                "Cash on Delivery",
                "Order Accepted");            
            dbconn.Close();
        }
        
    }
}