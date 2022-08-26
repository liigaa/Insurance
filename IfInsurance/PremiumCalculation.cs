using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    public class PremiumCalculation
    {
        public void SetPremium(IPolicy policy, Risk risk, short month)
        {
            FieldInfo fieldInfo = typeof(Policy).GetField("_premium", BindingFlags.Instance | BindingFlags.NonPublic);
            var newPremium = policy.Premium + GetPremium(risk, (short)month);
            fieldInfo.SetValue(policy, newPremium);
        }
        
        public decimal GetPremium(Risk risk, short month)
        {
            return risk.YearlyPrice / 12 * month;
        }
    }
}
