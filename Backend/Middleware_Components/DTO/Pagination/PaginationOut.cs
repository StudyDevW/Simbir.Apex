using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO.Pagination
{
    public class PaginationOut<T>
    {
        public int From { get; set; }

        public int Count { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public T Data { get; set; }


        public PaginationOut(T data, int from, int count, int totalCount)
        {
            Data = data;
            From = from;
            Count = count;
            TotalCount = totalCount;

            if (count > 0)
                TotalPages = (int)Math.Ceiling(totalCount / (double)count);
            else
                TotalPages = totalCount > 0 ? 1 : 0;
        }
    }
}
