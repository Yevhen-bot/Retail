using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Buildings;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Store_Worker : Worker
    {
        private const double PROGRESSION = 0.7;

        public Store_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, Store building) 
            : base(name, birthdate, email, adress, salary, new ExhaustionLevel(PROGRESSION), building)
        {
            building.AddWorker(this);
        }

    }
}
