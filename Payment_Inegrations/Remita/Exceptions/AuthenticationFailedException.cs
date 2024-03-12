using System;

namespace Remita.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message)
        {
        }
    }
}
