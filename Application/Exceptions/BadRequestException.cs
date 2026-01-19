namespace BootcampCLT.Application.Exceptions
{
    public abstract class BadRequestException : ApplicationException
    {
        public string ErrorCode { get; }
        protected BadRequestException(string message, string errorCode)
            : base("Bad Request", message)
        {
            ErrorCode = errorCode;
        }
    }
}
