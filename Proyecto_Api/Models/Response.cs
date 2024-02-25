using System.Net;

namespace Proyecto_Api.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool isFine { get; set; } = true;
        public List<string> errorMessage { get; set; }

        public object result { get; set; }

    }
}
