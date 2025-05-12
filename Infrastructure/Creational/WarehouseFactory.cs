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
    public class WarehouseFactory : IFactory
    {
        public IBuilding GetBuilding(double area, string name, Adress adress)
        {
            return new Warehouse(area, name, adress);
        }

        public Manager GetManager(Name name, Age birthdate, Email email, Adress adress, Salary salary, IBuilding building)
        {
            return new Manager(name, birthdate, email, adress, salary, building);
        }

        public Worker GetWorker(Name name, Age birthdate, Email email, Adress adress, Salary salary, IBuilding building)
        {
            if (building is not Warehouse)
            {
                throw new ArgumentException("Building must be a Warehouse");
            }
            return new Warehouse_Worker(name, birthdate, email, adress, salary, (Warehouse)building);
        }
    }
}
