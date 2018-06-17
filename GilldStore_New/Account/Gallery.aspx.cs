using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class Gallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Load_Galleries(dbconn);
                Load_Products(dbconn);
                dbconn.Close();
            }
        }

        protected void Load_Galleries(MySqlConnection dbconn)
        {
            CommonClass.FetchRecordsAndBind("GET_ALL_PHOTOS",
                dbconn,
                Gallery_ListView);
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
                Product_Id);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
            {
                string fileName = Session.SessionID + "_" + Path.GetFileName(postedFile.FileName);

                postedFile.SaveAs(Server.MapPath("~/img/") + fileName);
                string[] lValues = CommonClass.FetchRecords("ADD_ATTRIBUTES_BY_MASTER_NAME",
                    new string[] 
                    { 
                        "P_ATTRIBUTE_NAME", 
                        "P_MASTER_TYPE_NAME"
                    },
                    new string[] 
                    { 
                        fileName, 
                        "FILES"
                    },
                    new string[] { "P_ATTRIBUTE_ID" },
                    dbconn);
                CommonClass.ExecuteQuery("ADD_PRODUCT_PHOTOS",
                    new string[] 
                    { 
                        "P_PRODUCT_ID",
                        "P_FILE_ID" 
                    },
                    new string[] 
                    { 
                        Product_Id.SelectedValue,
                        lValues[0] 
                    },
                    dbconn);

            }
            Load_Galleries(dbconn);
            dbconn.Close();
        }

        protected void Delete_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.ExecuteQuery("REMOVE_FILE_FROM_GALLERY",
                new string[] { "P_FILE_ID" },
                new string[] { e.CommandArgument.ToString() },
                dbconn);
            Load_Galleries(dbconn);
            dbconn.Close();
        }
    }
}