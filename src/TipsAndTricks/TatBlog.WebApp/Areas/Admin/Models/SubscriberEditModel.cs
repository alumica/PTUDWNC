using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class SubscriberEditModel
	{
        public int Id { get; set; }

		[DisplayName("Email")]
		public string Email { get; set; }

		[DisplayName("Ngày đăng ký")]
		public DateTime SubscribeDate { get; set; }

		[DisplayName("Ngày hủy đăng ký")]
		public DateTime? UnsubscribeDate { get; set; }

		[DisplayName("Lý do hủy đăng ký")]
		public string ResonUnsubscribe { get; set; }

		[DisplayName("Loại hủy đăng ký")]
		public bool TypeReason { get; set; } // true: user unsubscribe | false: block

		[DisplayName("Ghi chú")]
		public string Notes { get; set; }
	}
}
