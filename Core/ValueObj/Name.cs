using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public readonly record struct Name(
        string FirstName,
        string LastName
        );
}
