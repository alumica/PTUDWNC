namespace TatBlog.WebApi.Models
{
    public class SubscriberEditModel
    {
        public DateTime UnsubscribeDate { get; set; }

        public string ResonUnsubscribe { get; set; }

        public bool TypeReason { get; set; } // true: user unsubscribe | false: block

        public string Notes { get; set; }
    }
}