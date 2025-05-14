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

        public void Add(Order entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            else throw new ArgumentNullException("Order not found.");
        }
        public Order GetById(int id)
        {
            var order = _context.Orders.Include(o => o.Client).Include(o => o.Products).FirstOrDefault(o => o.Id == id);
            if(order == null) throw new ArgumentNullException("Order not found.");

            return order;
        }
        public List<Order> GetAll()
        {
            return _context.Orders.Include(o => o.Client).Include(o => o.Products).ToList();
        }
        public void Update(Order entity)
        {
            throw new InvalidOperationException("Updation of order is forbidden");
        }
    }
}
