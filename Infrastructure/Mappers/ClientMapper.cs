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
        public Core.Models.People.Client MapFromDb(Data_Access.Entities.Client dbc)
        {
            return new Core.Models.People.Client(
                dbc.Name,
                dbc.Age,
                dbc.Salary,
                [],
                dbc.Money,
                dbc.Preferences.Select(x => x.Product).ToHashSet(),
                dbc.HashedPassword);
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
                HashedPassword = client.Password
            };
        }
    }
}
