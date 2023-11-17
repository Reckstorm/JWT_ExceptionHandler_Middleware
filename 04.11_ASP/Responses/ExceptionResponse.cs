namespace _04._11_ASP.Responses
{
    public class ExceptionResponse
    {
        public ExceptionResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
