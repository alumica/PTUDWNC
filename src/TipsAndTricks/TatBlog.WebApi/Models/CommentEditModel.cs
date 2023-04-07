namespace TatBlog.WebApi.Models
{
    public class CommentEditModel
    {
        
        public string FullName { get; set; }

        public bool Gender { get; set; }

        public bool Approved { get; set; }

        public string Description { get; set; }

        public int PostId { get; set; }
    }
}