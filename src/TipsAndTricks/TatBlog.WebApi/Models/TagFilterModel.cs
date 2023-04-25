using System.ComponentModel;

namespace TatBlog.WebApi.Models
{
    public class TagFilterModel : PagingModel
    {
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool? IsPaged { get; set; }
    }
}
