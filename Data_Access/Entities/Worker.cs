using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Buildings;
using Core.Models.People;
using Core.ValueObj;
using Org.BouncyCastle.Asn1.X509;

namespace Data_Access.Entities
{
    public class Worker : IUser
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Age Age { get; set; }
        public Email Email { get; set; }
        public Adress HomeAdress { get; set; }
        public Salary Salary { get; set; }
        public ExhaustionLevel ExaustionLevel { get; set; }
        public Role Role { get; set; }
        public string HashedPassword { get; set; } = null!;
        public Building Building { get; set; }
    }
}
