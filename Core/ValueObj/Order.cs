using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct Order
    {
        public double PREFERENCE { get; init; }
        public HashSet<Product> Prefernces { get; init; }
        public Dictionary<Product, int> Products { get; init; }

        public Order() { }
        public Order(Dictionary<Product, int> products, double PR, HashSet<Product> prefernces)
        {
            Products = products;
            PREFERENCE = PR;
            Prefernces = prefernces;
            Initialize();
        }

        private void Initialize()
        {
            foreach(var el in Products)
            {
                if(el.Value <= 0)
                {
                    throw new ArgumentException("Invalid Order quantity");
                }

                if(Prefernces.Contains(el.Key))
                {
                    el.Key.ChangePrice(el.Key.Price * PREFERENCE);
                }
            }
        }
    }
}
