using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct Email
    {
        private const string EMAILREGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public string EmailAddress { get; init; }
        public Email(string email)
        {
            EmailAddress = email;
            try
            {
                Validate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Email", ex);
            }
        }
        public Email() { }
        private void Validate()
        {
            if (!Regex.IsMatch(EmailAddress, EMAILREGEX))
            {
                throw new ArgumentException("Invalid Email");
            }
        }
    }
}
