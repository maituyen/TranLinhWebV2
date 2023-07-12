using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Constants;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.ViewModels;

namespace MyProject.API
{
    [ApiController]
    public class BlogController : ApiBaseController
    {
        private readonly DatabaseContext _context;
        public BlogController(DatabaseContext context)
        {
            _context = context;
        }

        #region Optimize code

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging(
            int? pageSize, 
            int? pageIndex, 
            string? search, 
            int? categoryId
        )
        {
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? PageConstant.PageSize;

            var query = _context.Blogs
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            var data = await query
                .Skip((int)((pageIndex - 1) * pageSize))
                .Take((int)pageSize)
                .ToListAsync();

            var result = new Pagination<Blog>
            {
                Items = data,
                PageIndex = (int)pageIndex,
                PageSize = (int)pageSize,
                TotalRecords = query.Count()
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var result = new ApiResponse<Blog>();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return BadRequest(new ApiBadRequestResponse<Blog>($"Can not find blog with {id}"));

            result.Data = blog;
            return Ok(result);
        }

        #endregion


        [HttpGet]
        [Route("pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public dynamic GetAll(int pageSize, int pageNumber)
        {
            var data = _context.Blogs.Include(m => m.Category);

            return Ok(new
            {
                data = data.OrderByDescending(m => m.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                count = data.Count()
            });
        }

        [HttpGet]
        [Route("category/{categoryId}/pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public dynamic GetBlogByCategory(int pageSize, int pageNumber, int categoryId)
        {
            try
            {
                var data = _context.Blogs.Include(m => m.Category).Where(m => m.CategoryId == categoryId && m.IsPublish == true);

                return Ok(new
                {
                    data = data.OrderByDescending(m => m.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                    count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("hot/{status}")]
        public dynamic GetBlogHot(int status)
        {
            try
            {
                var data = _context.BlogHomes.Include(m => m.Blog).Where(m => m.Status == status);

                return Ok(new
                {
                    data = data.OrderBy(m => m.NumericOrder).ToList(),
                    count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("remove/bloghome/{id}")]
        public dynamic RemoveItem(int id)
        {
            try
            {
                var data = _context.BlogHomes.FirstOrDefault(m => m.Id == id);
                if (data != null)
                {
                    _context.BlogHomes.Remove(data);
                    _context.SaveChanges();
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public dynamic AddNew(dynamic obj)
        {
            try
            {

                var blog = new Blog
                {
                    Title = obj["Title"],
                    //  Body = obj["Body"],
                    // CategoryId = (int)obj["CategoryId"],
                    IsPublish = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Views = 0,
                    IsHot = false,
                };
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update/image")]
        public dynamic UploadThumbnail([FromForm] FileUploadVm objFile)
        {
            try
            {
                var item = _context.Blogs.Find(objFile.Id);
                if (item == null)
                {
                    return BadRequest();
                }
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(objFile.File.FileName);
                item.Thumbnail = ImageName;

                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/upload", ImageName);
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    objFile.File.CopyTo(stream);
                }

                _context.SaveChanges();

                return Ok(new { Message = "Thành công", Status = true, Blog = item });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update")]
        public dynamic Update(dynamic obj)
        {
            try
            {
                int id = (int)obj.Id;
                var item = _context.Blogs.FirstOrDefault(m => m.Id == id);
                if (item == null)
                {
                    return BadRequest();
                }
                item.Title = obj.Title;
                item.Description = obj.Description;
                item.Body = obj.Body;
                item.IsPublish = obj.IsPublish;
                item.CategoryId = (int)obj.CategoryId;
                item.UpdatedAt = DateTime.Now;
                List<int> tags = obj["Products"].ToObject<List<int>>();
                List<TagBlog> tagBlogs = new List<TagBlog>();
                foreach (var tag in tags)
                {
                    var inputTag = new TagBlog
                    {
                        BlogTagId = tag,
                        BlogId = id,
                    };
                    tagBlogs.Add(inputTag);
                }

                _context.TagBlogs.AddRangeAsync(tagBlogs);
                _context.SaveChangesAsync();


                return Ok(new { Message = "Thành công", Status = true, Blog = item });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("update/taghome")]
        public dynamic UpdateTagHome(dynamic obj)
        {
            try
            {
                List<int> tags = obj["Products"].ToObject<List<int>>();
                int status = (int)obj.Status;
                List<BlogHome> blogHomes = new List<BlogHome>();
                var items = _context.BlogHomes.Where(m => m.Status == status).ToList();
                foreach (var item in items)
                {
                    blogHomes.Add(item);
                }
                _context.BlogHomes.RemoveRange(blogHomes);
                _context.SaveChanges();

                List<BlogHome> blogHomeInputs = new List<BlogHome>();
                foreach (var tag in tags)
                {
                    var input = new BlogHome
                    {
                        BlogId = tag,
                        NumericOrder = (int)obj.NumericOrder,
                        Status = (int)obj.Status,
                    };
                    blogHomeInputs.Add(input);
                }
                _context.BlogHomes.AddRangeAsync(blogHomeInputs);
                _context.SaveChanges();

                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("bloghome/order")]
        public dynamic UpdateOrderBy(dynamic obj)
        {
            try
            {
                List<dynamic> tags = obj["Products"].ToObject<List<dynamic>>();
                foreach (var item in tags)
                {
                    int Id = item.Id;
                    var checkItem = _context.BlogHomes.FirstOrDefault(m => m.Id == Id);
                    if (checkItem != null)
                    {
                        checkItem.NumericOrder = (int)item.NumericOrder;
                        _context.SaveChanges();

                    }

                }

                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
