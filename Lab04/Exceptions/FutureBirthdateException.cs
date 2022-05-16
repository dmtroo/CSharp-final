using System;

namespace ProgrammingInCSharp.Lab04.Exceptions
{
    [Serializable]
    public class FutureBirthdateException : Exception
    {
        public FutureBirthdateException()
        {

        }

        public FutureBirthdateException(string message)
            : base(message)
        {

        }

        public FutureBirthdateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
