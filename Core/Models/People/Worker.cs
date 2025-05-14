using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;

namespace Core.Models.People
{
    public abstract class Worker : IPerson
    {
        private const double RATIO = 1.5;

        protected readonly Name _name;
        protected readonly Age _birthDate;
        protected Email _email;
        protected Adress _adress;
        protected Salary _salary;
        protected ExhaustionLevel _lvl;
        protected IBuilding _building;
        protected Random _rnd;

        public Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, ExhaustionLevel lvl, IBuilding building)
        {
            _name = name;
            _birthDate = birthdate;
            _email = email;
            _adress = adress;
            _salary = salary;
            _lvl = lvl;
            _building = building;
            _rnd = new();
        }

        public int Years => _birthDate.GetAge();
        public event EventHandler<ExhaustionLevel> HighExLevel;

        public virtual void Sleep()
        {
            var r = _rnd.Next(0, 10);
            if (r == 0)
            {
                _lvl.GoodDay();
            }
            else if (r == 1)
            {
                _lvl.BadDay();
            }
        }
        public virtual void Work()
        {
            try
            {
                _lvl.Progress();
            }
            catch (ArgumentException ex)
            {
                HighExLevel?.Invoke(this, _lvl);
            }
        }
        public void RaiseSalary(decimal amount)
        {
            _lvl.Reduce((double)(amount / _salary.Amount) * RATIO * 100);
            _salary.Raise(amount);
        }

        public void RaiseSalary()
        {
            RaiseSalary(_salary.Amount/5);
        }
    }
}
