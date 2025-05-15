using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PasswordService<T> where T : class
    {
        private readonly PasswordHasher<T> _passwordHasher = new();

        public string HashPassword(T user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(T user, string password, string hash)
        {
            return (int)_passwordHasher.VerifyHashedPassword(user, hash, password)==1?true:false;
        }
    }
}
