using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Client : IPerson
    {
        private const int UP_SALARY = 7;
        private const int DOWN_SALARY = 4;
        private const decimal PERCENTAGE = 0.1M;

        private Name _name;
        private Age _age;
        private Salary _salary;
        private Email _email;
        private HashSet<Product> _preferences;
        private decimal _money;
        private Random _rnd;
        private List<Order> _orders;
        private string? _password;

        public int Years => _age.GetAge();
        public Age Age => _age;
        public Name Name => _name;
        public Email Email => _email;
        public Salary Salary => _salary;
        public decimal Money => _money;
        public string? Password => _password;
        public List<Product> Preferenc => _preferences.ToList();
        public List<Order> Orders => _orders.ToList();

        public Client(Email email, Name name, Age age, Salary salary, Dictionary<Product, int> products, decimal money, HashSet<Product> preferences = null, string? password = null, List<Order> orders = null)
        {
            _email = email;
            _name = name;
            _age = age;
            _salary = salary;
            if (products == null)
                _preferences = new();
            else
                _preferences = preferences;
            _money = money;
            _rnd = new();
            _orders = new();
            _password = password;
            if (orders == null)
                _orders = new();
            else
                _orders = orders;
        }

        public void Sleep()
        {
            var r = _rnd.Next(0, 10);
            if(r == UP_SALARY)
            {
                _salary.Raise(_salary.Amount * PERCENTAGE);
            } else if(r == DOWN_SALARY)
            {
                _salary.DeRaise(_salary.Amount * PERCENTAGE);
            }
        }

        public void Work()
        {
            _money += _salary.GetPayForDay();
        }

        public bool Buy(Dictionary<Product, int> products, Store store)
        {
            var (price, isnewclient) = store.TrySell(this, products, _preferences);
            if(price > (double)_money)
            {
                throw new ArgumentOutOfRangeException("Not enough money.");
            }
            else
            {
                store.Sell(products);
                _money -= (decimal)price;
            }
            _orders.Add(new Order(products, Store.GetPreferenceConst(), _preferences));
            
            return isnewclient;
        }

        public void AddPreference(Product product)
        {
            if (_preferences.Contains(product))
                throw new ArgumentOutOfRangeException(nameof(product), product, "You already have this product in your preferences.");
            _preferences.Add(product);
        }
    }
}
