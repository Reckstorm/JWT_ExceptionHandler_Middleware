namespace _04._11_ASP.Exceptions
{
    public class MyException : Exception
    {
        public MyException(string message = "my custom exception", string? localizerKey = null) : base(message)
        {
            Data.Add(AbstractExceptionHandlerMiddleware.LocalizationKey, localizerKey);
        }
    }
}
