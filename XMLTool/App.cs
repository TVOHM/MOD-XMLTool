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
    [XmlRoot("application")]
    public class App
    {
        public App()
        {
            mName = "MyApplication";
            mContent = new List<Content>();
            mActors = new HashSet<string>();
        }

        public App(App app)
        {
            mName = app.mName;
            mIcon = app.mIcon;
            mContent = app.mContent;
        }

        [XmlAttribute("name")]
        public string mName { get; set; }

        [XmlAttribute("icon")]
        public byte[] mIcon { get; set; }

        [XmlElement("content")]
        public List<Content> mContent { get; set; }

        [XmlElement("actor")]
        public HashSet<string> mActors { get; set; }

        public void save(string file)
        {
            Stream stream = File.Create(file);
            XmlSerializer serializer = new XmlSerializer(typeof(App));
            serializer.Serialize(stream, this);
            stream.Close();
        }

        public static App load(string file)
        {
            Stream stream = File.OpenRead(file);
            XmlSerializer serializer = new XmlSerializer(typeof(App));
            App application = (App)serializer.Deserialize(stream);
            stream.Close();
            return application;
        }
    }
}
