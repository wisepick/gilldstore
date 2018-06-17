using GilldStore_New.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class CustomerGrid : System.Web.UI.UserControl
    {
        public event EventHandler CustomerSelect;
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = CommonClass.connectionstring;
        }

        public void Load_Customers(string lCustomerName, string lMobileNumber, string lEmailAddress)
        {
            SqlDataSource1.SelectParameters["P_USER_NAME"].DefaultValue = lCustomerName;
            SqlDataSource1.SelectParameters["P_MOBILE_NUMBER"].DefaultValue = lMobileNumber;
            SqlDataSource1.SelectParameters["P_EMAIL_ADDRESS"].DefaultValue = lEmailAddress;
            SqlDataSource1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void Customer_View_Button_Command(object sender, CommandEventArgs e)
        {
            if (CustomerSelect != null)
            {
                CustomerSelect(this, e);
            }
        }
    }
}