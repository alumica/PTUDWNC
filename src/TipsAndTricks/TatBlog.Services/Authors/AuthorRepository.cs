using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;

        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<int> NumberAuthorsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .CountAsync(cancellationToken);
        }

        // Lấy danh sách tác giả và số lượng bài viết
        // nằm thuộc từng tác giả
        public async Task<IList<AuthorItem>> GetAuthorsAsync(
			CancellationToken cancellationToken = default)
        {
			IQueryable<Author> authors = _context.Set<Author>();


            return await authors
                .OrderBy(x => x.FullName)
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                }).ToListAsync(cancellationToken);
		}
		public async Task<Author> GetAuthorByIdAsync(
			int id,
			CancellationToken cancellationToken = default)
        {
			return await _context.Set<Author>()
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync(cancellationToken);
		}
		// 2. Tạo các lớp và định nghĩa các phương thức
		// cần thiết để truy vấn và cập nhật thông tin tác giả bài viết.
		// 2.a. Tạo interface IAuthorRepository và lớp AuthorRepository.

		// 2.b. Tìm một tác giả theo mã số.
		public async Task<Author> FindAuthorByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // 2.c. Tìm một tác giả theo tên định danh(slug). 
        public async Task<Author> FindAuthorBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Author> authorQuery = _context.Set<Author>();
            if (!string.IsNullOrWhiteSpace(slug))
            {
                authorQuery = authorQuery.Where(x => x.UrlSlug == slug);
            }
            return await authorQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // 2.d. Lấy và phân trang danh sách tác giả kèm theo
        // số lượng bài viết của tác giả đó.
        // Kết quả trả về kiểu IPagedList<AuthorItem>.
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams, 
            CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>()
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await authorQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

		public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
			int pageNumber,
			int pageSize,
			CancellationToken cancellationToken = default)
        {
			var authorQuery = _context.Set<Author>()
				.Select(x => new AuthorItem()
				{
					Id = x.Id,
					FullName = x.FullName,
					UrlSlug = x.UrlSlug,
					ImageUrl = x.ImageUrl,
					JoinedDate = x.JoinedDate,
					Email = x.Email,
					Notes = x.Notes,
					PostCount = x.Posts.Count(p => p.Published)
				});

			return await authorQuery
                .ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Author.FullName), "DESC",
                cancellationToken);
		}

		// 2.e. Thêm hoặc cập nhật thông tin một tác giả.
		public async Task AddOrUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default)
        {
            if (IsAuthorSlugExistedAsync(author.Id, author.UrlSlug).Result)
                Console.WriteLine("Error: Exsited Slug");
            else

                if (author.Id > 0) 
                await _context.Set<Author>()
                .Where(a => a.Id == author.Id)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(x => x.FullName, x => author.FullName)
                    .SetProperty(x => x.UrlSlug, x => author.UrlSlug)
                    .SetProperty(x => x.ImageUrl, x => author.ImageUrl)
                    .SetProperty(x => x.JoinedDate, author.JoinedDate)
                    .SetProperty(x => x.Email, author.Email)
                    .SetProperty(x => x.Notes, author.Notes)
                    .SetProperty(x => x.Posts, author.Posts),
                    cancellationToken);
            else
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
            }
        }

        public async Task<Author> CreateOrUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
            {
                _context.Set<Author>().Update(author);
            }
            else
            {
                _context.Set<Author>().Add(author);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return author;
        }

        public async Task<bool> IsAuthorSlugExistedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug,
                cancellationToken);
        }

        // 2.f. Tìm danh sách N tác giả có nhiều bài viết nhất.
        // N là tham số đầu vào.
        public async Task<IList<AuthorItem>> FindListAuthorsMostPostAsync(
            int n,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
            .Select(x => new AuthorItem()
            {
                Id = x.Id,
                FullName = x.FullName,
                UrlSlug = x.UrlSlug,
                ImageUrl = x.ImageUrl,
                JoinedDate = x.JoinedDate,
                Email = x.Email,
                Notes = x.Notes,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .OrderByDescending(x => x.PostCount)
            .Take(n)
            .ToListAsync(cancellationToken);
        }

		public async Task<bool> DeleteAuthorByIdAsync(
			int id,
			CancellationToken cancellationToken = default)
        {
			var author = await _context.Set<Author>().FindAsync(id);

			if (author is null) return false;

			_context.Set<Author>().Remove(author);
			var rowsCount = await _context.SaveChangesAsync(cancellationToken);

			return rowsCount > 0;
		}
	}
}
