using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.People;
using Core.ValueObj;

namespace Core.Interfaces
{
    public interface IBuilding
    {
        double Area { get; }
        string Name { get; }
        Adress Adress { get; }
        public IReadOnlyList<Worker> Workers { get; }
        public Manager Manager { get; }

        public void SimulateDay();
        public void AddManager(Manager manager);
        public void AddWorker(Worker worker);
    }
}
