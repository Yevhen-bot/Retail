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

        public void Add(Worker entity)
        {
            _context.Workers.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var worker = _context.Workers.Find(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
                _context.SaveChanges();
            }
            else throw new ArgumentNullException("Worker not found");
        }

        public Worker GetByIdWithTrack(int id)
        {
            var owner = _context.Workers.Include(e => e.Building).ThenInclude(e => e.Workers).FirstOrDefault(e => e.Id == id);

            if (owner == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return owner;
        }

        public List<Worker> GetAll()
        {
            return _context.Workers.AsNoTracking().ToList();
        }

        public Worker GetById(int id)
        {
            var worker = _context.Workers.Include(e => e.Building).ThenInclude(e => e.Workers).FirstOrDefault(o => o.Id == id);
            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return worker;
        }

        public Worker GetByEmail(string email)
        {
            var worker = _context.Workers.AsNoTracking().FirstOrDefault(o => o.Email.EmailAddress == email);
            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            return worker;
        }

        public void Update(Worker entity)
        {
            var worker = _context.Workers.AsNoTracking().Include(w => w.Building).FirstOrDefault(w => w.Id == entity.Id);

            if (worker == null)
            {
                throw new ArgumentNullException("Worker not found");
            }

            var b = worker.Building;
            _context.Workers.Update(entity);
            entity.Building = b;
            _context.SaveChanges();
        }
    }
}
