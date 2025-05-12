using Core.Interfaces;
using Core.Models.People;
using Core.ValueObj;

namespace Infrastructure.Creational
{
    public interface IFactory
    {
        IBuilding GetBuilding(double area, string name, Adress adress);
        Worker GetWorker(Name name, Age birthdate, Email email, Adress adress, Salary salary, IBuilding building);
        Manager GetManager(Name name, Age birthdate, Email email, Adress adress, Salary salary, IBuilding building);
    }
}
