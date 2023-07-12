using Google.Apis.PeopleService.v1.Data;
using SlugGenerator;

namespace MyProject.Helpers;

public static class SlugHelper
{
    public static string SlugNameUrl(string? str)
    {
        try
        {
            if (string.IsNullOrEmpty(str)) return "";

            str = SlugGenerator.SlugGenerator.GenerateSlug(str.ToLower());
            if (Exists(str))
                return str + Guid.NewGuid().ToString().Split("-")[0].ToString();
            else return str;
        }
        catch (Exception ex)
        {
            return str;
        }
    }
    public static bool Exists(string str)
    { 
        Helpers.Database db = new Helpers.Database();
        var data = db.Execute("CheckExistsSlug", new
        {
            Slug = str
        });
        return data > 0;
    }
    public static string CombineUrl(dynamic children, dynamic parent, bool isRemoveHtml = false)
    {
        children = (children ?? "").ToString().Split(".")[0];
        parent = (parent ?? "").ToString().Split(".")[0];

        var htmlSuffix = isRemoveHtml ? "" : ".html";
        if (string.IsNullOrEmpty(children) || string.IsNullOrEmpty(parent))
        {
            var urlPrefix = string.IsNullOrEmpty(children) ? parent : children;
            return $"{urlPrefix}{htmlSuffix}";
        }
        return $"{(string)parent}/{children}{htmlSuffix}";
    } 
    public static string CombineLevelUrl(
        dynamic? lv1,
        dynamic? lv2,
        dynamic? lv3,
        bool isRemoveHtml = false)
    {
        var url = new List<string>();
        var htmlSuffix = isRemoveHtml ? "" : ".html";

        lv1 = (lv1 ?? "").ToString();
        lv2 = (lv2 ?? "").ToString();
        lv3 = (lv3 ?? "").ToString();

        if (!string.IsNullOrEmpty(lv1)) url.Add(lv1.Split(".")[0]);
        if (!string.IsNullOrEmpty(lv2)) url.Add(lv2.Split(".")[0]);
        if (!string.IsNullOrEmpty(lv3)) url.Add(lv3.Split(".")[0]);

        return string.Join("/", url)  + htmlSuffix;
    }
}
