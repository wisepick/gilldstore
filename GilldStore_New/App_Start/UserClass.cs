using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;

namespace GilldStore_New.App_Start
{
    public class UserClass
    {
        public static UserAddress Get_User_Address(string p_AddressId,
            MySqlConnection dbconn)
        {
            UserAddress lUserAddress = new UserAddress();
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_ADDRESS_BY_ID",
                new string[]
                {
                    "P_ADDRESS_ID"
                },
                new string[]
                {
                    p_AddressId
                },
                dbconn);
            if (lMySqlDataReader.Read())
            {
                lUserAddress.User_Name = lMySqlDataReader["USER_NAME"].ToString();
                lUserAddress.Mobile_Number = lMySqlDataReader["MOBILE_NUMBER"].ToString();
                lUserAddress.Address = lMySqlDataReader["SHIPPING_ADDRESS"].ToString();
                lUserAddress.City = lMySqlDataReader["CITY_NAME"].ToString();
                lUserAddress.State = lMySqlDataReader["STATE_NAME"].ToString();
                lUserAddress.Pin_Code = lMySqlDataReader["PIN_CODE"].ToString();
            }
            lMySqlDataReader.Close();
            return lUserAddress;
        }

        public static void MigrateUser(MySqlConnection dbconn,
            string pUserid,
            string pFullName,
            string pEmail,
            string pEmailValidated)
        {
            Guid lGuid = Guid.NewGuid();
            Guid lUserGuid = Guid.NewGuid();            
            string[] lRecords = CommonClass.ExecuteQuery("MIGRATE_USER",
                new string[]
                {
                    "P_EXTERNAL_USER_ID",
                    "P_USER_NAME",
                    "P_EMAIL_ADDRESS",
                    "P_GUID",
                    "P_USERID_GUID",
                    "P_EMAIL_VALIDATED_BY_PROVIDER"
                },
                new string[]
                {
                    pUserid,
                    pFullName,
                    pEmail,
                    lGuid.ToString(),
                    lUserGuid.ToString(),
                    pEmailValidated
                },
                new string[]
                {
                    "P_USER_ID",
                    "P_NEW_USER",
                    "P_EMAIL_ADDRESS_VALIDATED",
                    "P_USER_TYPE_ID"
                },
                dbconn);




            if (lRecords[1] == "1")
            {
                if (pEmailValidated == "0")
                {
                    Messages.Send_Welcome_Email_Validate_Message(pEmail,
                        pFullName,
                        lGuid.ToString(),
                        lUserGuid.ToString(),
                        dbconn);
                }
                else
                {
                    Add_To_MailChimp(pEmail,
                        pFullName);
                    Messages.Send_Welcome_Message(pEmail,
                        pFullName,
                        dbconn);
                }
                //  Messages.Send_Mobile_Validate_Message("9980075754", "Karpaga Kumar", "12345", dbconn);
                Messages.send_sms("9980075754", "Dear Karpaga Kumar," + pEmail + " - " + "New User has been registered-Thanks");
            }
        }

