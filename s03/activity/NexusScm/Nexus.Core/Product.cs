using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;


namespace Nexus.Core
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        // Foreign Key
        public int SupplierId { get; set; }

        // Navigation Property
        public Supplier Supplier { get; set; }
    }
}
