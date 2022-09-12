using System;
namespace IfInsurance.Exceptions
{
    public class PolicyNotFoundException : Exception
    {
        public PolicyNotFoundException() : base("Policy not found")
        {

        }

        public PolicyNotFoundException(string message) : base(message)
        {

        }

        public PolicyNotFoundException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
