using System.Net;
using NbTrader.Brokers.TDAmeritrade.V2.Services;

namespace NbTrader.Brokers.TDAmeritrade.V2.Exceptions
{
    public class TDAmeritradeHttpException : HttpRequestException
    {
        public new HttpStatusCode? StatusCode { get; set; }

        public IEndPoint? EndPoint { get; set; }

        public HttpRequestMessage? RequestMessage { get; set; }

        public HttpResponseMessage? ResponseMessage { get; set; }

        public TDAmeritradeHttpException(string? message, HttpStatusCode? statusCode, HttpRequestMessage? httpRequestMessage, HttpResponseMessage? httpResponseMessage, IEndPoint endPoint) 
            : base(message)
        {
            StatusCode = statusCode;
            RequestMessage = httpRequestMessage;
            ResponseMessage = httpResponseMessage;
            EndPoint = endPoint;
        }

        public TDAmeritradeHttpException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
