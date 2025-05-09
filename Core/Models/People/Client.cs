using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.ValueObj;

namespace Core.Models.People
{
    public class Client : IPerson
    {
        public Name Name { get; set; }
        public int Age { get; set; }
        public Adress HomeAdress { get; set; }
        public decimal Money { get; set; }
        // List of preferences

        public void Sleep()
        {
            throw new NotImplementedException();
        }

        public void Work()
        {
            throw new NotImplementedException();
        }
    }
}
