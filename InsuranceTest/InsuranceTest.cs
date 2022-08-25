using NUnit.Framework;
using IfInsurance;
using FluentAssertions;
using System.Collections.Generic;
using System;
using System.Linq;

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
            _availablePolicies = new List<Policy>();
            _insuranceCompany = new InsuranceCompany("If", _availableRisks, _availablePolicies);

           
            //_availablePolicies = new List<Policy>
            //{
            //    new Policy("Car", new DateTime(2022,9,1), new DateTime(2023,1,1), 400, _availableRisks)
            //};
        }

        [Test]
        public void Company_CompanyName_Should_Be_If()
        {
            _insuranceCompany.Name.Should().Be("If");
        }

        [Test]
        public void Company_CompanyName_Should_NotBe_EmptyOrWhitespace()
        {
            _insuranceCompany.Name.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void Company_Add_AvailableRisk()
        {
            _availableRisks.Add(new Risk("Injuries", 150));
            _insuranceCompany.AvailableRisks.Should().BeEquivalentTo(_availableRisks);
        }

        [Test]
        public void Company_SellPolicy_PolicyAddedToPolicyList()
        {
            var risks = _availableRisks.Take(2).ToList();
            _insuranceCompany.SellPolicy("House", new DateTime(2022, 10, 1), 12, risks);
            _availablePolicies.Count.Should().Be(1);
            _availablePolicies.First().InsuredRisks.Should().BeEquivalentTo(risks);
        }

    }
}