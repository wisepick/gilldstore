using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Account
{
    public partial class SearchOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Search_Order_Button_Click(object sender, EventArgs e)
        {
            OrderGrid1.Order_Id = Server.HtmlEncode(Order_Id.Text);
            OrderGrid1.Search_Order();
        }
    }
}