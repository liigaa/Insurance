using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance
{
    public struct Risk
    {
        private string _name;
        private decimal _yearlyPrice;
        /// <summary>
        /// Unique name of the risk
        /// </summary>
        public string Name 
        { 
            get => _name; 
            set => _name = value; 
        }
        /// <summary>
        /// Risk yearly price
        /// </summary>
        public decimal YearlyPrice 
        { 
            get => _yearlyPrice; 
            set => _yearlyPrice = value; 
        }

        public Risk(string name, decimal yearlyPrice)
        {
            _name = name;
            _yearlyPrice = yearlyPrice;
        }
    }
}
