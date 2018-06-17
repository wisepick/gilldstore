using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GilldStore_New.Models
{
    public class PayTmPaymentGatewayRequest
    {
        [Required]
        public string REQUEST_TYPE { get; set; }
        [Required]
        public string MID { get; set; }
        [Required]
        public string ORDER_ID { get; set; }
        [Required]
        public string CUST_ID { get; set; }
        [Required]
        public double TXN_AMOUNT { get; set; }
        [Required]
        public string CHANNEL_ID { get; set; }
        [Required]
        public string INDUSTRY_TYPE_ID { get; set; }
        [Required]
        public string WEBSITE { get; set; }
        [Required]
        public int CHECKSUMHASH { get; set; }
        [Required]
        public int MOBILE_NO { get; set; }
        [Required]
        public string EMAIL { get; set; }
        public string ORDER_DETAILS { get; set; }
        public string VERIFIED_BY { get; set; }
        public string IS_USER_VERIFIED { get; set; }
        public string CALLBACK_URL { get; set; }
    }

    public class PayTmPaymentGatewayResponse
    {
        [Required]
        public string MID { get; set; }
        [Required]
        public long TXNID { get; set; }
        [Required]
        public string ORDERID { get; set; }
        [Required]
        public string BANKTXNID { get; set; }
        [Required]
        public decimal TXNAMOUNT { get; set; }
        [Required]
        public string CURRENCY { get; set; }
        [Required]
        public string STATUS { get; set; }
        [Required]
        public string RESPCODE { get; set; }
        [Required]
        public string RESPMSG { get; set; }
        [Required]
        public string TXNDATE { get; set; }
        [Required]
        public string GATEWAYNAME { get; set; }
        [Required]
        public string BANKNAME { get; set; }
        [Required]
        public string PAYMENTMODE { get; set; }
        [Required]
        public string CHECKSUMHASH { get; set; }
    }

    public class PayTmPaymentGatewayRefundRequest
    {
        [Required]
        public string MID { get; set; }
        [Required]
        public string TXNID { get; set; }
        [Required]
        public string ORDERID { get; set; }
        public string REFUNDAMOUNT { get; set; }
        public string TXNTYPE { get; set; }
        public string CHECKSUM { get; set; }
        public string COMMENTS { get; set; }
        public string REFID { get; set; }
    }

    public class PayTmConfiguration
    {
        public string Website_Name { get; set; }
        public string MID { get; set; }
        public string Merchant_Key { get; set; }
        public string Industry_Type { get; set; }
        public string Channel_Id { get; set; }
        public string Payment_Url { get; set; }
        public string Refund_Url { get; set; }
        public string Payment_Enquiry_Url { get; set; }
        public string Refund_Enquiry_Url { get; set; }
        public string Mobile_Number { get; set; }
    }

    public class CCAvanueConfiguration
    {
        public string Merchant_Key { get; set; }
        public string Access_Code { get; set; }
        public string Working_Key { get; set; }
        public string Payment_Url { get; set; }
    }

}