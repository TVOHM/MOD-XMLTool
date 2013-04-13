using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XMLTool
{
    public class Video
    {
        public Video()
        {
            mName = "";

            MemoryStream ms = new MemoryStream();
            Properties.Resources.default_image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            mIcon = ms.ToArray();

            mData = Properties.Resources.default_video;
        }

        [XmlAttribute("name")]
        public String mName { get; set; }

        [XmlAttribute("data")]
        public byte[] mData { get; set; }

        [XmlAttribute("icon")]
        public byte[] mIcon { get; set; }
    }
}
