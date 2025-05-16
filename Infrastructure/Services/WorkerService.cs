using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.ValueObj;
using Data_Access.Entities;
using Data_Access.Repos;
using Infrastructure.Auth;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class WorkerService
    {
        private readonly IUserRepository<Worker> _userRepo;
        private readonly IUserRepository<Owner> _ownerRepo;
        private readonly IRepository<Building> _buildingRepo;
        private readonly PasswordService<Worker> _passwordService;
        private readonly IFactory _storeF;
        private readonly IFactory _warehF;
        private readonly BuildingMapper _buildingmapper;
        private readonly HttpContext _context;
        private readonly JwtProvider _jwtProvider;

        public WorkerService(
            IUserRepository<Worker> ownerRepo,
            StoreFactory storeF,
            WarehouseFactory warehF,
            BuildingMapper buildingmapper,
            IRepository<Building> buildingRepo,
            IUserRepository<Owner> ownr,
            PasswordService<Worker> passwordService,
            IHttpContextAccessor context,
            JwtProvider jwtProvider)
        {
            _userRepo = ownerRepo ?? throw new ArgumentNullException(nameof(ownerRepo));
            _storeF = storeF ?? throw new ArgumentNullException(nameof(storeF));
            _warehF = warehF ?? throw new ArgumentNullException(nameof(warehF));
            _buildingmapper = buildingmapper ?? throw new ArgumentNullException(nameof(buildingmapper));
            _context = context.HttpContext ?? throw new ArgumentNullException(nameof(context));
            _buildingRepo = buildingRepo ?? throw new ArgumentNullException(nameof(buildingRepo));
            _ownerRepo = ownr ?? throw new ArgumentNullException(nameof(ownr));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _jwtProvider = jwtProvider;
        }

        public void AddManager(Name name, Age birthdate, Email email, Adress adress, Salary salary, string password, int buildingId)
        {
            var building = _buildingRepo.GetById(buildingId);
            var actualbuilding = _buildingmapper.MapFromDb(building);
            var manager = _storeF.GetManager(name, birthdate, email, adress, salary, _passwordService.HashPassword(null, password));
            actualbuilding.AddManager(manager);
            var b = _buildingmapper.MapToDb(actualbuilding, _ownerRepo.GetByIdWithTrack(int.Parse(_context.User.FindFirst("id")?.Value)));    
            b.Id = buildingId;
            _buildingRepo.Update(b);
        }

        public void AddWorker(Name name, Age birthdate, Email email, Adress adress, Salary salary, string password, int buildingId)
        {
            var building = _buildingRepo.GetById(buildingId);
            var actualbuilding = _buildingmapper.MapFromDb(building);
            Core.Models.People.Worker worker;
            if (actualbuilding is Store) worker = _storeF.GetWorker(name, birthdate, email, adress, salary, _passwordService.HashPassword(null, password));
            else worker = _warehF.GetWorker(name, birthdate, email, adress, salary, _passwordService.HashPassword(null, password));

            actualbuilding.AddWorker(worker);
            var b = _buildingmapper.MapToDb(actualbuilding, building.Owner);
            b.Id = buildingId;
            _buildingRepo.Update(b);
        }

        public void Login(string password, string email)
        {
            var user = _userRepo.GetByEmail(email);
            ArgumentNullException.ThrowIfNull(user, "User with such email not found");

            if (_passwordService.VerifyPassword(user, password, user.HashedPassword))
            {
                var token = _jwtProvider.GenerateToken(user);
                _context.Response.Cookies.Append("cookie", token);
            }
            else throw new ArgumentException("Invalid Password");
        }
    }
}
