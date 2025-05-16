using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Adapters;

namespace Infrastructure.Mappers
{
    public class OrderMapper
    {
        public Core.ValueObj.Order MapFromDb(Data_Access.Entities.Order dbo)
        {
            return new Core.ValueObj.Order() { Products = dbo.Products.ToDictionary(p => p.Product, p => p.Quantity) };
        }

        public Data_Access.Entities.Order MapToDb(Core.ValueObj.Order order)
        {
            return new Data_Access.Entities.Order()
            {
                Products = order.Products.Select(p => new ProductWrapper() { Product = p.Key, Quantity = p.Value }).ToList(),
                Date = DateTime.Now
            };
        }
    }
}
