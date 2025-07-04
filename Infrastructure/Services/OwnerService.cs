﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Data_Access.Entities;
using Data_Access.Repos;
using Infrastructure.Auth;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class OwnerService
    {
        private readonly PasswordService<Owner> _passwordService;
        private readonly IUserRepository<Owner> _ownerRepository;
        private readonly IRepository<Building> _buildingrepository;
        private readonly BuildingMapper _buildingMapper;
        private readonly JwtProvider _jwtProvider;
        private readonly HttpContext _context;

        public OwnerService(IUserRepository<Owner> rep, PasswordService<Owner> ps, JwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor, BuildingMapper buildingMapper, IRepository<Building> buildingrepository)
        {
            _ownerRepository = rep;
            _passwordService = ps;
            _context = httpContextAccessor.HttpContext!;
            _jwtProvider = jwtProvider;
            _buildingMapper = buildingMapper;
            _buildingrepository = buildingrepository;
        }

        public async Task Register(Owner owner)
        {
            await _ownerRepository.Add(owner);
        }

        public async Task Login(string password, string email)
        {
            var user = await _ownerRepository.GetByEmail(email);
            ArgumentNullException.ThrowIfNull(user, "User with such email not found");

            if(_passwordService.VerifyPassword(user, password, user.HashedPassword)) {
                var token = _jwtProvider.GenerateToken(user);
                _context.Response.Cookies.Append("cookie", token);
            } else throw new ArgumentException("Invalid Password");
        }

        public async Task SimulateDay(int ownerid)
        {
            var owner = await _ownerRepository.GetByIdWithTrack(ownerid);
            var buildings = owner.Buildings.Select(b => (_buildingMapper.MapFromDb(b), b.Id));

            foreach(var b in buildings)
            {
                b.Item1.Item1.SimulateDay();
                var tosave = _buildingMapper.MapToDb(b.Item1.Item1, b.Item1.Item2, b.Item1.Item3, owner);
                tosave.Id = b.Id;
                await _buildingrepository.Update(tosave);
            }
        }
    }
}
