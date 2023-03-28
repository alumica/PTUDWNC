using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Authors
{
    public interface IAuthorRepository
    {
        Task<int> NumberAuthorsAsync(
            CancellationToken cancellationToken = default);

		// Lấy danh sách tác giả và số lượng bài viết
		// nằm thuộc từng tác giả
		Task<IList<AuthorItem>> GetAuthorsAsync(
			CancellationToken cancellationToken = default);

		Task<Author> GetAuthorByIdAsync(
            int id,
			CancellationToken cancellationToken = default);

        Task<Author> GetAuthorBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 2. Tạo các lớp và định nghĩa các phương thức
        // cần thiết để truy vấn và cập nhật thông tin tác giả bài viết.
        // 2.a. Tạo interface IAuthorRepository và lớp AuthorRepository.

        Task<Author> GetCachedAuthorBySlugAsync(
            string slug, 
            CancellationToken cancellationToken = default);

        Task<Author> GetCachedAuthorByIdAsync(
            int authorId,
            CancellationToken cancellationToken = default);
        // 2.b. Tìm một tác giả theo mã số.
        Task<Author> FindAuthorByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 2.c. Tìm một tác giả theo tên định danh(slug). 
        Task<Author> FindAuthorBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 2.d. Lấy và phân trang danh sách tác giả kèm theo
        // số lượng bài viết của tác giả đó.
        // Kết quả trả về kiểu IPagedList<AuthorItem>.
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
            Func<IQueryable<Author>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);


        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
			int pageNumber,
            int pageSize,
			CancellationToken cancellationToken = default);

        // 2.e. Thêm hoặc cập nhật thông tin một tác giả.
        Task<bool> AddOrUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default);

        Task<Author> CreateOrUpdateAuthorAsync(
            Author author, 
            CancellationToken cancellationToken = default);

        Task<bool> IsAuthorSlugExistedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default);

        // 2.f. Tìm danh sách N tác giả có nhiều bài viết nhất.
        // N là tham số đầu vào.
        Task<IList<AuthorItem>> FindListAuthorsMostPostAsync(
            int n,
            CancellationToken cancellationToken = default);


        Task<bool> DeleteAuthorByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> SetImageUrlAsync(
            int authorId, string imageUrl,
            CancellationToken cancellationToken = default);
    }
}
