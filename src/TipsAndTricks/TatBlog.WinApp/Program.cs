
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Authors;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

// Tạo đối tượng DbContext để quản lý phiên làm việc
// với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

// Tạo đối tượng khởi tạo dữ liệu
var seeder = new DataSeeder(context);

// Gọi hàm Initialize để nhập dữ liệu mẫu
seeder.Initialize();

// Tạo đối tượng BlogRepository
IBlogRepository blogRepo = new BlogRepository(context);

var tagItems = await blogRepo.GetTagItemsAsync();

foreach (var item in tagItems)
{
    Console.WriteLine("{0,-5}{1,-50}{2,-10}",
        item.Id, item.Name, item.PostCount);
}


var authors = context.Authors.ToList();
foreach (var author in authors)
{
    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,-12}",
        author.Id, author.FullName, author.Email, author.JoinedDate);
}
 // await blogRepo.DeleteTagWithIdAsync(2);

var posts = context.Posts
    .Where(x => x.Published)
    .OrderBy(x => x.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name,
        Tags = p.Tags,
    })
    .ToList();

foreach (var post in posts)
{
    Console.WriteLine("ID       :{0}", post.Id);
    Console.WriteLine("Title    :{0}", post.Title);
    Console.WriteLine("View     :{0}", post.ViewCount);
    Console.WriteLine("Date     :{0}:MM/dd/yyyy", post.PostedDate);
    Console.WriteLine("Author   :{0}", post.Author);
    Console.WriteLine("Category :{0}", post.Category);
    Console.WriteLine("Tags :{0}", post.Tags.Count);
    Console.WriteLine("".PadRight(80, '-'));
}





var nmonth = await blogRepo.CountPostsNMonthAsync(3);

foreach (var post in nmonth)
{
    Console.WriteLine("Year       :{0}", post.Year);
    Console.WriteLine("Month    :{0}", post.Month);
    Console.WriteLine("Count     :{0}", post.PostCount);
    Console.WriteLine("".PadRight(80, '-'));
}
// 1.a. Tìm một thẻ (Tag) theo tên định danh (slug).
async void Cau1a()
{
    string slug = "";
    Tag t = await blogRepo.FindTagBySlugAsync(slug);
    Console.WriteLine("{0,-5}{1,-50}{2,-10}",
        "ID", "Name", "PostCount");
    Console.WriteLine("{0,-5}{1,-50}{2,-10}",
        t.Id, t.Name, t.Posts.Count);
}

// 1.c. Lấy danh sách tất cả các thẻ (Tag) kèm theo
// số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
async void Cau1c()
{
    var list = await blogRepo.GetTagItemsAsync();
    foreach (var item in list)
    {
        Console.WriteLine("{0,-5}{1,-50}{2,-20}{3,-20}{4,-5}",
            item.Id, item.Name, item.UrlSlug, item.Description, item.PostCount);
    }
}

// 1.d. Xóa một thẻ theo mã cho trước.
async void Cau1d()
{
    int id = 3;
    await blogRepo.DeleteCategoryByIdAsync(id);
}

// 1.e. Tìm một chuyên mục (Category) theo tên định danh (slug).
async void Cau1e()
{
    string slug = "";
    Category c = await blogRepo.FindCategoryBySlugAsync(slug);
    Console.WriteLine("{0,-5}{1,-50}{2,-20}{3,-20}{4,-5}",
            c.Id, c.Name, c.UrlSlug, c.Description, c.Posts.Count);
}

// 1.f. Tìm một chuyên mục theo mã số cho trước.
async void Cau1f()
{
    int id = 3;
    Category c = await blogRepo.FindCategoryByIdAsync(id);
    Console.WriteLine("{0,-5}{1,-50}{2,-20}{3,-20}{4,-5}",
            c.Id, c.Name, c.UrlSlug, c.Description, c.Posts.Count);
}

// 1.g. Thêm hoặc cập nhật một chuyên mục/chủ đề.
async void Cau1g()
{
    Category c = new Category()
    {
        Id = 0,
        Name = "AAA",
        UrlSlug = "aaa",
        Description = "aa",
        ShowOnMenu = true,

    };
    await blogRepo.AddOrUpdateCategoryAsync(c);
}

// 1.h. Xóa một chuyên mục theo mã số cho trước.
async void Cau1h()
{
    int id = 3;
    await blogRepo.DeleteCategoryByIdAsync(id);
}

