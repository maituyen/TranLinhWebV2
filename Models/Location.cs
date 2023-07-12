using System;
using static MyProject.Models.Location;

namespace MyProject.Models
{
	public class Location
	{
       
        public class City
        {
            Helpers.Database db = new Helpers.Database();
            public string CityId  {set;get;}
            public string Name { set; get; }
            public City()
            {

            }
            public City(string CityId)
            {
                LoadData(GetById(CityId));
            }
            public void LoadData(City data)
            {
                this.CityId = data.CityId;
                this.Name = data.Name;
            }
            public City GetById(string CityId)
            {
                try
                {
                    var data = db.Query<City>("LocationCity_SelectByID", new
                    {
                        CityId = CityId
                    }).FirstOrDefault() ?? new City();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new City();
                }
            }
            public List<City> GetAll()
            {
                try
                {
                    var data = db.Query<City>("LocationCity_SelectAll", new
                    {
                        CityId = CityId
                    }).ToList() ?? new List<City>();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<City>();
                }
            }
        }
        public class District
        {
            public District()
            {

            }
            public District(string DistrictId)
            {
                LoadData(GetById(DistrictId));
            }
            public void LoadData(District data)
            {
                this.DistrictId = data.DistrictId;
                this.Name = data.Name;
            }
            Helpers.Database db = new Helpers.Database();
            public string DistrictId { set; get; }
            public string Name { set; get; }
            public District GetById(string CityId)
            {
                try
                {
                    var data = db.Query<District>("LocationDistrict_SelectByID", new
                    {
                        DistrictId = DistrictId
                    }).FirstOrDefault() ?? new District();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new District();
                }
            }
            public List<District> GetAll(string CityId)
            {
                try
                {
                    var data = db.Query<District>("LocationDistrict_SelectByCityId", new
                    {
                        CityId = CityId
                    }).ToList() ?? new List<District>();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<District>();
                }
            }
        }
        public class Commune
        {
            Helpers.Database db = new Helpers.Database();
            public string CommuneId { set; get; }
            public string Name { set; get; }
            public Commune()
            {

            }
            public Commune(string CommuneId)
            {
                LoadData(GetById(CommuneId));
            }
            public void LoadData(Commune data)
            {
                this.CommuneId = data.CommuneId;
                this.Name = data.Name;
            }
            public Commune GetById(string CityId)
            {
                try 
                {
                    var data = db.Query<Commune>("LocationCommune_SelectByID", new
                    {
                        CommuneId = CommuneId
                    }).FirstOrDefault() ?? new Commune();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new Commune();
                }
            }
            public List<Commune> GetAll(string DistrictId)
            {
                try
                {
                    var data = db.Query<Commune>("LocationDistrict_SelectByDistrictId", new
                    {
                        DistrictId = DistrictId
                    }).ToList() ?? new List<Commune>();
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<Commune>();
                }
            }
        }
    }
}

