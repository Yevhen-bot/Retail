using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;
using Data_Access.Entities;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class BuildingService
    {
        private readonly IRepository<Building> _repository;
        private readonly IUserRepository<Owner> _ownerRepo;
        private readonly IFactory _storeF, _warehF;
        private readonly BuildingMapper _mapper;
        private readonly HttpContext _context;

        public BuildingService(IRepository<Building> r, StoreFactory s, WarehouseFactory w, BuildingMapper mapper, IUserRepository<Owner> wner, IHttpContextAccessor a)
        {
            _repository = r;
            _storeF = s;
            _warehF = w;
            _mapper = mapper;
            _ownerRepo = wner;
            _context = a.HttpContext;
        }

        public Building GetBuilding(int ownerid, int id)
        {
            var b = _repository.GetById(id);
            if (b.Owner.Id != ownerid) throw new InvalidOperationException("You don`t have building with such id");

            return b;
        }

        public List<Building> GetBuildings(int ownerid)
        {
            var b = _repository.GetAll().AsQueryable().Where(b => b.Owner.Id == ownerid).ToList();
            return b;
        }

        public void CreateBuilding(string name, double area, Adress adress, BuildingRole role)
        {
            switch(role.RoleName)
            {
                case "Store":
                    var store = _storeF.GetBuilding(area, name, adress);
                    _repository.Add(_mapper.MapToDb(store, _ownerRepo.GetByIdWithTrack(int.Parse(_context.User.FindFirst("Id")?.Value))));
                    break;
                case "Warehouse":
                    var w = _warehF.GetBuilding(area, name, adress);
                    _repository.Add(_mapper.MapToDb(w, _ownerRepo.GetByIdWithTrack(int.Parse(_context.User.FindFirst("Id")?.Value))));
                    break;
            }
        }

        public void DeleteBuilding(int ownerid, int id)
        {
            var b = _repository.GetById(id);
            if (b.Owner.Id != ownerid) throw new InvalidOperationException("You don`t have building with such id");
            _repository.Delete(id);
        }
    }
}
