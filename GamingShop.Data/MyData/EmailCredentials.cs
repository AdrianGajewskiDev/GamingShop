using Newtonsoft.Json;
using System;
using System.IO;

namespace GamingShop.Data.MyData
{
    public static class EmailCredentials
    {
        class EmailModel
        {
            public string Email;
            public string Password;
        }


        public static string GetEmail()
        {
            var json = File.ReadAllText(@"C:\Users\adria\Desktop\Email\Data.json");

            var result = JsonConvert.DeserializeObject<EmailModel>(json);

            return result.Email;
        }

        public static string GetPassword()
        {
            var json = File.ReadAllText(@"C:\Users\adria\Desktop\Email\Data.json");

            var result = JsonConvert.DeserializeObject<EmailModel>(json);

            return result.Password;
        }
    }
}
