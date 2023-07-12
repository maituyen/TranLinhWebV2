using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Data.Entities;

namespace MyProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public BankController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public dynamic GetAll()
        {
            return _context.Banks.ToList();
        }

        [HttpPost("add")]
        public bool AddNew(Bank bank)
        {
           _context.Banks.Add(bank);
           _context.SaveChanges();
            return true;
        }

        [HttpPost("edit")]
        public bool EditBank(Bank bank)
        {
            var item = _context.Banks.Find(bank.Id);
            if (item == null)
            {
                return false;
            }
            item.Name = bank.Name;
            item.Detail= bank.Detail;
            _context.SaveChanges();
            return true;
        }

        [HttpPost("delete/{id}")]
        public bool RemoveBank(int id)
        {
            var item = _context.Banks.Find(id);
            if (item == null)
            {
                return false;
            }
            _context.Banks.Remove(item);
            _context.SaveChanges();
            return true;
        }

    }
}
