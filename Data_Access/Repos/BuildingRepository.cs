using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repos
{
    public class BuildingRepository : IRepository<Building>
    {
        private readonly AppDbContext _context;
        public BuildingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Building entity)
        {
            _context.Buildings.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var building = _context.Buildings.Find(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Building not found");
            }
        }

        public List<Building> GetAll()
        {
            return _context.Buildings
                .AsNoTracking()
                .Include(b => b.Products)
                .Include(b => b.Workers)
                .Include(b => b.Clients)
                .Include(b => b.Owner)
                .ToList();
        }

        public Building GetById(int id)
        {
            var building = _context.Buildings
                .Include(b => b.Products)
                .Include(b => b.Workers)
                .Include(b => b.Clients)
                .Include(b => b.Owner)
                .FirstOrDefault(b => b.Id == id);

            if (building == null) throw new InvalidOperationException("Building not found");

            return building;
        }

        public void Update(Building entity)
        {
            var b = _context.Buildings.Find(entity.Id);

            if(b == null) throw new InvalidOperationException("Building not found");

            b.Name = entity.Name;
            b.Area = entity.Area;
            b.Adress = entity.Adress;
            b.Role = entity.Role;
            _context.SaveChanges();
        }
    }
}
