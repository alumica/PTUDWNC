namespace TatBlog.WebApi.Models
{
    public class ContactDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public DateTime ContactDate { get; set; }
    }
}
