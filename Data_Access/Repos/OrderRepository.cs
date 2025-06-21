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
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order entity)
        {
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException("Order not found.");
        }
        public async Task<Order> GetById(int id)
        {
            var order = await _context.Orders.Include(o => o.Client).Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);
            if(order == null) throw new ArgumentNullException("Order not found.");

            return order;
        }
        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders.Include(o => o.Client).Include(o => o.Products).ToListAsync();
        }
        public async Task Update(Order entity)
        {
            throw new InvalidOperationException("Updation of order is forbidden");
        }
    }
}
