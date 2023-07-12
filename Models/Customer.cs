using System;
using System.Collections.Generic;
using MyProject.Data.Entities;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MyProject.Models
{
    public partial class Customer
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; } = -1;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string? Address { get; set; }
        public string Phone { get; set; }
        public bool IsClone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Username { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public bool Gender { get; set; } 
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CustomerHistory> CustomerHistories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Customer()
        {
            Comments = new HashSet<Comment>();
            CustomerHistories = new HashSet<CustomerHistory>();
            Orders = new HashSet<Order>();
        }
        public Customer(int id)
        {
            LoadData(GetByUser(id));
        }
        public Customer(string PhoneOrMail)
        {
            LoadData(GetByUser(PhoneOrMail));
        }
        public void LoadData(Customer data)
        {
            this.Id = data.Id;
            this.Email = data.Email;
            this.Password = data.Password;
            this.Fullname = data.Fullname;
            this.Address = data.Address;
            this.Phone = data.Phone;
            this.IsClone = data.IsClone;
            this.CreatedAt = data.CreatedAt;
            this.Username = data.Username;
            this.Date = data.Date;
            this.Gender = data.Gender;
            this.Id = data.Id;
            this.Email = data.Email;
            this.Password = data.Password;
            this.Fullname = data.Fullname;
            this.Address = data.Address;
            this.Phone = data.Phone;
            this.IsClone = data.IsClone;
            this.CreatedAt = DateTime.Now;
            this.Username = data.Username;
            this.Date = data.Date;
            this.Gender = data.Gender;
        }
        public Customer GetByUser(int Id)
        {
            try
            {
                return db.Query<Customer>("Customers_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Customer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Customer();
            }
        }
        public Customer GetByUser(string PhoneOrMail)
        {
            try
            {
                return db.Query<Customer>("Customers_SelectByUser", new
                {
                    PhoneOrMail = PhoneOrMail
                }).FirstOrDefault() ?? new Customer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Customer();
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<Customer> GetAll()
        {
            try
            {
                var data = db.Query<Customer>("Customers_SelectAll").ToList() ?? new List<Customer>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Customer>();
            }
        }
        public Customer Save()
        {
            if (this.Id == -1)
            {
                return Insert();
            }
            else
            {
                return Update();
            }
        }
        private Customer Insert()
        {
            try
            {
                return db.Query<Customer>("Customers_Insert", new
                {
                    Id = Id,
                    Email = Email,
                    Password = Password,
                    Fullname = Fullname,
                    Address = Address,
                    Phone = Phone,
                    IsClone = IsClone,
                    CreatedAt = CreatedAt,
                    Username = Username,
                    Date = Date,
                    Gender = Gender, 
                }).FirstOrDefault() ?? new Customer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Customer();
            }
        }
        private Customer Update()
        {
            try
            {
                return db.Query<Customer>("Customers_Update", new
                {
                    Id = Id,
                    Email = Email,
                    Password = Password,
                    Fullname = Fullname,
                    Address = Address,
                    Phone = Phone,
                    IsClone = IsClone,
                    CreatedAt = CreatedAt,
                    Username = Username,
                    Date = Date,
                    Gender = Gender,
                }).FirstOrDefault() ?? new Customer();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Customer();
            }
        }
        public Helpers.ReturnClient Delete()
        {
            Customer Customer = new Customer(Id);
            if (Customer.Id == -1)
            {
                return new Helpers.ReturnClient
                {
                    sucess = false,
                    message = "Customer không tồn tại"
                };
            }
            db.Query<Customer>("Customers_Delete", new
            {
                Id = Id
            }); 
            return new Helpers.ReturnClient
            {
                sucess = true,
                message = "Xóa Customer thành công!"
            };
        }
    }
}