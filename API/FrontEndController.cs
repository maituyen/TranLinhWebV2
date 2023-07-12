using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyProject.Constants;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Extensions;
using MyProject.Helpers;
using MyProject.ViewModels;
using MyProject.ViewModels.Product;
using Newtonsoft.Json; 
namespace MyProject.API
{
    [Route("api/[controller]")] 
    public class FrontEndController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        public FrontEndController(IConfiguration configuration)
        {
            _configuration = configuration; 
        } 
        [HttpGet]
        [Route("home/chatbotgpt")]
        public async Task<dynamic> ChatBotGPT(string key)
        {
        SiteInfo siteInfo = new SiteInfo(Request.Host.Host);
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("authorization", "Bearer "+siteInfo.ChatGPTAPIKey);
                    var requestBody = new
                    {
                        model = "text-davinci-003",
                        prompt = key,
                        top_p = 1,
                        max_tokens = 4000
                    };
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

                    if (response.IsSuccessStatusCode == true)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        try
                        {
                            Models.ChatGPT.AskResponse? askResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ChatGPT.AskResponse>(responseString);
                            if (askResponse == null) return Ok("Lỗi !");
                            return Ok(askResponse.Choices[0].Text);
                        }
                        catch (Exception e)
                        { 
                            return Ok(e.Message);
                        }
                    }


                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Lấy toàn bộ cấu hình trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("home/config")]
        public dynamic GetHomeConfig(int Status = -1)
        {
            try
            {
                Models.WebConfig webConfig = new Models.WebConfig();
                var data = webConfig.GetAll(Status); 
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("home/config/id")]
        public dynamic GetHomeConfigByID(int id)
        {
            try
            {
                Models.WebConfig webConfig = new Models.WebConfig(id); 
                return Ok(webConfig);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("home/config/code")]
        public dynamic GetHomeConfigByCode(string Code)
        {
            try
            {
                Models.WebConfig webConfig = new Models.WebConfig();

                return Ok(webConfig.GetAllByCode(Code));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("home/menu")]
        public dynamic GetMenu()
        {
            try
            {
                Models.Category category = new Models.Category();

                return Ok(category.Menu());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("home/search_autocomplete")]
        public dynamic SearchAutocomplete(string KeyWord)
        {
            try
            {
                Models.FontEndHelpers.SearchAutoComplete searchAutoComplete = new Models.FontEndHelpers.SearchAutoComplete();

                return Ok(searchAutoComplete.Search(KeyWord));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("footer/getbycode")]
        public dynamic GetFooterByCode(string Code)
        {
            try
            {
                Models.Footer footer = new Models.Footer();

                return Ok(footer.GetAll(Code));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
        [HttpGet]
        [Route("categories/fillter/control")]
        public dynamic FillterControl()
        {
            try
            {
                Models.MetaData metaData = new Models.MetaData();
                return Ok(metaData.GetAllowFillter());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
        [HttpGet]
        [Route("home/top")]
        public dynamic GetTopHome()
        {
            try
            {

                var query = from webconfig in _context.WebConfigs
                            where webconfig.Status == 3
                            select new FrontEndTopVm
                            {
                                Code = webconfig.Code,
                                Sort = webconfig.Sort,
                                WebConfigImages = webconfig
                                                .WebConfigImages
                                                .Select(x => new WebConfigImagesVm
                                                {
                                                    LinkImage = x.Banner.LinkImage,
                                                    Name = x.Banner.Name
                                                }),
                                WebConfigKeywords = webconfig
                                                .WebConfigKeywords
                                                .Select(x => new WebConfigKeywordsVm
                                                {
                                                    Name = x.Keyword.Name,
                                                    Description = x.Keyword.Description,
                                                    LinkImage = x.Keyword.LinkImage,
                                                    CategoryId = x.Keyword.CategoryId,
                                                    Slug = x.Keyword.Category.Slug
                                                })
                            };

                var data = query.OrderBy(m => m.Sort).ToList();

                foreach (var item in data)
                {
                    item.WebConfigKeywords = item.WebConfigKeywords
                        .Select(keyWord =>
                        {
                            keyWord.Slug = ConvertUrl(keyWord.Slug);
                            return keyWord;
                        });
                }

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("home/footer")]
        public async Task<IActionResult> GetFooter()
        {
            var query = _context.WebConfigs
                    .Where(m => m.Status == 5)
                    .Select(m => new FrontEndFooterVm
                    {
                        Sort = m.Sort,
                        Name = m.Name,
                        WebConfigKeywords = m.WebConfigKeywords.Select(x => new WebConfigKeywordsVm
                        {
                            Name = x.Keyword.Name,
                            Description = x.Keyword.Description,
                            LinkImage = x.Keyword.LinkImage,
                            CategoryId = x.Keyword.CategoryId,
                            Slug = x.Keyword.Category.Slug
                        }).ToList()
                    });

            var data = await query
                .OrderBy(m => m.Sort)
                .ToListAsync();

            foreach (var item in data)
            {
                foreach (var keyWord in item.WebConfigKeywords)
                {
                    keyWord.Slug = ConvertUrl(keyWord.Slug);
                }
            }
            return Ok(data);
        } 
        [HttpGet]
        [Route("home/hot")]
        public async Task<IActionResult> GetProductHot()
        {
            try
            {
                var query = _context.WebConfigs.Where(m => m.Status == 2)
                    .OrderBy(m => m.Sort)
                     .Select(m => new WebConfigVm
                     {
                         Name = m.Name,
                         Id = m.Id,
                         Code = m.Code,
                         Sort = m.Sort,
                         WebConfigProducts = m.WebConfigProducts
                                              .Select(wf => new WebConfigProductsVm
                                              {
                                                  Product = new ProductConfigVm
                                                  {
                                                      ProductId = wf.ProductId,
                                                      WebConfigId = wf.WebConfigId,
                                                      ProductDetails = wf.Product.ProductDetails,
                                                      Name = wf.Product.Name,
                                                      Sale = wf.Product.Sale,
                                                      KiotVietPrice = wf.Product.KiotVietPrice,
                                                      Prepay = wf.Product.Prepay,
                                                      Id = wf.Product.Id,
                                                      Annotate = wf.Product.Annotate,
                                                      SeoUrl = wf.Product.SeoUrl,
                                                      AdvertisementDetail = wf.Product.Category.AdvertisementDetail,
                                                      AdvertisementLarge = wf.Product.Category.AdvertisementLarge,
                                                      AdvertisementSmall = wf.Product.Category.AdvertisementSmall,
                                                      Events = wf.Product.Category.Events,
                                                      ProductEvents = wf.Product.Events
                                                  }
                                              })

                     });

                var queryHasTag = _context
                                .Hastags?
                                .Where(x => x.Type == (int)EnumTypeHastag.Text)
                                .ToDictionary(x => x.Code ?? "", y => y.Name);
                var data = await query
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                foreach (var item in data)
                {
                    item.WebConfigProducts = item.WebConfigProducts
                        .Select(product =>
                        {
                            if (product.Product != null)
                            {
                                product.Product.Name = GetValueHastag(product.Product.Name, queryHasTag);
                            }
                            return product;
                        });
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
        [HttpGet]
        [Route("home/body")]
        public async Task<IActionResult> GetProductBody()
        {
            try
            {
                var query = _context.WebConfigs.Where(m => m.Status == 1)
                    .OrderBy(m => m.Sort)
                    .Select(m => new WebConfigVm
                    {
                        Name = m.Name,
                        Id = m.Id,
                        Code = m.Code,
                        Sort = m.Sort,
                        WebConfigProducts = m.WebConfigProducts
                                              .Select(wf => new WebConfigProductsVm
                                              {
                                                  Product = new ProductConfigVm
                                                  {
                                                      ProductId = wf.ProductId,
                                                      WebConfigId = wf.WebConfigId,
                                                      ProductDetails = wf.Product.ProductDetails,
                                                      Name = wf.Product.Name,
                                                      Sale = wf.Product.Sale,
                                                      KiotVietPrice = wf.Product.KiotVietPrice,
                                                      Prepay = wf.Product.Prepay,
                                                      Id = wf.Product.Id,
                                                      Annotate = wf.Product.Annotate,
                                                      SeoUrl = wf.Product.SeoUrl,
                                                      AdvertisementDetail = wf.Product.Category.AdvertisementDetail,
                                                      AdvertisementLarge = wf.Product.Category.AdvertisementLarge,
                                                      AdvertisementSmall = wf.Product.Category.AdvertisementSmall,
                                                      Events = wf.Product.Category.Events,
                                                      ProductEvents = wf.Product.Events
                                                  }
                                              }),
                        WebConfigKeywords = m.WebConfigKeywords
                                            .Select(x => new WebConfigKeywordsVm
                                            {
                                                Name = x.Keyword.Name,
                                                Description = x.Keyword.Description,
                                                LinkImage = x.Keyword.LinkImage,
                                                CategoryId = x.Keyword.CategoryId,
                                                Slug = x.Keyword.Category.Slug
                                            })
                    });
                var queryHasTag = _context
                               .Hastags?
                               .Where(x => x.Type == (int)EnumTypeHastag.Text)
                               .ToDictionary(x => x.Code ?? "", y => y.Name);

                var data = await query
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                foreach (var item in data)
                {

                    item.WebConfigKeywords = item.WebConfigKeywords
                        .Select(keyWord =>
                        {
                            keyWord.Slug = ConvertUrl(keyWord.Slug);
                            return keyWord;
                        });

                    item.WebConfigProducts = item.WebConfigProducts
                        .Select(product =>
                        {
                            if (product.Product != null)
                            {
                                product.Product.Name = GetValueHastag(product.Product.Name, queryHasTag);
                            }
                            return product;
                        });
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("keyword/hot")]
        public async Task<IActionResult> GetKeyHot()
        {
            var query = _context.WebConfigs
                    .Where(m => m.Status == 4)
                    .Select(m => new FrontEndFooterVm
                    {
                        Sort = m.Sort,
                        WebConfigKeywords = m.WebConfigKeywords.Select(x => new WebConfigKeywordsVm
                        {
                            Name = x.Keyword.Name,
                            Description = x.Keyword.Description,
                            LinkImage = x.Keyword.LinkImage,
                            CategoryId = x.Keyword.CategoryId,
                            Slug = x.Keyword.Category.Slug
                        }).ToList()
                    });

            var data = await query.OrderBy(m => m.Sort).ToListAsync();

            foreach (var item in data)
            {
                foreach (var keyWord in item.WebConfigKeywords)
                {
                    keyWord.Slug = ConvertUrl(keyWord.Slug);
                }
            } 
            return Ok(data);
        } 
        [HttpGet]
        [Route("menu")]
        public dynamic GetListMenu()
        {
            try
            {
                var data = _context.Categories
                    .Where(m => m.IsDelete == false && m.Status == 0 && m.IsShow == true)
                    .Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.ParentId,
                        m.Slug,
                        m.Icon,
                        m.SubDescription,
                        m.Description,
                        m.AdvertisementSmall,
                    }).ToList();

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("menu/{status}")]
        public dynamic GetListMenuOld(int status)
        {
            try
            {
                var data = _context.Categories.Include(m => m.Images)
                    .Where(m => m.IsDelete == false && m.Status == status)
                    .Where(m => m.IsShow == true)
                    .Select(m => new
                    {
                        m.Id,
                        m.Name,
                        m.ParentId,
                        m.Note,
                        m.Images,
                        m.Icon,
                        m.SubDescription,
                    }).ToList();

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("category/{id}")]
        public dynamic GetCategoryById(int id)
        {
            try
            {
                var category = _context
                    .Categories
                    .Include(m => m.Banners)
                    .Include(m => m.KeyWords)
                    .FirstOrDefault(m => m.Id == id);

                if (category == null) throw new Exception($"Can not find category with id: {id}");

                var categories = _context.Categories.Where(m => m.IsDelete == false & m.IsShow == true).ToList();
                var level3 = _context.Categories.FirstOrDefault(m => m.Id == category.ParentId);
                if (level3 == null)
                {
                    level3 = new Category();
                }
                var level2 = new Category();
                if (level3 != null)
                {
                    level2 = _context.Categories.FirstOrDefault(m => m.Id == (int)level3.ParentId);
                }
                if (level2 == null)
                {
                    level2 = new Category();
                }
                var level1 = new Category();
                if (level2 != null)
                {
                    level1 = _context.Categories.FirstOrDefault(m => m.Id == (int)level2.ParentId);
                }
                if (level1 == null)
                {
                    level1 = new Category();

                }
                var childrens = new List<Category>();
                if (category.Level != 3)
                {
                    childrens = GetChildren(categories, (int)category.Id);
                }
                else
                {
                    childrens = categories.Where(m => m.ParentId == category.ParentId).ToList();
                }
                return Ok(new
                {

                    Childrens = childrens,
                    Category = category,
                    Categories = categories,
                    Level1 = level1,
                    Level2 = level2,
                    Level3 = level3,
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("get-category-by-slug/{slug}")]
        public dynamic GetCategoryBySlug(string slug)
        {
            try
            {
                var getAllCategories = _context
                    .Categories
                    .Where(c => c.IsDelete == false && c.IsShow == true);

                var category = _context.Categories
                                        .Include(m => m.Banners)
                                        .Include(m => m.KeyWords)
                                        .FirstOrDefault(c => c.Slug == slug && c.IsDelete == false && c.IsShow == true);

                if (category == null) throw new Exception($"Can not find category with slug {slug}.");

                var categoriesRelated = getAllCategories
                                         .Where(c => c.ParentId == category.Id);

                var level3 = getAllCategories.FirstOrDefault(m => m.Id == category.ParentId);
                if (level3 == null)
                {
                    level3 = new Category();
                }

                var level2 = new Category();
                if (level3 != null)
                {
                    level2 = _context.Categories.FirstOrDefault(m => m.Id == (int)level3.ParentId);
                }

                if (level2 == null)
                {
                    level2 = new Category();
                }
                var level1 = new Category();
                if (level2 != null)
                {
                    level1 = _context.Categories.FirstOrDefault(m => m.Id == (int)level2.ParentId);
                }
                if (level1 == null)
                {
                    level1 = new Category();

                }
                var childrens = new List<Category>();
                if (category.Level != 3)
                {
                    childrens = GetChildren(categoriesRelated.ToList(), (int)category.Id);
                }
                else
                {
                    childrens = categoriesRelated.Where(m => m.ParentId == category.ParentId).ToList();
                }
                return Ok(new
                {

                    Childrens = childrens,
                    Category = category,
                    Categories = categoriesRelated.ToList(),
                    Level1 = level1,
                    Level2 = level2,
                    Level3 = level3,
                    Id = category.Id
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("product/category/page/{page}/size/{size}/price/{start}/{end}")]
        public dynamic GetProductByCategoryIdPrice(int id, int page, int size, int start, int end, dynamic obj)
        {
            try
            {
                List<int> categoryIds = obj["categoryIds"].ToObject<List<int>>();
                var data = _context.Products.Where(m => m.IsOld == false && m.IsPublish == true)
                    .Where(m => categoryIds.Any(cate => cate == (int)m.CategoryId))
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.CategoryId,
                        m.SubsidyPrice,
                        m.Categories,
                        m.Keywords,
                        m.IsOld,
                        m.Category.Events,
                        ProductEvents = m.Events,
                        m.Category.Banners,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         },
                        Images = from img in m.Images
                                 select new
                                 {
                                     img.Id,
                                     img.Name,
                                     img.Link
                                 }
                    })
                    .AsQueryable();
                if (start == 0 && end == 0)
                {
                    data = data;
                }
                else
                {
                    data = data.Where(m => m.KiotVietPrice >= start && m.KiotVietPrice <= end);
                }
                return Ok(new
                {
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/page/{page}/size/{size}/price/{start}/{end}/keyword/{keyword}")]
        public dynamic GetProductByCategoryIdPriceKeyword(int page, int size, int start, int end, int keyword)
        {
            try
            {
                var data = _context.Products.Where(m => m.Keywords.Any(k => k.Id == keyword) && m.IsOld == false && m.IsPublish == true)
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.Categories,
                        m.Keywords,
                        m.IsOld,
                        m.Category.Events,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         },
                        Images = from img in m.Images
                                 select new
                                 {
                                     img.Id,
                                     img.Name,
                                     img.Link
                                 }
                    })
                   .AsQueryable();
                if (end != 0)
                {
                    data = data.Where(m => m.KiotVietPrice >= start && m.KiotVietPrice <= end);
                }
                var category = _context.Categories.Select(m => new
                {
                    m.Id,
                    m.KeyWords,
                    m.Name,
                    m.Description,
                    m.Note
                })
                    .FirstOrDefault(m => m.KeyWords.Any(x => x.Id == keyword));
                return Ok(new
                {
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count(),
                    Keyword = _context.KeyWords.Find(keyword),
                    Images = _context.Banners.Where(m => m.Categories.Any(x => x.Id == category.Id)),
                    Category = category
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/category/{id}/page/{page}/size/{size}")]
        public dynamic GetProductByCategoryId(int id, int page, int size)
        {
            try
            {
                var data = _context.Products
                    .Where(m => m.IsPublish == true)
                    .Where(m => (int)m.CategoryId == id)
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.IsOld,
                        ProductTradeIns = from pt in m.ProductTradeIns
                                          select new
                                          {
                                              pt.Id,
                                              pt.Name,
                                              pt.Price,
                                              pt.Status
                                          },
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         },
                        Images = from img in m.Images
                                 select new
                                 {
                                     img.Id,
                                     img.Name,
                                     img.Link
                                 }
                    });

                return Ok(new
                {
                    Category = _context.Categories.Include(m => m.Banners).FirstOrDefault(m => m.Id == id),
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                }); ;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/categorytag/{id}/page/{page}/size/{size}")]
        public dynamic GetProductByCategoryIdModal(int id, int page, int size)
        {
            try
            {
                var data = _context.Products
                    .Where(m => m.Categories.Any(c => c.Id == id) && m.IsOld == false)
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.IsOld,
                        ProductTradeIns = from pt in m.ProductTradeIns
                                          select new
                                          {
                                              pt.Id,
                                              pt.Name,
                                              pt.Price,
                                              pt.Status
                                          },
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         },
                        Images = from img in m.Images
                                 select new
                                 {
                                     img.Id,
                                     img.Name,
                                     img.Link
                                 }
                    });

                return Ok(new
                {
                    Category = _context.Categories.Find(id),
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                }); ;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("Installment/category/page/{page}/size/{size}")]
        public dynamic GetInstallmentByCategoryId(int page, int size)
        {
            try
            {
                var data = _context.Products
                    .Include(m => m.Categories)
                    .Where(m => m.Categories.Any(c => c.Status == 2))
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.IsOld,
                        m.KeySearch,
                        m.Category.Events,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         },
                        Images = from img in m.Images
                                 select new
                                 {
                                     img.Id,
                                     img.Name,
                                     img.Link
                                 }
                    });

                return Ok(new
                {
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Count = data.Count()
                }); ;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("product/tags/{tag}")]
        public dynamic GetProductTagById(string tag)
        {
            return _context.Products.Where(m => m.Tag == tag && m.IsOld == false).Select(m => new
            {
                m.SeoDescription,
                m.SeoName,
                m.SeoUrl,
                m.Id,
                m.Name,
                m.Description,
                m.IsOld,
                m.CreatedAt,
                m.UpdatedAt,
                m.IsPublish,
                m.Sale,
                m.Count,
                m.UserId,
                m.Promotion,
                m.Vote,
                m.KiotVietCode,
                m.KiotVietName,
                m.KiotVietPrice,
                m.Prepay,
                m.TheFirmId,
                m.BundleOffer,
                m.Blog,
                m.Annotate,
                m.Status,
                m.EntryPrice,
                m.SubsidyPrice,
                m.CategoryId,
                m.MasterProductId,
                m.Capacity,
                m.KeySearch,
                m.Tag
            }).OrderBy(m => m.KiotVietPrice);
        }
        [HttpGet]
        [Route("product/id/{id}")]
        public dynamic GetProductById(int id)
        {
            try
            {
                var data = _context.Products
                        .Select(m => new
                        {
                            Comments = from c in m.Comments.Where(m => m.IsShow == true)
                                       select new
                                       {
                                           c.Name,
                                           c.Id,
                                           c.Description,
                                           c.InverseParent
                                       },
                            m.Views,
                            m.SeoDescription,
                            m.SeoName,
                            m.SeoUrl,
                            m.Id,
                            m.Name,
                            m.Description,
                            m.IsOld,
                            m.CreatedAt,
                            m.UpdatedAt,
                            m.IsPublish,
                            m.Sale,
                            m.Count,
                            m.UserId,
                            m.Promotion,
                            m.Vote,
                            m.KiotVietCode,
                            m.KiotVietName,
                            m.KiotVietPrice,
                            m.Prepay,
                            m.TheFirmId,
                            m.BundleOffer,
                            m.Blog,
                            m.Annotate,
                            m.Status,
                            m.EntryPrice,
                            m.SubsidyPrice,
                            m.CategoryId,
                            m.MasterProductId,
                            m.Capacity,
                            m.KeySearch,
                            m.Tag,
                            m.Category.Events,
                            m.SeoSlug,
                            m.Category.AdvertisementDetail,
                            Categories = from ct in m.Categories
                                         select new
                                         {
                                             ct.Id,
                                             ct.Name,
                                             ct.ParentId,
                                         },
                            ProductMetadata = from pm in m.ProductMetadata
                                              select new
                                              {
                                                  pm.MetadataId,
                                                  pm.ProductId,
                                                  pm.Value,
                                                  Metadata = new
                                                  {
                                                      pm.Metadata.Id,
                                                      pm.Metadata.Name,
                                                      pm.Metadata.Sort,
                                                      pm.Metadata.Description
                                                  }
                                              },
                            Keywords = from k in m.Keywords
                                       select new
                                       {
                                           k.Id,
                                           k.LinkImage,
                                           k.Name,
                                           k.Link,
                                           k.Description
                                       },
                            ProductContents = from pc in m.ProductContents
                                              select new
                                              {
                                                  pc.ContentId,
                                                  pc.ProductId,
                                                  pc.Sort,
                                                  pc.Status,
                                                  Content = new
                                                  {
                                                      pc.Content.Name,
                                                      pc.Content.Id,
                                                      pc.Content.Description
                                                  }
                                              },
                            ProductTradeIns = from ptr in m.ProductTradeIns
                                              select new
                                              {
                                                  ptr.Id,
                                                  ptr.ProductId,
                                                  ptr.Name,
                                                  ptr.Price,
                                                  ptr.Status,

                                              },
                            ProductDetails = from pd in m.ProductDetails
                                             select new
                                             {
                                                 pd.Id,
                                                 pd.ProductId,
                                                 pd.ProductCode,
                                                 pd.LinkImage,
                                                 pd.Name,
                                                 pd.Code,
                                                 pd.Color,
                                                 pd.Price
                                             },
                            Images = from img in m.Images
                                     select new
                                     {
                                         img.Id,
                                         img.ProductId,
                                         img.Name,
                                         img.Link,
                                         img.Size,
                                         img.LinkProduct,
                                         img.CategorId
                                     }
                        })
                    .FirstOrDefault(m => m.Id == id & m.IsPublish == true);
                if (data != null)
                {
                    UpdateProductItem(data.Id);
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("product/suggest/{masterProductId}")]
        public dynamic GetProductSugges(string masterProductId)
        {
            var suggest = _context.Products.Where(m => m.MasterProductId == masterProductId && m.Category.Status == 0)
                   .Select(m => new
                   {
                       m.Id,
                       m.Name,
                       m.AttributesSize,
                       m.KiotVietPrice,
                       m.Status,
                       m.SeoUrl
                   }).OrderBy(m => m.KiotVietPrice);
            return suggest;
        }

        [HttpGet]
        [Route("product/{slug}")]
        public dynamic GetProductBySlug(string slug)
        {
            try
            {
                var data = _context.Products
                        .Where(m => m.SeoUrl == slug & m.IsPublish == true)
                        .Select(m => new
                        {
                            Comments = from c in m.Comments.Where(m => m.IsShow == true)
                                       select new
                                       {
                                           c.Name,
                                           c.Id,
                                           c.Description,
                                           c.InverseParent
                                       },
                            m.Views,
                            m.SeoDescription,
                            m.SeoName,
                            m.SeoUrl,
                            m.Id,
                            m.Name,
                            m.Description,
                            m.IsOld,
                            m.CreatedAt,
                            m.UpdatedAt,
                            m.IsPublish,
                            m.Sale,
                            m.Count,
                            m.UserId,
                            m.Promotion,
                            m.Vote,
                            m.KiotVietCode,
                            m.KiotVietName,
                            m.KiotVietPrice,
                            m.Prepay,
                            m.TheFirmId,
                            m.BundleOffer,
                            m.Blog,
                            m.Annotate,
                            m.Status,
                            m.EntryPrice,
                            m.SubsidyPrice,
                            m.CategoryId,
                            m.MasterProductId,
                            m.Capacity,
                            m.KeySearch,
                            m.Tag,
                            m.Category.Events,
                            ProductEvent = from pe in m.Events
                                           select new
                                           {
                                               pe.Id,
                                               pe.AdvertisementDetail,
                                               pe.AdvertisementLarge,
                                               pe.AdvertisementSmall
                                           },
                            m.SeoSlug,
                            m.Category.AdvertisementDetail,
                            Categories = from ct in m.Categories
                                         select new
                                         {
                                             ct.Id,
                                             ct.Name,
                                             ct.ParentId,
                                         },
                            ProductMetadata = from pm in m.ProductMetadata
                                              select new
                                              {
                                                  pm.MetadataId,
                                                  pm.ProductId,
                                                  pm.Value,
                                                  Metadata = new
                                                  {
                                                      pm.Metadata.Id,
                                                      pm.Metadata.Name,
                                                      pm.Metadata.Sort,
                                                      pm.Metadata.Description
                                                  }
                                              },
                            Keywords = from k in m.Keywords
                                       select new
                                       {
                                           k.Id,
                                           k.LinkImage,
                                           k.Name,
                                           k.Link,
                                           k.Description
                                       },
                            ProductContents = from pc in m.ProductContents
                                              select new
                                              {
                                                  pc.ContentId,
                                                  pc.ProductId,
                                                  pc.Sort,
                                                  pc.Status,
                                                  Content = new
                                                  {
                                                      pc.Content.Name,
                                                      pc.Content.Id,
                                                      pc.Content.Description
                                                  }
                                              },
                            ProductTradeIns = from ptr in m.ProductTradeIns
                                              select new
                                              {
                                                  ptr.Id,
                                                  ptr.ProductId,
                                                  ptr.Name,
                                                  ptr.Price,
                                                  ptr.Status,

                                              },
                            ProductDetails = from pd in m.ProductDetails
                                             select new
                                             {
                                                 pd.Id,
                                                 pd.ProductId,
                                                 pd.ProductCode,
                                                 pd.LinkImage,
                                                 pd.Name,
                                                 pd.Code,
                                                 pd.Color,
                                                 pd.Price
                                             },
                            Images = from img in m.Images
                                     select new
                                     {
                                         img.Id,
                                         img.ProductId,
                                         img.Name,
                                         img.Link,
                                         img.Size,
                                         img.LinkProduct,
                                         img.CategorId
                                     }
                        }).FirstOrDefault();

                if (data != null)
                {
                    // Process tag in body 
                    var queryHasTag = _context
                    .Hastags
                    .Where(x => x.Type == (int)EnumTypeHastag.Text)
                    .AsEnumerable()
                    .Select(x =>
                    {
                        // We need to get date, month year of now
                        var newName = HasTagHelper.GetDateNow(x.Code,x.Name);
                        if (!string.IsNullOrEmpty(newName))
                        {
                            x.Name = newName;
                        };

                        return x;
                    });

                    data?.Set(x => x.Name, HasTagHelper.ReplaceHastag(data?.Name, queryHasTag.ToList()));
                    data?.Set(x => x.Blog, HasTagHelper.ReplaceHastag(data?.Blog, queryHasTag.ToList()));
                    data?.Set(x => x.Description, HasTagHelper.ReplaceHastag(data.Description, queryHasTag.ToList()));
                    data?.Set(x => x.SeoName, HasTagHelper.ReplaceHastag(data.SeoName, queryHasTag.ToList()));
                    data?.Set(x => x.SeoDescription, HasTagHelper.ReplaceHastag(data.SeoDescription, queryHasTag.ToList()));
                    UpdateProductItem(data.Id);
                }
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/{slug}/check-exist")]
        public IActionResult CheckProductExistBySlug(string slug)
        {
            try
            {
                var data = _context.Products
                        .FirstOrDefault(m => m.SeoUrl == slug & m.IsPublish == true && !m.IsDelete);
                var result = data != null ? true : false;
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(false);
            }
        }

        [HttpGet]
        [Route("product/search/all/{value}")]
        public dynamic GetProductAllSearching(string value)
        {
            try
            {
                var data = _context.Products
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.KeySearch,
                        m.IsOld,
                        m.IsProjectOld,
                        m.IsPublish,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         }
                    })
                    .Where(m => m.IsPublish == true)
                    .Where(m => m.KeySearch.Contains(value) || m.Name.Contains(value));

                int page = 1;
                int size = 32;
                return Ok(new
                {
                    Data = data.Skip((page - 1) * size).Take(size).ToList(),
                    Total = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/search/IsOld/{value}")]
        public dynamic GetProductIsOldBySearching(string value)
        {
            try
            {
                var data = _context.Products
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.KeySearch,
                        m.IsOld,
                        m.IsProjectOld,
                        m.Status,
                        m.IsPublish,
                        m.Category.Events,
                        ProductEvents = m.Events,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         }
                    })
                    .Where(m => m.IsProjectOld == true && m.IsPublish == true && (int)m.Status == 0).Where(m => m.KeySearch.Contains(value) || m.Name.Contains(value));

                return Ok(new
                {
                    Data = data,
                    Total = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/search/IsNew/{value}")]
        public dynamic GetProductIsNewBySearching(string value)
        {
            try
            {
                var data = _context.Products
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.KeySearch,
                        m.IsOld,
                        m.IsProjectOld,
                        m.Status,
                        m.IsPublish,
                        m.Category.Events,
                        ProductEvents = m.Events,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         }
                    })
                    .Where(m => m.IsProjectOld == false
                    && m.IsPublish == true && (int)m.Status == 0).Where(m => m.KeySearch.Contains(value) || m.Name.Contains(value));


                return Ok(new
                {
                    Data = data,
                    Total = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/search/Accessory/{value}")]
        public dynamic GetProductIsAccessoryBySearching(string value)
        {
            try
            {
                var data = _context.Products
                    .Select(m => new
                    {
                        m.SeoDescription,
                        m.SeoName,
                        m.SeoUrl,
                        m.Id,
                        m.Name,
                        m.Description,
                        m.KiotVietPrice,
                        m.Annotate,
                        m.Sale,
                        m.Prepay,
                        m.EntryPrice,
                        m.SubsidyPrice,
                        m.KeySearch,
                        m.IsOld,
                        m.IsProjectOld,
                        m.IsPublish,
                        m.Status,
                        m.Category.Events,
                        ProductEvents = m.Events,
                        ProductDetails = from pd in m.ProductDetails
                                         select new
                                         {
                                             pd.LinkImage,
                                             pd.Name,
                                             pd.Code,
                                             pd.Color
                                         }
                    })
                    .Where(m => m.IsPublish == true && (int)m.Status == 1).Where(m => m.KeySearch.Contains(value) || m.Name.Contains(value));


                return Ok(new
                {
                    Data = data,
                    Total = data.Count()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("product/order/{id}")]
        public dynamic GetProductOrderById(int id)
        {
            try
            {
                var data = _context.Products
                    .Include(m => m.Images)
                    .FirstOrDefault(m => m.Id == id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("captcha")]
        public dynamic CheckCaptchat(dynamic obj)
        {
            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={_configuration["Captcha:SecretKey"]}&response={obj.token}";
            using (var client = new WebClient())
            {
                var result = client.DownloadString(url);
                string stringValue = "true";
                try
                {
                    dynamic checkEmail = false;
                    if (result.ToString().ToLower().Contains(stringValue))
                    {
                        var comment = new Comment
                        {
                            CreatedAt = DateTime.Now,
                            Vote = 0,
                            Description = obj.detail,
                            IsShow = false,
                            Telephone = obj.telephone,
                            Name = obj.nickname

                        };
                        if ((int)obj.customerId != 0)
                        {
                            comment.CustomerId = (int)obj.customerId;
                        }
                        _context.Comments.Add(comment);
                        Product product = new Product { Id = (int)obj.id };
                        _context.Products.Add(product);
                        _context.Products.Attach(product);
                        product.Comments.Add(comment);
                        _context.SaveChanges();
                        return true;
                    };
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }

        [HttpPost]
        [Route("sell/product")]
        public dynamic SellProduct(dynamic obj)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={config["Captcha:SecretKey"]}&response={obj.Token}";
            using (var client = new WebClient())
            {
                var result = client.DownloadString(url);
                string stringValue = "true";
                try
                {
                    dynamic checkEmail = false;
                    if (result.ToString().ToLower().Contains(stringValue))
                    {
                        try
                        {
                            var contact = new Contact
                            {
                                CreatedAt = DateTime.Now,
                                Fullname = obj.FullName,
                                Phonenumber = obj.Telephone,
                                Description = obj.Note,
                                IsRead = false,
                                Action = obj.Action,
                                Title = obj.Title,
                                Email = obj.Email,
                                ProductId = (int)obj.ProductId,
                                ProductTradeInsStatus = (int)obj.ProductTradeInsStatus,
                                ProductDetails = obj.ProductDetails
                            };

                            _context.Contacts.Add(contact);
                            _context.SaveChanges();
                            return true;
                        }
                        catch (Exception e)
                        {
                            return BadRequest(e);
                        }


                    };
                    return true;
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }

        }

        [HttpPost]
        [Route("productdetails/listids")]
        public dynamic ProductDetailByIds(dynamic obj)
        {
            using (var client = new WebClient())
            {
                try
                {
                    List<int> Ids = obj["Ids"].ToObject<List<int>>();
                    var items = _context.ProductDetails.Where(m => Ids.Any(id => id == m.Id));
                    return items;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }

        // thu cũ đổi mới phiếu
        [HttpPost]
        [Route("product/tradein")]
        public dynamic SellTradein(dynamic obj)
        {
            var contact = new Contact
            {
                CreatedAt = DateTime.Now,
                Fullname = obj.fullname,
                Phonenumber = obj.telephone,
                Action = obj.action,
                Description = obj.description,
                Title = obj.title,
                IsRead = false,
                Email = obj.email,
                ProductTradeInsStatus = (int)obj.productTradeInsStatus,
                FromProjectId = (int)obj.fromProjectId,
                ToProjectId = (int)obj.toProjectId,

            };
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return true;

        }

        // trả góp
        [HttpPost]
        [Route("product/Installment")]
        public dynamic SellInstallment(dynamic obj)
        {
            var contact = new Contact
            {
                Fullname = obj.fullname,
                Phonenumber = obj.telephone,
                Email = obj.email,
                Cmnd = obj.cmnd,
                Birthday = obj.birthday,
                Address = obj.address,
                FromProjectId = (int)obj.productDetail,
                BankInstallment = obj.bank,
                RatioInstallment = obj.ratio,
                MonthInstallment = obj.monthIn,
                CreatedAt = DateTime.Now,
                Action = "tra_gop_cty_tai_chinh",
                Title = "Trả góp công ty tài chính",
                IsRead = false,

                InterestRate = obj.interestRate
            };
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return true;

        }

        //news
        [HttpGet]
        [Route("news/categories")]
        public dynamic GetCategoryInNew()
        {
            var items = _context.Categories.Select(m => new
            {
                m.Name,
                m.IsNew,
                m.Id,
                m.ParentId
            }).Where(m => m.IsNew == true).ToList();
            return items;

        }

        [HttpGet]
        [Route("news/hot/{status}")]
        public dynamic GetBlogHot(int status)
        {
            var data = _context.BlogHomes
                .Include(m => m.Blog)
                .Where(m => m.Status == status);

            return Ok(new
            {
                Data = data.OrderBy(m => m.NumericOrder).ToList(),
                Count = data.Count()
            });

        }

        [HttpGet]
        [Route("newsers")]
        public dynamic GetBlogNewsers()
        {
            var data = _context.Blogs.OrderByDescending(m => m.Views);

            return Ok(new
            {
                Data = data.Skip((1 - 1) * 2).Take(2).ToList(),
                Count = data.Count()
            });

        }

        [HttpGet]
        [Route("newsest/{page}/{size}")]
        public dynamic GetBlogNewest(int page, int size)
        {
            var data = _context.Blogs.OrderByDescending(m => m.CreatedAt);

            return Ok(new
            {
                Data = data.Skip((page - 1) * size).Take(size).ToList(),
                Count = data.Count()
            });

        }

        [HttpGet]
        [Route("news/category/{id}/{page}/{size}")]
        public dynamic GetBlogByCategory(int page, int size, int id)
        {
            var data = _context.Blogs.Where(m => m.CategoryId == id).OrderByDescending(m => m.CreatedAt);

            return Ok(new
            {
                Data = data.Skip((page - 1) * size).Take(size).ToList(),
                Count = data.Count(),
                Category = _context.Categories.Find(id)
            });

        }

        [HttpGet]
        [Route("news/blog/{id}")]
        public dynamic GetBlogItem(int id)
        {
            var data = _context.Blogs.Include(m => m.Category).FirstOrDefault(m => m.Id == id);
            var views = data.Views;
            data.Views = views + 1;
            _context.SaveChanges();

            return Ok(data);

        }

        [HttpGet]
        [Route("category/slug/{id}")]
        public dynamic GetCategoryBySlugId(int id)
        {
            int page = 1;
            int size = 25;
            var data = _context.Products.Where(m => m.CategoryId == id).Select(m => new
            {
                m.Id,
                m.Name,
                m.CreatedAt,
                m.UpdatedAt,
                m.Description,
                m.KiotVietPrice,
                m.SeoDescription,
                m.SeoUrl
            });

            return Ok(new
            {
                Data = data.Skip((page - 1) * size).Take(size).ToList(),
                Category = _context.Categories.FirstOrDefault(m => m.Id == id),
                Count = data.Count()
            });
        }

        #region Private method
        private string ConvertUrl(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return "";
            try
            {
                var getCategory = GetCategoryBySlug(slug).Value;
                var combineLevel = SlugHelper.CombineLevelUrl(
                    getCategory.Level1.Slug,
                    getCategory.Level2.Slug,
                    getCategory.Level3.Slug,
                    true
                );
                var comBineUrl = SlugHelper.CombineUrl(slug, combineLevel);

                return comBineUrl;
            }
            catch (Exception error)
            {
                Console.Write("Has error when converting slug: {0}", error.Message);
            }

            return "";
        }

        private string? GetValueHastag(string? value, Dictionary<string, string>? queryHasTag)
        {
            if (value?.Contains("#") == true)
            {
                var getCharacter = value.Split(" ");
                var getKeySame = getCharacter.Intersect(queryHasTag.Keys);
                foreach (var key in getKeySame)
                {
                    value = value.Replace(key, queryHasTag[key]);
                }
            }

            return value;
        }

        private List<Category> GetChildren(List<Category> foos, int id)
        {
            return foos
                        .Where(x => x.ParentId == id)
                        .Union(foos.Where(x => x.ParentId == id)
                            .SelectMany(y => GetChildren(foos, y.Id))
                        ).ToList();
        }

        private List<Category> GetParent(List<Category> foos, int parentId)
        {
            return foos
                        .Where(x => x.Id == parentId)
                        .Union(foos.Where(x => x.Id == parentId)
                            .SelectMany(y => GetChildren(foos, y.ParentId))
                        ).ToList();
        }

        private bool UpdateProductItem(int id)
        {
            var item = _context.Products.Find(id);
            if (item != null)
            {
                item.Views = item.Views + 1;
                _context.SaveChanges();
            }
            return true;
        }

        #endregion
    }

}
