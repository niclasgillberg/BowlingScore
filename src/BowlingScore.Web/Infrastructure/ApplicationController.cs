using System.Text;
using System.Web.Mvc;
using BowlingScore.Web.Helpers;

namespace BowlingScore.Web.Infrastructure
{
    public class ApplicationController : Controller
    {
        protected JsonNetResult JsonNet(object data, string contentType = "application/json", Encoding contentEncoding = null)
        {
            var result = new JsonNetResult
            {
                Data = data, 
                ContentType = contentType, 
                ContentEncoding = contentEncoding ?? Encoding.UTF8
            };
            return result;
        }
    }
}