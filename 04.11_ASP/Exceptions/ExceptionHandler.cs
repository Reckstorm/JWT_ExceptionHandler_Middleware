using _04._11_ASP.Responses;
using System.Net;
using System.Text.Json;

namespace _04._11_ASP.Exceptions
{
    public class ExceptionHandler : AbstractExceptionHandlerMiddleware
    {
        public ExceptionHandler(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode statusCode;
            switch (exception)
            {
                case KeyNotFoundException
                or FileNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException
                or InvalidOperationException
                or ArgumentOutOfRangeException
                or MyException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            return (statusCode, JsonSerializer.Serialize(new ExceptionResponse((int)statusCode, exception.Message)));
        }
    }
}
