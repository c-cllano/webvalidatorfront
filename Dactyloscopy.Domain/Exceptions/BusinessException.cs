namespace Dactyloscopy.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; }
        public string Error { get; }

        public BusinessException(string error, int statusCode)
            : base(error)
        {
            StatusCode = statusCode;
            Error = error;
        }
    }
}
