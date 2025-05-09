using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct BuildingRole
    {
        private static readonly string Store = "Store";
        private static readonly string Warehouse = "Warehouse";
        private static readonly List<string> roles = [Store, Warehouse];

        public string RoleName { get; init; }
        public BuildingRole(string role)
        {
            if (!roles.Contains(role))
                throw new ArgumentException("Invalid role");

            RoleName = role;
        }
        public BuildingRole() { }
    }
}
