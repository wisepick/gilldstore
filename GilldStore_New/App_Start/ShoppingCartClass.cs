using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;

namespace GilldStore_New.App_Start
{
    public class ShoppingCartClass
    {
        public static string Add_To_Shopping_Cart(int pProductId,     
            double pMeasurementUnit,
            int pQuantity,
            double pPrice)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string lReturn = Add_To_Shopping_Cart(dbconn,
                pProductId,                
                pMeasurementUnit,
                pQuantity,
                pPrice);
            dbconn.Close();
            return lReturn;                
        }

        public static void Swap_Shopping_Cart(MySqlConnection dbconn,
            string pUserId)
        {
            if (HttpContext.Current.Session["SHOPPING_CART_ID"] != null)
            {
                string[] lRecords = CommonClass.FetchRecords("SWAP_SHOPPING_CART",
                    new string[]
                {
                    "P_EXTERNAL_USER_ID",
                    "P_SHOPPING_CART_ID"
                },
                    new string[]
                {
                    pUserId,
                    HttpContext.Current.Session["SHOPPING_CART_ID"].ToString()
                },
                    new string[]
                {
                    "P_END_SESSION_FLAG"
                },
                    dbconn);
                if (lRecords[0] != null && lRecords[0] != "")
                {
                    HttpContext.Current.Session.Abandon();
                }
            }
            else
            {
                HttpContext.Current.Session.Abandon();
            }

        }

