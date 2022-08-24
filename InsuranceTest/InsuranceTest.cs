using NUnit.Framework;
using IfInsurance;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace InsuranceTest
{
    public class InsuranceTests
    {
        private IInsuranceCompany _insuranceCompany;
        private IList<Risk> _availableRisks;
        private IList<Policy> _availablePolicies;
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

           
            _availablePolicies = new List<Policy>
            {
                new Policy("Car", new DateTime(2022,9,1), new DateTime(2023,1,1), 400, _availableRisks)
            };
        }

        [Test]
        public void CompanyAddAvailableRisk()
        {
            _availableRisks.Add(new Risk("Injuries", 150));
            _insuranceCompany.AvailableRisks.Should().BeEquivalentTo(_availableRisks);
        }

        [Test]
        public void CompanyAddPolicy()
        {
            _availablePolicies.Add(new Policy("House", new DateTime(2022, 10, 1), new DateTime(2023, 1, 1), 600, _availableRisks));
           // _insuranceCompany
        }

    }
}