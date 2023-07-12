using System;
namespace MyProject.Models
{
	public class HastagRender
	{
        Helpers.Database db = new Helpers.Database();
        public int Id { set; get; }
        public string Name { set; get; }
        public string CodeRender { set; get; }
        public HastagRender()
		{
		}
        public List<HastagRender> GetAll()
        {
            try
            {
                var data = db.Query<HastagRender>("HastagRender_SelectAll").ToList() ?? new List<HastagRender>();

                return data;
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz(ex.Message);
                return new List<HastagRender>();
            }
        }
    }
}

