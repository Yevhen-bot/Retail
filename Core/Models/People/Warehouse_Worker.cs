using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Buildings;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Warehouse_Worker : Worker
    {
        private const double PROGRESSION = 1.2;

        public Warehouse_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary)
            : base(name, birthdate, email, adress, salary, new ExhaustionLevel(PROGRESSION))
        {
        }

        public Warehouse_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, ExhaustionLevel lvl)
            : base(name, birthdate, email, adress, salary, lvl)
        {
        }
    }
}
