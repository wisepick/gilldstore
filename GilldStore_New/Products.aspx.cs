using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Refresh_Products(dbconn);
                Load_Product_Categories(dbconn);
                dbconn.Close();

            }
        }

        protected void Load_Product_Categories(MySqlConnection dbconn)
        {
            CommonClass.FetchRecordsAndBind("GET_PRODUCT_CATEGORY",
                dbconn,
                Product_Category_Repeater);
        }

        protected void Refresh_Products(MySqlConnection dbconn)
        {
            MySqlCommand lMySqlCommand = new MySqlCommand();
            lMySqlCommand.CommandText = "GET_ACIVE_PRODUCTS";
            lMySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            lMySqlCommand.Connection = dbconn;
            lMySqlCommand.Parameters.AddWithValue("PHOTO_IND", "0");

            using (MySqlDataAdapter lMySqlDataAdapter = new MySqlDataAdapter(lMySqlCommand))
            {
                DataTable dt = new DataTable();
                lMySqlDataAdapter.Fill(dt);
                Product_ListView.DataSource = dt;
                Product_ListView.DataBind();
            }
        }

        protected void Refresh_Products()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Refresh_Products(dbconn);
            dbconn.Close();
        }

        protected void OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (Product_ListView.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, 
                e.MaximumRows, 
                false);
            (Product_ListView.FindControl("DataPager2") as DataPager).SetPageProperties(e.StartRowIndex,
                e.MaximumRows,
                false);
            Refresh_Products();
        }

        protected void Product_Repeater_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                string lProductId = DataBinder.Eval(e.Item.DataItem, "PRODUCT_ID").ToString();
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                CommonClass.FetchRecordsAndBind("GET_PRODUCT_PRICE",
                    new string[] 
                { 
                    "P_PRODUCT_ID", 
                    "P_CUSTOMER_ID", 
                    "P_EXTERNAL_USER_ID" 
                },
                    new string[] 
                { 
                    lProductId, 
                    null, 
                    null 
                },
                    dbconn,
                    e.Item.FindControl("Product_Price_List"));
                dbconn.Close();
            }
        }

        protected void Product_Price_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CommonClass.Populate_DropDownList_WithNumbers(20,
                e.Item.FindControl("Quantity") as DropDownList);     
            
        }

        protected void Add_To_Cart_Button_Click(object sender, EventArgs e)
        {
            LinkButton lLinkButton = (LinkButton)sender;
            Repeater lRepeater = lLinkButton.Parent.FindControl("Product_Price_List") as Repeater;
            for (int lCounter = 0; lCounter < lRepeater.Items.Count; lCounter++)
            {
                DropDownList lQuantity = lRepeater.Items[lCounter].FindControl("Quantity") as DropDownList;
               if (lQuantity.SelectedValue != "0")
               {
                   HiddenField lProduct_Id = lRepeater.Items[lCounter].FindControl("Product_Id") as HiddenField;
                   HiddenField lMeasurement_Unit = lRepeater.Items[lCounter].FindControl("Measurement_Unit") as HiddenField;
                   HiddenField lPrice = lRepeater.Items[lCounter].FindControl("Price") as HiddenField;
                   this.Master.Shopping_Cart_Amount = ShoppingCartClass.Add_To_Shopping_Cart(int.Parse(lProduct_Id.Value),
                       double.Parse(lMeasurement_Unit.Value),
                       int.Parse(lQuantity.SelectedValue),
                       double.Parse(lPrice.Value));
                   
                   
               }
            }
            Refresh_Products();
        }

        protected void Product_Category_Id_Command(object sender, CommandEventArgs e)
        {
            LinkButton lLinkButton = Product_Category_Repeater.Controls[0].Controls[0].FindControl("Product_Category_Id") as LinkButton;
            if (e.CommandArgument.ToString() == "All")
            {
                lLinkButton.CssClass = "btn-primary";
            }
            else
            {
                lLinkButton.CssClass = "";
            }
            foreach (RepeaterItem lRepeaterItem in  Product_Category_Repeater.Items)
            {
                lLinkButton = lRepeaterItem.FindControl("Product_Category_Id") as LinkButton;
                if (lLinkButton.CommandArgument == e.CommandArgument.ToString())
                {
                    lLinkButton.CssClass = "btn-primary";
                }                
                else
                {
                    lLinkButton.CssClass = "";
                }
            }
            if (e.CommandArgument.ToString() == "All")
            {
                Refresh_Products();
            }
            else
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                MySqlCommand lMySqlCommand = new MySqlCommand();
                lMySqlCommand.CommandText = "GET_ACIVE_PRODUCTS_BY_CATEGORY";
                lMySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lMySqlCommand.Connection = dbconn;
                lMySqlCommand.Parameters.AddWithValue("P_PRODUCT_CATEGORY_ID", e.CommandArgument.ToString());
                (Product_ListView.FindControl("DataPager1") as DataPager).SetPageProperties(0,
                              12,
                              false);
                (Product_ListView.FindControl("DataPager2") as DataPager).SetPageProperties(0,
                    12,
                    false);               
                using (MySqlDataAdapter lMySqlDataAdapter = new MySqlDataAdapter(lMySqlCommand))
                {
                    DataTable dt = new DataTable();
                    lMySqlDataAdapter.Fill(dt);
                    Product_ListView.DataSource = dt;
                    Product_ListView.DataBind();
                }
                dbconn.Close();

            }
        }

        protected void Product_Detail_Button_Command(object sender, CommandEventArgs e)
        {

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            ProductDetailControl1.Refresh_Product_Details(dbconn,
                e.CommandArgument.ToString());

            dbconn.Close();
            MultiView1.SetActiveView(Product_Detail_View);
        }

        protected void Cart_Modified(object sender, CommandEventArgs e)
        {
            string[] lCommandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

            this.Master.Shopping_Cart_Amount = ShoppingCartClass.Add_To_Shopping_Cart(int.Parse(lCommandArgs[0]),
                        double.Parse(lCommandArgs[1]),
                        int.Parse(lCommandArgs[2]),
                        double.Parse(lCommandArgs[3]));
        }

        protected void Back_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Products_View);
        }
    }
}