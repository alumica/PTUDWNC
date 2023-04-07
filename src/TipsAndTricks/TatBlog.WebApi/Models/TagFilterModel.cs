namespace TatBlog.WebApi.Models
{
    public class TagFilterModel : PagingModel
    {
        public string Name { get; set; }

        public bool? IsPaged { get; set; } = false;
    }
}
