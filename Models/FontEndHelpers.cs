using System;
namespace MyProject.Models
{
	public class FontEndHelpers
	{
		public class SearchAutoComplete
		{
			public SearchAutoComplete()
			{
                Products = new HashSet<Product>();
                Categories = new HashSet<Category>();
            }
            public string KeyWord { set; get; }
			public virtual ICollection<Models.Category> Categories { set; get; }
            public virtual ICollection<Models.Product> Products { get; set; }
			public SearchAutoComplete Search(string KeyWord)
			{
				Models.Category category = new Models.Category();
                Models.Product product = new Models.Product();
				SearchAutoComplete data = new SearchAutoComplete()
				{
					KeyWord = KeyWord,
					Products = product.SearchByKey(KeyWord),
					Categories = category.SearchByKey(KeyWord)

                };
				return data;
            }
        }
        public class FillterOptions
        {

            public FillterOptions()
            {
                Products = new HashSet<Product>();
                Categories = new HashSet<Category>();
            }
            public string KeyWord { set; get; }
            public virtual ICollection<Models.Category> Categories { set; get; }
            public virtual ICollection<Models.Product> Products { get; set; }
            public SearchAutoComplete Search(string KeyWord)
            {
                Models.Category category = new Models.Category();
                Models.Product product = new Models.Product();
                SearchAutoComplete data = new SearchAutoComplete()
                {
                    KeyWord = KeyWord,
                    Products = product.SearchByKey(KeyWord),
                    Categories = category.SearchByKey(KeyWord)

                };
                return data;
            }
        }
        public FontEndHelpers()
		{

		}
	}
}

