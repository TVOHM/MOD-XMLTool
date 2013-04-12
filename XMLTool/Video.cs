using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XMLTool
{
    public class Video
    {
        [XmlAttribute("name")]
        public String mName { get; set; }

        [XmlAttribute("data")]
        public byte[] mData { get; set; }

        [XmlAttribute("icon")]
        public byte[] mIcon { get; set; }
    }
}
