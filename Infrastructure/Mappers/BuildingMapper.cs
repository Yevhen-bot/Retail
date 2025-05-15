using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.Models.People;
using Core.ValueObj;
using Data_Access.Entities;

namespace Infrastructure.Mappers
{
    public class BuildingMapper
    {
        private readonly WorkerMapper _workerMapper;
        private readonly ClientMapper _clientMapper;

        public BuildingMapper(WorkerMapper wp, ClientMapper cp)
        {
            _workerMapper = wp;
            _clientMapper = cp;
        }

        public IBuilding MapFromDb(Building b)
        {
            var workers = b.Workers.Select(w => _workerMapper.MapFromDb(w)).ToList();
            var manager = workers.FirstOrDefault(w => w is Manager) as Manager;
            workers.Remove(manager ?? throw new ArgumentNullException("No manager in this building"));

            IBuilding v = b.Role.RoleName switch
            {
                nameof(Store) => new Store(b.Name, b.Adress, b.Area, b.Products.ToDictionary(p => p.Product, p => p.Quantity), b.Clients.Select(c => _clientMapper.MapFromDb(c)).ToList()),
                nameof(Warehouse) => new Warehouse(b.Name, b.Adress, b.Area, b.Products.ToDictionary(p => p.Product, p => p.Quantity)),
                _ => throw new ArgumentException("Unknown building type")
            };

            v.AddManager(manager);
            foreach (var worker in workers)
            {
                v.AddWorker(worker);
            }

            return v;
        }

        public Building MapToDb(IBuilding b, Owner owner)
        {
            var workers = b.Workers.Select(w => _workerMapper.MapToDb(w)).ToList();
            if(b.Manager!=null)
                workers.Add(_workerMapper.MapToDb(b.Manager));

            var role = b switch
            {
                Store => new BuildingRole(nameof(Store)),
                Warehouse => new BuildingRole(nameof(Warehouse)),
                _ => throw new ArgumentException("Unknown building type")
            };

            var newb =  new Building()
            {
                Name = b.Name,
                Area = b.Area,
                Adress = b.Adress,
                Role = role,
                Workers = workers,
            };

            if(b is Store store)
            {
                newb.Clients = store.Clients.Select(c => _clientMapper.MapToDb(c)).ToList();
            }
            newb.Owner = owner;

            return newb;
        }
    }
}
