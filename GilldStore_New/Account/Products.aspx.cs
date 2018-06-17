using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Load_Products(dbconn);
                dbconn.Close();
            }
        }

        protected void Load_Products(MySqlConnection dbconn)
        {

            CommonClass.FetchRecordsAndBind("GET_ACIVE_PRODUCTS",
                new string[] 
                {
                    "PHOTO_IND"
                },
                new string[]
                {
                    "0"
                },
                dbconn,
                Product_View);
        }

        protected void Load_Products()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Load_Products(dbconn);
            dbconn.Close();
        }

        protected void Add_Button_Click(object sender, EventArgs e)
        {
            //Product_View.EditIndex = -1;
            //Product_View.InsertItemPosition = InsertItemPosition.FirstItem;
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            //Load_Products(dbconn);
            Product_Form_View.DefaultMode = FormViewMode.Insert;
            CommonClass.FetchRecordsAndBind("GET_ACIVE_PRODUCTS",
                new string[] 
                {
                    "PHOTO_IND"
                },
                new string[]
                {
                    "1"
                },
                dbconn,
                Product_Form_View);
            CommonClass.FetchRecordsAndBind("GET_PRODUCT_CATEGORY",                
                dbconn,
                Product_Form_View.FindControl("PRODUCT_CATEGORY_ID"));
            CommonClass.Load_Attributes(dbconn,
                "MEASUREMENT",
                Product_Form_View.FindControl("Measurement_Id"));            
            (Product_Form_View.FindControl("Product_Name") as TextBox).Focus();
            dbconn.Close();
            MultiView1.SetActiveView(Product_Edit_View);
        }

        protected void Edit_Image_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Product_Form_View.DefaultMode = FormViewMode.Edit;
            CommonClass.FetchRecordsAndBind("GET_PRODUCT_BY_ID",
                new string[]
                {
                    "P_PRODUCT_ID"
                },
                new string[]
                {
                    e.CommandArgument.ToString()
                },
                dbconn,
                Product_Form_View);
            DropDownList lDropDownList = Product_Form_View.FindControl("Product_Category_Id") as DropDownList;
            CommonClass.FetchRecordsAndBind("GET_PRODUCT_CATEGORY",                
                dbconn,
                lDropDownList);
            lDropDownList.SelectedValue = Product_Form_View.DataKey.Values[1].ToString();
            lDropDownList = Product_Form_View.FindControl("Measurement_Id") as DropDownList;
            CommonClass.Load_Attributes(dbconn,
                "MEASUREMENT",
                Product_Form_View.FindControl("Measurement_Id"));
            lDropDownList.SelectedValue = Product_Form_View.DataKey.Values[2].ToString();
            (Product_Form_View.FindControl("Product_Name") as TextBox).Focus();
            
            dbconn.Close();
            MultiView1.SetActiveView(Product_Edit_View);
        }

        protected void Add_Save_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string lPhotoFileName = null;
            FileUpload lFileUpload1 = Product_Form_View.FindControl("FileUpload1") as FileUpload;
            if (lFileUpload1.HasFile)
            {
                Random lRandom = new Random();
                lPhotoFileName = lRandom.Next().ToString() + lFileUpload1.FileName;
                lFileUpload1.SaveAs(Server.MapPath("~/img/" + lPhotoFileName));
            }
            else if (Product_Form_View.CurrentMode == FormViewMode.Edit)
            {
                Image lImage1 = Product_Form_View.FindControl("Product_Image") as Image;

                if (lImage1.ImageUrl != "" && lImage1.ImageUrl != null)
                {
                    string[] lImageUrgl = lImage1.ImageUrl.Split(new char[] { '/' });
                    lPhotoFileName = lImageUrgl[lImageUrgl.Length - 1];
                }
            }
            
            string lProduct_Name = Server.HtmlEncode((Product_Form_View.FindControl("Product_Name") as TextBox).Text);
            string lProductId = null;
            if (Product_Form_View.CurrentMode == FormViewMode.Insert)
            {
                string[] lValues = CommonClass.ExecuteQuery("ADD_PRODUCT",
                    new string[]
                    {
                        "P_PRODUCT_NAME",
                        "P_PRODUCT_CATEGORY_ID",
                        "P_PHOTO_FILE_NAME",
                        "P_MEASUREMENT_ID",
                        "P_SUMMARY",
                        "P_HIGHLIGHT_TEXT"
                    },
                    new string[]
                    {
                        lProduct_Name,
                        (Product_Form_View.FindControl("PRODUCT_CATEGORY_ID") as DropDownList).SelectedValue.ToString(),
                        lPhotoFileName,
                        (Product_Form_View.FindControl("Measurement_Id") as DropDownList).SelectedValue.ToString(),
                        Server.HtmlEncode((Product_Form_View.FindControl("SUMMARY") as Obout.Ajax.UI.HTMLEditor.Editor).Content),
                        Server.HtmlEncode((Product_Form_View.FindControl("HIGHLIGHT_TEXT") as TextBox).Text)
                    },
                    new string[]
                    {
                        "P_PRODUCT_ID"
                    },
                    dbconn);
                lProductId = lValues[0];
            }
            else
            {
                lProductId = Product_Form_View.DataKey.Values[0].ToString();
                CommonClass.ExecuteQuery("UPDATE_PRODUCT",
                    new string[]
                    {
                        "P_PRODUCT_ID",
                        "P_PRODUCT_CATEGORY_ID",
                        "P_PRODUCT_NAME",                        
                        "P_MEASUREMENT_ID",
                        "P_HIGHLIGHT_TEXT",
                        "P_SUMMARY",
                        "P_PHOTO_FILE_NAME"
                    },
                    new string[]
                    {
                        lProductId,
                        (Product_Form_View.FindControl("Product_Category_Id") as DropDownList).SelectedValue.ToString(),
                        Server.HtmlEncode((Product_Form_View.FindControl("Product_Name") as TextBox).Text),
                        (Product_Form_View.FindControl("Measurement_Id") as DropDownList).SelectedValue.ToString(),                        
                        Server.HtmlEncode((Product_Form_View.FindControl("HIGHLIGHT_TEXT") as TextBox).Text),
                        Server.HtmlEncode((Product_Form_View.FindControl("SUMMARY") as AjaxControlToolkit.HTMLEditor.Editor).Content),
                        lPhotoFileName
                    },
                    dbconn);
            }

                
            Refresh_Product_View(dbconn);
            dbconn.Close();
            MultiView1.SetActiveView(Product_All_View);
        }

        protected void Refresh_Product_View(MySqlConnection dbconn)
        {
            Product_View.EditIndex = -1;
            Product_View.InsertItemPosition = InsertItemPosition.None;
            Load_Products(dbconn);
        }

        protected void Refresh_Product_View()
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Refresh_Product_View(dbconn);
            dbconn.Close();
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Product_All_View);
            Refresh_Product_View();
        }

        protected void Remove_Image_Button_Click(object sender, EventArgs e)
        {
            Button lButton1 = (Button)sender;
            Image lImage = lButton1.Parent.FindControl("Product_Image") as Image;
            lImage.ImageUrl = "";
            lImage.Visible = false;
            lButton1.Visible = false;
        }
    }
}