using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Authors;

namespace TatBlog.WebApp.Components
{
    public class BestAuthorsWidget : ViewComponent
    {
        private readonly IAuthorRepository _authorRepository;

        public BestAuthorsWidget(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách chủ đề
            var top4 = await _authorRepository.FindListAuthorsMostPostAsync(4);

            return View(top4);
        }
    }
}
