using System;
using System.Linq.Expressions;

namespace Inventory.Data
{
    public class PageRequest<T>
    {
        public PageRequest() : this(0, Int32.MaxValue)
        {
        }
        public PageRequest(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public Expression<Func<T, bool>> Where { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Descending { get; set; }
    }
}
