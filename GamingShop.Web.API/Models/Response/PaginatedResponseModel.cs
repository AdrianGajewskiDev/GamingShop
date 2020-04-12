using GamingShop.Web.API.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Models.Response
{
    public class PaginatedResponseModel<T>
    {
        public List<T> Items { get; set; }
        public PaginatedResponseInfo ResponseInfo { get; set; }
    }
}
