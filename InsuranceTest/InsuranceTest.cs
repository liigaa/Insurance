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
        private IList<IPolicy> _soldPolicies;       
        
        [SetUp]
        public void Setup()
        {
            _availableRisks = new List<Risk>
            {
                new Risk("Octa", 154),
                new Risk("Casko", 300),
                new Risk("Home", 500)
            };
            _soldPolicies = new List<IPolicy>();            
            _insuranceCompany = new InsuranceCompany("If", _availableRisks, _soldPolicies);
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
        public void Company_SellPolicy_Policy_Added_To_PolicyList()
        {
            var risks = _availableRisks.Take(2).ToList();
            _insuranceCompany.SellPolicy("House", new DateTime(2022, 10, 1), 12, risks);
            _soldPolicies.Count.Should().Be(1);
            _soldPolicies.First().InsuredRisks.Should().BeEquivalentTo(risks);
        }

        [Test]
        public void Company_GetPremium_When_Sell_Policy()
        {
            var risks = _availableRisks.Take(3).ToList();
            _insuranceCompany.SellPolicy("House", new DateTime(2022, 10, 1), 12, risks);          
            _soldPolicies.Last().Premium.Should().Be(954);
        }

        [Test]
        public void Company_Dont_Sell_Policy_In_Past()
        {
            var risks = _availableRisks.Take(2).ToList();           
                _insuranceCompany.Invoking(x => x.SellPolicy("House", new DateTime(2022, 8, 1), 12, risks))
                .Should()
                .Throw<InvalidDateException>()
                .WithMessage("*past");                       
        }

        [Test]
        public void Company_Dont_Sell_Policy_When_Start_Is_In_Same_Period()
        {
            var risks = _availableRisks.Take(2).ToList();
            var date = new DateTime(2022, 11, 1);
            var policyObject = "House";
            _soldPolicies.Add(new Policy("House", new DateTime(2022, 10, 1), new DateTime(2023, 10, 31), 450, _availableRisks));
            _insuranceCompany.Invoking(x => x.SellPolicy(policyObject, date, 12, risks))
                    .Should()
                    .Throw<InvalidDateException>()
                    .Where(t => t.Message.Contains("same"));           
        }

        [Test]
        public void Company_Dont_Sell_Policy_When_End_Is_In_Same_Period()
        {
            var risks = _availableRisks.Take(2).ToList();
            var date = new DateTime(2022, 9, 1);
            var policyObject = "House";
            _soldPolicies.Add(new Policy("House", new DateTime(2022, 10, 1), new DateTime(2023, 10, 31), 450, _availableRisks));
            _insuranceCompany.Invoking(x => x.SellPolicy(policyObject, date, 12, risks))
                    .Should()
                    .Throw<InvalidDateException>()
                    .Where(t => t.Message.Contains("same"));
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
        public void Company__Cannot_Find_Policy_By_ObjectName_And_ValidFrom_Date_Throw_Exeption()
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
            var policy = new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 154, _availableRisks.Take(1).ToList());
            _soldPolicies.Add(policy);
            _insuranceCompany.AddRisk(policyObject, newRisk, date);
            policy.InsuredRisks.Count.Should().Be(2);
        }

        [Test]
        public void Company_GetNewPremium_When_Add_Risk()
        {
            var newRisk = new Risk("Fire", 150);
            var date = new DateTime(2023, 1, 1);
            var policyObject = "Mazda";
            var policy = new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 454, _availableRisks.Take(2).ToList());
            _soldPolicies.Add(policy);
            _insuranceCompany.AddRisk(policyObject, newRisk, date);           
            _soldPolicies.Last().Premium.Should().Be(579);
        }

        [Test]
        public void Company_Cannot_Add_New_Risk_To_Policy_Not_Valid_Date()
        {
            var newRisk = new Risk("Fire", 150);
            var date = new DateTime(2022, 10, 1);
            var policyObject = "Mazda";
            var policy = new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 154, _availableRisks.Take(1).ToList());
            _soldPolicies.Add(policy);
            _insuranceCompany.Invoking(x => x.AddRisk(policyObject, newRisk, date))
                    .Should()
                    .Throw<PolicyNotFoundException>()
                    .Where(t => t.Message.Contains("not"));
        }

        [Test]
        public void Company_Cannot_Add_New_Risk_To_Policy_Not_Valid_Object()
        {
            var newRisk = new Risk("Fire", 150);
            var date = new DateTime(2022, 12, 1);
            var policyObject = "Renault";
            var policy = new Policy("Mazda", new DateTime(2022, 11, 1), new DateTime(2023, 10, 31), 154, _availableRisks.Take(1).ToList());
            _soldPolicies.Add(policy);
            _insuranceCompany.Invoking(x => x.AddRisk(policyObject, newRisk, date))
                    .Should()
                    .Throw<PolicyNotFoundException>()
                    .Where(t => t.Message.Contains("not"));
        }
    }
}