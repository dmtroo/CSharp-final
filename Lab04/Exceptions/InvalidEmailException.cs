using System;

namespace ProgrammingInCSharp.Lab04.Exceptions
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
        {

        }

        public InvalidEmailException(string message)
            : base(message)
        {

        }

        public InvalidEmailException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
