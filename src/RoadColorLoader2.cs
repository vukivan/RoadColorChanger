using ICities;
using UnityEngine;
using ColossalFramework.Steamworks;
using ColossalFramework.IO;

using System;

namespace RoadColorChangerContinued
{
    public class RoadColorLoader2 : LoadingExtensionBase
    {

        public Configuration2 config;
        public static readonly string configPath = "RoadColorConfig.xml";

        public bool arActive;

        GameObject hookGo;
        Hook4 hook;

        public static string getModPath()
        {
            string workshopPath = ".";
            foreach (PublishedFileId mod in Steam.workshop.GetSubscribedItems())
            {
                if (mod.AsUInt64 == RoadColorMod.workshop_id)
                {
                    workshopPath = Steam.workshop.GetSubscribedItemPath(mod);
                    Debug.Log("Road Color Changer: Workshop path: " + workshopPath);
                    break;
                }
            }
            string localPath = DataLocation.modsPath + "/RoadColorChanger";
            Debug.Log("Road Color Changer: " + localPath);
            if (System.IO.Directory.Exists(localPath))
            {
                Debug.Log("Road Color Changers: Local path exists, looking for assets here: " + localPath);
                return localPath;
            }
            return workshopPath;

        }

#if false
        public static bool americanRoadsActive()
        {
            foreach (var plugin in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo())
            {
                if (plugin.isEnabled)
                {
                    if (plugin.name.Equals("418637762"))
                    {
                        //Debug.Log ("AR ENABLED");
                        return true;
                    }
                }
            }
            return false;
        } 
#endif

        public override void OnLevelLoaded(LoadMode mode)
        {
            config = Configuration2.Deserialize(configPath);
            if (config == null)
            {
                config = new Configuration2();
            }
            SaveConfig();
            //RoadColorChanger.fixAPR ();
            //RoadColorChanger.highwayfix ();
            String path = getModPath();
#if false
            //Hook4.arActive = arActive;
            arActive = americanRoadsActive();
#else
            arActive = false;
#endif

            //if (!arActive) {
            hookGo = new GameObject("RCC hook");
            hook = hookGo.AddComponent<Hook4>();

            //}

            RoadColorChanger2.ChangeColor(arActive, config.large_road_red, config.large_road_green, config.large_road_blue, "Large Road", path);

            RoadColorChanger2.ChangeColor(arActive, config.medium_road_red, config.medium_road_green, config.medium_road_blue, "Medium Road", path);
            RoadColorChanger2.ChangeColor(arActive, config.small_road_red, config.small_road_green, config.small_road_blue, "Small Road", path);
            RoadColorChanger2.ChangeColor(arActive, config.small_road_red, config.small_road_green, config.small_road_blue, "Electricity Dam", path);
            RoadColorChanger2.ChangeColor(arActive, config.small_road_red, config.small_road_green, config.small_road_blue, "Train Track", path);
            RoadColorChanger2.ReplaceLodAprAtlas(path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "Highway", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExt2LAlley", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExt1LOneway", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtSmall3LRoad", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtSmall4LRoad", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtMediumRoad", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtMediumRoadTL", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtXLargeRoad", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtLargeRoad", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtSmall3LRoadTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtSmall4LRoadTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtMediumRoadTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtMediumRoadTLTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtXLargeRoadTunnel", path);

            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtPedRoad", path);

            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighway1L", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighway2L", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighway4L", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighway5L", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighway6L", path);

            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel1LTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel2LTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel4LTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel5LTunnel", path);
            RoadColorChanger2.ChangeColor(arActive, config.highway_red, config.highway_green, config.highway_blue, "NExtHighwayTunnel6LTunnel", path);

            //Singleton<NetManager>.instance.Update
        }

        public override void OnLevelUnloading()
        {
            //if (!arActive) {
            hook.DisableHook();

            GameObject.Destroy(hookGo);
            hook = null;
            //}
            base.OnLevelUnloading();
        }

        void SaveConfig()
        {
            Configuration2.Serialize(configPath, config);
        }

    }
}
