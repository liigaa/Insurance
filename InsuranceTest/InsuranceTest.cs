using NUnit.Framework;
using IfInsurance;
using FluentAssertions;
using System.Collections.Generic;

namespace InsuranceTest
{
    public class InsuranceTests
    {
        private IInsuranceCompany _insuranceCompany;
        private IList<Risk> _availableRisks;
        [SetUp]
        public void Setup()
        {
            _availableRisks = new List<Risk>
            {
                new Risk("Octa", 154),
                new Risk("Casko", 300),
                new Risk("Home", 500)
            };
            _insuranceCompany = new InsuranceCompany("If", _availableRisks);
        }

        [Test]
        public void CompanyAddAvailableRisks()
        {
            _availableRisks.Add(new Risk("Injuries", 150));
            _insuranceCompany.AvailableRisks.Should().BeEquivalentTo(_availableRisks);
        }
    }
}