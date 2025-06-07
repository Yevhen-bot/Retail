using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
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
                    _repository.Add(_mapper.MapToDb(store, new List<int>(), new List<int>(), _ownerRepo.GetByIdWithTrack(int.Parse(_context.User.FindFirst("Id")?.Value))));
                    break;
                case "Warehouse":
                    var w = _warehF.GetBuilding(area, name, adress);
                    _repository.Add(_mapper.MapToDb(w, new List<int>(), new List<int>(), _ownerRepo.GetByIdWithTrack(int.Parse(_context.User.FindFirst("Id")?.Value))));
                    break;
            }
        }

        public void DeleteBuilding(int ownerid, int id)
        {
            var b = _repository.GetById(id);
            if (b.Owner.Id != ownerid) throw new InvalidOperationException("You don`t have building with such id");
            _repository.Delete(id);
        }

        public void AddGoods(Product p, int q, int buildingId)
        {
            var b = _repository.GetById(buildingId);
            var (trueb, indx, cindx) = _mapper.MapFromDb(b);
            if (trueb is Warehouse warehouse)
            {
                warehouse.AddProduct(p, q);
                var newb = _mapper.MapToDb(warehouse, indx, cindx, b.Owner);
                newb.Id = b.Id;
                _repository.Update(newb);
            }
            else throw new InvalidOperationException("Can`t get goods on store");
        }

        public void Export(Product product, int q, int fromid, int toid)
        {
            var w = _repository.GetById(fromid);
            var s = _repository.GetById(toid);
            var (warehouse, indx1, c) = _mapper.MapFromDb(w);
            var (store, indx2, cindx) = _mapper.MapFromDb(s);

            if (warehouse is Warehouse wh && store is Store st)
            {
                wh.Export(new Dictionary<Product, int>() { { product, q } }, st);
            }
            else throw new InvalidOperationException("Not warehouse and store chosen");

            var neww = _mapper.MapToDb(wh, indx1, c, w.Owner);
            var news = _mapper.MapToDb(st, indx2, cindx, w.Owner);

            neww.Id = w.Id;
            news.Id = s.Id;

            _repository.Update(neww);
            _repository.Update(news);
        }

        public void Import(Product product, int q, int fromid, int toid)
        {
            var w = _repository.GetById(fromid);
            var s = _repository.GetById(toid);
            var (warehouse, indx1, cindx1) = _mapper.MapFromDb(w);
            var (store, indx2, cindx2) = _mapper.MapFromDb(s);

            if (warehouse is Warehouse wh && store is Store st)
            {
                st.Import(new Dictionary<Product, int>() { { product, q } }, wh);
            }
            else throw new InvalidOperationException("Not warehouse and store chosen");

            var neww = _mapper.MapToDb(wh, indx1, cindx1, w.Owner);
            var news = _mapper.MapToDb(st, indx1, cindx2, w.Owner);

            neww.Id = w.Id;
            news.Id = s.Id;

            _repository.Update(neww);
            _repository.Update(news);
        }
    }
}
