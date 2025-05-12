using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct Product
    {
        public string Name { get; init; }
        public double Price { get; init; }
        // meters per unit
        public double MPU { get; init; } 

        public Product() { }
        public Product(string name, double price, double upm)
        {
            Name = name;
            Price = price;
            MPU = upm;

            Validate();
        }

        public void Validate()
        {
            if (Price <= 0)
            {
                throw new ArgumentException("Invalid Product price");
            }

            if (MPU <= 0)
            {
                throw new ArgumentException("Invalid Product UPM");
            }
        }
    }
}
