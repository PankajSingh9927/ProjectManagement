using System.Runtime.Serialization;

namespace ProductManagement.Configration
{
    internal class HttpResponseException : Exception
    {
        public HttpResponseException() { }

        public HttpResponseException(string errorDescription)
        {
            ErrorDescription = errorDescription;
        }

        public HttpResponseException(int status, string errorDescription)
        {
            Status = status;
            ErrorDescription = errorDescription;
        }

        public HttpResponseException(int status, string error, string errorDescription)
        {
            Status = status;
            Error = error;
            ErrorDescription = errorDescription;
        }

        public int Status { get; set; } = 400;
        public string ErrorDescription { get; set; }
        public string Error { get; set; }
    }
}