using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Order_Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);

                if (lUserModel != null)
                {
                    if (lUserModel.Mobile_Validated == 0 || lUserModel.Email_Address_Validated == 0)
                    {
                        Response.Redirect("~/Account/Validate.aspx");
                    }
                }

                dbconn.Close();
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["Customer_Id"] != null)
                {
                    OrderForm1.Set_Customer_Id = Request.QueryString["Customer_Id"].ToString();
                }
                //else
                //{
                //   if (Session["PIN_CODE"] == null)
                //  {
                //     Response.Redirect("Pin_Code_Validation.aspx");
                //}
                //}
                OrderForm1.Prepare_Order_Form();
            }


        }

        protected void OrderForm1_SuccessOrder(object sender, EventArgs e)
        {
            Response.Redirect("~/OrderSummary.aspx?Order_Id=" + OrderForm1.OrderId);
        }
    }
}