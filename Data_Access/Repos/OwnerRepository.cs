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
    public class OwnerRepository : IUserRepository<Owner>   
    {
        private readonly AppDbContext _context;
        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Owner entity)
        {
            _context.Owners.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            } else throw new ArgumentNullException("Owner not found");
        }

        public async Task<List<Owner>> GetAll()
        {
            return await _context.Owners.AsNoTracking().ToListAsync();
        }

        public async Task<Owner> GetById(int id)
        {
            var owner = await _context.Owners
                .AsNoTracking()
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Products)
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Clients)
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Workers)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public async Task<Owner> GetByIdWithTrack(int id)
        {
            var owner = await _context.Owners.Include(o => o.Buildings).ThenInclude(b => b.Workers).FirstAsync(o => o.Id == id);

            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public async Task<Owner> GetByEmail(string email)
        {
            var owner = await _context.Owners.AsNoTracking().FirstOrDefaultAsync(o => o.Email.EmailAddress == email);
            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public async Task Update(Owner entity)
        {
            var owner = await _context.Owners.FindAsync(entity.Id);

            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            owner.Name = entity.Name;
            owner.Email = entity.Email;
            owner.HashedPassword = entity.HashedPassword;
        }
    }
}
