using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int AuthorId { get; set; }

        public string AuthorSlug { get; set; } = default;

        public int PostId { get; set; }

        public int CategoryId { get; set; }

        public string CategorySlug { get; set; } = default;

        public int PostedYear { get; set; }

        public int PostedMonth { get; set; }

        public int PostedDay { get; set; }

        public int TagId { get; set; }

        public string TagSlug { get; set; } = default;

        public bool PublishedOnly { get; set; }

        public string Keyword { get; set; } = default;

        public string PostSlug { get; set; }
    }
}
