using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;

namespace GilldStore_New.App_Start
{
    public class CommonClass
    {
        public static string connectionstring = Get_ConnectionString();

        public static string SMTP_Password = "Friday*27";
        public static string SMS_User_Id = "bookmedico";
        public static string SMS_Pasword = "8unique";



        protected static string Get_ConnectionString()
        {
            if (Is_Production())
            {
                return ConfigurationManager.ConnectionStrings["MainConnectionStr"].ConnectionString;
            }
            return ConfigurationManager.ConnectionStrings["LocalConnectionStr"].ConnectionString;
        }

        public static void Set_Application_Values()
        {
            MySqlConnection dbconn = new MySqlConnection(connectionstring);
            dbconn.Open();
            MySqlDataReader reader = FetchRecords("GET_COMPANY_INFO",
                new string[] { },
                new string[] { },
                dbconn
            );
            if (reader.Read())
            {
                for (int lCounter = 0; lCounter < reader.FieldCount; lCounter++)
                {
                    HttpContext.Current.Application[reader.GetName(lCounter)] = reader[lCounter];
                }
            }
            reader.Close();

            dbconn.Close();
        }

        public static bool Is_Production()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].Contains("localhost"))
            {
                return false;
            }
            return true;
        }


        protected static string ConvertEmptyStringToNull(string lInputString)
        {
            if (lInputString == "")
            {
                return null;
            }
            else
            {
                return lInputString;
            }
        }

        public static string[] FetchRecords(string SQL,
                                                           string[] IN_SQLParams,
                                                           string[] IN_SQLValues,
                                                           string[] OUT_SQLParams,
                                                           MySqlConnection dbconn)
        {

            MySqlCommand dbcomm = new MySqlCommand(SQL, dbconn);
            dbcomm.CommandType = CommandType.StoredProcedure;
            int counter;
            for (counter = 0; counter < IN_SQLParams.Length; counter++)
            {
                dbcomm.Parameters.AddWithValue(IN_SQLParams[counter], ConvertEmptyStringToNull(IN_SQLValues[counter]));
            }
            for (counter = 0; counter < OUT_SQLParams.Length; counter++)
            {
                if (dbcomm.Parameters.Contains(OUT_SQLParams[counter]) == true)
                {
                    dbcomm.Parameters[OUT_SQLParams[counter]].Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    MySqlParameter param;
                    param = new MySqlParameter(OUT_SQLParams[counter], MySqlDbType.String);
                    param.Direction = ParameterDirection.Output;
                    dbcomm.Parameters.Add(param);
                }
            }
            string[] lOUT_SQLParamValues = new string[counter];
            dbcomm.ExecuteScalar();
            for (counter = 0; counter < OUT_SQLParams.Length; counter++)
            {
                if (dbcomm.Parameters[OUT_SQLParams[counter]].Value != DBNull.Value)
                {
                    lOUT_SQLParamValues[counter] = dbcomm.Parameters[OUT_SQLParams[counter]].Value.ToString();
                }
            }
            return lOUT_SQLParamValues;
        }

        public static MySqlDataReader FetchRecords(string SQL,
                                                          string[] IN_SQLParams,
                                                          string[] IN_SQLValues,
                                                          MySqlConnection dbconn)
        {

            MySqlCommand dbcomm = new MySqlCommand(SQL, dbconn);
            dbcomm.CommandType = CommandType.StoredProcedure;
            int counter;
            for (counter = 0; counter < IN_SQLParams.Length; counter++)
            {
                dbcomm.Parameters.AddWithValue(IN_SQLParams[counter], ConvertEmptyStringToNull(IN_SQLValues[counter]));
            }

            MySqlDataReader Reader;
            Reader = dbcomm.ExecuteReader();

            return Reader;
        }

        public static void ExecuteQuery(string SQL,
                                                            string[] IN_SQLParams,
                                                            string[] IN_SQLValues,
                                                            MySqlConnection dbconn)
        {

            using (MySqlCommand dbcomm = new MySqlCommand(SQL, dbconn))
            {
                dbcomm.CommandType = CommandType.StoredProcedure;
                int counter;
                for (counter = 0; counter < IN_SQLParams.Length; counter++)
                {
                    dbcomm.Parameters.AddWithValue(IN_SQLParams[counter], ConvertEmptyStringToNull(IN_SQLValues[counter]));
                }
                dbcomm.ExecuteScalar();
            }
        }

        public static string[] ExecuteQuery(string SQL,
                                                            string[] IN_SQLParams,
                                                            string[] IN_SQLValues,
                                                            string[] OUT_SQLParams,
                                                            MySqlConnection dbconn)
        {
            return FetchRecords(SQL, IN_SQLParams, IN_SQLValues, OUT_SQLParams, dbconn);
        }

        public static void FetchRecordsAndBind(string SQL,
                                                            string[] IN_SQLParams,
                                                            string[] IN_SQLValues,
                                                            MySqlConnection dbconn,
                                                            object sender)
        {

            MySqlCommand dbcomm = new MySqlCommand(SQL, dbconn);
            dbcomm.CommandType = CommandType.StoredProcedure;
            int counter;
            for (counter = 0; counter < IN_SQLParams.Length; counter++)
            {
                dbcomm.Parameters.AddWithValue(IN_SQLParams[counter], ConvertEmptyStringToNull(IN_SQLValues[counter]));
            }
            MySqlDataReader Reader;
            Reader = dbcomm.ExecuteReader();
            Bind_Data_Set(ref Reader, sender);
        }

        public static void FetchRecordsAndBind(string SQL,
                                                           MySqlConnection dbconn,
                                                           object sender)
        {

            MySqlCommand dbcomm = new MySqlCommand(SQL, dbconn);
            dbcomm.CommandType = CommandType.StoredProcedure;
            MySqlDataReader Reader;
            Reader = dbcomm.ExecuteReader();
            Bind_Data_Set(ref Reader, sender);
        }



        public static void Bind_Data_Set(ref MySqlDataReader Reader, object sender)
        {
            if (sender is ListView)
            {
                ListView lv = (ListView)sender;
                lv.DataSource = Reader;
                lv.DataBind();
            }
            else if (sender is DropDownList)
            {
                DropDownList ddl = (DropDownList)sender;
                ddl.DataSource = Reader;
                ddl.DataBind();
            }
            else if (sender is GridView)
            {
                GridView gv = (GridView)sender;
                gv.DataSource = Reader;
                gv.DataBind();
            }
            else if (sender is TreeView)
            {
                TreeView rpt = (TreeView)sender;
                rpt.DataSource = Reader;
                rpt.DataBind();
            }
            else if (sender is FormView)
            {
                FormView fv = (FormView)sender;
                fv.DataSource = Reader;
                fv.DataBind();
            }
            else if (sender is DetailsView)
            {
                DetailsView dv = (DetailsView)sender;
                dv.DataSource = Reader;
                dv.DataBind();
            }
            else if (sender is CheckBoxList)
            {
                CheckBoxList cbl = (CheckBoxList)sender;
                cbl.DataSource = Reader;
                cbl.DataBind();
            }
            else if (sender is RadioButtonList)
            {
                RadioButtonList rbl = (RadioButtonList)sender;
                rbl.DataSource = Reader;
                rbl.DataBind();
            }
            else if (sender is ListBox)
            {
                ListBox lb = (ListBox)sender;
                lb.DataSource = Reader;
                lb.DataBind();
            }
            else if (sender is Repeater)
            {
                Repeater rpt = (Repeater)sender;
                rpt.DataSource = Reader;
                rpt.DataBind();
            }
            else if (sender is TreeView)
            {
                TreeView rpt = (TreeView)sender;
                rpt.DataSource = Reader;
                rpt.DataBind();
            }
            else if (sender is Menu)
            {
                Menu mnu = (Menu)sender;
                while (Reader.Read())
                {
                    mnu.Items.Add(new MenuItem(Reader[1].ToString(), Reader[0].ToString(), "", ""));
                }
            }

            Reader.Close();
        }

        public static void Load_Attributes(MySqlConnection dbconn,
            string pMasterTypeName,
            object pObject)
        {
            FetchRecordsAndBind("GET_ACTIVE_ATTRIBUTES_BY_MASTER_NAME",
                new string[]
                {
                    "P_MASTER_TYPE_NAME"
                },
                new string[]
                {
                    pMasterTypeName
                },
                dbconn,
                pObject);
        }

        public static void Create_Empty_DropDown_Value(DropDownList pDropDownList)
        {
            pDropDownList.Items.Clear();
            ListItem lEmptyItem = new ListItem();
            lEmptyItem.Text = "";
            lEmptyItem.Value = "";
            pDropDownList.Items.Add(lEmptyItem);
        }

        public static string Format_Address(string pUserName,
            string pMobileNumber,
            string pAddressLine1,
            string pCityName,
            string pStateName,
            string pPinCode)
        {
            string lCommandBreak = ",<br>";
            string lAddress = pUserName;
            lAddress += lCommandBreak + pAddressLine1;


            lAddress += lCommandBreak + pCityName;
            lAddress += lCommandBreak + pStateName + " - " + pPinCode;
            lAddress += lCommandBreak + "<b>Mobile Number : </b>" + pMobileNumber;
            return lAddress;
        }

        public static string Format_Address(string[] pAddress)
        {
            string lCommandBreak = ",<br>";
            string lAddress = pAddress[0];
            lAddress += lCommandBreak + pAddress[2];


            lAddress += lCommandBreak + pAddress[3];
            lAddress += lCommandBreak + pAddress[4] + " - " + pAddress[5];
            lAddress += lCommandBreak + "<b>Mobile Number : </b>" + pAddress[1];
            return lAddress;
        }

        public static bool Display_Attributes_IfMathes(string lString1, string lString2)
        {
            if (lString1 == lString2)
            {
                return true;
            }
            return false;
        }

        public static string[] Get_Address_By_Id(string lAddressId)
        {
            MySqlConnection dbconn = new MySqlConnection(connectionstring);
            dbconn.Open();
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_ADDRESS_BY_ID",
                new string[]
                {
                    "P_ADDRESS_ID"
                },
                new string[]
                {
                    lAddressId
                },
                dbconn);
            string[] lAddress = null;
            if (lMySqlDataReader.Read())
            {
                lAddress = new string[] {
                                            lMySqlDataReader["USER_NAME"].ToString(), 
                                            lMySqlDataReader["MOBILE_NUMBER"].ToString(), 
                                            lMySqlDataReader["SHIPPING_ADDRESS"].ToString(), 
                                            lMySqlDataReader["CITY_NAME"].ToString(), 
                                            lMySqlDataReader["STATE_NAME"].ToString(), 
                                            lMySqlDataReader["PIN_CODE"].ToString()
                                            };
            }
            lMySqlDataReader.Close();
            dbconn.Close();
            return lAddress;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";
            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }


            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }


            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static UserModel Get_External_User_Profile(MySqlConnection dbconn)
        {
            UserModel lUserModel = new UserModel();
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated == true)
            {
                MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_USER_BY_EXTERNAL_ID",
                        new string[] { "P_EXTERNAL_ID" },
                        new string[] { ClaimsPrincipal.Current.FindFirst("user_id").Value },
                    dbconn);
                if (lMySqlDataReader.Read())
                {
                    lUserModel.User_Id = int.Parse(lMySqlDataReader["USER_ID"].ToString());
                    lUserModel.External_User_Id = lMySqlDataReader["EXTERNAL_USER_ID"].ToString();
                    lUserModel.User_Name = lMySqlDataReader["USER_NAME"].ToString();
                    if (lMySqlDataReader["MOBILE_NUMBER"] != null)
                    {
                        lUserModel.Mobile_Number = lMySqlDataReader["MOBILE_NUMBER"].ToString();
                    }
                    lUserModel.Mobile_Validated = int.Parse(lMySqlDataReader["MOBILE_VALIDATED"].ToString());
                    lUserModel.Email_Address = lMySqlDataReader["EMAIL_ADDRESS"].ToString();
                    lUserModel.Email_Address_Validated = int.Parse(lMySqlDataReader["EMAIL_ADDRESS_VALIDATED"].ToString());
                    lUserModel.User_Type_Id = int.Parse(lMySqlDataReader["USER_TYPE_ID"].ToString());
                    lUserModel.Email_Address_Validated_Message = lMySqlDataReader["EMAIL_ADDRESS_VALIDATED_MESSAGE"].ToString();
                    lUserModel.Mobile_Delivery_Option = int.Parse(lMySqlDataReader["MOBILE_DELIVERY_OPTION"].ToString());
                    lUserModel.Email_Delivery_Option = int.Parse(lMySqlDataReader["EMAIL_DELIVERY_OPTION"].ToString());
                }
                lMySqlDataReader.Close();
            }
            return lUserModel;
        }

        public static void Populate_DropDownList_WithNumbers(int pNumber,
            DropDownList pDropDownList)
        {
            int lCounter;
            DataTable dt = new DataTable();
            dt.Columns.Add("SR_NO");
            for (lCounter = 0; lCounter < pNumber + 1; lCounter++)
            {
                dt.Rows.Add(lCounter.ToString());
            }            
            pDropDownList.DataSource = dt;
            pDropDownList.DataBind();  
        }

        public static string Get_Http_Method()
        {
            if (Is_Production() == true)
            {
                return "https://";
            }
            else
            {
                return "http://";
            }
        }
    }
}