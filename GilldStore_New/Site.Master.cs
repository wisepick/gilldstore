using GilldStore_New.App_Start;
using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

         //   navlogo.ImageUrl = "~/img/" + Application["COMPANY_LOGO"].ToString();
          //  navlogo.ToolTip = Application["COMPANY_NAME"].ToString();
           // navlogo.AlternateText = Application["COMPANY_NAME"].ToString();
            if (Request.ServerVariables["HTTPS"].ToString() != "on" && !Request.ServerVariables["HTTP_HOST"].ToString().Contains("localhost"))
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString());
            }

            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            if (Session["USER_TYPE"] == null)
            {
                UserModel lUserModel = CommonClass.Get_External_User_Profile(dbconn);
                if (lUserModel != null)
                {
                    Session["USER_TYPE"] = lUserModel.User_Type_Id.ToString();
                }
            }
            
            CommonClass.ExecuteQuery("ADD_SESSION",
                new string[] 
                {
                    "P_SESSION_ID"
                },
                new string[]
                {
                    Session.SessionID
                },
                dbconn);
            dbconn.Close();

            if (!IsPostBack)
            {
                Shopping_Cart_Amount = ShoppingCartClass.Get_Shopping_Cart_Item_Count();
            }
        }

        public string Shopping_Cart_Amount
        {
            set
            {
                Cart_Item_Count.Text = value;
                //Cart_Item_Count1.Text = value;
                Cart_Item_Count2.Text = value;
            }
        }
    }
}