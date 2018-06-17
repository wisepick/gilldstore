using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GilldStore_New
{
    public class CustomHttpsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //if (!String.Equals(actionContext.Request.RequestUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            //    {
            //        Content = new StringContent("HTTPS Required")
            //    };
            //    return;
            //}
            //  MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            //    dbconn.Open();
            //    Task<string> content = actionContext.Request.Content.ReadAsStringAsync();
            //    string body = content.Result;
            //        CommonClass.ExecuteQuery("TEMP_ADD_MAIL_CONTENTS",
            //        new string[] { "P_SUBJECT", "P_USER_NAME", "P_MESSAGE" },
            //        new string[] { "hi", "hi", body},
            //        dbconn);                                    
            //    dbconn.Close();

            //string token;
            //try
            //{


            //    token = actionContext.Request.Headers.GetValues("X-Appery-Api-Express-Api-Key").First();

            //}
            //catch (Exception)
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            //    {
            //        Content = new StringContent("Missing Authorization-Token")
            //    };
            //    return;
            //}
            //if (token != "65b324a2-b15f-4dde-a167-c56efedacfa4")
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            //    {
            //        Content = new StringContent("Unauthorised key")
            //    };
            //    return;
            //}
        }
    }
}