using System;
namespace MyProject.Models
{
	public class MetaDataValues
    {
        Helpers.Database db = new Helpers.Database();
        public int Id { get; set; }
        public string MetaDataId { get; set; }
        public string Value { get; set; }
        public MetaDataValues()
        {

        }
        public MetaDataValues(int Id)
        {
            LoadData(GetById(Id));
        }
        public void LoadData(MetaDataValues data)
        {
            this.Id = data.Id;
            this.MetaDataId = data.MetaDataId;
            this.Value = data.Value;
        }
        public MetaDataValues GetById(int Id)
        {
            try
            {
                return db.Query<MetaDataValues>("MetaDataValues_SelectById", new
                {
                    Id = Id
                }).FirstOrDefault() ?? new MetaDataValues();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaDataValues();
            }
        }
        /// <summary>
        /// Status=-1 lấy tất cả
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<MetaDataValues> GetAllByMetaDataId(int MetaDataId)
        {
            try
            {
                var data = db.Query<MetaDataValues>("MetaDataValues_SelectByMetaDataId", new
                {
                    MetaDataId = MetaDataId
                }).ToList() ?? new List<MetaDataValues>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<MetaDataValues>();
            }
        }
        private MetaDataValues Save()
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
        public MetaDataValues Insert()
        {
            try
            {
                return db.Query<MetaDataValues>("MetaDataValues_Insert", new
                {
                    Id = Id,
                    MetaDataId = MetaDataId,
                    Value = Value,
                }).FirstOrDefault() ?? new MetaDataValues();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaDataValues();
            }
        }
        public MetaDataValues Update()
        {
            try
            {
                return db.Query<MetaDataValues>("MetaDataValues_Update", new
                {
                    Id = Id,
                    MetaDataId = MetaDataId,
                    Value = Value,
                }).FirstOrDefault() ?? new MetaDataValues();
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new MetaDataValues();
            }
        }
    }
} 