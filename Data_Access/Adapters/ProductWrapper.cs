using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Data_Access.Adapters
{
    public class ProductWrapper
    {
        public string Name { get; set; }
        public double Price { get; set; }
        // meters per unit
        public double MPU { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public Product Product 
        {
            get => new(Name, Price, MPU);
            set
            {
                Name = value.Name;
                Price = value.Price;
                MPU = value.MPU;
            }
        }
    }
}
