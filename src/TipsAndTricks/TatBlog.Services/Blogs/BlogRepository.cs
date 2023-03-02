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

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        // Tìm bài viết có tên định danh là 'slug'
        // và được đăng vào tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // Tìm Top N bài viết phổ biến được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
                cancellationToken);
        }

        // Tăng số lượng lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                    cancellationToken);
        }

        // Lấy danh sách chuyện mục và số lương bài viết
        // nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                { 
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                }).ToListAsync(cancellationToken);
        }

        // Lấy danh sách từ khóa/thẻ và phân trang theo
        // các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }


        // C. Bài tập thực hành
        // 1.
        // 1.a. Tìm một thẻ (Tag) theo tên định danh (slug).
        public async Task<Tag> FindTagWithSlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagsQuery = _context.Set<Tag>();
            if (!string.IsNullOrWhiteSpace(slug))
            {
                tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
            }
            return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // 1.c. Lấy danh sách tất cả các thẻ (Tag) kèm theo
        // số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
        public async Task<IList<TagItem>> GetTagItemsAsync(
            CancellationToken cancellationToken = default)
        {
            var tagItemsQuery = _context.Set<Tag>();
            return await tagItemsQuery
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description= x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        // 1.d. Xóa một thẻ theo mã cho trước.
        public async Task DeleteTagWithIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            //await _context.Set<Tag>()
            //    .Include(t => t.Posts)
            //    .Where(x => x.Id == id)
            //    .ExecuteDeleteAsync(cancellationToken);

            //await _context.Set<Tag>()
            //    .Where(t => t.Id == id)
            //    .ExecuteDeleteAsync(cancellationToken);


           await _context.Database
               .ExecuteSqlRawAsync("DELETE FROM PostTags WHERE TagId = " + id, cancellationToken);

            await _context.Set<Tag>()
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        // 1.e. Tìm một chuyên mục (Category) theo tên định danh (slug).
        public async Task<Category> FindCategoryWithSlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categoriesQuery = _context.Set<Category>();
            if (!string.IsNullOrWhiteSpace(slug))
            {
                categoriesQuery = categoriesQuery.Where(x => x.UrlSlug == slug);
            }
            return await categoriesQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // 1.f. Tìm một chuyên mục theo mã số cho trước.
        public async Task<Category> FindCategoryWithIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        // 1.g. Thêm hoặc cập nhật một chuyên mục/chủ đề. 
        public async Task AddOrUpdateCategoryAsync(
            Category category,
            CancellationToken cancellationToken = default)
        {
            if (IsCategorySlugExistedAsync(category.Id, category.UrlSlug).Result)
                Console.WriteLine("Error: Exsited Slug");
            else

                if (category.Id > 0) // true: update || false: add
                    await _context.Set<Category>()
                    .Where(c => c.Id == category.Id)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(x => x.Name, x => category.Name)
                        .SetProperty(x => x.UrlSlug, x => category.UrlSlug)
                        .SetProperty(x => x.Description, x => category.Description)
                        .SetProperty(x => x.ShowOnMenu, category.ShowOnMenu)
                        .SetProperty(x => x.Posts, category.Posts),
                        cancellationToken);
                else
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
        }

        // 1.h. Xóa một chuyên mục theo mã số cho trước.
        public async Task DeleteCategoryWithIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            //await _context.Database
            //   .ExecuteSqlRawAsync("DELETE FROM PostTags WHERE TagId = " + id, cancellationToken);

            //await _context.Set<Tag>()
            //    .Where(t => t.Id == id)
            //    .ExecuteDeleteAsync(cancellationToken);
        }

        // 1.i. Kiểm tra tên định danh (slug) của
        // một chuyên mục đã tồn tại hay chưa.
        public async Task<bool> IsCategorySlugExistedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug,
                cancellationToken);
        }

        // 1.j. Lấy và phân trang danh sách chuyên mục,
        // kết quả trả về kiểu IPagedList<CategoryItem>.
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var categoryQuery = _context.Set<Category>()
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // 1.k. Đếm số lượng bài viết trong N tháng gần nhất.
        // N là tham số đầu vào. Kết quả là một danh sách
        // các đối tượng chứa các thông tin sau:
        // Năm, Tháng, Số bài viết.
        public async Task<IList<PostItem>> CountPostsNMonthAsync(int n, CancellationToken cancellationToken = default)
        {
            
            throw new NotImplementedException();
        }

        // 1.l. Tìm một bài viết theo mã số
        public async Task<Post> FindPostWithIdAsync(
            int id, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // 1.m. Thêm hay cập nhật một bài viết.
        public async Task AddOrUpdatePostAsync(
            Post post,
            CancellationToken cancellationToken = default)
        {
            if (IsPostSlugExistedAsync(post.Id, post.UrlSlug).Result)
                Console.WriteLine("Error: Existed Slug");
            else

            if (post.Id > 0) // true: update || false: add
                await _context.Set<Post>()
                .Where(p => p.Id == post.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.Title, x => post.Title)
                    .SetProperty(x => x.UrlSlug, x => post.UrlSlug)
                    .SetProperty(x => x.ShortDescription, x => post.ShortDescription)
                    .SetProperty(x => x.Description, x => post.Description)
                    .SetProperty(x => x.Meta, x => post.Meta)
                    .SetProperty(x => x.ImageUrl, x => post.ImageUrl)
                    .SetProperty(x => x.ViewCount, x => post.ViewCount)
                    .SetProperty(x => x.Published, x =>  post.Published)
                    .SetProperty(x => x.PostedDate, x => post.PostedDate)
                    .SetProperty(x => x.ModifiedDate, x => post.ModifiedDate)
                    .SetProperty(x => x.CategoryId, x => post.CategoryId)
                    .SetProperty(x => x.AuthorId, x => post.AuthorId)
                    .SetProperty(x => x.Category, x => post.Category)
                    .SetProperty(x => x.Author, x => post.Author)
                    .SetProperty(x => x.Tags, x => post.Tags),
                    cancellationToken);
            else
            {
                _context.AddRange(post);
                _context.SaveChanges();
            }
                
        }

        // 1.n. Chuyển đổi trạng thái Published của bài viết. 
        public async Task SwitchPublisedAsync(
            int id,
            bool b,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Published, p => b),
                cancellationToken);
        }

        // 1.o. Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
        public async Task<IList<Post>> GetRandomNPostsAsync(
            int n,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(n)
                .ToListAsync();
        }

        // 1.p. Tạo lớp PostQuery để lưu trữ các điều kiện
        // tìm kiếm bài viết. Chẳng hạn: mã tác giả, mã chuyên mục,
        // tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, ...

        // 1.q. Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được
        // cho trong đối tượng PostQuery(kết quả trả về kiểu IList<Post>).
        public async Task<IList<Post>> FindAllPostsWithPostQueryAsync(
            PostQuery pq,
            CancellationToken cancellationToken = default)
        {
            // return await _context.Set<Post>().ToPostQueryAsync(pq, cancellationToken);
            throw new NotImplementedException();
        }

        // 1.r. Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery.
        public async Task<int> CountPostsWithPostQueryAsync(PostQuery pq, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        // 1.s.Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        public async Task<IPagedList<Post>> GetPagedPostQueryAsync(PostQuery pq, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
