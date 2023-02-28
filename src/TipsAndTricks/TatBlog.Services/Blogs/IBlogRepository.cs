﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là 'slug'
        // và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        // Tìm Top N bài viết phổ biến được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default);

        // Tăng số lượng lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        // Lấy danh sách chuyên mục và số lượng bài viết
        // nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);

        // Lấy danh sách từ khóa/thẻ và phân trang theo
        // các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        // C. Bài tập thực hành
        // 1.
        // 1.a. Tìm một thẻ (Tag) theo tên định danh (slug).
        Task<Tag> FindTagWithSlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 1.c. Lấy danh sách tất cả các thẻ (Tag) kèm theo
        // số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
        Task<IList<TagItem>> GetTagItemsAsync(
            CancellationToken cancellationToken = default);

        // 1.d. Xóa một thẻ theo mã cho trước.
        Task DeleteTagWithId(
            int id,
            CancellationToken cancellationToken = default);

        // 1.e. Tìm một chuyên mục (Category) theo tên định danh (slug).
        Task<Category> FindCategoryWithSlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 1.f. Tìm một chuyên mục theo mã số cho trước.
        Task<Category> FindCategoryWithIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 1.g. Thêm hoặc cập nhật một chuyên mục/chủ đề. 
        Task AddOrUpdateCategory(
            CancellationToken cancellationToken = default);

        // 1.h. Xóa một chuyên mục theo mã số cho trước.
        Task DeleteCategoryWithId(
            int id,
            CancellationToken cancellationToken = default);

        // 1.i. Kiểm tra tên định danh (slug) của
        // một chuyên mục đã tồn tại hay chưa.
        Task IsCategorySlugExistedAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 1.j. Lấy và phân trang danh sách chuyên mục,
        // kết quả trả về kiểu IPagedList<CategoryItem>.
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

    }
}