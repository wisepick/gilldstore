namespace GilldStore_New
{
    using System;
    using System.Threading.Tasks;
    using Auth0.AuthenticationApi;
    using Auth0.AuthenticationApi.Models;
    using Auth0.AspNet;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Web;
    using MySql.Data.MySqlClient;
    using GilldStore_New.App_Start;
    using GilldStore_New.Models;
    using System.Web.SessionState;

    public class LoginCallback : HttpTaskAsyncHandler, System.Web.SessionState.IRequiresSessionState
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            AuthenticationApiClient client = new AuthenticationApiClient(
                new Uri(string.Format("https://{0}", ConfigurationManager.AppSettings["auth0:Domain"])));

            var token = await client.ExchangeCodeForAccessTokenAsync(new ExchangeCodeRequest
            {
                ClientId = ConfigurationManager.AppSettings["auth0:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"],
                AuthorizationCode = context.Request.QueryString["code"],
                RedirectUri = context.Request.Url.ToString()
            });

            var profile = await client.GetUserInfoAsync(token.AccessToken);

            string lEmail = "";
            if (profile.Email != null)
            {
                lEmail = profile.Email;
            }

            var user = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", profile.UserName ?? lEmail),
                new KeyValuePair<string, object>("email", lEmail),
                new KeyValuePair<string, object>("family_name", profile.LastName),
                new KeyValuePair<string, object>("given_name", profile.FirstName),
                new KeyValuePair<string, object>("nickname", profile.NickName),
                new KeyValuePair<string, object>("picture", profile.Picture),
                new KeyValuePair<string, object>("user_id", profile.UserId),
                new KeyValuePair<string, object>("id_token", token.IdToken),
                new KeyValuePair<string, object>("access_token", token.AccessToken),
                new KeyValuePair<string, object>("refresh_token", token.RefreshToken),
                new KeyValuePair<string, object>("connection", profile.Identities.First().Connection),
                new KeyValuePair<string, object>("provider", profile.Identities.First().Provider)
            };

            // NOTE: Uncomment the following code in order to include claims from associated identities
            //profile.Identities.ToList().ForEach(i =>
            //{
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".access_token", i.AccessToken));
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".provider", i.Provider));
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".user_id", i.UserId));
            //});

            // NOTE: uncomment this if you send roles
            // user.Add(new KeyValuePair<string, object>(ClaimTypes.Role, profile.ExtraProperties["roles"]));

            // NOTE: this will set a cookie with all the user claims that will be converted 
            //       to a ClaimsPrincipal for each request using the SessionAuthenticationModule HttpModule. 
            //       You can choose your own mechanism to keep the user authenticated (FormsAuthentication, Session, etc.)
            FederatedAuthentication.SessionAuthenticationModule.CreateSessionCookie(user);
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();

            
            string lEmailValidated = "0";
            if (profile.EmailVerified == true)
            {
                lEmailValidated = "1";
            }

            UserClass.MigrateUser(dbconn,
                profile.UserId,
                profile.FullName,
                profile.Email,
                lEmailValidated
                );

            ShoppingCartClass.Swap_Shopping_Cart(dbconn,
                profile.UserId);
            
           
            dbconn.Close();
           

            context.Response.Redirect("~/Account/Validate.aspx");

            //if (lRecords[3] == "3")
            //{
            //   if (lRecords[1] == "1")
            //  {
            //     context.Response.Redirect("~/Account/Manage.aspx");
            //}
            //if (context.Request.QueryString["r_url"] != null)
            //{
            //  if (context.Request.QueryString["r_url"] == "order")
            //{
            //   context.Response.Redirect("~/order_form.aspx");
            //}
            //}
            //context.Response.Redirect("/");
            //}
            //else
            //{
            //    context.Response.Redirect("~/Account/dashboard.aspx");
            //}
        }


    }
}