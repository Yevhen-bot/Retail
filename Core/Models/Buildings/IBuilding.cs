using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.People;
using Core.ValueObj;

namespace Core.Models.Buildings
{
    public interface IBuilding
    {
        [Range(0, 700)]
        public int Size { get; set; }
        //Usable area ratio
        [Range(0, 1)]
        public double UAR { get; set; }
        public string Name { get; set; }
        public Adress Adress { get; set; }
        public List<Worker> Workers { get; set; }
        public List<Client> Clients { get; set; }

        public void PrintWorkers();
        public void PrintClients();
    }
}
