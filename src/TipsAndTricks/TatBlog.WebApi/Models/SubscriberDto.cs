namespace TatBlog.WebApi.Models
{
    public class SubscriberDto
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
