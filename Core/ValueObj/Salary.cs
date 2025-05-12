using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct Salary
    {
        public decimal Amount { get; init; }

        public Salary() { }
        public Salary(decimal amount)
        {
            Amount = amount;
            try
            {
                Validate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Salary", ex);
            }
        }
        private readonly void Validate()
        {
            if (Amount < 0)
            {
                throw new ArgumentException("Invalid Salary");
            }
        }

        public void Raise(decimal am)
        {
            if(am <= 0)
            {
                throw new ArgumentException("Invalid Salary raise");
            }
            this = this with { Amount = Amount + am };
        }

        public void DeRaise(decimal am)
        {
            if (am >= 0)
            {
                throw new ArgumentException("Invalid Salary deraise");
            }
            this = this with { Amount = Amount - am };
        }

        public decimal GetPayForDay()
        {
            return Amount / 7;
        }
    }
}
