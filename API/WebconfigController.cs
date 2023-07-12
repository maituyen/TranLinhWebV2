using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebconfigController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public WebconfigController(DatabaseContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("all")]
        public dynamic GetAll()
        {
            try
            {
                var data = _context.WebConfigs.Where(m => m.Status == 1)
                    .OrderBy(m => m.Sort)
                    .Select(m => new
                    {
                        m.Name,
                        m.Id,
                        m.Code,
                        m.Sort,
                        WebConfigProducts = from wf in m.WebConfigProducts
                                            select new
                                            {
                                                wf.ProductId,
                                                wf.WebConfigId,
                                                Images = wf.Product.Images,
                                                Name = wf.Product.Name,
                                                Sale = wf.Product.Sale,
                                                wf.Product.ProductDetails,
                                                KiotVietPrice = wf.Product.KiotVietPrice
                                            },
                        WebConfigKeywords = from wk in m.WebConfigKeywords
                                            select new
                                            {
                                                wk.WebconfigId,
                                                wk.KeywordId,
                                                wk.Index,
                                                Id = wk.Keyword.Id,
                                                Name = wk.Keyword.Name,
                                                Link = wk.Keyword.Link,
                                                LinkImage = wk.Keyword.LinkImage,
                                                wk.Keyword.Description
                                            }
                    })
                    .ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("all/{status}")]
        public dynamic GetStatus(int status)
        {
            try
            {
                var data = _context.WebConfigs
                    .Include(m => m.WebConfigProducts).ThenInclude(m => m.Product).ThenInclude(m => m.Images)
                    .Include(m => m.WebConfigKeywords).ThenInclude(m => m.Keyword)
                    .OrderBy(m => m.Sort)
                    .Where(m => m.Status == status)
                    .ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("key/hot")]
        public dynamic GetKeyHot()
        {
            try
            {
                var data = _context.WebConfigs.Where(m => m.Status == 4)
                    .Include(m => m.WebConfigKeywords).ThenInclude(m => m.Keyword)
                    .OrderBy(m => m.Sort)
                    .ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("hot")]
        public dynamic GetWebconfigHot()
        {
            try
            {
                var data = _context.WebConfigs.Where(m => m.Status == 2)
                    .Include(m => m.WebConfigProducts).ThenInclude(m => m.Product).ThenInclude(m => m.ProductDetails)
                    .OrderBy(m => m.Sort)
                    .ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("top")]
        public dynamic GetWebconfigTop()
        {
            try
            {
                var data = _context.WebConfigs.Where(m => m.Status == 3)
                    .Include(m => m.WebConfigKeywords).ThenInclude(m => m.Keyword)
                    .Include(m => m.WebConfigImages).ThenInclude(m => m.Banner)
                    .OrderBy(m => m.Sort)
                    .ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("add/status")]
        public dynamic AddNewStatus(dynamic obj)
        {
            try
            {
                var web = new WebConfig
                {
                    Name = obj.Name,
                    Status = (int)obj.Status,
                    Sort = (int)obj.Sort,
                    Code = obj.Code,

                };
                _context.WebConfigs.Add(web);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public dynamic AddNew(dynamic obj)
        {
            try
            {
                var web = new WebConfig
                {
                    Name = obj.Name,
                    Status = 1,
                    Sort = (int)obj.Sort

                };
                _context.WebConfigs.Add(web);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("update/{id}")]
        public dynamic Update(dynamic obj, int id)
        {
            try
            {
                var item = _context.WebConfigs.Find(id);
                item.Name = obj.Name;
                item.Sort = (int)obj.Sort;
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("remove/{id}")]
        public dynamic RemoveItem(int id)
        {
            try
            {
                var item = _context.WebConfigs.Find(id);
                _context.WebConfigs.Remove(item);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("remove/product")]
        public dynamic RemoveProduct(dynamic obj)
        {
            try
            {
                int productId = (int)obj.ProductId;
                int webConfigId = (int)obj.WebConfigId;
                var item = _context.WebConfigProducts.FirstOrDefault(m => m.ProductId == productId && m.WebConfigId == webConfigId);
                _context.WebConfigProducts.Remove(item);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
   /*     [Authorize]
        [HttpPost]
        [Route("remove/keyword")]
        public dynamic RemoveKeyword(dynamic obj)
        {
            try
            {
                int keywordId = (int)obj.KeywordId;
                int webConfigId = (int)obj.WebConfigId;
                var item = _context.WebConfigKeywords.FirstOrDefault(m => m.KeywordId == keywordId && m.WebconfigId == webConfigId);
                _context.WebConfigKeywords.Remove(item);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/
        [Authorize]
        [HttpPost]
        [Route("update/products")]
        public dynamic UpdateProduct(dynamic obj)
        {
            try
            {
                List<int> lists = obj["List"].ToObject<List<int>>();
                int Id = (int)obj.WebConfigId;
                var items = _context.WebConfigProducts.Where(m => m.WebConfigId == Id);
                foreach (var i in items)
                {
                    _context.WebConfigProducts.Remove(i);
                }
                foreach (var id in lists)
                {
                    var webProduct = new WebConfigProduct
                    {
                        Index = 1,
                        ProductId = id,
                        WebConfigId = (int)obj.WebConfigId
                    };
                    _context.WebConfigProducts.Add(webProduct);
                }
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update/keywords")]
        public dynamic UpdateKeywords(dynamic obj)
        {
            try
            {
                List<int> lists = obj["List"].ToObject<List<int>>();
                int Id = (int)obj.WebConfigId;
                var items = _context.WebConfigKeywords.Where(m => m.WebconfigId == Id);
                foreach (var i in items)
                {
                    _context.WebConfigKeywords.Remove(i);
                }
                foreach (var id in lists)
                {
                    var webKey = new WebConfigKeyword
                    {
                        Index = 1,
                        KeywordId = id,
                        WebconfigId = (int)obj.WebConfigId
                    };
                    _context.WebConfigKeywords.Add(webKey);
                }
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("remove/keyword")]
        public dynamic RemoveWfKeyword(dynamic obj)
        {
            try
            {
                int KeywordId = (int)obj.KeywordId;
                int WebconfigId = (int)obj.WebconfigId;
                var item = _context.WebConfigKeywords.FirstOrDefault(m => m.KeywordId == KeywordId && m.WebconfigId == WebconfigId);
                _context.WebConfigKeywords.Remove(item);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update/image")]
        public dynamic UpdateImage(dynamic obj)
        {
            try
            {
                List<int> lists = obj["List"].ToObject<List<int>>();
                int Id = (int)obj.WebConfigId;
                var items = _context.WebConfigImages.Where(m => m.WebconfigId == Id);
                foreach (var i in items)
                {
                    _context.WebConfigImages.Remove(i);
                }
                foreach (var id in lists)
                {
                    var webKey = new WebConfigImage
                    {
                        Index = 1,
                        BannerId = id,
                        WebconfigId = (int)obj.WebConfigId
                    };
                    _context.WebConfigImages.Add(webKey);
                }
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("remove/image")]
        public dynamic RemoveImageSelected(dynamic obj)
        {
            try
            {
                int ImageId = (int)obj.BannerId;
                int WebconfigId = (int)obj.WebconfigId;
                var item = _context.WebConfigImages.FirstOrDefault(m => m.BannerId == ImageId && m.WebconfigId == WebconfigId);
                _context.WebConfigImages.Remove(item);
                _context.SaveChanges();
                return Ok(new { Message = "Thành công", Status = true });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
