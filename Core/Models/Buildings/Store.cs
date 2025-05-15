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
        private const double PREFERENCE = 0.9;
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
        public List<Client> Clients => _clients;
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

        public Store(string name, Adress adress, double area, Dictionary<Product, int> products, List<Client> clients)
        {
            Name = name;
            _adress = adress;
            Area = area;
            _products = products;
            _workers = new();
            _clients = clients;
        }

        public void AddManager(Manager manager)
        {
            ArgumentNullException.ThrowIfNull(manager);
            if (_manager != null)
                throw new InvalidOperationException("Manager already assigned to this building.");

            _manager = manager;
            foreach (var worker in _workers)
            {
                worker.HighExLevel += _manager.Manage;
            }
        }

        public void AddWorker(Worker worker)
        {
            if(worker is not Store_Worker)
                throw new InvalidOperationException("Worker is not a Store_Worker.");

            ArgumentNullException.ThrowIfNull(worker);

            _workers.Add((Store_Worker)worker);

            if (_manager != null)
            {
                worker.HighExLevel += _manager.Manage;
            }
        }

        public void AddClient(Client client)
        {
            ArgumentNullException.ThrowIfNull(client);
            _clients.Add(client);
        }

        public void SimulateDay()
        {
            if (_manager == null) throw new InvalidOperationException("Manager is not assigned to this building.");
            if (_workers.Count == 0) throw new InvalidOperationException("No workers in this building.");

            foreach (var el in _workers)
            {
                el.Work();
            }
            _manager.Work();
            foreach (var el in _clients)
            {
                el.Work();
            }

            foreach (var el in _clients)
            {
                el.Sleep();
            }
            _manager.Sleep();
            foreach (var el in _workers)
            {
                el.Sleep();
            }
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

        public double TrySell(Client client, Dictionary<Product, int> products, HashSet<Product> pref)
        {
            if(!_clients.Contains(client))
            {
                _clients.Add(client);
            }

            double price = 0;
            foreach (var el in products)
            {
                if (_products.ContainsKey(el.Key))
                {
                    if (_products[el.Key] < el.Value)
                        throw new ArgumentOutOfRangeException(nameof(el.Key), el.Value, "We don`t have enough");

                    if(pref.Contains(el.Key))
                    {
                        price += el.Key.Price * el.Value * PREFERENCE;
                    }
                    else price += el.Key.Price * el.Value;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(el.Key), el.Key, "We don`t have this item");
            }

            return price;
        }

        public void Sell(Dictionary<Product, int> products)
        {
            foreach (var el in products)
            {
                _products[el.Key] -= el.Value;
            }
        }

        public static double GetPreferenceConst() => PREFERENCE;
    }
}
