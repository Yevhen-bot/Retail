using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Client : IPerson
    {
        private const int UP_SALARY = 7;
        private const int DOWN_SALARY = 4;
        private const decimal PERCENTAGE = 0.1M;
        private const double RATIO_FOR_PREFERENCES = 1.2;

        private Name _name;
        private Age _age;
        private Adress _homeAdress;
        private Salary _salary;
        private List<Product> _preferences;
        private Dictionary<Product, int> _products;
        private decimal _money;
        private Random _rnd;

        public int Years => _age.GetAge();

        public Client(Name name, Age age, Adress homeAdress, Salary salary, List<Product> preferences, Dictionary<Product, int> products, decimal money)
        {
            _name = name;
            _age = age;
            _homeAdress = homeAdress;
            _salary = salary;
            _preferences = preferences;
            _products = products;
            _money = money;
            _rnd = new();
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

        public void Buy()
    }
}
