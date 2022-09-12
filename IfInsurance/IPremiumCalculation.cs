using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    internal interface IPremiumCalculation
    {
        public void SetPremium(IPolicy policy, Risk risk, short month);
        public decimal GetPremium(Risk risk, short month);
    }
}
