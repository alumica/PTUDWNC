using Microsoft.EntityFrameworkCore;
using System;
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

        // Tìm bài viết có mã số là 'id'
        Task<Post> GetPostByIdAsync(
            int id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        // Tìm Top N bài viết phổ biến được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

		Task<IPagedList<Post>> GetPagedPopularArticlesAsync(
			int numPosts,
			int pageNumber = 1, int pageSize = 10,
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

        Task<Category> GetCategoryByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> ToggleShowOnMenuAsync(
            int id = 0,
            CancellationToken cancellationToken = default);

        // Lấy danh sách từ khóa/thẻ và phân trang theo
        // các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

		Task<IPagedList<TagItem>> GetPagedTagsAsync(
			int pageNumber = 1, int pageSize = 10,
			CancellationToken cancellationToken = default);

		Task<Tag> CreateOrUpdateTagAsync(
			Tag tag,
			CancellationToken cancellationToken = default);

		// C. Bài tập thực hành
		// 1.
		// 1.a. Tìm một thẻ (Tag) theo tên định danh (slug).
		Task<Tag> FindTagBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

		// 1.c. Lấy danh sách tất cả các thẻ (Tag) kèm theo
		// số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.

		Task<bool> IsTagSlugExistedAsync(
			int id,
			string slug,
			CancellationToken cancellationToken = default);

		Task<IList<TagItem>> GetTagItemsAsync(
            CancellationToken cancellationToken = default);

		Task<Tag> GetTagByIdAsync(
			int id,
			CancellationToken cancellationToken = default);

		// 1.d. Xóa một thẻ theo mã cho trước.
		Task<bool> DeleteTagByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 1.e. Tìm một chuyên mục (Category) theo tên định danh (slug).
        Task<Category> FindCategoryBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 1.f. Tìm một chuyên mục theo mã số cho trước.
        Task<Category> FindCategoryByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 1.g. Thêm hoặc cập nhật một chuyên mục/chủ đề. 
        Task AddOrUpdateCategoryAsync(
            Category category,
            CancellationToken cancellationToken = default);
        Task<Category> CreateOrUpdateCategoryAsync(
            Category category,
            CancellationToken cancellationToken = default);
      

        // 1.h. Xóa một chuyên mục theo mã số cho trước.
        Task<bool> DeleteCategoryByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 1.i. Kiểm tra tên định danh (slug) của
        // một chuyên mục đã tồn tại hay chưa.
        Task<bool> IsCategorySlugExistedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default);

        // 1.j. Lấy và phân trang danh sách chuyên mục,
        // kết quả trả về kiểu IPagedList<CategoryItem>.
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        // 1.k. Đếm số lượng bài viết trong N tháng gần nhất.
        // N là tham số đầu vào. Kết quả là một danh sách
        // các đối tượng chứa các thông tin sau:
        // Năm, Tháng, Số bài viết.
        Task<IList<PostItem>> CountPostsNMonthAsync(
            int n,
            CancellationToken cancellationToken = default);

        // 1.l. Tìm một bài viết theo mã số
        Task<Post> FindPostByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> DeletePostByIdAsync(
            int id, CancellationToken cancellationToken = default);

        // 1.m. Thêm hay cập nhật một bài viết.
        Task AddOrUpdatePostAsync(
            Post post,
            List<string> tags = null,
            CancellationToken cancellationToken = default);

        Task<Post> CreateOrUpdatePostAsync(
            Post post,
            IEnumerable<string> tags,
            CancellationToken cancellationToken = default);

		// 1.n. Chuyển đổi trạng thái Published của bài viết. 
		Task<bool> SwitchPublisedAsync(
            int id,
            CancellationToken cancellationToken = default);


        // 1.o. Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
        Task<IList<Post>> GetRandomNPostsAsync(
            int n,
            CancellationToken cancellationToken = default);

        // 1.p. Tạo lớp PostQuery để lưu trữ các điều kiện
        // tìm kiếm bài viết. Chẳng hạn: mã tác giả, mã chuyên mục,
        // tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, ...

        // 1.q. Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được
        // cho trong đối tượng PostQuery(kết quả trả về kiểu IList<Post>).
        Task<IList<Post>> FindAllPostsByPostQueryAsync(
            PostQuery pq,
            CancellationToken cancellationToken = default);

        // 1.r. Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery.
        Task<int> CountPostsByPostQueryAsync(
            PostQuery pq,
            CancellationToken cancellationToken = default);

        Task<int> GetTotalPostsAsync(
            CancellationToken cancellationToken = default);

        Task<int> NumberPostsUnpublishedAsync(
            CancellationToken cancellationToken = default);

        Task<int> NumberCategoriesAsync(
            CancellationToken cancellationToken = default);

        Task<int> NumberCommentsUnApprovedAsync(
            CancellationToken cancellationToken = default);

        // 1.s.Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        Task<IPagedList<Post>> GetPagedPostQueryAsync(
            PostQuery pq,
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPagedPostQueryAsync(
            PostQuery pq,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);



        IQueryable<Post> FilterPost(PostQuery pq);

        // 1.t. Tương tự câu trên nhưng yêu cầu trả về kiểu IPagedList<T>.Trong đó T là kiểu dữ liệu của đối tượng mới được tạo từ đối tượng Post.Hàm này có thêm một đầu vào là Func<IQueryable<Post>, IQueryable<T>> mapper để ánh xạ các đối tượng Post thành các đối tượng T theo yêu cầu.
        Task<IPagedList<T>> GetPagedPostQueryAsync<T>(
            PostQuery pq,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default);

        Task<IPagedList<Comment>> GetPagedCommentAsync(
            int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);

		Task<Comment> GetCommentByIdAsync(
			int id,
			CancellationToken cancellationToken = default);

        Task<Comment> CreateCommentAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

		Task<bool> ToggleApprovedAsync(
            int id = 0,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteCommentByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
