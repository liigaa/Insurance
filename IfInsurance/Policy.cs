using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    public class Policy : IPolicy
    {
        public string NameOfInsuredObject { get; }

        public DateTime ValidFrom { get; }

        public DateTime ValidTill { get; }

        public decimal Premium { get; }

        public IList<Risk> InsuredRisks { get; }
    }
}
