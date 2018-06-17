using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.Models
{
    public class PolicyModel
    {
        public int Policy_Id { get; set; }
        public string Policy_Name { get; set; }
        public string Policy_Updated_Date { get; set; }
        public List<Policy_Details> Policy_Details { get; set; }
    }

    public class Policy_Details
    {
        public string Policy_Header_Text { get; set; }
        public List<Policy_Contents> Policy_Contents { get; set; }

    }

    public class Policy_Contents
    {
        public string Policy_Content { get; set; }
    }
}