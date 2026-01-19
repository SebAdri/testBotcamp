namespace BootcampCLT.Application.Exceptions
{
    public sealed class ApiBadRequestException : BadRequestException
    {
        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }

        public ApiBadRequestException(string errorCode, string message)
                : base($"Error en la solicitud: {message}", errorCode)
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
