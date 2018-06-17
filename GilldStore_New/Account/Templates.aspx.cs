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
    public partial class Templates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                Load_Templates(dbconn);
                dbconn.Close();
            }
        }

        protected void Load_Templates(MySqlConnection dbconn)
        {
            CommonClass.FetchRecordsAndBind("GET_TEMPLATES",                
                dbconn,
                Templates_ListView
                );
        }

        protected void Load_Template_By_Id(MySqlConnection dbconn,
            string TemplateId)
        {
            CommonClass.FetchRecordsAndBind("GET_TEMPLATE_BY_ID",
                new string[]
                {
                    "P_TEMPLATE_ID"
                },
                new string[]
                {
                    TemplateId
                },
                dbconn,
                Detail_Form_View);
        }

        protected void Add_New_Button_Click(object sender, EventArgs e)
        {            
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            MultiView1.SetActiveView(Detail_View);
            Detail_Form_View.ChangeMode(FormViewMode.Insert);
            Load_Template_By_Id(dbconn,
                "");
            dbconn.Close();
        }

        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Templates_View);
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Load_Templates(dbconn);
            dbconn.Close();
        }


        protected void Insert_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            string[] lRecords = CommonClass.FetchRecords("ADD_TEMPLATES",
                new string[] 
                {
                    "P_TEMPLATE_TYPE",
                    "P_SMS_TEMPLATE_MESSAGE",
                    "P_EMAIL_TEMPLATE_MESSAGE",
                    "P_EMAIL_SUBJECT"
                },
                new string[]
                {
                    Server.HtmlEncode((Detail_Form_View.FindControl("Template_Type") as TextBox).Text),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Sms_Template_Message") as TextBox).Text),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Email_Template_Message") as AjaxControlToolkit.HTMLEditor.Editor).Content),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Email_Subject") as TextBox).Text)
                },
                new string[]
                {
                    "P_ERROR_MESSAGE"
                },
                dbconn);
            if (lRecords[0] != null && lRecords[0] != "")
            {
                (Detail_Form_View.FindControl("Message") as Label).Text = lRecords[0];
                return;
            }
            MultiView1.SetActiveView(Templates_View);
            Load_Templates(dbconn);
            dbconn.Close();
        }

        protected void Update_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CommonClass.ExecuteQuery("UPDATE_TEMPLATE",
                new string[] 
                {
                    "P_TEMPLATE_ID",
                    "P_TEMPLATE_TYPE",
                    "P_SMS_TEMPLATE_MESSAGE",
                    "P_EMAIL_TEMPLATE_MESSAGE",
                    "P_EMAIL_SUBJECT"
                },
                new string[]
                {
                    Detail_Form_View.DataKey.Value.ToString(),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Template_Type") as TextBox).Text),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Sms_Template_Message") as TextBox).Text),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Email_Template_Message") as AjaxControlToolkit.HTMLEditor.Editor).Content),
                    Server.HtmlEncode((Detail_Form_View.FindControl("Email_Subject") as TextBox).Text)
                },
                dbconn);            
            MultiView1.SetActiveView(Templates_View);
            Load_Templates(dbconn);
            dbconn.Close();
        }

        protected void Templates_ListView_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            //Templates_ListView.EditIndex = e.NewEditIndex;
            MultiView1.SetActiveView(Detail_View);
            Detail_Form_View.ChangeMode(FormViewMode.Edit);
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Load_Template_By_Id(dbconn,
                Templates_ListView.DataKeys[e.NewEditIndex].Value.ToString());
            dbconn.Close();
        }
    }
}