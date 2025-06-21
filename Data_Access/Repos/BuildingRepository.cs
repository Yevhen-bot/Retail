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

        public async Task Add(Building entity)
        {
            _context.Buildings.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Building not found");
            }
        }

        public async Task<List<Building>> GetAll()
        {
            return await _context.Buildings
                .AsNoTracking()
                .Include(b => b.Products)
                .Include(b => b.Workers)
                .Include(b => b.Clients)
                .Include(b => b.Owner)
                .ToListAsync();
        }

        public async Task<Building> GetById(int id)
        {
            var building = await _context.Buildings
                .Include(b => b.Products)
                .Include(b => b.Workers)
                .Include(b => b.Clients)
                .Include(b => b.Owner)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null) throw new InvalidOperationException("Building not found");

            return building;
        }

        public async Task Update(Building entity)
        {
            var b = await _context.Buildings.Include(e => e.Clients).Include(e => e.Workers).FirstAsync(e => e.Id == entity.Id);

            if(b == null) throw new InvalidOperationException("Building not found");

            b.Name = entity.Name;
            b.Area = entity.Area;
            b.Adress = entity.Adress;
            b.Role = entity.Role;
            b.Workers = entity.Workers;
            b.Products = entity.Products;
            await _context.SaveChangesAsync();
        }
    }
}
