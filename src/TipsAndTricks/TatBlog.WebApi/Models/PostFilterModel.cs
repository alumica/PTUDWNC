namespace TatBlog.WebApi.Models
{
    public class PostFilterModel : PagingModel
    {
        public string Keyword { get; set; }

        public int? PostId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? PostedDate { get; set; }
    }
}
