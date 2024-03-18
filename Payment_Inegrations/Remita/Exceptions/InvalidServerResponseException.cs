using System;

namespace Integrations.Exceptions
{
    public class InvalidServerResponseException : Exception
    {
        public InvalidServerResponseException(string message) : base(message) { }
    }
}
