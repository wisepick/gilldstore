using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Invoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["oid"] != null)
            {
                string lOrderid = Request.QueryString["oid"].ToString();
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_ORDER_BY_ID",
                    new string[]
                    {
                        "P_ORDER_ID",
                        "P_EXTERNAL_USER_ID"
                    },
                    new string[]
                    {
                        lOrderid,
                        ClaimsPrincipal.Current.FindFirst("user_id").Value
                    },
                    dbconn);
                string lAddressId = null;
                if (lMySqlDataReader.Read())
                {
                    Invoice_No.Text = lMySqlDataReader["ORDER_NUMBER"].ToString();
                    Invoice_Date.Text = lMySqlDataReader["ORDER_DATE"].ToString();
                    User_Name.Text = lMySqlDataReader["USER_NAME"].ToString();
                    if (lMySqlDataReader["ADDRESS_ID"] != null)
                    {
                        lAddressId = lMySqlDataReader["ADDRESS_ID"].ToString();
                    }
                    Subtotal.Text = (double.Parse(lMySqlDataReader["ORDER_TOTAL"].ToString()) + double.Parse(lMySqlDataReader["DISCOUNTS"].ToString())).ToString();
                    Shipping_charges.Text = lMySqlDataReader["SHIPPING_CHARGE"].ToString();
                    Discounts.Text = lMySqlDataReader["DISCOUNTS"].ToString();
                    VAT.Text = (double.Parse(Subtotal.Text) * 5 / 100).ToString();
                    Grand_Total.Text = (double.Parse(lMySqlDataReader["ORDER_TOTAL"].ToString()) + double.Parse(VAT.Text)).ToString();
                }

                lMySqlDataReader.Close();

                if (lAddressId != null)
                {
                    lMySqlDataReader = CommonClass.FetchRecords("GET_ADDRESS_BY_ID",
                            new string[]
                        {
                            "P_ADDRESS_ID"
                        },
                            new string[]
                        {
                            lAddressId
                        },
                            dbconn);
                    if (lMySqlDataReader.Read())
                    {
                        User_Name.Text += ", " + lMySqlDataReader["SHIPPING_ADDRESS"].ToString();
                        User_Name.Text += ", " + lMySqlDataReader["CITY_NAME"].ToString();
                        User_Name.Text += ", " + lMySqlDataReader["STATE_NAME"].ToString();
                        User_Name.Text += " - " + lMySqlDataReader["PIN_CODE"].ToString();
                    }
                    lMySqlDataReader.Close();
                }

                CommonClass.FetchRecordsAndBind("GET_ORDER_DETAILS_BY_ID",
                     new string[]
                    {
                        "P_ORDER_ID",
                        "P_EXTERNAL_USER_ID"
                    },
                    new string[]
                    {
                        lOrderid,
                        ClaimsPrincipal.Current.FindFirst("user_id").Value
                    },
                    dbconn,
                    Repeater1);
                dbconn.Close();
            }
        }
    }
}