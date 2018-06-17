using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GilldStore_New.App_Start
{
    public class AuthoClass
    {

        public static string[] Get_Client_Authentication_Details()
        {
            if (HttpContext.Current.Session["AUTHO_TOKEN_TYPE"] == null)
            {
                RestClient lRestClient = new RestClient("https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/oauth/token");
                RestRequest lRestRequest = new RestRequest(Method.POST);
                lRestRequest.AddHeader("content-type", "application/json");
                lRestRequest.AddParameter("application/json", "{\"client_id\":\"" + ConfigurationManager.AppSettings["auth0:ClientId"].ToString() + "\",\"client_secret\":\"" + ConfigurationManager.AppSettings["auth0:ClientSecret"].ToString() + "\",\"audience\":\"https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
                IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
               
                JObject lJObject = JObject.Parse(lRestResponse.Content);

                HttpContext.Current.Session["AUTHO_TOKEN_TYPE"] = lJObject["token_type"];
                HttpContext.Current.Session["AUTHO_TOKEN"] = lJObject["access_token"];
            }
            return new string[] {
                HttpContext.Current.Session["AUTHO_TOKEN_TYPE"].ToString() ,
                HttpContext.Current.Session["AUTHO_TOKEN"].ToString()};
        }

        public static string[] Create_User(string pEmailAddress, string pPassword, string pFullName)
        {
            string[] lConnectionDetails = Get_Client_Authentication_Details();
            string[] lResponse = new string[] { null, null };
            string lJsonData = "{";
            //lJsonData += "\"username\": \"" + pEmailAddress + "\",";
            lJsonData += "\"password\": \"" + pPassword + "\",";
            lJsonData += "\"email\": \"" + pEmailAddress + "\",";
            lJsonData += "\"connection\": \"Username-Password-Authentication\",";
            
            
            lJsonData += "\"email_verified\": false,";
            lJsonData += "\"verify_email\": false";
            lJsonData += "}";


            RestClient lRestClient = new RestClient("https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/api/v2/users");


            RestRequest lRestRequest = new RestRequest(Method.POST);
            lRestRequest.AddHeader("authorization", lConnectionDetails[0] + " " + lConnectionDetails[1]);
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
            JObject lJObject = JObject.Parse(lRestResponse.Content);
            if (lJObject["message"] != null)
            {
                lResponse[0] = lJObject["message"].ToString();
            }
            else
            {
                lResponse[1] = lJObject["user_id"].ToString();
                MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
                dbconn.Open();
                UserClass.MigrateUser(dbconn,
                    lResponse[1],
                    pFullName,
                    pEmailAddress,
                    "0");
                dbconn.Close();
            }
            return lResponse;

        }

        public static string[] Register_User()
        {
            string[] lConnectionDetails = Get_Client_Authentication_Details();


            string[] lResponse = new string[] { null, null };
            string lPassword = RandPassClass.Generate(8);
            string lJsonData = "{";
            lJsonData += "\"client_id \": \"" + ConfigurationManager.AppSettings["auth0:ClientId"].ToString() + "\",";                        
            lJsonData += "\"response_type \": \"code\",";
            lJsonData += "\"redirect_uri \": \"http://localhost:56073/LoginCallback.ashx\",";
            lJsonData += "\"verify_email\": false";
            lJsonData += "\"connection\": facebook";
            lJsonData += "}";


            RestClient lRestClient = new RestClient("https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/authorize");


            RestRequest lRestRequest = new RestRequest(Method.GET);
            lRestRequest.AddHeader("authorization", lConnectionDetails[0] + " " + lConnectionDetails[1]);
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
            return lResponse;

        }

        public static string[] Update_User(string pEmailAddress)
        {
            string[] lConnectionDetails = Get_Client_Authentication_Details();


            string[] lResponse = new string[] { null, null };
            string lPassword = RandPassClass.Generate(8);
            string lJsonData = "{";
            lJsonData += "\"username\": \"" + pEmailAddress + "\",";
            lJsonData += "\"email\": \"" + pEmailAddress + "\",";    
            lJsonData += "\"password\": \"" + lPassword + "\",";
            lJsonData += "\"connection\": \"Username-Password-Authentication\",";
            lJsonData += "\"email_verified\": false,";
            lJsonData += "\"verify_email\": false";
            lJsonData += "}";


            RestClient lRestClient = new RestClient("https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/api/v2/users/");


            RestRequest lRestRequest = new RestRequest(Method.POST);
            lRestRequest.AddHeader("authorization", lConnectionDetails[0] + " " + lConnectionDetails[1]);
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
            JObject lJObject = JObject.Parse(lRestResponse.Content);
            if (lJObject["message"] != null)
            {
                lResponse[0] = lJObject["message"].ToString();
            }
            else
            {
                lResponse[1] = lJObject["user_id"].ToString();
            }
            return lResponse;

        }
        

        protected static void Add_Authorisation_Header(RestRequest pRestRequest)
        {
            string[] lConnectionDetails = Get_Client_Authentication_Details();
            pRestRequest.AddHeader("authorization", lConnectionDetails[0] + " " + lConnectionDetails[1]);

        }

        public static void Delete_User(string pExternalUserId)
        {

            RestClient lRestClient = new RestClient("https://" + ConfigurationManager.AppSettings["auth0:Domain"].ToString() + "/api/v2/users/" + pExternalUserId);
            RestRequest lRestRequest = new RestRequest(Method.DELETE);
            Add_Authorisation_Header(lRestRequest);
            //lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);
            //JObject lJObject = JObject.Parse(lRestResponse.Content);
            return;
        }

    }
}