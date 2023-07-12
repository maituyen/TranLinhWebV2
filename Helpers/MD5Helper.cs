using System.Security.Cryptography;
using System.Text;

namespace MyProject.Helpers;

public class MD5Helper
{
    public static string GetMD5(string? str)
    {
        if (str == null) return "";
        var md5 = MD5.Create();
        byte[] fromData = Encoding.UTF8.GetBytes(str);
        byte[] targetData = md5.ComputeHash(fromData);
        string byte2String = "";

        for (int i = 0; i < targetData.Length; i++)
        {
            byte2String += targetData[i].ToString("x2");

        }
        return byte2String;
    }
}
