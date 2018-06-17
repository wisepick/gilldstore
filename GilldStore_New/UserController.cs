using GilldStore_New.App_Start;
using GilldStore_New.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GilldStore_New
{
    public class UserController : ApiController
    {
        public List<ProductModel> GetAllProducts()
        {
            return ProductClass.Get_Active_Products();
        }
    }
}