using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ContactController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("all")]
        public dynamic GetAll(dynamic obj)
        {
            try
            {
                int page = obj.page;
                int size = obj.size;
                string action = obj.action;
                string action2 = obj.action2;
                var data = _context.Contacts.OrderByDescending(m => m.CreatedAt).Where(m => m.Action == action || m.Action == action2).Select(m => new
                {
                    m.Action,
                    m.Address,
                    m.BankInstallment,
                    m.Birthday,
                    m.Cmnd,
                    m.CreatedAt,
                    m.CustomerId,
                    m.Description,
                    m.Email,
                    m.Fullname,
                    m.Id,
                    m.InterestRate,
                    m.IsRead,
                    m.Phonenumber,
                    m.ProductDetails,
                    m.ProductId,
                    m.ProductTradeInsStatus,
                    m.RatioInstallment,
                    m.Title,
                    m.ToProjectId,
                    m.TradeInType,
                    m.FromProjectId,
                    FromProject = _context.Products.Include(x => x.ProductDetails).Include(x => x.Images).FirstOrDefault(x => x.Id == m.FromProjectId),
                    ToProject = _context.Products.Include(x => x.ProductDetails).FirstOrDefault(x => x.Id == m.ToProjectId),
                    Selling = _context.Products.Include(x => x.ProductDetails)
                    .Include(x => x.Images).FirstOrDefault(x => x.Id == m.ProductId),
                }).AsQueryable();
                /*if(action2 != null)
                {
                    data = data.orww(m => m.Action == action2).AsQueryable();
                }*/
                return Ok(new
                {
                    Data = data.OrderByDescending(m => m.Id).Skip((page - 1) * size).Take(size).ToList(),
                    Total = data.Count(),

                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("totals/notRead")]
        public dynamic GetTotals()
        {
            try
            {
                
                return Ok(new
                {
                    News = _context.Contacts.Where(m => m.Action=="thu_cu_doi_moi" & m.IsRead ==false).Count(),
                    Sells = _context.Contacts.Where(m => m.Action == "ban_may" & m.IsRead == false).Count(),
                    Reserves = _context.Contacts.Where(m => m.Action == "dat_may" & m.IsRead == false).Count(),
                    Installment = _context.Contacts.Where(m => m.Action == "tra_gop_cty_tai_chinh" & m.IsRead == false).Count(),
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("add")]
        public dynamic Add(Contact contact)
        {
            try
            {
                contact.IsRead = false;
                contact.CreatedAt = DateTime.Now;
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }

        [HttpPost]
        [Route("update")]
        public dynamic Update(dynamic obj)
        {
            try
            {
                bool isRead = (bool)obj.IsRead;
                int id = (int)obj.Id;
                var item = _context.Contacts.Find(id);
                item.IsRead = isRead;
                _context.SaveChanges();
                return Ok(new { message = "Thành công", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
        }
    }
}
