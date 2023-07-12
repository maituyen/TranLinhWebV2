using System;
namespace MyProject.Helpers
{
	public class ReturnClient
	{
		public bool sucess { set; get; } = true;
        public string message { set; get; } = "";
		public object result { set; get; }
        public ReturnClient()
		{
		}
	}
}

