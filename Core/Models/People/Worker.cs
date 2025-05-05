using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Core.Models.People
{
    public abstract class Worker : IPerson
    {
        public Name Name { get; set; }
        public int Age { get; set; }
        public Adress HomeAdress { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Phone]
        public string Phone { get; set; } = null!;
        public decimal Salary { get; set; }
        [Range(0, 100)]
        public int ExaustionLevel { get; set; } = 0;

        public abstract void Sleep();
        public abstract void Work();
        public abstract void RaiseSalary(decimal amount);
    }
}
