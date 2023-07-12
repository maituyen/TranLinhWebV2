using MyProject.Constants;
using MyProject.Data.Entities;

namespace MyProject.Helpers;

public static class HasTagHelper
{
    static IEnumerable<string> codeDate = new List<string> { "#ngay", "#ngày", "#date" };
    static IEnumerable<string> codeMonth = new List<string> { "#thang", "#tháng", "#month" };
    static IEnumerable<string> codeYear = new List<string> { "#nam", "#năm", "#year" };

    public static string GetDateNow(string? value,string Name)
    {
        if (string.IsNullOrEmpty(value)) return "";
        var date = DateTime.Now;

        if (codeDate.Contains(value)) return $"{date.Day}/{date.Month}/{date.Year}";
        if (codeMonth.Contains(value)) return $"{date.Month}/{date.Year}";
        if (codeYear.Contains(value)) return date.Year.ToString();

        return Name;
    }

    public static string ReplaceHastag(string? value, List<Hastag?> tags)
    {
        if (string.IsNullOrEmpty(value)) return "";

        if (tags == null && tags?.Count > 0) return value;

        tags = tags
            .OrderByDescending(x => x?.Name)
            .Where(x => x?.Type == (int)EnumTypeHastag.Text)
            .ToList();

        foreach (var item in tags)
        {
            if (value.Contains(item?.Code))
            {
                value = value.Replace(item.Code, item.Name);
            }
        }

        return value;
    }
} 