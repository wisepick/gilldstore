using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Product_Detail_Control : System.Web.UI.UserControl
    {
        public event CommandEventHandler CartModified;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Refresh_Product_Details(MySqlConnection dbconn,
            string ProductId)
        {
                
                MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_PRODUCT_BY_ID",
                    new string[] { "PRODUCT_ID" },
                    new string[] { ProductId },
                    dbconn);
                
                if (lMySqlDataReader.Read())
                {
                    Product_Name.Text = lMySqlDataReader["PRODUCT_NAME"].ToString();
                    Highlight_Text.Text = Server.HtmlDecode(lMySqlDataReader["HIGHLIGHT_TEXT"].ToString());
                    Summary.Text = Server.HtmlDecode(lMySqlDataReader["SUMMARY"].ToString());
                    Highlight_Text_1.Text = Server.HtmlDecode(lMySqlDataReader["HIGHLIGHT_TEXT"].ToString());
                    Product_Category_Name.Text = lMySqlDataReader["PRODUCT_CATEGORY_NAME"].ToString();
                }
                lMySqlDataReader.Close();

                CommonClass.FetchRecordsAndBind("GET_PRODUCT_PRICE",
                new string[] 
                    { 
                        "P_PRODUCT_ID", 
                        "P_CUSTOMER_ID", 
                        "P_EXTERNAL_USER_ID" 
                    },
                new string[] 
                    { 
                        ProductId, 
                        null, 
                        null 
                    },
                dbconn,
                Product_Price_List);
                Product_Id.Value = ProductId;

                CommonClass.FetchRecordsAndBind("GET_PRODUCT_PHOTOS",
                    new string[]
                    {
                        "P_PRODUCT_ID"
                    },
                    new string[]
                    {
                        Product_Id.Value
                    },
                    dbconn,
                    Product_Gallery_Content_Repeater);
                CommonClass.FetchRecordsAndBind("GET_PRODUCT_PHOTOS",
                    new string[]
                    {
                        "P_PRODUCT_ID"
                    },
                    new string[]
                    {
                        Product_Id.Value
                    },
                    dbconn,
                    Product_Gallery_Thumblist_Repeater);
                Load_Review_Comments(dbconn);
            
        }

        protected void Load_Review_Comments(MySqlConnection dbconn)
        {
            string lUserId = null;
            
            if (ClaimsPrincipal.Current.FindFirst("user_id") != null)
            {
                lUserId = ClaimsPrincipal.Current.FindFirst("user_id").Value;                
            }
            
            CommonClass.FetchRecordsAndBind("GET_REVIEW_COMMENTS",
                new string[]
                {
                    "P_PRODUCT_ID",
                    "P_EXTERNAL_USER_ID"
                },
                new string[]
                {
                    Product_Id.Value,
                    lUserId
                },
                dbconn,
                Review_List);
            Review_List.InsertItem.FindControl("Add_Comment_PlaceHolder").Visible = false;
            Review_List.InsertItem.FindControl("Review_Login_PlaceHolder").Visible = false;
            if (lUserId != null)
            {
                Review_List.InsertItem.FindControl("Add_Comment_PlaceHolder").Visible = true;
            }
            else
            {
                Review_List.InsertItem.FindControl("Review_Login_PlaceHolder").Visible = true;
            }
            
            //Review_Count.Text = Review_List.Items.Count.ToString();
            
        }

        protected void Product_Price_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CommonClass.Populate_DropDownList_WithNumbers(20,
                e.Item.FindControl("Quantity") as DropDownList); 
        }

        protected void Add_To_Cart_Button_Click(object sender, EventArgs e)
        {            
            string lProductId = null;
            for (int lCounter = 0; lCounter < Product_Price_List.Items.Count; lCounter++)
            {
                DropDownList lQuantity = Product_Price_List.Items[lCounter].FindControl("Quantity") as DropDownList;
                if (lQuantity.SelectedValue != "0")
                {
                    HiddenField lProduct_Id = Product_Price_List.Items[lCounter].FindControl("Product_Id") as HiddenField;
                    HiddenField lMeasurement_Unit = Product_Price_List.Items[lCounter].FindControl("Measurement_Unit") as HiddenField;
                    HiddenField lPrice = Product_Price_List.Items[lCounter].FindControl("Price") as HiddenField;
                    lProductId = lProduct_Id.Value;
                    

                    if (CartModified != null)
                    {
                        CommandEventArgs lCommandEventArgs = new CommandEventArgs("Add", lProduct_Id.Value + "," + lMeasurement_Unit.Value + "," + lQuantity.SelectedValue + "," + lPrice.Value);
                        CartModified(this, lCommandEventArgs);
                    }

                }
            }
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Refresh_Product_Details(dbconn,
                lProductId);
            dbconn.Close();
            
        }

        protected void Submit_Button_Command(object sender, CommandEventArgs e)
        {
            Label lCompletionMessage = Review_List.InsertItem.FindControl("Completion_Message") as Label;
            lCompletionMessage.Visible = false;
            Label lRatingMessage = Review_List.InsertItem.FindControl("Rating_Message") as Label;
            lRatingMessage.Text = "";
            if ((Review_List.InsertItem.FindControl("Rating1") as AjaxControlToolkit.Rating).CurrentRating == 0)
            {
                lRatingMessage.Text = "Select the Rating";
                return;
            }

            Guid lGuid = Guid.NewGuid();
            
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.ExecuteQuery("ADD_REVIEW_COMMENTS",
                new string[]
                {
                    "P_PRODUCT_ID",
                    "P_HEAD_LINE",
                    "P_REVIEW_COMMENT",
                    "P_RATING",
                    "P_EXTERNAL_USER_ID",
                    "P_APPROVAL_CODE"
                },
                new string[]
                {
                    Product_Id.Value,
                    Server.HtmlEncode((Review_List.InsertItem.FindControl("Headline") as TextBox).Text),
                    Server.HtmlEncode((Review_List.InsertItem.FindControl("Comment") as TextBox).Text),
                    (Review_List.InsertItem.FindControl("Rating1") as AjaxControlToolkit.Rating).CurrentRating.ToString(),
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    lGuid.ToString()
                },
                dbconn);
            string lMessage = "New Review comments received <br><br><hr>";
            lMessage+= "Heading - " + Server.HtmlEncode((Review_List.InsertItem.FindControl("Headline") as TextBox).Text) + "<br><br>";
            lMessage+= "Comment - " + Server.HtmlEncode((Review_List.InsertItem.FindControl("Comment") as TextBox).Text)+ "<br><br>";
            lMessage+= "Rating - " + (Review_List.InsertItem.FindControl("Rating1") as AjaxControlToolkit.Rating).CurrentRating.ToString()+ "<br><br>";
            lMessage+= "To Approve " + CommonClass.Get_Http_Method() + Request.ServerVariables["HTTP_HOST"].ToString() + "/approve_comments.aspx?approval_id=" + lGuid.ToString() + "<br><br>";
            Messages.send_Email("karpaga.kumar@gmail.com",
                "Karpaga Kumar",
                "New Review comments received",
                lMessage);
            Load_Review_Comments(dbconn);
            dbconn.Close();
            Review_List.Focus();
            lCompletionMessage = Review_List.InsertItem.FindControl("Completion_Message") as Label;
            lCompletionMessage.Visible = true;
        }

        protected void Details_Button_Click(object sender, EventArgs e)
        {
            MultiView2.SetActiveView(Product_Description_View);
            //Details_Button.CssClass = "btn btn-primary";
            //Review_Button.CssClass = "btn";
            Details_Button_Active_PlaceHolder.Visible = true;
            Details_Button_InActive_PlaceHolder.Visible = false;
            Review_Button_Active_PlaceHolder.Visible = false;
            Review_Button_InActive_PlaceHolder.Visible = true;
        }

        protected void Review_Button_Click(object sender, EventArgs e)
        {
            MultiView2.SetActiveView(Review_View);
            //Details_Button.CssClass = "btn";
            //Review_Button.CssClass = "btn btn-primary";
            Details_Button_Active_PlaceHolder.Visible = false;
            Details_Button_InActive_PlaceHolder.Visible = true;
            Review_Button_Active_PlaceHolder.Visible = true;
            Review_Button_InActive_PlaceHolder.Visible = false;
        }    

        protected string check_reviewed(string ReviewStatus)
        {
            if (ReviewStatus == "1")
            {
                return "( Under Review )";
            }
            else
            {
                return "";
            }
        }

        protected string Check_Rank(string lRank,
            string lClass)
        {
            if (lRank == "1")
            {
                return "class=\"" + lClass + "\"";
            }
            else
            {
                return "";
            }
        }
    }
}