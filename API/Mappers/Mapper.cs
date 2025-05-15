using API.DTOs;
using Core.ValueObj;
using Data_Access.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace API.Mappers
{
    public class Mapper
    {
        private readonly PasswordService<Owner> _passwordHasher;
        public Mapper(PasswordService<Owner> passwordHasher)
        {
            _passwordHasher = passwordHasher;
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
                Products = b.Products,
                Workers = b.Workers,
                Clients = b.Clients
            }).ToList();
        }

        public List<object> CreateBuilding(CreateBuildingModel m)
        {
            Adress ad = new Adress(m.HouseNumber, m.Street, m.City, m.Country);
            BuildingRole role = new BuildingRole(m.Type);
            return [m.Name, m.Area, ad, role];
        }
    }
}
