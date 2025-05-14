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
        private readonly Dictionary<Product, int> _products;

        public double Area { get; private set; }
        public string Name { get; private set; } = null!;
        public Adress Adress => _adress;
        public IReadOnlyList<Worker> Workers => _workers;
        public Manager Manager => _manager;
        public IReadOnlyDictionary<Product, int> Products => _products;

        public Warehouse(double area, string name, Adress adress)
        {
            _workers = [];
            Area = area;
            Name = name;
            _adress = adress;
            _products = [];
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
            if (worker is not Warehouse_Worker)
                throw new InvalidOperationException("Worker is not a Warehouse_Worker.");

            ArgumentNullException.ThrowIfNull(worker);

            _workers.Add((Warehouse_Worker)worker);

            if (_manager != null)
            {
                worker.HighExLevel += _manager.Manage;
            }
        }

        public void AddProduct(Product product, int quantity)
        {
            try
            {
                var v = new Dictionary<Product, int>();
                v.Add(product, quantity);
                ValidateSpace(v);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("Something wrong.", ex);
            }

            if (_products.ContainsKey(product))
                _products[product] += quantity;
            else
                _products[product] = quantity;
        }

        public void AddProduct(Dictionary<Product, int> newpr)
        {
            ValidateSpace(newpr);
            foreach (var el in newpr)
            {
                if (_products.ContainsKey(el.Key))
                    _products[el.Key] += el.Value;
                else
                    _products[el.Key] = el.Value;
            }
        }

        public void SimulateDay()
        {
            if(_manager == null) throw new InvalidOperationException("Manager is not assigned to this building.");
            if (_workers.Count == 0) throw new InvalidOperationException("No workers in this building.");

            foreach (var worker in _workers)
            {
                worker.Work();
            }
            _manager.Work();
            _manager.Sleep();
            foreach(var worker in _workers)
            {
                worker.Sleep();
            }
        }

        public void Export(Dictionary<Product, int> products, Store to)
        {

            foreach (var el in products)
            {
                if (_products.ContainsKey(el.Key))
                {
                    if (_products[el.Key] < el.Value)
                        throw new ArgumentOutOfRangeException("Not enough product in warehouse.");
                }
                else
                    throw new ArgumentOutOfRangeException("Product not found in warehouse.");
            }

            try
            {
                to.Import(products);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("Not enough space in the store.", ex);
            }

            foreach (var el in products)
            {
                _products[el.Key] -= el.Value;
            }
        }

        public void Export(Dictionary<Product, int> products)
        {
            foreach (var el in products)
            {
                if (_products.ContainsKey(el.Key))
                {
                    if (_products[el.Key] < el.Value)
                        throw new ArgumentOutOfRangeException("Not enough product in warehouse.");
                }
                else
                    throw new ArgumentOutOfRangeException("Product not found in warehouse.");
            }

            foreach (var el in products)
            {
                _products[el.Key] -= el.Value;
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
