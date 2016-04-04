using System.Xml.Serialization;

using System.IO;

namespace RoadColorChangerContinued
{
    public class Configuration2
    {

        public float highway_red = 0.25f;
        public float highway_green = 0.25f;
        public float highway_blue = 0.25f;

        public float large_road_red = 0.25f;
        public float large_road_green = 0.25f;
        public float large_road_blue = 0.25f;

        public float medium_road_red = 0.25f;
        public float medium_road_green = 0.25f;
        public float medium_road_blue = 0.25f;

        public float small_road_red = 0.25f;
        public float small_road_green = 0.25f;
        public float small_road_blue = 0.25f;


        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration2 config)
        {
            var serializer = new XmlSerializer(typeof(Configuration2));

            using (var writer = new StreamWriter(filename))
            {
                config.OnPreSerialize();
                serializer.Serialize(writer, config);
            }
        }

        public static Configuration2 Deserialize(string filename)
        {
            var serializer = new XmlSerializer(typeof(Configuration2));

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var config = (Configuration2)serializer.Deserialize(reader);
                    config.OnPostDeserialize();
                    return config;
                }
            }
            catch { }

            return null;
        }
    }
}
