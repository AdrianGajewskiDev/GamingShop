using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Pagination
{
    public class PaginatedResponseInfo
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}
