using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                Populate_Products();
            }
        }


        protected void Load_Review_Comments(MySqlConnection dbconn)
        {
            CommonClass.FetchRecordsAndBind("GET_ACTIVE_COMMENTS",
                dbconn,
                Review_Repeater);
        }


        protected void Populate_Products()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.FetchRecordsAndBind("GET_NEW_PRODUCTS",
               dbconn,
               New_Products);

            CommonClass.FetchRecordsAndBind("GET_POPULAR_PRODUCTS",
               dbconn,
               Popular_Products);

            CommonClass.FetchRecordsAndBind("GET_ALL_PHOTOS",                
                dbconn,
                Brand_ListView);
            Load_Review_Comments(dbconn);
            dbconn.Close();
        }

        protected void Product_ListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
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
            Populate_Products();
        }

        
    }
}