using System;
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
            foreach (var policy in _policies)
            {
                if (policy.NameOfInsuredObject == nameOfInsuredObject && policy.ValidFrom == validFrom)
                {
                    policy.InsuredRisks.Add(risk);
                }
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

            if(validFrom < DateTime.Now)
            {
                throw new DataMisalignedException("Cannot be policy effective date in past");
            }
            var policy = FindPolicy(nameOfInsuredObject, validFrom);
            if(policy != null)
            {
                throw new ArgumentException();
            }

            var validTill = validFrom.AddMonths(validMonths);
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
    }
}
