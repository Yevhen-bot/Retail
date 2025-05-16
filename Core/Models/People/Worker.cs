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
        protected Random _rnd;
        protected readonly string _hashPassword;

        public Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, ExhaustionLevel lvl, string hashPassword)
        {
            _name = name;
            _birthDate = birthdate;
            _email = email;
            _adress = adress;
            _salary = salary;
            _lvl = lvl;
            _rnd = new();
            _hashPassword = hashPassword;
        }

        public Name Name => _name;
        public Age BirthDate => _birthDate;
        public Email Email => _email;
        public Adress Adress => _adress;
        public Salary Salary => _salary;
        public ExhaustionLevel ExhaustionLevel => _lvl;
        public string Password => _hashPassword;
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
