using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyProject.ViewModels.SiteMap
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMap
    {
        private ArrayList map;
        public SiteMap()
        {
            map = new ArrayList();
        }

        [XmlElement("url")]
        public SiteMapLocation[] Locations
        {
            get
            {
                SiteMapLocation[] items = new SiteMapLocation[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value != null)
                    return;
                SiteMapLocation[] items = (SiteMapLocation[])value!;
                map.Clear();
                foreach (SiteMapLocation item in items)
                    map.Add(item);
            }
        }

        public string GetSiteMapXml()
        {
            return string.Empty;
        }

        public int Add(SiteMapLocation item)
        {
            return map.Add(item);
        }

        public void AddRange(IEnumerable<SiteMapLocation> locs)
        {
            foreach (var i in locs)
                map.Add(i);
        }

        public void WriteSiteMapToFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //ns.Add("images", "http://www.google.com/schemas/sitemap/1.1");
                XmlSerializer xs = new XmlSerializer(typeof(SiteMap));
                xs.Serialize(fs, this, ns);
            }
        }
    }
}