using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingShop.Web.API.Pagination
{
    public class PaginatedResponse<T>
    {
        const int MAX_PAGE_SIZE = 15;

        private int _pageSize;
        public int PageSize { get => _pageSize; private set 
            {
                if (value > MAX_PAGE_SIZE)
                    _pageSize = MAX_PAGE_SIZE;
                else
                    _pageSize = value;
            }
        }

        public int PageNumber { get; private set; }

        public int TotalItemsCount { get => _items.Count(); }

        public int TotalPages { get
            {
                if (TotalItemsCount % _pageSize != 0) 
                {
                    var result = TotalItemsCount / _pageSize;

                    return result + 1;
                }
                  return TotalItemsCount / _pageSize;
            } }

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        private readonly List<T> _items;

        public PaginatedResponse(IEnumerable<T> items, int pageSize, int pageNumber)
        {
                this.PageSize = pageSize;
                this.PageNumber = pageNumber;
                _items = new List<T>();
                _items.AddRange(items);
        }

        public List<T> GetResult()
        {
            var response = _items.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            return response;
        }

        public PaginatedResponseInfo GetResponseInfo()
        {
            return new PaginatedResponseInfo
            {
                HasNext = this.HasNext,
                HasPrevious = this.HasPrevious,
                ItemsCount = this.TotalItemsCount,
                PagesCount = this.TotalPages
            };
        }
    }
}
