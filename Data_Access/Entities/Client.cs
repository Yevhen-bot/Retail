using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;
using Data_Access.Adapters;

namespace Data_Access.Entities
{
    public class Client : IUser
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Age Age { get; set; }
        public Salary Salary { get; set; }
        public Email Email { get; set; }
        public decimal Money { get; set; }
        public string HashedPassword { get; set; } = null!;
        public List<ProductWrapper> Preferences { get; set; } = [];
        public List<Order> Orders { get; set; } = [];
        public List<Building> Buildings { get; set; } = [];

        [NotMapped]
        public Role Role { get; set; } = new("Client");
    }
}
