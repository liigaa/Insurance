using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfInsurance.Exceptions
{
    public class DateException : Exception
    {
        public DateException() : base("Cannot be policy effective date in past")
        {
        }

        public DateException(string message) : base(message)
        {
        }

        public DateException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
