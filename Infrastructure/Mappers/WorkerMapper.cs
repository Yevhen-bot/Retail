using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.Models.People;
using Core.ValueObj;
using Data_Access.Entities;

namespace Infrastructure.Mappers
{
    public class WorkerMapper
    {
        public Core.Models.People.Worker MapFromDb(Data_Access.Entities.Worker dbWorker)
        {
            var name = dbWorker.Name;
            var age = dbWorker.Age;
            var email = dbWorker.Email;
            var adress = dbWorker.HomeAdress;
            var salary = dbWorker.Salary;
            var exhaustionLevel = dbWorker.ExaustionLevel;

            switch (dbWorker.Role.RoleName)
            {
                case nameof(Manager):
                    return new Core.Models.People.Manager(
                        name,
                        age,
                        email,
                        adress,
                        salary,
                        exhaustionLevel);

                case nameof(Store_Worker):
                    return new Core.Models.People.Store_Worker(
                            name,
                            age,
                            email,
                            adress,
                            salary,
                            exhaustionLevel);

                case nameof(Warehouse_Worker):
                    return new Core.Models.People.Warehouse_Worker(
                            name,
                            age,
                            email,
                            adress,
                            salary,
                            exhaustionLevel);

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported worker role: {dbWorker.Role}");
            }
        }

        public Data_Access.Entities.Worker MapToDb(Core.Models.People.Worker coreWorker)
        {
            if (coreWorker == null)
                throw new ArgumentNullException(nameof(coreWorker));

            var dbWorker = new Data_Access.Entities.Worker();

            dbWorker.Name = coreWorker.Name;
            dbWorker.Age = coreWorker.BirthDate;
            dbWorker.Email = coreWorker.Email;
            dbWorker.HomeAdress = coreWorker.Adress;
            dbWorker.Salary = coreWorker.Salary;
            dbWorker.ExaustionLevel = coreWorker.ExhaustionLevel;

            if (coreWorker is Manager)
            {
                dbWorker.Role = new Role { RoleName = nameof(Manager) };
            }
            else if (coreWorker is Store_Worker)
            {
                dbWorker.Role = new Role { RoleName = nameof(Store_Worker) };
            }
            else if (coreWorker is Warehouse_Worker)
            {
                dbWorker.Role = new Role { RoleName = nameof(Warehouse_Worker) };
            }
            else
            {
                throw new ArgumentException($"Unknown worker type: {coreWorker.GetType().Name}");
            }

            return dbWorker;
        }

    }

}
