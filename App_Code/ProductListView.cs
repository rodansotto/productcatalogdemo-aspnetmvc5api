using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc5App
{
    public class ProductListView
    {
        public int ProductID { get; set; }

        public string ProductNumber { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        public string Size { get; set; }

        public decimal? Weight { get; set; }

        public string ProductCategory { get; set; }

        public string ProductModel { get; set; }

        public byte[] LargePhoto { get; set; }
    }
}