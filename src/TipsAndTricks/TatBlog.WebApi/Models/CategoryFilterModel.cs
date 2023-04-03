using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApi.Models
{
    public class CategoryFilterModel : PagingModel
    {
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsPaged { get; set; }
    }
}
