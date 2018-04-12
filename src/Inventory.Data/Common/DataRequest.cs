using System;
using System.Linq.Expressions;

namespace Inventory.Data
{
    public class DataRequest<T>
    {
        public string Query { get; set; }

        public Expression<Func<T, bool>> Where { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
    }
}
