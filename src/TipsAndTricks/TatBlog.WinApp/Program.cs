using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

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

var ctg = await blogRepo.FindCategoryWithIdAsync(4);

var authors = context.Authors.ToList();
foreach (var author in authors)
{
    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,-12}",
        author.Id, author.FullName, author.Email, author.JoinedDate);
}
  await blogRepo.DeleteTagWithIdAsync(2);

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

var findtag = await blogRepo.FindTagWithSlugAsync("google-application");

Console.WriteLine(findtag.Id + " " + findtag.Name);

Console.ReadLine();