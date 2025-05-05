namespace Med.Domain.Exceptions
{
    public class UserInvalidDataException : Exception
    {
        public UserInvalidDataException()
        {}

        public UserInvalidDataException(string message) : base(message)
        { }
    }
}
