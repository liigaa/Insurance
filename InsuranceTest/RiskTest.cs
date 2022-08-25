using IfInsurance;
using NUnit.Framework;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceTest
{
    public class RiskTest
    {
        private Risk _risk;
        [SetUp]
        public void Setup()
        {
            _risk = new Risk("Home", 145);            
        }

        [Test]
        public void Risk_RiskNameShouldBeHome()
        {
            _risk.Name.Should().Be("Home");
        }
        [Test]
        public void Risk_RiskName_Shoul_NotBe_Car()
        {
            _risk.Name.Should().NotBe("Car");
        }

        [Test]
        public void Risk_RiskYearlyPriceShouldBe_145()
        {
            _risk.YearlyPrice.Should().Be(145);
        }
    }
}
