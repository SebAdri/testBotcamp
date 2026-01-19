namespace BootcampCLT.Application.Exceptions
{
    public class ApiException : ApplicationException
    {
        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }

        public ApiException(string errorCode, string message)
                : base($"Error en el servidor: {message}", errorCode)
        {
            ErrorsDictionary = new Dictionary<string, string[]>()
        {
            {
                errorCode,new string[] {message}
            }
        };
        }
    }
}
