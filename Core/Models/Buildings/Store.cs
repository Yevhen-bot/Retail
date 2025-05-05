using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.People;
using Core.ValueObj;

namespace Core.Models.Buildings
{
    public class Store : IBuilding
    {
        public int Size { get; set; }
        public double UAR { get; set; } = 0.6;
        public string Name { get; set; } = null!;
        public Adress Adress { get; set; }
        public List<Worker> Workers { get; set; } = [];
        public List<Client> Clients { get; set; } = [];
        public void PrintClients()
        {
            throw new NotImplementedException();
        }
        public void PrintWorkers()
        {
            throw new NotImplementedException();
        }
    }
}
