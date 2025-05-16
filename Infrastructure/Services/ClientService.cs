using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.ValueObj;
using Data_Access.Entities;
using Infrastructure.Auth;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class ClientService
    {
        private readonly IUserRepository<Client> _repository;
        private readonly IRepository<Building> _brepository;
        private readonly BuildingMapper _buildingmapper;
        private readonly ClientMapper _clientMapper;
        private readonly HttpContext _context;
        private readonly JwtProvider _jwtProvider;
        private readonly PasswordService<Client> _passwordService;

        public ClientService(IUserRepository<Client> repository,
                             BuildingMapper buildingmapper,
                             ClientMapper clientMapper,
                             IHttpContextAccessor a,
                             JwtProvider jwtProvider,
                             PasswordService<Client> passwordService,
                             IRepository<Building> brepository)
        {
            _repository = repository;
            _buildingmapper = buildingmapper;
            _clientMapper = clientMapper;
            _context = a.HttpContext;
            _jwtProvider = jwtProvider;
            _passwordService = passwordService;
            _brepository = brepository;
        }

        public void Register(Client owner)
        {
            _repository.Add(owner);
        }

        public void Login(string password, string email)
        {
            var user = _repository.GetByEmail(email);
            ArgumentNullException.ThrowIfNull(user, "User with such email not found");

            if (_passwordService.VerifyPassword(user, password, user.HashedPassword))
            {
                var token = _jwtProvider.GenerateToken(user);
                _context.Response.Cookies.Append("cookie", token);
            }
            else throw new ArgumentException("Invalid Password");
        }

        public void MakeOrder(Product product, int Quantity, int storeID)
        {
            var store = _brepository.GetById(storeID);
            var actualstore = _buildingmapper.MapFromDb(store);
            var me = _repository.GetById(int.Parse(_context.User.FindFirst("Id")?.Value));
            var actualclient = _clientMapper.MapFromDb(me);

            if (actualstore is Store st)
            {
                actualclient.Buy(new Dictionary<Core.ValueObj.Product, int>() { { product, Quantity } }, st);
            }
            else throw new InvalidOperationException("Can't buy from warehouse");

            _buildingmapper.MapToDb(actualstore, store.Owner);
        }
    }
}
