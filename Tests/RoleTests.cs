using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Tests
{
    public class RoleTests
    {
        [Theory]
        [InlineData("Owner")]
        [InlineData("Manager")]
        [InlineData("Store_Worker")]
        [InlineData("Warehouse_Worker")]
        [InlineData("Client")]
        public void Constructor_WithValidRole_ShouldCreateInstance(string validRole)
        {
            var role = new Role(validRole);

            Assert.Equal(validRole, role.RoleName);
        }

        [Theory]
        [InlineData("owner")]
        [InlineData("admin")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithInvalidRole_ShouldThrow(string invalidRole)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Role(invalidRole));
            Assert.Equal("Invalid role", ex.Message);
        }

        [Fact]
        public void DefaultConstructor_ShouldHaveNullRoleName()
        {
            var role = new Role();

            Assert.Null(role.RoleName);
        }
    }

}