// 1.i.Kiểm tra tên định danh (slug) của
// một chuyên mục đã tồn tại hay chưa.
async void Cau1i()
{
    int id = 3;
    string slug = "";
    Console.WriteLine(await blogRepo.IsCategorySlugExistedAsync(id, slug));
}

// 1.j. Lấy và phân trang danh sách chuyên mục,
// kết quả trả về kiểu IPagedList<CategoryItem>.
async void Cau1j()
{
    var pagingParams = new PagingParams()
    {
        PageNumber = 1,
        PageSize = 5,
        SortColumn = "Name",
        SortOrder = "DESC"
    };

    var categoriesList = await blogRepo.GetPagedCategoriesAsync(pagingParams);
    Console.WriteLine("{0,-5}{1,-50}{2,10}",
        "ID", "Name", "Count");
    foreach (var item in categoriesList)
    {
        Console.WriteLine("{0,-5}{1,-50}{2,10}",
            item.Id, item.Name, item.PostCount);
    }
}

// 1.k. Đếm số lượng bài viết trong N tháng gần nhất.
// N là tham số đầu vào. Kết quả là một danh sách
// các đối tượng chứa các thông tin sau:
// Năm, Tháng, Số bài viết.

// 1.l. Tìm một bài viết theo mã số
async void Cau1l()
{
    int id = 3;
    Post post = await blogRepo.FindPostByIdAsync(id);
    Console.WriteLine("ID       :{0}", post.Id);
    Console.WriteLine("Title    :{0}", post.Title);
    Console.WriteLine("View     :{0}", post.ViewCount);
    Console.WriteLine("Date     :{0}:MM/dd/yyyy", post.PostedDate);
    Console.WriteLine("Author   :{0}", post.Author);
    Console.WriteLine("Category :{0}", post.Category);
    Console.WriteLine("Tags :{0}", post.Tags.Count);
    Console.WriteLine("".PadRight(80, '-'));
}

// 1.m. Thêm hay cập nhật một bài viết.
async void Cau1m()
{
    Post post = new Post()
    {

    };
    await blogRepo.AddOrUpdatePostAsync(post);
}

// 1.n. Chuyển đổi trạng thái Published của bài viết. 
async void Cau1n()
{
    int id = 3;
    await blogRepo.SwitchPublisedAsync(id);
}

// 1.o. Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
async void Cau1o()
{
    int id = 3;
    var random = await blogRepo.GetRandomNPostsAsync(id);

    foreach (var post in random)
    {
        Console.WriteLine("ID       :{0}", post.Id);
        Console.WriteLine("Title    :{0}", post.Title);
        Console.WriteLine("View     :{0}", post.ViewCount);
        Console.WriteLine("Date     :{0}:MM/dd/yyyy", post.PostedDate);
        Console.WriteLine("Author   :{0}", post.Author);
        Console.WriteLine("Category :{0}", post.Category);
        Console.WriteLine("Tags :{0}", post.Tags.Count);
        Console.WriteLine("".PadRight(80, '-'));
    }

}

// 1.q.Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được
// cho trong đối tượng PostQuery(kết quả trả về kiểu IList<Post>).
async void Cau1q()
{
    PostQuery pq = new PostQuery()
    {
        AuthorId = 3
    };
    var posts = await blogRepo.FindAllPostsByPostQueryAsync(pq);

    foreach (var post in posts)
    {
        Console.WriteLine("ID       :{0}", post.Id);
        Console.WriteLine("Title    :{0}", post.Title);
        Console.WriteLine("View     :{0}", post.ViewCount);
        Console.WriteLine("Date     :{0}:MM/dd/yyyy", post.PostedDate);
        Console.WriteLine("Author   :{0}", post.Author);
        Console.WriteLine("Category :{0}", post.Category);
        //Console.WriteLine("Tags :{0}", post.Tags.Count);
        Console.WriteLine("".PadRight(80, '-'));
    }
}

// 1.r. Đếm số lượng bài viết thỏa mãn điều kiện
// tìm kiếm được cho trong đối tượng PostQuery.
async void Cau1r()
{
    PostQuery pq = new PostQuery()
    {
        AuthorId = 3
    };
    int count = await blogRepo.CountPostsByPostQueryAsync(pq);
    Console.WriteLine("Count post: ", count);
}


// 2.
IAuthorRepository authorRepo = new AuthorRepository(context);
// 2.b. Tìm một tác giả theo mã số.
async void Cau2b()
{
    int id = 3;
    Author author = await authorRepo.FindAuthorByIdAsync(id);
    Console.WriteLine();
}
Console.ReadLine();