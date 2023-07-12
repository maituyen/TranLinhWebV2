using System;
using Microsoft.AspNetCore.SignalR;

namespace MyProject.Helpers
{
	public static class Files
	{
		enum style
		{
			categories,
			blog,
			product,
		} 
        /// <summary>
        /// Cấu trúc thư mục
        /// Danh mục
        /// Files/Danh-muc/
        /// Sản phẩm
        /// Files/danh-muc/san-pham
        /// </summary>
        /// <param name="root"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        private static string UrlFolder(string root, string sub)
        { 
            if (string.IsNullOrWhiteSpace(sub))
                return "/files/" + NonUnicode(root) + "/";
            else
              return "/files/" + NonUnicode(root) + "/" + NonUnicode(sub) + "/";
           
        }
        public static void Delete(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var folderName = Path.Combine("wwwroot", filePath);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!System.IO.File.Exists(pathToSave))
                    System.IO.File.Delete(pathToSave);
            }
        }
        public static string SaveFile(string root,string sub, string fileName, string base64String)
        {

            try
            {
                fileName = NonUnicode(fileName.Split(".")[0].ToString()) + ".webp";
                var folderName = "wwwroot"+ UrlFolder(root,sub);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!System.IO.Directory.Exists(pathToSave))
                    System.IO.Directory.CreateDirectory(pathToSave);
                base64String = base64String.Split(',')[1].ToString();
                byte[] bytes = Convert.FromBase64String(base64String);
                using (MemoryStream webpStream = new MemoryStream(bytes))
                {
                    using (FileStream fileStream = new FileStream(pathToSave + "/" + fileName, FileMode.Create, FileAccess.Write))
                    {
                        webpStream.CopyTo(fileStream);
                    }
                }
                return Path.Combine(UrlFolder(root, sub), fileName);
            }
            catch (Exception ex)
            {
                Helpers.SocialTelegram.Buzz("SaveFile " + ex.Message);
                return "";
            }
        }
        public static void SaveFileV1(string path, string fileName, string base64String)
        {

            try
            {
                var folderName = Path.Combine("wwwroot", path);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!System.IO.Directory.Exists(pathToSave))
                    System.IO.Directory.CreateDirectory(pathToSave);
                base64String = base64String.Split(',')[1].ToString();
                byte[] bytes = Convert.FromBase64String(base64String);
                using (MemoryStream webpStream = new MemoryStream(bytes))
                {
                    using (FileStream fileStream = new FileStream(pathToSave + "/" + fileName, FileMode.Create, FileAccess.Write))
                    {
                        webpStream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text.ToLower();
        }
    }
}

