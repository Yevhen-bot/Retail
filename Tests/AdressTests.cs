using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ValueObj;

namespace Tests
{
    public class AdressTests
    {
        [Fact]
        public void Create_ValidAdress_DoesNotThrow()
        {
            var houseNumber = "123A";
            var street = "Main Street";
            var city = "Kyiv";
            var country = "UA";

            var address = new Adress(houseNumber, street, city, country);
            Assert.Equal(houseNumber, address.HouseNumber);
            Assert.Equal(street, address.Street);
            Assert.Equal(city, address.City);
            Assert.Equal(country, address.Country);
        }

        [Theory]
        [InlineData("123!", "Main", "Kyiv", "UA")]
        [InlineData("12", "Main", "Kyiv", "Ukraine")]
        public void Create_InvalidAdress_ThrowsException(string houseNumber, string street, string city, string country)
        {
            var ex = Assert.Throws<Exception>(() => new Adress(houseNumber, street, city, country));
            Assert.Contains("Invalid Adress", ex.InnerException?.Message ?? ex.Message);
        }

        [Fact]
        public void Distance_ReturnsValueInExpectedRange()
        {
            var a1 = new Adress("1", "Street", "City", "UA");
            var a2 = new Adress("2", "Another", "City", "PL");

            double distance = a1.Distance(a2);

            Assert.InRange(distance, 0, 100);
        }
    }
}
