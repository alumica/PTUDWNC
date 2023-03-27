using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class AuthorEditModel
    {
        public int Id { get; set; }

        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [DisplayName("Slug")]
        [Remote(action: "VerifyAuthorSlug", controller: "Authors", areaName: "Admin",
            HttpMethod = "post", AdditionalFields = "Id")]
        public string UrlSlug { get; set; }

        [DisplayName("Chọn hình ảnh")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Hình ảnh hiện tại")]
        public string ImageUrl { get; set; }

		[DisplayName("Email")]
		public string Email { get; set; }

		[DisplayName("Ghi chú")]
		public string Notes { get; set; }
	}
}
