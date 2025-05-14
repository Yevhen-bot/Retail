using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Manager : Worker
    {
        private const double PROGRESSION = 0.5;

        public Manager(Name name, Age birthdate, Email email, Adress adress, Salary salary, IBuilding building)
            : base(name, birthdate, email, adress, salary, new ExhaustionLevel(PROGRESSION), building)
        {
            building.AddManager(this);
        }

        public override void Work()
        {
            try
            {
                _lvl.Progress();
            }
            catch (ArgumentException ex)
            {
                RaiseSalary();
            }
        }

        public void Manage(object sender, ExhaustionLevel lvl)
        {
            var v = (Worker)sender;
            v.RaiseSalary();
        }
    }
}
