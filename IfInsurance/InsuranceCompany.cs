using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    public class InsuranceCompany : IInsuranceCompany
    {
        private string _name;
        private IList<Risk> _availableRisks;
        private IList<Policy> _policies;

        public InsuranceCompany(string name, IList<Risk> availableRisks)
        {
            _name = name;
            _availableRisks = availableRisks;
            _policies = new List<Policy>();
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
            foreach (var policy in _policies)
            {
                if(policy.NameOfInsuredObject == nameOfInsuredObject && policy.ValidFrom == effectiveDate)
                {
                    return policy;
                }
            }
            throw new ArgumentException("There are not such a policy");
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {

            if(validFrom < DateTime.Now)
            {
                throw new DataMisalignedException("Cannot be policy effective date in past");
            }
            foreach(Policy policy in _policies)
            {
                if(policy.NameOfInsuredObject == nameOfInsuredObject && policy.ValidFrom == validFrom)
                {
                    throw new ArgumentException("Cannot be policy with same efective date");
                }
            }

            var validTill = validFrom.AddMonths(validMonths);
            var premium = selectedRisks.Sum(risk => risk.YearlyPrice / 12 * validMonths);
            var soldPolicy =  new Policy(nameOfInsuredObject, validFrom, validTill, premium, selectedRisks);
            _policies.Add(soldPolicy);
            return soldPolicy;
        }
    }
}
