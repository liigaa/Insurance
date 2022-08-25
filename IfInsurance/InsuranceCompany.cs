using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IfInsurance.Exceptions;

namespace IfInsurance
{
    public class InsuranceCompany : IInsuranceCompany
    {
        private string _name;
        private IList<Risk> _availableRisks;
        private IList<Policy> _policies;

        public InsuranceCompany(string name, IList<Risk> availableRisks, IList<Policy> policies)
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

        public IList<Policy> Policies
        {
            get => _policies;
            set => _policies = value;
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
        {
            var policy = FindPolicy(nameOfInsuredObject, validFrom);
            if(policy != null)
            {
                policy.InsuredRisks.Add(risk);                
                var month = ((policy.ValidTill.Year - validFrom.Year) * 12) + (policy.ValidTill.Month - validFrom.Month) + 1;
                GetChangedPremium((Policy)policy, risk, (short)month);
            }
            else
            {
                throw new PolicyNotFoundException();
            }            
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            var policy = FindPolicy(nameOfInsuredObject, effectiveDate);
            if (policy != null)
                return policy;
            throw new PolicyNotFoundException();
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            var validTill = validFrom.AddMonths(validMonths);

            if (validFrom < DateTime.Now)
            {
                throw new DateException();
            }

            foreach (var policy in _policies)
            {
                if (policy.NameOfInsuredObject == nameOfInsuredObject)
                {
                    if ((validFrom <= policy.ValidTill && validFrom >= policy.ValidFrom) || (validTill >= policy.ValidFrom && validTill <= policy.ValidTill))
                    {
                        throw new DateException("Can not be insurance in same period");
                    }
                }
            }

            var premium = selectedRisks.Sum(risk => risk.YearlyPrice / 12 * validMonths);
            var soldPolicy =  new Policy(nameOfInsuredObject, validFrom, validTill, premium, selectedRisks);
            _policies.Add(soldPolicy);
            return soldPolicy;
        }

        private IPolicy FindPolicy(string insuredObject, DateTime validFrom)
        {
            return _policies.FirstOrDefault(p => p.NameOfInsuredObject == insuredObject && p.ValidFrom == validFrom);
            //^ tas pats kas apakšā
            //foreach (var policy in _policies)
            //{
            //    if (policy.NameOfInsuredObject == insuredObject && policy.ValidFrom == validFrom)
            //    {
            //        return policy;
            //    }
            //}
            //return null;
        }

        private void GetChangedPremium(Policy policy, Risk risks, short month)
        {           
            var premiumForNewRisk = risks.YearlyPrice / 12 * month;
            policy.Premium += premiumForNewRisk;
        }
    }
}
