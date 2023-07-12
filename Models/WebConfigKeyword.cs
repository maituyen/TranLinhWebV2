using System;
namespace MyProject.Models
{
	public class WebConfigKeyword
	{ 
            Helpers.Database db = new Helpers.Database();
        public int WebconfigId { get; set; }
        public int KeywordId { get; set; }
        public int  Index { get; set; }
        public virtual KeyWord KeyWordInfo { get; set; } = null!;
        public WebConfigKeyword()
            {
            }
            public WebConfigKeyword(int Id)
            {
                LoadData(GetById(Id));
            }
            public void LoadData(WebConfigKeyword data)
            {
            this.WebconfigId = data.WebconfigId;
            this.KeywordId = data.KeywordId;
            this.Index = data.Index;
            this.WebconfigId = data.WebconfigId;
            this.KeywordId = data.KeywordId;
            this.Index = data.Index;
        }

            public WebConfigKeyword GetById(int Id)
            {
                try
                {
                    return db.Query<WebConfigKeyword>("WebConfigKeyword_SelectById", new
                    {
                        Id = Id
                    }).FirstOrDefault() ?? new WebConfigKeyword();
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new WebConfigKeyword();
                }
            }
            public List<WebConfigKeyword> GetByWebConfigId(int WebconfigId)
            {
                try
                {
                    var data = db.Query<WebConfigKeyword>("WebConfigKeyword_SelectByWebConfigId", new
                    {
                        WebconfigId = WebconfigId
                    }).ToList() ?? new List<WebConfigKeyword>();
                    foreach (var item in data)
                    {
                        item.KeyWordInfo = new KeyWord(item.KeywordId);
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new List<WebConfigKeyword>();
                }
            }
            private WebConfigKeyword Save()
            {
                if (this.WebconfigId == -1)
                {
                    return Insert();
                }
                else
                {
                    return Update();
                }
            }
            public WebConfigKeyword Insert()
            {
                try
                {
                    return db.Query<WebConfigKeyword>("WebConfigKeyword_Insert", new
                    {
                        WebconfigId = WebconfigId,
                        KeywordId = KeywordId,
                        Index = Index, 
                    }).FirstOrDefault() ?? new WebConfigKeyword();
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new WebConfigKeyword();
                }
            }
            public WebConfigKeyword Update()
            {
                try
                {
                    return db.Query<WebConfigKeyword>("WebConfigKeyword_Update", new
                    {
                        WebconfigId = WebconfigId,
                        KeywordId = KeywordId,
                        Index = Index,
                    }).FirstOrDefault() ?? new WebConfigKeyword();
                }
                catch (Exception ex)
                {
                    Helpers.SocialTelegram.Buzz(ex.Message);
                    return new WebConfigKeyword();
                }
            }
        }
    } 