using Core.ValueObj;
using System;
using Xunit;

namespace Tests
{
    public class SalaryTests
    {
        [Fact]
        public void Constructor_WithValidAmount_ShouldCreateInstance()
        {
            var salary = new Salary(1000m);

            Assert.Equal(1000m, salary.Amount);
        }

        [Fact]
        public void Constructor_WithNegativeAmount_ShouldThrow()
        {
            var ex = Assert.Throws<Exception>(() => new Salary(-100));
            Assert.IsType<ArgumentException>(ex.InnerException);
            Assert.Equal("Invalid Salary", ex.InnerException.Message);
        }

        [Fact]
        public void Raise_WithPositiveAmount_ShouldIncreaseSalary()
        {
            var salary = new Salary(1000m);
            salary.Raise(200m);

            Assert.Equal(1200m, salary.Amount);
        }

        [Fact]
        public void Raise_WithZero_ShouldThrow()
        {
            var salary = new Salary(1000m);
            var ex = Assert.Throws<ArgumentException>(() => salary.Raise(0));
            Assert.Equal("Invalid Salary raise", ex.Message);
        }

        [Fact]
        public void Raise_WithNegative_ShouldThrow()
        {
            var salary = new Salary(1000m);
            var ex = Assert.Throws<ArgumentException>(() => salary.Raise(-100));
            Assert.Equal("Invalid Salary raise", ex.Message);
        }

        [Fact]
        public void DeRaise_WithPositiveAmount_ShouldDecreaseSalary()
        {
            var salary = new Salary(1000m);
            salary.DeRaise(100m);

            Assert.Equal(900m, salary.Amount);
        }

        [Fact]
        public void DeRaise_WithNegativeAmount_ShouldThrow()
        {
            var salary = new Salary(1000m);
            var ex = Assert.Throws<ArgumentException>(() => salary.DeRaise(-100));
            Assert.Equal("Invalid Salary deraise", ex.Message);
        }

        [Fact]
        public void DeRaise_MoreThanPayment_ShouldThrow()
        {
            var salary = new Salary(1000m);
            var ex = Assert.Throws<ArgumentException>(() => salary.DeRaise(2000));
            Assert.Equal("Invalid Salary", ex.Message);
        }

        [Fact]
        public void GetPayForDay_ShouldReturnCorrectValue()
        {
            var salary = new Salary(700m);

            decimal perDay = salary.GetPayForDay();

            Assert.Equal(100m, perDay);
        }
    }
}
