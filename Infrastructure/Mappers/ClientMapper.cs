using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.People;
using Data_Access.Adapters;
using Data_Access.Entities;

namespace Infrastructure.Mappers
{
    public class ClientMapper
    {
        private readonly OrderMapper _orderMapper;

        public ClientMapper(OrderMapper orderMapper)
        {
            _orderMapper = orderMapper;
        }

        public Core.Models.People.Client MapFromDb(Data_Access.Entities.Client dbc)
        {
            return new Core.Models.People.Client(
                dbc.Email,
                dbc.Name,
                dbc.Age,
                dbc.Salary,
                [],
                dbc.Money,
                dbc.Preferences.Select(x => x.Product).ToHashSet(),
                dbc.HashedPassword,
                dbc.Orders.Select(x => _orderMapper.MapFromDb(x)).ToList());
        }

        public Data_Access.Entities.Client MapToDb(Core.Models.People.Client client)
        {
            return new Data_Access.Entities.Client
            {
                Name = client.Name,
                Age = client.Age,
                Salary = client.Salary,
                Email = client.Email,
                Money = client.Money,
                Preferences = client.Preferenc.Select(x => new ProductWrapper() { Product = x }).ToList(),
                HashedPassword = client.Password,
                Orders = client.Orders.Select(x => _orderMapper.MapToDb(x)).ToList()
            };
        }
    }
}
