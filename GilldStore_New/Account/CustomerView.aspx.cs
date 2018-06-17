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
    public partial class CustomerView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    Search_Customer_Form.Visible = false;
                    CustomerGrid1.Load_Customers("",
                    "",
                    "");
                }
            }
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
            if (!IsPostBack)
            {
                if (Request.QueryString["Customer_Id"] != null && Request.QueryString["ref"] != null)
                {
                    CustomerId.Value = Request.QueryString["Customer_Id"].ToString();
                    CommandEventArgs lCommandEventArgs = new CommandEventArgs("Select", Request.QueryString["Customer_Id"].ToString());
                    //lCommandEventArgs.CommandArgument = ;
                    Back_Button.Visible = false;
                    Due_Back_Button.Visible = true;
                    CustomerGrid1_OnCustomerSelect(sender, lCommandEventArgs);
                    Due_Orders_Click(sender, e);
                }
            }
        }

        protected void Customer_View_OnSuccessfullyUpdated(object sender, EventArgs e)
        {
            if (Request.QueryString["Customer_Id"] != null)
            {
                OrderForm1.Set_Customer_Id = Request.QueryString["Customer_Id"].ToString();
            }
        }

        protected void OrderForm1_OnSuccessOrder(object sender, EventArgs e)
        {
            MultiView2.SetActiveView(OrderSummaryView);
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderInfo1.Populate_Orders(dbconn, OrderForm1.OrderId);
            CustomerControl1.Get_Customer_Details(dbconn, CustomerId.Value);
            dbconn.Close();
        }



        protected void OrderForm1_OnLoad(object sender, EventArgs e)
        {
            //OrderForm1.Show_Address_Book = false;
            OrderForm1.Show_Store_PlaceHolder = true;
        }


        protected void Search_Customer_Button_Click(object sender, EventArgs e)
        {
            CustomerGrid1.Load_Customers(Customer_Name.Text,
                Mobile_Number.Text,
                Email_Address.Text);
        }

        protected void CustomerGrid1_OnCustomerSelect(object sender, EventArgs e)
        {
            CommandEventArgs lCommandEventArgs = (CommandEventArgs)e;
            CustomerId.Value = lCommandEventArgs.CommandArgument.ToString();
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CustomerControl1.Get_Customer_Details(dbconn, CustomerId.Value);
            CustomerControl1.CustomerViewMode = FormViewMode.ReadOnly;

            dbconn.Close();
            OrderForm1.Set_Customer_Id = CustomerId.Value;
            MultiView1.SetActiveView(Customer_View);
            MultiView2.ActiveViewIndex = -1;

        }

        protected void Back_Button_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(Search_Customer_View);
        }

        protected void View_Order_Button_Command(object sender, CommandEventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            OrderInfo2.Populate_Orders(dbconn, e.CommandArgument.ToString());
            dbconn.Close();
            MultiView2.SetActiveView(Order_Details_View);
        }

        protected void Orders_GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }


        protected void Order_Back_Button_Click(object sender, EventArgs e)
        {
            MultiView2.SetActiveView(AllOrdersView);
        }

        protected void Due_Back_Button_Click(object sender, EventArgs e)
        {
            Response.Redirect("DueList.aspx");
        }



        protected void Post_Payment_Button_Click(object sender, EventArgs e)
        {
            PaymentForm1.Clear_Payment_Form();
            PaymentForm1.Set_Customer_Id = CustomerId.Value;
            PaymentForm1.Set_Payment_Amount = CustomerControl1.Get_Due_Amount;
            MultiView2.SetActiveView(Payments_View);
        }

        protected void Create_New_Order_Button_Click(object sender, EventArgs e)
        {
            OrderForm1.Prepare_Order_Form();
            MultiView2.SetActiveView(Order_View);
        }

        protected void Address_Book_Button_Click(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            AddressBook1.Set_User_Id = CustomerId.Value;
            AddressBook1.Load_Address(dbconn);
            dbconn.Close();
            MultiView2.SetActiveView(Address_View);
        }

        protected void All_Orders_Button_Click(object sender, EventArgs e)
        {
            SqlDataSource1.SelectParameters["P_USER_ID"].DefaultValue = CustomerId.Value;
            SqlDataSource1.SelectParameters["P_DUE_ORDER"].DefaultValue = "0";
            SqlDataSource1.DataBind();
            Orders_GridView.DataBind();
            MultiView2.SetActiveView(AllOrdersView);
        }

        protected void Due_Orders_Click(object sender, EventArgs e)
        {
            SqlDataSource1.SelectParameters["P_USER_ID"].DefaultValue = CustomerId.Value;
            SqlDataSource1.SelectParameters["P_DUE_ORDER"].DefaultValue = "1";
            SqlDataSource1.DataBind();
            Orders_GridView.DataBind();
            MultiView2.SetActiveView(AllOrdersView);
        }

        protected void Post_Adjustment_Button_Click(object sender, EventArgs e)
        {

        }

        protected void Customer_OnSuccessPayment(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            CustomerControl1.Get_Customer_Details(dbconn, CustomerId.Value);
            dbconn.Close();
        }
    }
}