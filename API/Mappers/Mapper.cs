using System.Data.SqlTypes;
using API.DTOs;
using Core.ValueObj;
using Data_Access.Adapters;
using Data_Access.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace API.Mappers
{
    public class Mapper
    {
        private readonly PasswordService<Owner> _passwordHasher;
        private readonly PasswordService<Client> _passwordHasherC;
        public Mapper(PasswordService<Owner> passwordHasher, PasswordService<Client> c)
        {
            _passwordHasher = passwordHasher;
            _passwordHasherC = c;
        }

        public Owner Registration(RegisterModel dto)
        {
            Owner owner = new();
            Email email = new(dto.Email);
            Name name = new(dto.FirstName, dto.LastName);

            owner.Email = email;
            owner.Name = name;
            owner.HashedPassword = _passwordHasher.HashPassword(owner, dto.Password);

            return owner;
        }

        public List<GetBuildingModel> GetBuildingModels(List<Building> buildings)
        {
            return buildings.Select(b => new GetBuildingModel
            {
                Id = b.Id,
                Name = b.Name,
                Area = b.Area,
                Type = b.Role.RoleName,
                Adress = $"{b.Adress.Country}, {b.Adress.City}, {b.Adress.Street}, {b.Adress.HouseNumber}",
                Products = GetProductModels(b.Products),
                Workers = GetWorkerModels(b.Workers),
                Clients = GetClientsModels(b.Clients)
            }).ToList();
        }

        public List<GetClientModel> GetClientsModels(List<Client> clients)
        {
            return clients.Select(c => new GetClientModel
            {
                Id = c.Id,
                Money = c.Money,
                Name = c.Name,
                Preferences = GetProductModels(c.Preferences),
                Age = c.Age,
                Email = c.Email,
                Salary = c.Salary
            }).ToList();
        }

        public List<GetWorkerModel> GetWorkerModels(List<Worker> workers)
        {
            return workers.Select(w => new GetWorkerModel
            {
                Name = w.Name,
                Salary = w.Salary,
                Age = w.Age,
                Email = w.Email,
                ExaustionLevel = w.ExaustionLevel,
                HomeAdress = w.HomeAdress,
                Id = w.Id
            }).ToList();
        }

        public List<GetProductModel> GetProductModels(List<ProductWrapper> products)
        {
            return products.Select(p => new GetProductModel
            {
                MPU = p.MPU,
                Price = p.Price,
                Quantity = p.Quantity,
                Name = p.Name
            }).ToList();
        }

        public List<object> CreateBuilding(CreateBuildingModel m)
        {
            Adress ad = new Adress(m.HouseNumber, m.Street, m.City, m.Country);
            BuildingRole role = new BuildingRole(m.Type);
            return [m.Name, m.Area, ad, role];
        }

        public List<object> GetWorker(CreateWorkerModel m)
        {
            Adress ad = new Adress(m.HouseNumber, m.Street, m.City, m.Country);
            Name name = new Name(m.RegisterModel.FirstName, m.RegisterModel.LastName);
            Email email = new Email(m.RegisterModel.Email);
            Age age = new Age(m.Datebirth);
            Salary salary = new Salary(m.Salary);
            string password = m.RegisterModel.Password;

            return [name, age, email, ad, salary, password, m.BuildingId];
        }

        public List<object> GetPairGoodQuant(BringGoodsModel m)
        {
            var p = new Product(m.ProductName, m.Price, m.MPU);

            return [p, m.Quantity, m.BuildingId];
        }

        public List<object> ExportImport(ExportImportModel m)
        {
            var p = new Product(m.ProductName, m.Price, m.MPU);

            return [p, m.Quantity, m.BuildingFromId, m.BuildingToId];
        }

        public Client RegisterClient(ClientRegistration m)
        {
            Client owner = new();
            Email email = new(m.Register.Email);
            Name name = new(m.Register.FirstName, m.Register.LastName);
            Salary salary = new(m.Salary);
            Age age = new(m.BirthDate);

            owner.Email = email;
            owner.Name = name;
            owner.Age = age;
            owner.Salary = salary;
            owner.Money = m.Money;
            owner.HashedPassword = _passwordHasherC.HashPassword(owner, m.Register.Password);

            return owner;
        }

        public List<object> Buy(SellModel m)
        {
            return [new Product(m.ProductName, m.Price, m.MPU), m.ProductCount, m.StoreId];
        }
    }
}
