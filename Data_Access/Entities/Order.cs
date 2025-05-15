using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;
using Data_Access.Adapters;

namespace Data_Access.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Client Client { get; set; }
        public List<ProductWrapper> Products { get; set; }
    }
}
