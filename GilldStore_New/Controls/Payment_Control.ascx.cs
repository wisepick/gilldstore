using GilldStore_New.App_Start;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GilldStore_New.Controls
{
    public partial class Payment_Control : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void Prepare_Payment_Form(MySqlConnection dbconn)
        {

        }

        public string ChequeNumber
        {
            get
            {
                return Cheque_Number.Text;
            }
        }
        public string ChequeDate
        {
            get
            {
                return Cheque_Date.Text;
            }
        }
        public string MICRCode
        {
            get
            {
                return MICR_Code.Text;
            }
        }
        public string BankName
        {
            get
            {
                return Bank_Name.Text;
            }
        }
        public string BranchName
        {
            get
            {
                return Branch_Name.Text;
            }
        }

        public void Clear_Cheque_Details()
        {
            Cheque_Date.Text = "";
            Cheque_Number.Text = "";
            MICR_Code.Text = "";
            Bank_Name.Text = "";
            Branch_Name.Text = "";
        }

        public string Validate_Cheque_Details()
        {
            string lMessage = null;
            if (Cheque_Date.Text == "")
            {
                lMessage += "Cheque Date is Mandatory<br>";
            }
            if (Cheque_Number.Text == "")
            {
                lMessage += "Cheque Number is Mandatory<br>";
            }
            if (MICR_Code.Text == "")
            {
                lMessage += "MICR Code is Mandatory<br>";
            }
            if (Bank_Name.Text == "")
            {
                lMessage += "Bank Name is Mandatory<br>";
            }
            if (Branch_Name.Text == "")
            {
                lMessage += "Branch Name is Mandatory<br>";
            }
            return lMessage;
        }


        protected void MICR_Code_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            dbconn.Open();
            MySqlDataReader lMySqlDataReader = CommonClass.FetchRecords("GET_BANK_DETAILS_BY_MICR_CODE",
                new string[]
                {
                    "P_MICR_CODE"
                },
                new string[]
                {
                    MICR_Code.Text
                },
                dbconn);
            Bank_Name.Text = "";
            Branch_Name.Text = "";
            if (lMySqlDataReader.Read())
            {
                Bank_Name.Text = lMySqlDataReader["BANK_NAME"].ToString();
                Branch_Name.Text = lMySqlDataReader["BRANCH_NAME"].ToString();
            }
            dbconn.Close();

        }
    }
}