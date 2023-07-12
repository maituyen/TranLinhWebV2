using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Data.Entities;
using MyProject.Helpers;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        public AsyncController(IConfiguration configuration, DatabaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("categories")]
        public async Task<dynamic> AsyncByCategories(dynamic obj)
        {
            try
            {
                var domainApi = _configuration["Domain:Url"];
                List<int> categories = obj["Categories"].ToObject<List<int>>();
                int totalCount = obj.Total;

                foreach (var category in categories)
                {
                    var checkData = false;

                    for (int i = 0; i < totalCount; i += 100)
                    {
                        if (checkData == false)
                        {
                            using (var httpClient = new HttpClient())
                            {
                                httpClient.BaseAddress = new Uri(domainApi);
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                                string url = "/api/KiotViet/products/category/" + category + "/currentItem/" + i + "/size/" + 100;
                                var responseMessage = await httpClient.GetAsync(url);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    var inserts = new List<Product>();
                                    var res = await responseMessage.Content.ReadAsAsync<dynamic>();

                                    if (res.data.Count > 0)
                                    {
                                        foreach (var item in res.data)
                                        {
                                            int KiotVietId = (int)item.id;
                                            var product = new Product
                                            {
                                                Views = 0,
                                                KiotVietName = item.name,
                                                KiotVietCode = item.code,
                                                KiotVietPrice = (int)item.basePrice,
                                                SeoUrl = SlugHelper.SlugNameUrl(item.name.ToString()) + ".html",
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now,
                                                IsPublish = false,
                                                Sale = 0,
                                                Prepay = 0,
                                                Name = item.fullName,
                                                IsOld = false,
                                                Status = 0,
                                                EntryPrice = 0,
                                                SubsidyPrice = 0,
                                                KeySearch = slugName((string)item.fullName),
                                                MasterProductId = item.masterProductId,
                                                Capacity = "",
                                                IsProjectOld = false,
                                                KiotVietId = (int)item.id,
                                                KiotVietCategoryId = (int)item.categoryId,
                                                KiotVietCategoryName = item.name,
                                                KiotVietMasterProductId = item.masterProductId,
                                                KiotVietFullName = item.fullName,
                                                KiotVietTradeMarkName = item.tradeMarkName,
                                                IsDelete = false
                                            };

                                            if (item.attributes != null)
                                            {
                                                foreach (var attr in item.attributes)
                                                {
                                                    if (attr.attributeName == "DUNG LƯỢNG")
                                                    {
                                                        product.AttributesSize = attr.attributeValue;
                                                    }
                                                    if (attr.attributeName == "MÀU SẮC")
                                                    {
                                                        product.AttributesColor = attr.attributeValue;
                                                    }
                                                }
                                            }

                                            if (product.MasterProductId == null)
                                            {
                                                product.MasterProductId = "0";
                                            }

                                            if (product.AttributesSize != null)
                                            {
                                                product.Name = item.name + " " + product.AttributesSize;
                                                inserts.Add(product);
                                            }
                                        }
                                    }
                                    // sản phẩm
                                    var details = inserts
                                        .GroupBy(pd => pd.KiotVietName)
                                        .Select(m =>
                                        {
                                            var value = new
                                            {
                                                m.Key,
                                                Total = m.Count(),
                                                Lists = new
                                                {
                                                    Format = m.GroupBy(d => d.AttributesSize).Select(x =>
                                                    {
                                                        var value1 = new
                                                        {
                                                            x.Key,
                                                            Data = x.OrderBy(o => o.Id).ToList(),
                                                            Item = x.FirstOrDefault(),
                                                        };
                                                        return value1;
                                                    })
                                                },
                                            };
                                            return value;
                                        });

                                    var productInputs = new List<Product>();
                                    foreach (var d in details)
                                    {
                                        foreach (var detail in d.Lists.Format)
                                        {

                                            var productInput = new Product
                                            {
                                                Views = 0,
                                                KiotVietName = detail?.Item?.KiotVietName,
                                                KiotVietCode = detail?.Item?.KiotVietCode,
                                                KiotVietPrice = detail?.Item?.KiotVietPrice,
                                                SeoUrl = SlugHelper.SlugNameUrl(detail.Item.Name.ToString()) + ".html",
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now,
                                                IsPublish = false,
                                                Sale = 0,
                                                Prepay = 0,
                                                Name = detail?.Item?.Name,
                                                IsOld = false,
                                                Status = 0,
                                                EntryPrice = 0,
                                                SubsidyPrice = 0,
                                                KeySearch = detail?.Item?.KeySearch,
                                                MasterProductId = detail?.Item?.MasterProductId,
                                                Capacity = "",
                                                IsProjectOld = false,
                                                KiotVietId = detail.Item.KiotVietId,
                                                KiotVietCategoryId = detail.Item.KiotVietCategoryId,
                                                KiotVietCategoryName = detail?.Item?.KiotVietCategoryName,
                                                KiotVietMasterProductId = detail?.Item?.KiotVietMasterProductId,
                                                KiotVietFullName = detail?.Item?.KiotVietFullName,
                                                KiotVietTradeMarkName = detail?.Item?.KiotVietTradeMarkName,
                                                AttributesSize = detail.Key,
                                                AttributesColor = detail?.Item?.AttributesColor,
                                                AttributesType = detail?.Item?.AttributesType,
                                                IsDelete = false
                                            };

                                            var checkItem = _context.Products.FirstOrDefault(p => p.KiotVietId == productInput.KiotVietId);

                                            if (checkItem != null)
                                            {
                                                checkItem.Name = productInput.Name;
                                                checkItem.KiotVietPrice = (int)productInput.KiotVietPrice;
                                                checkItem.MasterProductId = productInput.MasterProductId;
                                                checkItem.AttributesSize = productInput.AttributesSize;
                                                checkItem.AttributesColor = productInput.AttributesColor;
                                                checkItem.SeoUrl = productInput.SeoUrl;
                                                if (checkItem.AttributesType != null)
                                                {
                                                    checkItem.AttributesType = productInput.AttributesType;
                                                }
                                                foreach (var data in detail.Data)
                                                {
                                                    var checkItemDetail = _context.ProductDetails.FirstOrDefault(m => m.KiotVietId == data.KiotVietId);
                                                    if (checkItemDetail != null)
                                                    {
                                                        checkItemDetail.Name = data.Name;
                                                        checkItemDetail.Code = data.KiotVietCode;
                                                        checkItemDetail.Price = data.KiotVietPrice.ToString();
                                                        checkItemDetail.Color = data.AttributesColor;
                                                        checkItemDetail.KiotVietId = data.KiotVietId;
                                                        if (checkItemDetail.AttributesType != null)
                                                        {
                                                            checkItemDetail.AttributesType = data.AttributesType;
                                                        }

                                                    }
                                                }
                                                //await _context.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                foreach (var data in detail.Data)
                                                {
                                                    if (data.MasterProductId != "0")
                                                    {
                                                        productInput.MasterProductId = data.MasterProductId;
                                                        productInput.KiotVietMasterProductId = data.MasterProductId;
                                                    }
                                                    var detailInput = new ProductDetail
                                                    {
                                                        Name = data.Name,
                                                        Color = data.AttributesColor,
                                                        Price = data.KiotVietPrice.ToString(),
                                                        Code = data.KiotVietCode,
                                                        KiotVietId = data.KiotVietId,
                                                        AttributesType = data.AttributesType,
                                                    };
                                                    productInput.ProductDetails.Add(detailInput);
                                                }
                                                productInputs.Add(productInput);
                                            }
                                        }
                                    }

                                    await _context.Asyncs.AddAsync(new Async { Date = DateTime.Now });
                                    await _context.Products.AddRangeAsync(productInputs);
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("categories/tradein")]
        public async Task<dynamic> AsyncByCategoriesTradein(dynamic obj)
        {
            try
            {
                var domainApi = _configuration["Domain:Url"];
                List<int> categories = obj["Categories"].ToObject<List<int>>();
                int totalCount = obj.Total;
                foreach (var category in categories)
                {
                    var checkData = false;

                    for (int i = 0; i < totalCount; i += 100)
                    {
                        if (checkData == false)
                        {
                            using (HttpClient httpClient = new HttpClient())
                            {
                                httpClient.BaseAddress = new Uri(domainApi);
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                                string url = "/api/KiotViet/products/category/" + category + "/currentItem/" + i + "/size/" + 100;
                                var responseMessage = await httpClient.GetAsync(url);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    List<Product> inserts = new List<Product>();
                                    var res = await responseMessage.Content.ReadAsAsync<dynamic>();
                                    if (res.data.Count > 0)
                                    {
                                        foreach (var item in res.data)
                                        {
                                            int KiotVietId = (int)item.id;
                                            var product = new Product
                                            {
                                                Views = 0,
                                                KiotVietName = item.name,
                                                KiotVietCode = item.code,
                                                KiotVietPrice = (int)item.basePrice,
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now,
                                                IsPublish = false,
                                                Sale = 0,
                                                Prepay = 0,
                                                Name = item.fullName,
                                                IsOld = false,
                                                Status = 0,
                                                EntryPrice = 0,
                                                SubsidyPrice = 0,
                                                KeySearch = slugName((string)item.fullName),
                                                MasterProductId = item.masterProductId,
                                                Capacity = "",
                                                IsProjectOld = false,
                                                KiotVietId = (int)item.id,
                                                KiotVietCategoryId = (int)item.categoryId,
                                                KiotVietCategoryName = item.name,
                                                KiotVietMasterProductId = item.masterProductId,
                                                KiotVietFullName = item.fullName,
                                                KiotVietTradeMarkName = item.tradeMarkName,
                                                IsDelete = false
                                            };
                                            if (item.attributes != null)
                                            {
                                                foreach (var attr in item.attributes)
                                                {
                                                    if (attr.attributeName == "DUNG LƯỢNG")
                                                    {
                                                        product.AttributesSize = attr.attributeValue;
                                                    }
                                                    if (attr.attributeName == "MẦU SẮC")
                                                    {
                                                        product.AttributesColor = attr.attributeValue;
                                                    }
                                                    if (attr.attributeName == "PHÂN LOẠI")
                                                    {
                                                        product.AttributesType = attr.attributeValue;
                                                    }
                                                }
                                            }
                                            if (product.MasterProductId == null)
                                            {
                                                product.MasterProductId = "0";
                                            }
                                            if (product.AttributesType != null)
                                            {
                                                inserts.Add(product);
                                            }
                                        }
                                    }
                                    // thu cũ đổi mới
                                    var details = inserts.GroupBy(pd => pd.KiotVietName).Select(m => new
                                    {
                                        m.Key,
                                        Total = m.Count(),
                                        Data = m.OrderBy(x => x.Id).ToList(),
                                        Item = m.FirstOrDefault(),
                                    });
                                    List<Product> productInputs = new List<Product>();
                                    foreach (var detail in details)
                                    {
                                        var productInput = new Product
                                        {
                                            Views = 0,
                                            KiotVietName = detail?.Item?.KiotVietName,
                                            KiotVietCode = detail?.Item?.KiotVietCode,
                                            KiotVietPrice = detail?.Item?.KiotVietPrice,
                                            CreatedAt = DateTime.Now,
                                            UpdatedAt = DateTime.Now,
                                            IsPublish = false,
                                            Sale = 0,
                                            Prepay = 0,
                                            Name = detail?.Item?.KiotVietName,
                                            IsOld = false,
                                            Status = 0,
                                            EntryPrice = 0,
                                            SubsidyPrice = 0,
                                            KeySearch = detail?.Item?.KeySearch,
                                            MasterProductId = detail?.Item?.MasterProductId,
                                            Capacity = "",
                                            IsProjectOld = false,
                                            KiotVietId = detail.Item.KiotVietId,
                                            KiotVietCategoryId = detail.Item.KiotVietCategoryId,
                                            KiotVietCategoryName = detail?.Item?.KiotVietCategoryName,
                                            KiotVietMasterProductId = detail?.Item?.KiotVietMasterProductId,
                                            KiotVietFullName = detail?.Item?.KiotVietFullName,
                                            KiotVietTradeMarkName = detail?.Item?.KiotVietTradeMarkName,
                                            AttributesSize = detail?.Key,
                                            AttributesColor = detail?.Item?.AttributesColor,
                                            AttributesType = detail?.Item?.AttributesType,
                                            IsDelete = false
                                        };

                                        var checkItem = _context.Products.FirstOrDefault(p => p.KiotVietId == productInput.KiotVietId);
                                        if (checkItem != null)
                                        {
                                            checkItem.Name = productInput.Name;
                                            checkItem.KiotVietPrice = (int)productInput.KiotVietPrice;
                                            checkItem.MasterProductId = productInput.MasterProductId;
                                            checkItem.AttributesSize = productInput.AttributesSize;
                                            checkItem.AttributesColor = productInput.AttributesColor;

                                            if (checkItem.AttributesType != null)
                                            {
                                                checkItem.AttributesType = productInput.AttributesType;
                                            }

                                            foreach (var data in detail.Data)
                                            {
                                                var checkItemDetail = _context.ProductDetails.FirstOrDefault(m => m.KiotVietId == data.KiotVietId);
                                                if (checkItemDetail != null)
                                                {
                                                    checkItemDetail.Name = data.Name;
                                                    checkItemDetail.Code = data.KiotVietCode;
                                                    checkItemDetail.Price = data.KiotVietPrice.ToString();
                                                    checkItemDetail.Color = data.AttributesColor;
                                                    checkItemDetail.KiotVietId = data.KiotVietId;
                                                    if (checkItemDetail.AttributesType != null)
                                                    {
                                                        checkItemDetail.AttributesType = data.AttributesType;
                                                    }

                                                }
                                            }
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            foreach (var data in detail.Data)
                                            {
                                                if (data.MasterProductId != "0")
                                                {
                                                    productInput.MasterProductId = data.MasterProductId;
                                                    productInput.KiotVietMasterProductId = data.MasterProductId;
                                                }

                                                var detailInput = new ProductDetail
                                                {
                                                    Name = data.Name,
                                                    Color = data.AttributesColor,
                                                    Price = data.KiotVietPrice.ToString(),
                                                    Code = data.KiotVietCode,
                                                    KiotVietId = data.KiotVietId,
                                                    AttributesType = data.AttributesType,
                                                };

                                                productInput.ProductDetails.Add(detailInput);
                                            }
                                            productInputs.Add(productInput);
                                        }



                                    }
                                    _context.Asyncs.Add(new Async
                                    {
                                        Date = DateTime.Now,
                                    });
                                    await _context.Products.AddRangeAsync(productInputs);
                                    await _context.SaveChangesAsync();


                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        [Authorize]
        [HttpPost]
        [Route("categories/Accessory")]
        public async Task<dynamic> AsyncByCategoriesAccessory(dynamic obj)
        {
            try
            {
                var domainApi = _configuration["Domain:Url"];
                List<int> categories = obj["Categories"].ToObject<List<int>>();
                int totalCount = obj.Total;
                foreach (var category in categories)
                {
                    var checkData = false;

                    for (int i = 0; i < totalCount; i += 100)
                    {
                        if (checkData == false)
                        {
                            using (HttpClient httpClient = new HttpClient())
                            {
                                httpClient.BaseAddress = new Uri(domainApi);
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                httpClient.DefaultRequestHeaders.Add("Retailer", _configuration["KiotVietToken:Shop"]);
                                string url = "/api/KiotViet/products/category/" + category + "/currentItem/" + i + "/size/" + 100;
                                var responseMessage = await httpClient.GetAsync(url);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    var inserts = new List<Product>();
                                    var res = await responseMessage.Content.ReadAsAsync<dynamic>();
                                    if (res.data.Count > 0)
                                    {
                                        foreach (var item in res.data)
                                        {

                                            int KiotVietId = (int)item.id;
                                            var product = new Product
                                            {
                                                Views = 0,
                                                KiotVietName = item.name,
                                                KiotVietCode = item.code,
                                                KiotVietPrice = (int)item.basePrice,
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now,
                                                SeoUrl = SlugHelper.SlugNameUrl(item.name.ToString()) + ".html",
                                                IsPublish = false,
                                                Sale = 0,
                                                Prepay = 0,
                                                Name = item.fullName,
                                                IsOld = false,
                                                Status = 0,
                                                EntryPrice = 0,
                                                SubsidyPrice = 0,
                                                KeySearch = slugName((string)item.fullName),
                                                MasterProductId = item.masterProductId,
                                                Capacity = "",
                                                IsProjectOld = false,
                                                KiotVietId = (int)item.id,
                                                KiotVietCategoryId = (int)item.categoryId,
                                                KiotVietCategoryName = item.name,
                                                KiotVietMasterProductId = item.masterProductId,
                                                KiotVietFullName = item.fullName,
                                                KiotVietTradeMarkName = item.tradeMarkName,
                                                IsDelete = false
                                            };

                                            if (item.attributes != null)
                                            {
                                                foreach (var attr in item.attributes)
                                                {
                                                    if (attr.attributeName == "DUNG LƯỢNG")
                                                    {
                                                        product.AttributesSize = attr.attributeValue;
                                                    }
                                                    if (attr.attributeName == "MẦU SẮC")
                                                    {
                                                        product.AttributesColor = attr.attributeValue;
                                                    }
                                                    if (attr.attributeName == "PHÂN LOẠI")
                                                    {
                                                        product.AttributesType = attr.attributeValue;
                                                    }
                                                }
                                            }

                                            if (product.MasterProductId == null)
                                            {
                                                product.MasterProductId = "0";
                                            }

                                            var checkItem = _context
                                                .Products
                                                .FirstOrDefault(m => m.KiotVietId == KiotVietId);

                                            if (checkItem == null)
                                            {
                                                if (product.AttributesType == null && product.AttributesSize == null)
                                                {
                                                    inserts.Add(product);
                                                }
                                            }
                                            else
                                            {
                                                checkItem.KiotVietPrice = product.KiotVietPrice;
                                            }
                                        }

                                        // Phụ kiện
                                        await _context.Asyncs.AddAsync(new Async
                                        {
                                            Date = DateTime.Now,
                                        });
                                        await _context.Products.AddRangeAsync(inserts);
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("lastModifiedFrom")]
        public dynamic GetLastAsync()
        {
            return _context.Asyncs.ToList();
        }

        private string slugName(string str)
        {
            str = str.ToLower();
            return SlugGenerator.SlugGenerator.GenerateSlug(str);
        }

    }
}
