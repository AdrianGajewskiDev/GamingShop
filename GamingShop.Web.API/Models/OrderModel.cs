using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Models
{
    public class OrderModel
    {
        public int CartID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string AlternativeEmailAdress { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Placed { get; set; }
    }
}
