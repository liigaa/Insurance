using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IfInsurance.Exceptions;
using System.Reflection;

namespace IfInsurance
{
    public class InsuranceCompany : IInsuranceCompany
    {
        private string _name;
        private IList<Risk> _availableRisks;
        private IList<IPolicy> _policies;
        PremiumCalculation _premiumCalculation = new PremiumCalculation();

        public InsuranceCompany(string name, IList<Risk> availableRisks, IList<IPolicy> policies)
        {
            _name = name;
            _availableRisks = availableRisks;
            _policies = policies;           
        }

        public string Name => _name;

        public IList<Risk> AvailableRisks 
        { 
            get =>_availableRisks; 
            set => _availableRisks = value; 
        }

        public IList<IPolicy> Policies
        {
            get => _policies;
            set => _policies = value;
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
        {
            var policy = FindPolicyWithInIntervalWithStartDate(nameOfInsuredObject, validFrom);

            if (policy != null)
            {
                policy.InsuredRisks.Add(risk);
                var month = ((policy.ValidTill.Year - validFrom.Year) * 12) + (policy.ValidTill.Month - validFrom.Month) + 1;
                _premiumCalculation.SetPremium(policy, risk, (short)month);
                return;
            }
            
            throw new PolicyNotFoundException();
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            var policy = FindPolicyWithEfectiveDate(nameOfInsuredObject, effectiveDate);
            if (policy != null)
                return policy;
            throw new PolicyNotFoundException();
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            var validTill = validFrom.AddMonths(validMonths);

            if (validFrom < DateTime.Now)
            {
                throw new InvalidDateException();
            }

            var policy = FindPolicyWithInIntervalWithStartAndEndDate(nameOfInsuredObject, validFrom, validTill);

            if (policy != null)
            {
                throw new InvalidDateException("Can not be new insurance in same period");
            }            

            var premium = selectedRisks.Sum(risk => _premiumCalculation.GetPremium(risk, validMonths));
            var soldPolicy =  new Policy(nameOfInsuredObject, validFrom, validTill, premium, selectedRisks);
            _policies.Add(soldPolicy);
            return soldPolicy;
        }

        private IPolicy FindPolicyWithEfectiveDate(string insuredObject, DateTime efectiveDate)
        {
            return _policies.FirstOrDefault(p => p.NameOfInsuredObject == insuredObject && p.ValidFrom == efectiveDate);           
        }
        
        private IPolicy FindPolicyWithInIntervalWithStartDate(string insuredObject, DateTime validFrom)
        {
            return _policies.FirstOrDefault(p => p.NameOfInsuredObject == insuredObject && p.ValidFrom <= validFrom && p.ValidTill > validFrom);
        } 
        
        private IPolicy FindPolicyWithInIntervalWithStartAndEndDate(string insuredObject, DateTime validFrom, DateTime validTill)
        {
            return _policies.FirstOrDefault(p => p.NameOfInsuredObject == insuredObject
                                                && p.ValidFrom <= validFrom && p.ValidTill >= validFrom
                                                || p.ValidFrom <= validTill && p.ValidTill >= validTill);
        }
    }
}
