using System.Net;

namespace SupportWave.Librarian.Api.Exceptions
{
    public class LibrarianException 
    {
        public LibrarianException(HttpStatusCode statusCode, string? message, string? suggestion, object? payload)
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Suggestion = suggestion;
            this.PayloadReceived = payload;
        }

        public HttpStatusCode StatusCode {get;set;}
        public string? Message {get;set;}
        public string? Suggestion {get;set;}
        public object? PayloadReceived {get;set;}
    }
}