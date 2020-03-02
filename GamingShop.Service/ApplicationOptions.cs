using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Service
{
    public class ApplicationOptions
    {
        public string Secret_Key { get; set; }
        public string ClientURL { get; set; }
        public string SendGridAPIKey { get; set; }
        public string JWTSecretKey { get; set; }
    }
}
