using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.Models.People;
using Core.ValueObj;
using Data_Access.Adapters;
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

        public (IBuilding, List<int>, List<int>) MapFromDb(Building b)
        {
            List<int> ids = [];
            List<int> cids = [];
            int managerid = 0;
            var workers = b.Workers.Select(w => {
                var r = _workerMapper.MapFromDb(w);
                if(r is Manager) managerid = w.Id;
                else ids.Add(w.Id);
                return r;
            }).ToList();
            ids = ids.Prepend(managerid).ToList();
            var manager = workers.FirstOrDefault(w => w is Manager) as Manager;
            if (manager != null)
                workers.Remove(manager);

            IBuilding v = b.Role.RoleName switch
            {
                nameof(Store) => new Store(b.Name, b.Adress, b.Area, b.Products.ToDictionary(p => p.Product, p => p.Quantity), b.Clients.Select(c =>
                {
                    cids.Add(c.Id);
                    return _clientMapper.MapFromDb(c);
                }).ToList()),
                nameof(Warehouse) => new Warehouse(b.Name, b.Adress, b.Area, b.Products.ToDictionary(p => p.Product, p => p.Quantity)),
                _ => throw new ArgumentException("Unknown building type")
            };

            if(manager != null)
                v.AddManager(manager);
            foreach (var worker in workers)
            {
                v.AddWorker(worker);
            }

            return (v, ids, cids);
        }

        public Building MapToDb(IBuilding b, List<int> ids, List<int> cids, Owner owner)
        {
            int counter = 1;
            var workers = b.Workers.Select(w =>
            {
                var v = _workerMapper.MapToDb(w);
                if(counter < ids.Count) v.Id = ids[counter++];
                return v;
            }).ToList();
            if (b.Manager != null)
            {
                var v = _workerMapper.MapToDb(b.Manager);
                v.Id = ids[0];
                workers.Add(v);
            }

            var role = b switch
            {
                Store => new BuildingRole(nameof(Store)),
                Warehouse => new BuildingRole(nameof(Warehouse)),
                _ => throw new ArgumentException("Unknown building type")
            };

            var newb = new Building()
            {
                Name = b.Name,
                Area = b.Area,
                Adress = b.Adress,
                Role = role,
                Workers = workers,
                Products = b.Products.Select(p => {
                    var pr = new ProductWrapper();
                    pr.Product = p.Key;
                    pr.Quantity = p.Value;
                    return pr;
                }).ToList()
            };

            if(b is Store store)
            {
                counter = 0;
                newb.Clients = store.Clients.Select(c =>
                {
                    var x = _clientMapper.MapToDb(c);
                    if(counter < cids.Count)
                        x.Id = cids[counter++];
                    return x;
                }).ToList();
            }
            newb.Owner = owner;

            return newb;
        }
    }
}
