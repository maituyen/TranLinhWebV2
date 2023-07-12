using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class Event
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; } = -1;
        public string AdvertisementSmall { get; set; }
        public string AdvertisementLarge { get; set; }
        public string AdvertisementDetail { get; set; }
        public DateTime Start { get; set; }
        public string Name { get; set; }
        public DateTime End { get; set; }
        public Event()
        {

        }
        public Event(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(Event data)
        {
            this.Id = data.Id;
            this.AdvertisementSmall = data.AdvertisementSmall;
            this.AdvertisementLarge = data.AdvertisementLarge;
            this.AdvertisementDetail = data.AdvertisementDetail;
            this.Start = data.Start;
            this.Name = data.Name;
            this.End = data.End; 
        }
        public Event GetById(int Id)
        {
            try
            {
                return db.Query<Event>("Event_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new Event();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Event();
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<Event> GetAll(int Status = -1)
        {
            try
            {
                var data = db.Query<Event>("Event_SelectByStatus", new
                {
                    Status = Status
                }).ToList() ?? new List<Event>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<Event>();
            }
        }
        private Event Save()
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
        public Event Insert()
        {
            try
            {
                return db.Query<Event>("Event_Insert", new
                {
                    Id = Id,
                    AdvertisementSmall = AdvertisementSmall,
                    AdvertisementLarge = AdvertisementLarge,
                    AdvertisementDetail = AdvertisementDetail,
                    Start = Start,
                    Name = Name,
                    End = End, 
                }).FirstOrDefault() ?? new Event();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Event();
            }
        }
        public Event Update()
        {
            try
            {
                return db.Query<Event>("Event_Update", new
                {
                    Id = Id,
                    AdvertisementSmall = AdvertisementSmall,
                    AdvertisementLarge = AdvertisementLarge,
                    AdvertisementDetail = AdvertisementDetail,
                    Start = Start,
                    Name = Name,
                    End = End,
                }).FirstOrDefault() ?? new Event();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new Event();
            }
        }
    }
}