using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    public class Policy : IPolicy
    {
        private string _nameOfInsuredObject;
        private DateTime _validFrom;
        private DateTime _validTill;
        private decimal _premium;
        private IList<Risk> _insuredRisks;

        public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, decimal premium, IList<Risk> insuredRisks)
        {
            _nameOfInsuredObject = nameOfInsuredObject;
            _validFrom = validFrom;
            _validTill = validTill;
            _premium = premium;
            _insuredRisks = insuredRisks;
        }

        public Policy()
        {
        }

        public string NameOfInsuredObject => _nameOfInsuredObject;

        public DateTime ValidFrom => _validFrom;

        public DateTime ValidTill => _validTill;

        public decimal Premium
        {
            get => _premium;
        }

        public IList<Risk> InsuredRisks => _insuredRisks;

        public decimal GetPremium(Risk risk, short month)
        {
            return risk.YearlyPrice / 12 * month;
        }
    }
}
