using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi
{
    public class ProductInfoOptions
    {
        public static string SectionName = "productInfo";
        public decimal Markup { get; set; }
        public bool BackOrderAllowed { get; set; }
    }
}
