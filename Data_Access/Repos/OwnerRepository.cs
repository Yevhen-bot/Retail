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

        public void Add(Owner entity)
        {
            _context.Owners.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var owner = _context.Owners.Find(id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                _context.SaveChanges();
            } else throw new ArgumentNullException("Owner not found");
        }

        public List<Owner> GetAll()
        {
            return _context.Owners.AsNoTracking().ToList();
        }

        public Owner GetById(int id)
        {
            var owner = _context.Owners
                .AsNoTracking()
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Products)
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Clients)
                .Include(o => o.Buildings)
                .ThenInclude(b => b.Workers)
                .FirstOrDefault(o => o.Id == id);
            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public Owner GetByIdWithTrack(int id)
        {
            var owner = _context.Owners.Find(id);

            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public Owner GetByEmail(string email)
        {
            var owner = _context.Owners.AsNoTracking().FirstOrDefault(o => o.Email.EmailAddress == email);
            if (owner == null)
            {
                throw new ArgumentNullException("Owner not found");
            }

            return owner;
        }

        public void Update(Owner entity)
        {
            var owner = _context.Owners.Find(entity.Id);

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
