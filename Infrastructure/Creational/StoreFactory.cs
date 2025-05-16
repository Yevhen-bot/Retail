using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.Models.People;
using Core.ValueObj;

namespace Infrastructure.Creational
{
    public class StoreFactory : IFactory
    {
        public IBuilding GetBuilding(double area, string name, Adress adress)
        {
            return new Store(area, name, adress);
        }

        public Manager GetManager(Name name, Age birthdate, Email email, Adress adress, Salary salary, string pw)
        {
            return new Manager(name, birthdate, email, adress, salary, pw);
        }

        public Worker GetWorker(Name name, Age birthdate, Email email, Adress adress, Salary salary, string pw)
        {
            return new Store_Worker(name, birthdate, email, adress, salary, pw);
        }
    }
}
