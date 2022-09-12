using IfInsurance;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceTest
{
    public class PolicyTest
    {
        private IPolicy _policy;
        private IList<Risk> _policyRisks;
        [SetUp]
        public void Setup()
        {
            _policyRisks = new List<Risk> { new Risk("Home", 450)};
            _policy = new Policy("Private home", new DateTime(2022, 10, 1), new DateTime(2022, 9, 30), 450, _policyRisks);
        }

        [Test]
        public void Policy_NameOfInsuredObject_ShouldBe_Private_home()
        {          
            _policy.NameOfInsuredObject.Should().Be("Private home");
        }

        [Test]
        public void Policy_ValidFromDate_Should_Be_2022_10_1()
        {
            _policy.ValidFrom.Should().BeSameDateAs(new DateTime(2022,10,1));
        }
        [Test]
        public void PolicyRisk_Should_Be_Equal_PolicyInsuredRisks()
        {
            _policy.InsuredRisks.Should().BeEquivalentTo(_policyRisks);
        }
    }
}
