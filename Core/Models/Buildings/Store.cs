using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.People;
using Core.ValueObj;

namespace Core.Models.Buildings
{
    public class Store : IBuilding
    {
        private const double UAR = 0.6;

        private Manager? _manager;
        private readonly Adress _adress;
        private readonly List<Store_Worker> _workers;
        private readonly List<Client> _clients;
        private readonly Dictionary<Product, int> _products;

        public double Area { get; private set; }
        public string Name { get; private set; } = null!;
        public Adress Adress => _adress;
        public IReadOnlyList<Worker> Workers => _workers;
        public Manager Manager => _manager;
        public IReadOnlyDictionary<Product, int> Products => _products;

        public Store(double area, string name, Adress adress)
        {
            _workers = [];
            Area = area;
            Name = name;
            _adress = adress;
            _clients = [];
            _products = [];
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
            if(worker is not Store_Worker)
                throw new InvalidOperationException("Worker is not a Store_Worker.");

            ArgumentNullException.ThrowIfNull(worker);

            _workers.Add((Store_Worker)worker);
        }

        public void AddClient(Client client)
        {
            ArgumentNullException.ThrowIfNull(client);
            _clients.Add(client);
        }

        public void SimulateDay()
        {
            throw new NotImplementedException();
        }

        public void Import(Dictionary<Product, int> products)
        {
            ValidateSpace(products);

            foreach (var el in products)
            {
                if (_products.ContainsKey(el.Key))
                    _products[el.Key] += el.Value;
                else
                    _products[el.Key] = el.Value;
            }
        }

        public void Import(Dictionary<Product, int> products, Warehouse from)
        {
            ValidateSpace(products);

            try
            {
                from.Export(products);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("There not enough species on the warehouse.", ex);
            }

            foreach (var el in products)
            {
                if (_products.ContainsKey(el.Key))
                    _products[el.Key] += el.Value;
                else
                    _products[el.Key] = el.Value;
            }
        }

        private void ValidateSpace(Dictionary<Product, int> newpr)
        {
            double totalspace = 0;
            foreach (var el in newpr)
            {
                if (el.Value < 0)
                    throw new ArgumentOutOfRangeException(($"{el.Key},{el.Value}"), "Quantity cannot be negative.");
                totalspace += el.Key.MPU * el.Value;

                if (totalspace > Area * UAR)
                    throw new ArgumentOutOfRangeException("Total space exceeds warehouse area.");
            }
        }
    }
}
