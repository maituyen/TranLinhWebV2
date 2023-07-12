using System;
namespace MyProject.Data.Entities
{
    public class SiteInfo
    {
        Helpers.Database db = new Helpers.Database();
        public decimal Id { get; set; } = -1;
        public string SiteName { get; set; }
        public string Host { get; set; }
        public string Logo { get; set; }
        public string LogoMobile { set; get; }
        public string SocialZalo { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialYoutube { get; set; }
        public string SocialTiktok { get; set; }
        public string HotlineSale { get; set; }
        public string HotlineComplaint { get; set; }
        public string HotlineWarranty { get; set; }
        public string DeliveryPayment { get; set; }
        public string ReturnPolicy { get; set; }

        public string GPKD { set; get; }
        public string ChatGPTAPIKey { set; get; }

        public string ShopName { set; get; }
        public string Address { set; get; }
        public string AddressFull { set; get; }

        public string CityId { set; get; }
        public string DistrictId { set; get; }
        public string CommuneId { set; get; }

        public Models.Location.City CityInfo { set; get; }
        public Models.Location.District DistrictInfo { set; get; }
        public Models.Location.Commune CommuneInfo { set; get; } 
        public string CloseTime { set; get; }
        public string OpenTime { set; get; }
        public bool isOpen { set; get; }
        public SiteInfo()
        { 
        }
        public SiteInfo(decimal Id)
        {
            LoadData(GetById(Id));
        }
        public SiteInfo(string Host)
        {
            LoadData(GetByHost(Host));
        }
        public void LoadData(SiteInfo data)
        {
            this.Id = data.Id;
            this.SiteName = data.SiteName;
            this.Host = data.Host;
            this.Logo = data.Logo;
            this.LogoMobile = data.LogoMobile;
            this.SocialZalo = data.SocialZalo;
            this.SocialFacebook = data.SocialFacebook;
            this.SocialYoutube = data.SocialYoutube;
            this.SocialTiktok = data.SocialTiktok;
            this.HotlineSale = data.HotlineSale;
            this.HotlineComplaint = data.HotlineComplaint;
            this.HotlineWarranty = data.HotlineWarranty;
            this.DeliveryPayment = data.DeliveryPayment;
            this.ReturnPolicy = data.ReturnPolicy;
            this.GPKD = data.GPKD;
            this.ShopName = data.ShopName;
            this.Address = data.Address;
            this.CityInfo = new Models.Location.City(data.CityId);
            this.DistrictInfo = new Models.Location.District(data.DistrictId);
            this.CommuneInfo = new Models.Location.Commune(data.CommuneId);

            this.AddressFull = string.Concat(this.Address, this.CommuneInfo.Name, this.DistrictInfo.Name, this.CityInfo.Name);
           
            this.OpenTime = data.OpenTime;
            this.CloseTime = data.CloseTime;
            this.isOpen = _isOpen(this.OpenTime, this.CloseTime);
        }
        private bool _isOpen(string OpenTime,string CloseTime)
        {
            int o = int.Parse(OpenTime.Split(":")[0]);
            int c = int.Parse(CloseTime.Split(":")[0]);
            return (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= c);
        }
        public SiteInfo GetByHost(string Host)
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_SelectByHost", new
                {
                    Host = Host
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
        public List<SiteInfo> GetAll()
        {
            try
            {
                var data= db.Query<SiteInfo>("SiteInfo_SelectAll").ToList() ?? new List<SiteInfo>();
                foreach (var item in data)
                {
                    item.CityInfo = new Models.Location.City(item.CityId);
                    item.DistrictInfo = new Models.Location.District(item.DistrictId);
                    item.CommuneInfo = new Models.Location.Commune(item.CommuneId);
                    item.AddressFull = string.Concat(item.Address, item.CommuneInfo.Name, item.DistrictInfo.Name, item.CityInfo.Name);
                    this.isOpen = _isOpen(item.OpenTime, item.CloseTime);
                    this.ChatGPTAPIKey = "";
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<SiteInfo>();
            }
        }
        public SiteInfo GetById(decimal Id)
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            { 
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
        private SiteInfo Save()
        {
            if(this.Id==-1)
            {
               return Insert();
            }else
            {
                return Update();
            }
        }
        public SiteInfo Insert()
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_Insert", new
                {
                    Id = Id,
                    SiteName = SiteName,
                    Host = Host,
                    Logo = Logo,
                    SocialZalo = SocialZalo,
                    SocialFacebook = SocialFacebook,
                    SocialYoutube = SocialYoutube,
                    SocialTiktok = SocialTiktok,
                    HotlineSale = HotlineSale,
                    HotlineComplaint = HotlineComplaint,
                    HotlineWarranty = HotlineWarranty,
                    DeliveryPayment = DeliveryPayment,
                    ReturnPolicy = ReturnPolicy,
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
        public SiteInfo Update()
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_Update", new
                {
                    Id = Id,
                    SiteName = SiteName,
                    Host = Host,
                    Logo = Logo,
                    SocialZalo = SocialZalo,
                    SocialFacebook = SocialFacebook,
                    SocialYoutube = SocialYoutube,
                    SocialTiktok = SocialTiktok,
                    HotlineSale = HotlineSale,
                    HotlineComplaint = HotlineComplaint,
                    HotlineWarranty = HotlineWarranty,
                    DeliveryPayment = DeliveryPayment,
                    ReturnPolicy = ReturnPolicy,
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
        public SiteInfo UpdateReturnPolicy()
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_UpdateReturnPolicy", new
                {
                    Id = Id,
                    ReturnPolicy = ReturnPolicy
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
        public SiteInfo UpdateDeliveryPayment()
        {
            try
            {
                return db.Query<SiteInfo>("SiteInfo_UpdateDeliveryPayment", new
                {
                    Id = Id,
                    DeliveryPayment = DeliveryPayment
                }).FirstOrDefault() ?? new SiteInfo();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new SiteInfo();
            }
        }
    }
}

