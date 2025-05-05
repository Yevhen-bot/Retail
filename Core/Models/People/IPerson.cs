using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Core.Models.People
{
    public interface IPerson
    {
        public Name Name { get; set; }
        [Range(0, 120)]
        public int Age { get; set; }
        public Adress HomeAdress { get; set; }

        public void Sleep();
    }
}
