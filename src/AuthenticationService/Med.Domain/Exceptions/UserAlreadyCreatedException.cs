namespace Med.Domain.Exceptions
{
    public class UserAlreadyCreatedException : Exception
    {
        public UserAlreadyCreatedException()
        { }

        public UserAlreadyCreatedException(string message) : base(message)
        { }
    }
}
