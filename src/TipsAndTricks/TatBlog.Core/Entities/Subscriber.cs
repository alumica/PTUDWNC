using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }

        // public string FullName { get; set; }
    
        public string Email { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime UnsubscribeDate { get; set; }

        public string ResonUnsubscribe { get; set; }

        public byte Flag { get; set; }

        public string Notes { get; set; }
    }
}
