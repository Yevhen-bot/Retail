using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.People;
using Core.ValueObj;

namespace Core.Models.Buildings
{
    public class Warehouse : IBuilding
    {
        private const double UAR = 0.8;

        private readonly List<Warehouse_Worker> _workers;
        private Manager? _manager;
        private readonly Adress _adress;

        public double Area { get; private set; }
        public string Name { get; private set; } = null!;
        public Adress Adress => _adress;
        public IReadOnlyList<Worker> Workers => _workers;
        public Manager Manager => _manager;

        public Warehouse(double area, string name, Adress adress)
        {
            _workers = [];
            Area = area;
            Name = name;
            _adress = adress;
        }

        public void AddManager(Manager manager)
        {
            ArgumentNullException.ThrowIfNull(manager);
            if (_manager != null)
                throw new InvalidOperationException("Manager already assigned to this building.");

            _manager = manager;
        }

        public void AddWorker(Worker worker)
        {
            if (worker is not Warehouse_Worker)
                throw new InvalidOperationException("Worker is not a Warehouse_Worker.");

            ArgumentNullException.ThrowIfNull(worker);

            _workers.Add((Warehouse_Worker)worker);
        }

        public void SimulateDay()
        {
            throw new NotImplementedException();
        }
    }
}
