using ICities;
using ColossalFramework.Steamworks;
using ColossalFramework.IO;

using System;

namespace RoadColorChangerContinued
{
    public class RoadColorLoader2 : LoadingExtensionBase
    {

        public Configuration2 config;
        public static readonly string configPath = "RoadColorConfig.xml";

        public static string getModPath()
        {
            string workshopPath = ".";
            foreach (PublishedFileId mod in Steam.workshop.GetSubscribedItems())
            {
                if (mod.AsUInt64 == RoadColorMod.workshop_id)
                {
                    workshopPath = Steam.workshop.GetSubscribedItemPath(mod);                    
                    break;
                }
            }
            string localPath = DataLocation.modsPath + "/RoadColorChanger";
            if (System.IO.Directory.Exists(localPath))
            {
                return localPath;
            }
            return workshopPath;

        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            config = Configuration2.Deserialize(configPath);
            if (config == null)
            {
                config = new Configuration2();
            }
            SaveConfig();
            String path = getModPath();

            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "highway", path);
            RoadColorChanger2.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "large", path);
            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "medium", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "small", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "dam", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "alley", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "oneway", path);
        }

        void SaveConfig()
        {
            Configuration2.Serialize(configPath, config);
        }

    }
}
