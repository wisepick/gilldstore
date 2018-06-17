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
    public partial class Policy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["POLICY_ID"] != null)
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                MySqlDataReader reader = CommonClass.FetchRecords("GET_POLICY_DETAILS",
                    new string[] { "P_POLICY_ID" },
                    new string[] { Request.QueryString["POLICY_ID"].ToString() },
                    dbconn);
                if (reader.Read())
                {
                    Policy_Header.Text = reader["POLICY_NAME"].ToString();
                    Policy_Update_Date.Text = "[ Last Updated : " + reader["LAST_UPDATED_DATE"].ToString() + "]";
                }
                reader.Close();

                CommonClass.FetchRecordsAndBind("GET_POLICY_HEADERS",
                    new string[] { "P_POLICY_ID" },
                    new string[] { Request.QueryString["POLICY_ID"].ToString() },
                    dbconn,
                    Policy_Repeater);
                dbconn.Close();
            }
        }
        protected void Policy_Header_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex != -1)
            {
                HiddenField hf = e.Item.FindControl("Header_Id") as HiddenField;
                Repeater rp1 = e.Item.FindControl("Policy_Content_Repeater") as Repeater;
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                CommonClass.FetchRecordsAndBind("GET_POLICY_CONTENT",
                    new string[] { "P_POLICY_ID", "P_POLICY_HEADER_ID" },
                    new string[] { Request.QueryString["POLICY_ID"].ToString(), hf.Value },
                    dbconn,
                    rp1);
                dbconn.Close();
            }
        }
    }
}