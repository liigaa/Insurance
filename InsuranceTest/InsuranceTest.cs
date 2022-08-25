using NUnit.Framework;
using IfInsurance;
using FluentAssertions;
using System.Collections.Generic;
using System;
using System.Linq;
using IfInsurance.Exceptions;

namespace InsuranceTest
{
    
    public class InsuranceTests
    {
        private IInsuranceCompany _insuranceCompany;
        private IList<Risk> _availableRisks;
        private IList<Policy> _soldPolicies;       
        
        [SetUp]
        public void Setup()
        {
            _availableRisks = new List<Risk>
            {
                new Risk("Octa", 154),
                new Risk("Casko", 300),
                new Risk("Home", 500)
            };
            _soldPolicies = new List<Policy>();            
            _insuranceCompany = new InsuranceCompany("If", _availableRisks, _soldPolicies);

           
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
            _soldPolicies.Count.Should().Be(1);
            _soldPolicies.First().InsuredRisks.Should().BeEquivalentTo(risks);
        }

        [Test]
        public void Company_Dont_Sell_Policy_In_Past()
        {
            var risks = _availableRisks.Take(2).ToList();           
                _insuranceCompany.Invoking(x => x.SellPolicy("House", new DateTime(2022, 8, 1), 12, risks))
                .Should()
                .Throw<DateException>()
                .WithMessage("*past");                       
        }

        [Test]
        public void Company_Dont_Sell_Policy_With_Same_Efective_Date()
        {
            var risks = _availableRisks.Take(2).ToList();
            var date = new DateTime(2022, 10, 1);
            var policyObject = "House";
            _soldPolicies.Add(new Policy("House", new DateTime(2022, 10, 1), new DateTime(2023, 10, 31), 450, _availableRisks));
            _insuranceCompany.Invoking(x => x.SellPolicy(policyObject, date, 12, risks))
                    .Should()
                    .Throw<DateException>()
                    .WithMessage("Cannot be policy with same efective date");           
        }

        [Test]
        public void Company_Find_Policy_By_ObjectName_And_ValidFrom_Date()
        {           
            var date = new DateTime(2022, 11, 1);
            var policyObject = "Mazda";
            _soldPolicies.Add(new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 450, _availableRisks));
            _insuranceCompany.GetPolicy(policyObject, date).Should().NotBeNull();
        }

        [Test]
        public void Company_Find_Policy_By_ObjectName_And_ValidFrom_Date_Throw_Exeption()
        {
            var date = new DateTime(2022, 11, 1);
            var policyObject = "Mazda";

            _insuranceCompany.Invoking(x => x.GetPolicy(policyObject, date))
                .Should()
                .Throw<PolicyNotFoundException>()
                .Where(t => t.Message.Contains("not"));
        }

        [Test]
        public void Add_NewRisk_To_Policy()
        {
            var newRisk = new Risk("Fire", 150);
            var date = new DateTime(2022, 11, 1);
            var policyObject = "Mazda";
            var policy = new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 450, _availableRisks.Take(1).ToList());
            _soldPolicies.Add(policy);
            _insuranceCompany.AddRisk(policyObject, newRisk, date);
            policy.InsuredRisks.Count.Should().Be(2);
        }
    }
}