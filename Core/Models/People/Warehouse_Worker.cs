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

        Warehouse_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, Warehouse building)
            : base(name, birthdate, email, adress, salary, new ExhaustionLevel(PROGRESSION), building)
        {
            building.AddWorker(this);
        }

        public override void Sleep()
        {
            throw new NotImplementedException();
        }

        public override void Work()
        {
            throw new NotImplementedException();
        }
    }
}
