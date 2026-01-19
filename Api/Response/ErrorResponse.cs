using System.Text.Json.Serialization;

namespace BootcampCLT.Api.Response
{
    public class ErrorResponse
    {
        public string Codigo { get; init; } = string.Empty;
        public string Mensaje { get; init; } = string.Empty;
    }
}
