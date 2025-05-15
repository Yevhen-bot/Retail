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
    public class ClientRepository : IUserRepository<Client>
    {
        private readonly AppDbContext _context;
        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Client entity)
        {
            _context.Clients.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
            else throw new ArgumentNullException("Client not found");
        }

        public Client GetByIdWithTrack(int id)
        {
            var owner = _context.Clients.Find(id);

            if (owner == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return owner;
        }

        public List<Client> GetAll()
        {
            return _context.Clients.AsNoTracking().ToList();
        }

        public Client GetById(int id)
        {
            var client = _context.Clients.AsNoTracking().FirstOrDefault(o => o.Id == id);
            if (client == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return client;
        }

        public Client GetByEmail(string email)
        {
            var client = _context.Clients.AsNoTracking().FirstOrDefault(o => o.Email.EmailAddress == email);
            if (client == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return client;
        }

        public void Update(Client entity)
        {
            var client = _context.Clients.FirstOrDefault(w => w.Id == entity.Id);

            if (client == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            client.Email = entity.Email;
            client.Name = entity.Name;
            client.Age = entity.Age;
            client.Salary = entity.Salary;
            client.Money = entity.Money;
            client.HashedPassword = entity.HashedPassword;
            _context.SaveChanges();
        }
    }
}
