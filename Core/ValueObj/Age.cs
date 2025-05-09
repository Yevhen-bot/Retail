using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public readonly record struct Age
    {
        public DateOnly BirhtDate { get; init; }

        public Age() { }
        public Age(DateOnly y)
        {
            BirhtDate = y;
            try
            {
                Validate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Age", ex);
            }
        }

        private readonly void Validate()
        {
            int age = GetAge();
            if (age <= 0 || age >= 100)
            {
                throw new ArgumentException("Invalid Age");
            }
        }

        public int GetAge()
        {
            TimeSpan age = DateTime.Now - BirhtDate.ToDateTime(new TimeOnly(0, 0));
            return age.Days / 365;
        }
    }
}
