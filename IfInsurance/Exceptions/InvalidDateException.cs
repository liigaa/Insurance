using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance.Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException() : base("Cannot be policy effective date in past")
        {
        }

        public InvalidDateException(string message) : base(message)
        {
        }

        public InvalidDateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
