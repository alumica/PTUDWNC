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


Console.ReadLine();