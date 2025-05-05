using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public readonly record struct Adress { 

        private const string HNREGEX = @"^[0-9]{1,3}[A-Z]?$";
        private const string COUNTRYREGEX = @"^[A-Z]{2,3}$";

        public string HouseNumber { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string Country { get; init; }

        public Adress(string houseNumber, string street, string city, string country)
        {
            HouseNumber = houseNumber;
            Street = street;
            City = city;
            Country = country;

            try
            {
                Validate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Adress", ex);
            }
        }

        private void Validate()
        {
            if(!Regex.IsMatch(HouseNumber, HNREGEX) || !Regex.IsMatch(Country, COUNTRYREGEX))
            {
                throw new Exception("Invalid Adress");
            }
        }
    }


}
