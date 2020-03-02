using System;
using System.Collections.Generic;
using System.Text;

namespace GamingShop.Service.Extensions
{
    public class JWTConfiguration
    {
        public string Key { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public TimeSpan ClockSkew { get; set; } = TimeSpan.Zero;
    }
}
