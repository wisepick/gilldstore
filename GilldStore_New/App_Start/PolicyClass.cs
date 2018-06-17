using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.App_Start
{
    public class PolicyClass
    {
        public static List<PolicyModel> Get_All_Policies()
        {
            List<PolicyModel> lPolicies = new List<PolicyModel>();
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            MySqlConnection dbconn1 = new MySqlConnection(CommonClass.connectionstring);
            MySqlConnection dbconn2 = new MySqlConnection(CommonClass.connectionstring);

            dbconn.Open();
            MySqlDataReader reader = CommonClass.FetchRecords("GET_ALL_POLICIES",
                new string[] { },
                new string[] { },
                dbconn);
            while (reader.Read())
            {
                PolicyModel lPolicy = new PolicyModel();
                lPolicy.Policy_Id = int.Parse(reader["POLICY_ID"].ToString());
                lPolicy.Policy_Name = reader["POLICY_NAME"].ToString();
                lPolicy.Policy_Updated_Date = reader["LAST_UPDATED_DATE"].ToString();
                dbconn1.Open();
                MySqlDataReader reader1 = CommonClass.FetchRecords("GET_POLICY_HEADERS",
                    new string[] { "P_POLICY_ID" },
                    new string[] { reader["POLICY_ID"].ToString() },
                    dbconn1);
                List<Policy_Details> lPolicyDetails = new List<Policy_Details>();
                while (reader1.Read())
                {
                    Policy_Details lPolicyDetail = new Policy_Details();
                    lPolicyDetail.Policy_Header_Text = reader1["HEADER_TEXT"].ToString();
                    List<Policy_Contents> lPolicyContents = new List<Policy_Contents>();
                    dbconn2.Open();
                    MySqlDataReader reader2 = CommonClass.FetchRecords("GET_POLICY_CONTENT",
                        new string[] { "P_POLICY_ID", "P_POLICY_HEADER_ID" },
                        new string[] { reader["POLICY_ID"].ToString(), reader1["POLICY_HEADER_ID"].ToString() },
                        dbconn2);
                    while (reader2.Read())
                    {
                        Policy_Contents lPolicyContent = new Policy_Contents();
                        lPolicyContent.Policy_Content = reader2["CONTENT"].ToString();
                        lPolicyContents.Add(lPolicyContent);
                    }
                    lPolicyDetail.Policy_Contents = lPolicyContents;
                    reader2.Close();
                    dbconn2.Close();
                    lPolicyDetails.Add(lPolicyDetail);
                }
                lPolicy.Policy_Details = lPolicyDetails;
                reader1.Close();
                dbconn1.Close();
                lPolicies.Add(lPolicy);
            }
            reader.Close();
            dbconn.Close();
            return lPolicies;
        }
    }
}