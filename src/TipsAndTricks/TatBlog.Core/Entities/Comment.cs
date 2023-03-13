using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public bool Gender { get; set; }

        public bool? Approved { get; set; }

        public DateTime PostedDate { get; set; }

        public string Description { get; set; }

        public Post Post { get; set; }
    }
}
