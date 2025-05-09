using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Data_Access.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Adress Adress { get; set; }
        public double Area { get; set; }
        public BuildingRole Role { get; set; }
        public List<Worker> Workers { get; set; } = [];
    }
}
