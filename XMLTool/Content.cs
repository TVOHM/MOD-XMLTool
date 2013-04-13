using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XMLTool
{
    public class Content
    {
        public Content()
        {
            mName = "My Content";
            mText = "";
            mActors = new List<string>();
            mVideos = new List<Video>();
        }

        [XmlAttribute("name")]
        public string mName { get; set; }

        [XmlAttribute("text")]
        public string mText { get; set; }

        [XmlElement("actor")]
        public List<string> mActors { get; set; }

        [XmlElement("video")]
        public List<Video> mVideos { get; set; }
    }
}