        public static void Create_Shopping_Cart(MySqlConnection dbconn)
        {
            if (HttpContext.Current.Session["SHOPPING_CART_ID"] == null)
            {
                string lUserId = null;
                if (ClaimsPrincipal.Current.Identity.IsAuthenticated == true)
                {
                    lUserId = ClaimsPrincipal.Current.FindFirst("user_id").Value;
                }

                Random rand = new Random();
                string lSurprise_Discount_Percentage = rand.Next(1, 3).ToString();
                int lNoOfShippingItems = 0;
                double lShippingValue = 0.0;
                string[] lRecords = CommonClass.FetchRecords("CREATE_SHOPPING_CART",
                    new string[]
                    {
                        
                        "P_EXTERNAL_USER_ID",                        
                        "P_SESSION_ID",
                        "P_SURPRISE_DISCOUNT_PERCENTAGE"
                    },
                    new string[]
                    {
                        lUserId,
                        HttpContext.Current.Session.SessionID,                      
                        lSurprise_Discount_Percentage
                    },
                    new string[]
                    {
                        "P_SHIPPING_CART_ID",
                        "P_NO_OF_ITEMS",
                        "P_ITEM_VALUE"
                    },
                    dbconn);
                MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_SHOPPING_CART",
                    new string[]
                    {
                        "P_SHOPPING_CART_ID"
                    },
                    new string[]
                    {
                        lRecords[0]
                    },
                    dbconn);
                if (lMySqlDataReader.Read())
                {
                    HttpContext.Current.Session["SHIPPING_STATE"] = lMySqlDataReader["STATE_ID"].ToString();
                    HttpContext.Current.Session["SURPRISE_DISCOUNT_PERCENTAGE"] = lMySqlDataReader["SURPRISE_DISCOUNT_PERCENTAGE"].ToString();
                    HttpContext.Current.Session["PROMOTION_CODE"] = lMySqlDataReader["PROMOTION_CODE"].ToString();
                    HttpContext.Current.Session["DELIVERY_ADDRESS_ID"] = lMySqlDataReader["DELIVERY_ADDRESS_ID"].ToString();
                }
                else
                {   
                    HttpContext.Current.Session["SURPRISE_DISCOUNT_PERCENTAGE"] = lSurprise_Discount_Percentage;
                }

                if (lRecords[1] != "" && lRecords[1] != null)
                {
                    lNoOfShippingItems = int.Parse(lRecords[1]);
                    lShippingValue = double.Parse(lRecords[2]);
                }
                HttpContext.Current.Session["SHOPPING_ITEMS"] = lNoOfShippingItems;
                HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"] = lShippingValue;
                HttpContext.Current.Session["SHOPPING_CART_ID"] = lRecords[0];
                lMySqlDataReader.Close();
            }
        }
        public static string Add_To_Shopping_Cart(MySqlConnection dbconn,
            int pProductId,
            double pMeasurementUnit,
            int pQuantity,
            double pPrice)
        {            
            Create_Shopping_Cart(dbconn);
            
            
            CommonClass.ExecuteQuery("ADD_TO_SHOPPING_CART",
                new string[]
                {
                    "P_SHOPPING_CART_ID",
                    "P_PRODUCT_ID",                    
                    "P_MEASUREMENT_UNIT",
                    "P_QUANTITY"
                },
                new string[]
                {
                    HttpContext.Current.Session["SHOPPING_CART_ID"].ToString(),
                    pProductId.ToString(),
                    pMeasurementUnit.ToString(),                    
                    pQuantity.ToString()                    
                },
                dbconn);
            string message = pQuantity + " item";
            if (pQuantity > 1)
            {
                message += "s";
            }
            message +=  " added to shopping cart";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");

            HttpContext.Current.Response.Write(sb.ToString());
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            HttpContext.Current.Session["SHOPPING_ITEMS"] = (int.Parse(HttpContext.Current.Session["SHOPPING_ITEMS"].ToString()) + pQuantity).ToString();
            HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"] = (double.Parse(HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"].ToString()) + (pQuantity * pPrice)).ToString();
            return Get_Shopping_Cart_Item_Count();     
        }

        public static string Get_Shopping_Cart_Item_Count()
        {
            if (HttpContext.Current.Session["SHOPPING_CART_ID"] == null)
            {
                return "0 items worth <i class=\"fa fa-rupee\"></i> 0.0";
            }
            else
            {
                return HttpContext.Current.Session["SHOPPING_ITEMS"].ToString() + " items worth <i class=\"fa fa-rupee\"></i>" + HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"].ToString();
            }
        }

        public static string Remove_From_Shopping_Cart(int pProductId,
            double pMeasurementUnit,
            int pQuantity,
            double pAmount)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string lReturn = Remove_From_Shopping_Cart(dbconn,
                pProductId,
                pMeasurementUnit,
                pQuantity,
                pAmount);
            dbconn.Close();
            return lReturn;
        }

        public static string Remove_From_Shopping_Cart(MySqlConnection dbconn,
            int pProductId,
            double pMeasurementUnit,
            int pQuantity,
            double pAmount)
        {
            if (HttpContext.Current.Session["SHOPPING_CART_ID"] != null)
            {
                CommonClass.ExecuteQuery("REMOVE_FROM_SHOPPING_CART",
                   new string[]
                {
                    "P_SHOPPING_CART_ID",
                    "P_PRODUCT_ID",
                    "P_MEASUREMENT_UNIT"
                },
                   new string[]
                {
                    HttpContext.Current.Session["SHOPPING_CART_ID"].ToString(),
                    pProductId.ToString(),
                    pMeasurementUnit.ToString()
                },
                   dbconn);

               
                HttpContext.Current.Session["SHOPPING_ITEMS"] = (int.Parse(HttpContext.Current.Session["SHOPPING_ITEMS"].ToString()) - pQuantity).ToString();
                HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"] = (double.Parse(HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"].ToString()) - (pAmount )).ToString();
                


            }
            return Get_Shopping_Cart_Item_Count();  
        }

        public static string Update_Shopping_Cart(MySqlConnection dbconn,
            int pProductId,
            double pMeasurementUnit,
            int pOldQuantity,
            int pQuantity,
            double pPrice)
        {
            if (HttpContext.Current.Session["SHOPPING_CART_ID"] != null)
            {
          

                CommonClass.ExecuteQuery("UPDATE_SHOPPING_CART",
                   new string[]
                {
                    "P_SHOPPING_CART_ID",
                    "P_PRODUCT_ID",                    
                    "P_MEASUREMENT_UNIT",
                    "P_QUANTITY"
                },
                   new string[]
                {
                    HttpContext.Current.Session["SHOPPING_CART_ID"].ToString(),
                    pProductId.ToString(),
                    pMeasurementUnit.ToString(),
                    pQuantity.ToString()
                },
                   dbconn);
                HttpContext.Current.Session["SHOPPING_ITEMS"] = (int.Parse(HttpContext.Current.Session["SHOPPING_ITEMS"].ToString()) - pOldQuantity + pQuantity).ToString();
                HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"] = (double.Parse(HttpContext.Current.Session["SHOPPING_ITEMS_VALUE"].ToString()) - (pOldQuantity * pPrice) + (pQuantity * pPrice)).ToString();

            }
            return Get_Shopping_Cart_Item_Count();
        }
    }
}