using System.Text;

namespace MyProject.Helpers;

public class UnicodeHelper
{
    public static string RemoveUnicode(string? text)
    {
        if (text == null) return "";
        string[] arr1 = new string[] {
            "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ", "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ"
        };
        string[] arr2 = new string[] {
            "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "d",
            "e","e","e","e","e","e","e","e","e","e","e", "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y","@","%"
        };

        for (int i = 0; i < arr1.Length; i++)
        {
            text = text.Replace(arr1[i], arr2[i]);
            text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
        }

        var array = text.Split(' ');
        var list = new List<string>();
        foreach (var arr in array)
        {
            if (arr != "%" || arr != "@" || arr != "!" || arr != "$")
            {
                list.Add(RemoveSpecialCharacters(arr).Trim());
            }
        }
        return string.Join("-", list);
    }
    public static string RemoveSpecialCharacters(string str)
    {
        var sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                c == '.' ||
                c == '_'
            )
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
