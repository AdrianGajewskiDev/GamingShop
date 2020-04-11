using Newtonsoft.Json;

namespace GamingShop.Web.API.Exceptions
{
    public class ExceptionInfo
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
