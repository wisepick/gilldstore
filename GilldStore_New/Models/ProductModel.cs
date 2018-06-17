using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.Models
{
    public class ProductModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public List<Product_Price> Productrices { get; set; }
    }

    public class Product_Price
    {
        public string Measurement_Name { get; set; }
        public double Price { get; set; }
        public double ShippingCharge { get; set; }
        public double Measurement_Unit { get; set; }
        public string DisplayMeasurement { get; set; }
    }
}