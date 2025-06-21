using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.People;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using Worker = Data_Access.Entities.Worker;

namespace Data_Access.Repos
{
    public class WorkerRepository : IUserRepository<Worker>
    {
        private readonly AppDbContext _context;
        public WorkerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Worker entity)
        {
            _context.Workers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Worker not found");
        }

        public async Task<Worker> GetByIdWithTrack(int id)
        {
            var owner = await _context.Workers.Include(e => e.Building).ThenInclude(e => e.Workers).FirstOrDefaultAsync(e => e.Id == id);

            if (owner == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return owner;
        }

        public async Task<List<Worker>> GetAll()
        {
            return await _context.Workers.AsNoTracking().ToListAsync();
        }

        public async Task<Worker> GetById(int id)
        {
            var worker = await _context.Workers.Include(e => e.Building).ThenInclude(e => e.Workers).FirstOrDefaultAsync(o => o.Id == id);
            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return worker;
        }

        public async Task<Worker> GetByEmail(string email)
        {
            var worker = await _context.Workers.AsNoTracking().FirstOrDefaultAsync(o => o.Email.EmailAddress == email);
            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return worker;
        }

        public async Task Update(Worker entity)
        {
            var worker = await _context.Workers.AsNoTracking().Include(w => w.Building).FirstOrDefaultAsync(w => w.Id == entity.Id);

            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            var b = worker.Building;
            _context.Workers.Update(entity);
            entity.Building = b;
            await _context.SaveChangesAsync();
        }
    }
}
