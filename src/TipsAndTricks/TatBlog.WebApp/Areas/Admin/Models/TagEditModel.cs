using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class TagEditModel
	{
		public int Id { get; set; }

		[DisplayName("Tên thẻ")]
		public string Name { get; set; }

		[DisplayName("Nội dung")]
		public string Description { get; set; }

		[DisplayName("Slug")]
		[Remote(action: "VerifyTagSlug", controller: "Tags", areaName: "Admin",
			HttpMethod = "post", AdditionalFields = "Id")]
		public string UrlSlug { get; set; }
	}
}
