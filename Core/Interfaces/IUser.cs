using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Core.Interfaces
{
    public interface IUser
    {
        public int Id { get; set; }
        public Role Role { get; set; }
    }
}
