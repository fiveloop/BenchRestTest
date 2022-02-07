using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchRestClient
{
    /// <summary>
    /// Represents an error returned by a web service, including the HTTP response status code and content.
    /// </summary>
    public class WebServiceException : Exception
    {
        /// <summary>
        /// HTTP response status code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// HTTP response content
        /// </summary>
        public string Content { get; set; }
    }
}
