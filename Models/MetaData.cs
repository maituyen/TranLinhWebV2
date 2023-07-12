using System;
using MyProject.Data.Entities;

namespace MyProject.Models
{
	public class MetaData
	{
        Helpers.Database db = new Helpers.Database(); 
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int Sort { set; get; }
        public int AllowFillter { set; get; }
        public int AllowInput { set; get; } 
        public virtual ICollection<MetaDataValues> MetaDataValues { get; set; }
        public MetaData()
        {

        }
        public MetaData(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(MetaData data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.Description = data.Description;
            this.Sort = data.Sort;
            this.AllowFillter = data.AllowFillter;
            this.AllowInput = data.AllowInput;
            this.Id = data.Id;
            this.Name = data.Name;
            this.Description = data.Description;
            this.Sort = data.Sort;
        }
        public MetaData GetById(int Id)
        {
            try
            {
                return db.Query<MetaData>("MetaData_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new MetaData();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaData();
            }
        } 
        public List<MetaData> GetAllowFillter()
        {
            try
            {
                var data = db.Query<MetaData>("MetaData_SelectAllowFillter").ToList() ?? new List<MetaData>();
                MetaDataValues metaDataValue = new MetaDataValues();
                foreach (var item in data)
                {
                    item.MetaDataValues = metaDataValue.GetAllByMetaDataId(item.Id);
                }
                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<MetaData>();
            }
        }
        private MetaData Save()
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
        public MetaData Insert()
        {
            try
            {
                return db.Query<MetaData>("MetaData_Insert", new
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Sort = Sort,
                    AllowFillter = AllowFillter,
                    AllowInput = AllowInput, 
                }).FirstOrDefault() ?? new MetaData();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaData();
            }
        }
        public MetaData Update()
        {
            try
            {
                return db.Query<MetaData>("MetaData_Update", new
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Sort = Sort,
                    AllowFillter = AllowFillter,
                    AllowInput = AllowInput,
                }).FirstOrDefault() ?? new MetaData();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaData();
            }
        }
    }
}