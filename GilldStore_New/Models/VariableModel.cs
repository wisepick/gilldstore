using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.Models
{
    public class VariableModel
    {
        public string User_Name { get; set; }
        public string Activation_Code { get; set; }
        public string Guid { get; set; }
        public string UserGuid { get; set; }
        public string Email { get; set; }
        public string Feedback_Message { get; set; }
        public string Order_Number { get; set; }
        public string Order_Amount { get; set; }
        public string Subtotal_Amount { get; set; }
        public string Shipping_Charges { get; set; }
        public string Grand_Total { get; set; }
        public string Order_Summary { get; set; }
        public string User_Address { get; set; }
        public string Order_Date { get; set; }
        public string Discount { get; set; }
        public string Shipping_Details { get; set; }
        public string Refund_Amount { get; set; }
        public string Rejection_Reason { get; set; }
        public string Delivered_Date { get; set; }
        public string Undelivered_Reason { get; set; }
        public string Payment_Type { get; set; }
    }
}