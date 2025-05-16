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

        public Store_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, string pw) 
            : base(name, birthdate, email, adress, salary, new ExhaustionLevel(PROGRESSION), pw)
        {
        }

        public Store_Worker(Name name, Age birthdate, Email email, Adress adress, Salary salary, ExhaustionLevel lvl, string pw)
            : base(name, birthdate, email, adress, salary, lvl, pw)
        {
        }

    }
}
