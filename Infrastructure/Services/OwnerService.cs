using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Data_Access.Entities;
using Data_Access.Repos;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class OwnerService
    {
        private readonly PasswordService<Owner> _passwordService;
        private readonly IUserRepository<Owner> _ownerRepository;
        private readonly JwtProvider _jwtProvider;
        private readonly HttpContext _context;

        public OwnerService(IUserRepository<Owner> rep, PasswordService<Owner> ps, JwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
        {
            _ownerRepository = rep;
            _passwordService = ps;
            _context = httpContextAccessor.HttpContext!;
            _jwtProvider = jwtProvider;
        }

        public void Register(Owner owner)
        {
            _ownerRepository.Add(owner);
        }

        public void Login(string password, string email)
        {
            var user = _ownerRepository.GetByEmail(email);
            ArgumentNullException.ThrowIfNull(user, "User with such email not found");

            if(_passwordService.VerifyPassword(user, password, user.HashedPassword)) {
                var token = _jwtProvider.GenerateToken(user);
                _context.Response.Cookies.Append("cookie", token);
            } else throw new ArgumentException("Invalid Password");
        }

        public void SimulateDay(int ownerid)
        {
            //TODO
        }
    }
}
