using Core.Interfaces;
using Core.Models.Buildings;
using Core.ValueObj;
using Data_Access;
using Data_Access.Entities;
using Infrastructure.Auth;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly AppDbContext _dbcontext;

        public ClientService(IUserRepository<Client> repository,
                             BuildingMapper buildingmapper,
                             ClientMapper clientMapper,
                             IHttpContextAccessor a,
                             JwtProvider jwtProvider,
                             PasswordService<Client> passwordService,
                             IRepository<Building> brepository,
                             AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repository = repository;
            _buildingmapper = buildingmapper;
            _clientMapper = clientMapper;
            _context = a.HttpContext;
            _jwtProvider = jwtProvider;
            _passwordService = passwordService;
            _brepository = brepository;
        }

        public async Task Register(Client owner)
        {
            await _repository.Add(owner);
        }

        public async Task Login(string password, string email)
        {
            var user = await _repository.GetByEmail(email);
            ArgumentNullException.ThrowIfNull(user, "User with such email not found");

            if (_passwordService.VerifyPassword(user, password, user.HashedPassword))
            {
                var token = _jwtProvider.GenerateToken(user);
                _context.Response.Cookies.Append("cookie", token);
            }
            else throw new ArgumentException("Invalid Password");
        }

        public async Task MakeOrder(Product product, int Quantity, int storeID)
        {
            var store = await _brepository.GetById(storeID);
            var (actualstore, indx, cindx) = _buildingmapper.MapFromDb(store);

            Client me = store.Clients.Find(c => c.Id == int.Parse(_context.User.FindFirst("Id")?.Value));
            if (me == null)
            {
                me = await _repository.GetById(int.Parse(_context.User.FindFirst("Id")?.Value));
            }

            var actualclient = _clientMapper.MapFromDb(me);
            bool isnewclient;
            if (actualstore is Store st)
            {
                isnewclient = actualclient.Buy(new Dictionary<Core.ValueObj.Product, int>() { { product, Quantity } }, st);
            }
            else throw new InvalidOperationException("Can't buy from warehouse");

            using var tr = _dbcontext.Database.BeginTransaction();
            try
            {
                var g = _clientMapper.MapToDb(actualclient);
                g.Id = me.Id;
                await _repository.Update(g);

                var id = store.Id;
                //var entryStore = _dbcontext.ChangeTracker.Entries<Building>()
                //    .FirstOrDefault(e => e.Entity.Id == id);

                //if (entryStore != null)
                //{
                //    entryStore.State = EntityState.Detached;
                //}
                store = _buildingmapper.MapToDb(actualstore, indx, cindx, store.Owner);
                store.Id = id;
                await _brepository.Update(store);

                if(isnewclient)
                {
                    _dbcontext.Buildings.Find(storeID).Clients.Add(_dbcontext.Clients.Find(int.Parse(_context.User.FindFirst("Id")?.Value)));
                    await _dbcontext.SaveChangesAsync();
                }

                tr.Commit();
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
