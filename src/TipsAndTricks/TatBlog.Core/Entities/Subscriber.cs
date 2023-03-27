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
    
        public string Email { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime? UnsubscribeDate { get; set; }

        public string ResonUnsubscribe { get; set; }

        public bool? TypeReason { get; set; } // true: user unsubscribe | false: block

        public string Notes { get; set; }
    }
}
