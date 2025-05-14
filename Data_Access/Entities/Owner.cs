using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Data_Access.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Email Email { get; set; }
        public string HashedPassword { get; set; } = null!;
        public List<Building> Buildings { get; set; } = [];
    }
}
