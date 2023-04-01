using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public DateTime ContactDate { get; set; }
    }
}