using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.Models
{
    public class UserModel
    {
        public int User_Id { get; set; }
        public string External_User_Id { get; set; }
        public string User_Name { get; set; }
        public string Mobile_Number { get; set; }
        public int Mobile_Validated { get; set; }
        public string Email_Address { get; set; }
        public int Email_Address_Validated { get; set; }
        public int User_Type_Id { get; set; }
        public string Email_Address_Validated_Message { get; set; }
        public int Mobile_Delivery_Option { get; set; }
        public int Email_Delivery_Option { get; set; }

    }

    public class UserDetails
    {
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string User_Name { get; set; }
        public string PUblic_Mobile_Number { get; set; }
    }

    public class UserAddress
    {
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string Address { get; set; }
        public string User_Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin_Code { get; set; }

    }
}