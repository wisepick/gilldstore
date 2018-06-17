using GilldStore_New.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GilldStore_New.App_Start
{
    public class ProductClass
    {
        public static List<ProductModel> Get_Active_Products()
        {
            List<ProductModel> Products = new List<ProductModel>();
            MySqlConnection dbconn = new MySqlConnection(CommonClass.connectionstring);
            MySqlConnection dbconn1 = new MySqlConnection(CommonClass.connectionstring);

            dbconn.Open();
            MySqlDataReader reader = CommonClass.FetchRecords("GET_ACIVE_PRODUCTS",
                new string[] { "PHOTO_IND" },
                new string[] { "0"},
                dbconn);
            while (reader.Read())
            {
                ProductModel product = new ProductModel();
                product.id = int.Parse(reader["PRODUCT_ID"].ToString());
                product.Name = reader["PRODUCT_NAME"].ToString();
                product.Picture = "http://www.gilldstore.in/img/" + reader["PHOTO_FILE_NAME"].ToString();
                List<Product_Price> ProductPrices = new List<Product_Price>();
                dbconn1.Open();
                MySqlDataReader reader1 = CommonClass.FetchRecords("GET_PRODUCT_PRICE",
                    new string[] { "P_PRODUCT_ID", "P_CUSTOMER_ID", "P_EXTERNAL_USER_ID" },
                    new string[] { reader["PRODUCT_ID"].ToString(), null, null },
                    dbconn1);
                while (reader1.Read())
                {
                    Product_Price productprice = new Product_Price();
                    productprice.Measurement_Name = reader1["MEASUREMENT_NAME"].ToString();
                    productprice.Measurement_Unit = double.Parse(reader1["MEASUREMENT_UNIT"].ToString());
                    productprice.Price = double.Parse(reader1["PRICE"].ToString());
                    productprice.ShippingCharge = double.Parse(reader1["SHIPPING_CHARGE"].ToString());
                    productprice.DisplayMeasurement = reader1["MEASUREMENT_UNIT"].ToString() + " " + reader1["MEASUREMENT_NAME"].ToString();
                    ProductPrices.Add(productprice);
                }
                reader1.Close();
                dbconn1.Close();
                product.Productrices = ProductPrices;
                Products.Add(product);
            }
            dbconn.Close();
            return Products;
        }


    }
}