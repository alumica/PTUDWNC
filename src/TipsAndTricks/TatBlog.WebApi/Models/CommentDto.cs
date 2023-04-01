using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public bool Gender { get; set; }

        public bool Approved { get; set; }

        public DateTime PostedDate { get; set; }

        public string Description { get; set; }

        public PostDto Post { get; set; }
    }
}
