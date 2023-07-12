using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyProject.ViewModels.SiteMap
{
    [XmlType(Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
    public class SiteMapImage
    {
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