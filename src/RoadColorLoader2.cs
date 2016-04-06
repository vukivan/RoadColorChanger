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


            RoadColorChanger2.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "Large Road", path);

            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "Medium Road", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "Small Road", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "Electricity Dam", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "Highway", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExt2LAlley", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExt1LOneway", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExtSmall3LRoad", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExtSmall4LRoad", path);
            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "NExtMediumRoad", path);
            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "NExtMediumRoadTL", path);
            RoadColorChanger2.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "NExtXLargeRoad", path);
            RoadColorChanger2.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "NExtLargeRoad", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExtSmall3LRoadTunnel", path);
            RoadColorChanger2.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "NExtSmall4LRoadTunnel", path);
            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "NExtMediumRoadTunnel", path);
            RoadColorChanger2.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "NExtMediumRoadTLTunnel", path);
            RoadColorChanger2.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "NExtXLargeRoadTunnel", path);

            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtPedRoad", path);

            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighway1L", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighway2L", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighway4L", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighway5L", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighway6L", path);

            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel1LTunnel", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel2LTunnel", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel4LTunnel", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel5LTunnel", path);
            RoadColorChanger2.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel6LTunnel", path);
        }

        void SaveConfig()
        {
            Configuration2.Serialize(configPath, config);
        }

    }
}
