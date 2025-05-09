using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Core.Interfaces
{
    public interface IPerson
    {
        public void Work();
        public void Sleep();
    }
}
