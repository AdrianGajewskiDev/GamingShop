using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GamingShop.Service
{
    public class EmailTemplates
    {
        public static string VerificationEmailTemplate { get; } = "GamingShop.Service.EmailTemplates.Templates.VerificationTemplate.htm";
    }
}
