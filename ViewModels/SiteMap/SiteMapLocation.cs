using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyProject.ViewModels.SiteMap
{
    public class SiteMapLocation
    {
        public enum eChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        } 
        [XmlElement("loc")]
        public string loc { get; set; } = string.Empty;
        [XmlElement("lastmod")]
        public string lastmod { get; set; } = string.Empty;
        [XmlElement("changefreq")]
        public string changefreq { get; set; } = string.Empty;
        [XmlElement("priority")]
        public string priority { get; set; } = string.Empty;
    }
}