        public static void Generate_OTP(string pMobileNumber,
            string pUserName,
            Label pLabel,
            PlaceHolder pPlaceHolder)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Generate_OTP(pMobileNumber,
                pUserName,
                pLabel,
                pPlaceHolder,
                dbconn);
            dbconn.Close();
        }

        public static void Add_To_MailChimp(string pEmailAddress,
            string pUserName)
        {
            string lJsonData = "{";
            lJsonData += "\"email_address\": \"" + pEmailAddress + "\",";
            lJsonData += "\"status\": \"" + "subscribed" + "\",";
            lJsonData += "\"merge_fields\": {";
            lJsonData += "\"FNAME\": \"" + pUserName + "\"}";
            lJsonData += "}";


            RestClient lRestClient = new RestClient("https://us16.api.mailchimp.com/3.0/lists/9c6aecda0d/members/");
            RestRequest lRestRequest = new RestRequest(Method.POST);
            lRestRequest.AddHeader("Authorization", "apikey" + " " + "451abacce83271f95f721014795de9c7-us16");
            lRestRequest.AddParameter("application/json", lJsonData, ParameterType.RequestBody);
            IRestResponse lRestResponse = lRestClient.Execute(lRestRequest);                   
        }

        public static void Generate_OTP(string pMobileNumber,
            string pUserName,
            Label pLabel,
            PlaceHolder pPlaceHolder,
            MySqlConnection dbconn)
        {
            
            MySqlTransaction Transaction = dbconn.BeginTransaction();
            //Repeater lMobileNumberRepeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            //Label lOTPMessageLabel = User_FormView.FindControl("OTP_Message") as Label;
            //string lOTPMessageLabel;
            //lOTPMessageLabel.Visible = true;
            try
            {
                string lMobileNumber = HttpContext.Current.Server.HtmlEncode(pMobileNumber);
                string lUserName = HttpContext.Current.Server.HtmlEncode(pUserName);
                string[] lRecords = CommonClass.FetchRecords("GENERATE_MOBILE_ACTIVATION_CODE",
                    new string[] 
                    { 
                        "P_USER_ID",
                        "P_MOBILE_NUMBER"
                    },
                    new string[] 
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        lMobileNumber
                    },
                    new string[] 
                    {
                        "P_ACTIVATION_CODE"
                    },
                    dbconn);

                if (lRecords[0] != null)
                {
                    Messages.Send_Mobile_Validate_Message(lMobileNumber, lUserName, lRecords[0], dbconn);                    
                    pPlaceHolder.Visible = true;
                    pLabel.Text = "OTP Successfully Generated";
                    pLabel.ForeColor = Color.Green;
                }
                else
                {
                    pLabel.ForeColor = Color.Red;
                    pLabel.Text = "Unable to Generate OTP";
                }
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                pLabel.Text = "Server Error, Try after sometime";
                pLabel.ForeColor = Color.Red;
            }
            
        }

        public static void Generate_GUID(string pEmail,
            string pUserName,
            Label pLabel)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Generate_GUID(pEmail,
                pUserName,
                pLabel,
                dbconn);
            dbconn.Close();
        }
        public static void Generate_GUID(string pEmail,
            string pUserName,
            Label pLabel,
            MySqlConnection dbconn)
        {
            
            MySqlTransaction Transaction = dbconn.BeginTransaction();
            //            Repeater lEmailRepeater = User_FormView.FindControl("Email_Repeater") as Repeater;
            //Label lEmailMessageLabel = User_FormView.FindControl("Email_Validation_Message") as Label;
            //lEmailMessageLabel.Visible = true;
            try
            {                
                Guid g = Guid.NewGuid();
                Guid lUserGuid = Guid.NewGuid();
                CommonClass.ExecuteQuery("ADD_GUID",
                    new string[]
                    {
                        "P_EXTERNAL_USER_ID",
                        "P_EMAIL",
                        "P_GUID",
                        "P_USER_GUID"
                    },
                    new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,                   
                        pEmail,
                        g.ToString(),
                        lUserGuid.ToString()
                    },
                    dbconn);


                Messages.Send_Welcome_Email_Validate_Message(pEmail, 
                    pUserName, 
                    g.ToString(), 
                    lUserGuid.ToString(), 
                    dbconn);

                // Bind_Contacts("2", dbconn, lEmailRepeater);
                Transaction.Commit();

                pLabel.Text = "Validation Email Sent";
                //User_FormView.FindControl("Email_PlaceHolder").Visible = true;
                pLabel.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                pLabel.Text = ex.Message;
                pLabel.Visible = true;
                pLabel.ForeColor = Color.Red;
            }
            
        }

        public static void Validate_Mobile(string pMobileNumber,
            string pMobileOTP,
            Label pLabel,
            PlaceHolder pPlaceHolder)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            Validate_Mobile(pMobileNumber,
                pMobileOTP,
                pLabel,
                pPlaceHolder,
                dbconn
            );
            dbconn.Close();
        }

        public static void Validate_Mobile(string pMobileNumber,
            string pMobileOTP,
            Label pLabel,
            PlaceHolder pPlaceHolder,
            MySqlConnection dbconn
            )
        {
            
            //TextBox lMobile_Number_TextBox = User_FormView.FindControl("Mobile_Number") as TextBox;
           // Repeater lMobile_Number_Repeater = User_FormView.FindControl("Mobile_Number_Repeater") as Repeater;
            //RepeaterItem lRepeaterItem1 = lMobile_Number_Repeater.Items[0];
            string[] lRecords = CommonClass.FetchRecords("VALIDATE_MOBILE",
                new string[]
                    {
                        "P_USER_ID",
                        "P_MOBILE_NUMBER",
                        "P_VALIDATION_CODE"
                    },
                new string[]
                    {
                        ClaimsPrincipal.Current.FindFirst("user_id").Value,
                        pMobileNumber,
                        pMobileOTP
                    },
                new string[]
                    {
                        "P_STATUS"
                    },
                dbconn);

           // Bind_Contacts("1", dbconn, lMobile_Number_Repeater);
            

            //Label OTP_Message_Label = User_FormView.FindControl("OTP_Message") as Label;
            pLabel.Visible = true;
            if (lRecords[0] != null)
            {
                pLabel.Text = lRecords[0];
                pLabel.ForeColor = Color.Red;
            }
            else
            {
                pPlaceHolder.Visible = false;
                pLabel.Text = "OTP Successfully Validated";
                pLabel.ForeColor = Color.Green;
            }
        }

    }
}