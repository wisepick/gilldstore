using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class CustomerControl : System.Web.UI.UserControl
    {
        public event EventHandler SuccessfullyUpdated;

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        public void Get_Customer_Details(MySqlConnection dbconn, string lCustomerId)
        {
            CommonClass.FetchRecordsAndBind("GET_CUSTOMER_BY_ID",
                    new string[] { "P_CUSTOMER_ID" },
                    new string[] { lCustomerId },
                    dbconn,
                    Customer_FormView);
            if (Customer_FormView.DefaultMode == FormViewMode.ReadOnly)
            {
                if (Customer_FormView.DataKey.Values[1].ToString() == "Online User")
                {
                    Customer_FormView.FindControl("Edit_Button").Visible = false;
                }
            }
            if (Customer_FormView.CurrentMode == FormViewMode.Edit)
            {
                ListItem item = (Customer_FormView.FindControl("Discount_Measurement_Id") as RadioButtonList).Items.FindByValue(Customer_FormView.DataKey.Values[2].ToString());
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        protected void Submit_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            if (e.CommandName == "Add_New" || e.CommandName == "Update_Customer")
            {
                if ((Customer_FormView.FindControl("Discount") as TextBox).Text.Trim() != "" && (Customer_FormView.FindControl("Discount_Measurement_Id") as RadioButtonList).SelectedValue == "")
                {
                    (Customer_FormView.FindControl("Message") as Label).Text = "Discount / Discount Type is mandatory";
                }
                else
                {
                    if (e.CommandName == "Add_New")
                    {
                        string[] lRecords = CommonClass.FetchRecords("ADD_CUSTOMER",
                            new string[] 
                {
                    "P_USER_NAME", 
                    "P_EMAIL_ADDRESS", 
                    "P_MOBILE_NUMBER", 
                    "P_EXTERNAL_USER_ID",
                    "P_DISCOUNT",
                    "P_DISOUNT_MEASUREMENT_ID"
                },
                            new string[] 
                {
                    Server.HtmlEncode((Customer_FormView.FindControl("User_Name") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Email_Address") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Mobile_Number") as TextBox).Text),
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    Server.HtmlEncode((Customer_FormView.FindControl("Discount") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Discount_Measurement_Id") as RadioButtonList).SelectedValue)
                },
                            new string[]
                {
                    "P_CUSTOMER_ID",
                    "P_ERROR_STR"
                },
                            dbconn);
                        if (lRecords[1] != null)
                        {
                            (Customer_FormView.FindControl("Message") as Label).Text = lRecords[1];
                        }
                        else
                        {
                            Response.Redirect("CustomerView.aspx?Customer_Id=" + lRecords[0]);
                        }
                    }
                    else if (e.CommandName == "Update_Customer")
                    {
                        CommonClass.ExecuteQuery("UPDATE_CUSTOMER",
                            new string[] 
                {
                    "P_CUSTOMER_ID",
                    "P_USER_NAME", 
                    "P_EMAIL_ADDRESS", 
                    "P_MOBILE_NUMBER", 
                    "P_EXTERNAL_USER_ID",
                    "P_DISCOUNT",
                    "P_DISCOUNT_MEASUREMENT_ID"
                },
                new string[] 
                {
                    Customer_FormView.DataKey.Values[0].ToString(),
                    Server.HtmlEncode((Customer_FormView.FindControl("User_Name") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Email_Address") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Mobile_Number") as TextBox).Text),    
                    ClaimsPrincipal.Current.FindFirst("user_id").Value,
                    Server.HtmlEncode((Customer_FormView.FindControl("Discount") as TextBox).Text),
                    Server.HtmlEncode((Customer_FormView.FindControl("Discount_Measurement_Id") as RadioButtonList).SelectedValue),
                },
                            dbconn);
                        Customer_FormView.ChangeMode(FormViewMode.ReadOnly);
                        Get_Customer_Details(dbconn, Customer_FormView.DataKey.Values[0].ToString());
                        if (SuccessfullyUpdated != null)
                        {
                            SuccessfullyUpdated(this, EventArgs.Empty);
                        }


                    }
                }
            }

            if (e.CommandName == "Cancel_Form")
            {
                Customer_FormView.ChangeMode(FormViewMode.ReadOnly);
                Get_Customer_Details(dbconn, Customer_FormView.DataKey.Values[0].ToString());
            }
        }

        public FormViewMode CustomerViewMode
        {
            get
            {
                return Customer_FormView.CurrentMode;
            }
            set
            {
                Customer_FormView.DefaultMode = value;
            }
        }

        protected void Customer_FormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Customer_FormView.ChangeMode(FormViewMode.Edit);
            Get_Customer_Details(dbconn, Customer_FormView.DataKey.Values[0].ToString());
            dbconn.Close();
        }

        public string Get_Due_Amount
        {
            get
            {
                return (Customer_FormView.FindControl("Due_Amount") as Label).Text;
            }
        }

        protected void Allocate_Payment_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            PaymentClass.Allocate_Order(dbconn, Customer_FormView.DataKey.Values[0].ToString());
            dbconn.Close();
        }

        protected void Add_To_MailChimp_Button_Click(object sender, EventArgs e)
        {
            string lJsonData = "{";                        
            lJsonData += "\"email_address\": \"" + Customer_FormView.DataKey.Values[3].ToString() + "\",";
            lJsonData += "\"status\": \"" + "subscribed" + "\",";
            lJsonData += "\"merge_fields\": {";
            lJsonData += "\"FNAME\": \"" + Customer_FormView.DataKey.Values[4].ToString() + "\"}";
            lJsonData += "}";

            
            RestClient lRestClient = new RestClient("https://us16.api.mailchimp.com/3.0/lists/9c6aecda0d/members/");
            RestRequest lRestRequest = new RestRequest(Method.POST);            
            lRestRequest.AddHeader("Authorization", "apikey" + " " + "451abacce83271f95f721014795de9c7-us16");
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);            
        }
    }
}