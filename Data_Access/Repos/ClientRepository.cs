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

        public async Task Add(Client entity)
        {
            _context.Clients.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Client not found");
        }

        public async Task<Client> GetByIdWithTrack(int id)
        {
            var owner = await _context.Clients.FindAsync(id);

            if (owner == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return owner;
        }

        public async Task<List<Client>> GetAll()
        {
            return await _context.Clients.AsNoTracking().ToListAsync();
        }

        public async Task<Client> GetById(int id)
        {
            var client = await _context.Clients.AsNoTracking().Include(c => c.Orders).Include(c => c.Preferences).FirstOrDefaultAsync(o => o.Id == id);
            if (client == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return client;
        }

        public async Task<Client> GetByEmail(string email)
        {
            var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(o => o.Email.EmailAddress == email);
            if (client == null)
            {
                throw new ArgumentNullException("Client not found");
            }

            return client;
        }

        public async Task Update(Client entity)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(w => w.Id == entity.Id);

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
            client.Orders = entity.Orders;
            client.Preferences = entity.Preferences;

            await _context.SaveChangesAsync();
        }
    }
}
