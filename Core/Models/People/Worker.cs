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

        public Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, ExhaustionLevel lvl, IBuilding building)
        {
            _name = name;
            _birthDate = birthdate;
            _email = email;
            _adress = adress;
            _salary = salary;
            _lvl = lvl;
            _building = building;
        }

        public int Years => _birthDate.GetAge();

        public abstract void Sleep();
        public abstract void Work();
        public void RaiseSalary(decimal amount)
        {
            _lvl.Reduce((double)(amount / _salary.Amount) * RATIO * 100);
            _salary.Raise(amount);
        }
    }
}
