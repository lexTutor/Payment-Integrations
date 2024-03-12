using System;

namespace Remita.Exceptions
{
    public class InvalidServerResponseException : Exception
    {
        public InvalidServerResponseException(string message) : base(message) { }
    }
}
