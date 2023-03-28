using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections
{
    public class PaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PagingMetadata Metadata { get; set; }

        public PaginationResult(IPagedList<T> pagedList)
        {
            Items = pagedList;
            Metadata = new PagingMetadata(pagedList);
        }
    }
}
