using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct Role
    {
        private static readonly string Owner = "Owner";
        private static readonly string Manager = "Manager";
        private static readonly string Store_Worker = "Store_Worker";
        private static readonly string Warehouse_Worker = "Warehouse_Worker";
        private static readonly List<string> roles = [Manager, Store_Worker, Warehouse_Worker, Owner];

        public string RoleName { get; init; }

        public Role() { }
        public Role(string role)
        {
            if(!roles.Contains(role))
                throw new ArgumentException("Invalid role");

            RoleName = role;
        }
    }
}
