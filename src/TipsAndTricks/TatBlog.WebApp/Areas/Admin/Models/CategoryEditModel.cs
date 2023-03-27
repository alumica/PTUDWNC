using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoryEditModel
	{
        public int Id { get; set; }

        [DisplayName("Tên chủ đề")]
        public string Name { get; set; }

        [DisplayName("Nội dung")]
        public string Description { get; set; }

        [DisplayName("Slug")]
        [Remote(action: "VerifyCategorySlug", controller: "Categories", areaName: "Admin",
            HttpMethod = "post", AdditionalFields = "Id")]
        public string UrlSlug { get; set; }

        [DisplayName("Hiển thị trên menu")]
        public bool ShowOnMenu { get; set; }
    }
}
