using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Subscribers;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subcriberRepository;

        public NewsletterController(ISubscriberRepository subcriberRepository)
        {
            _subcriberRepository = subcriberRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(
            string email)
        {
            var subscribe = await _subcriberRepository.SubscribeAsync(email);
            //if (subscribe)
            //    await _subcriberRepository.SendEmailUnsubscribe(email);
            return View(subscribe);
        }

        public async Task<IActionResult> Unsubscribe()
        {
            return View();
        }
    }
}
