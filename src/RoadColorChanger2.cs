﻿using UnityEngine;
using ColossalFramework;

using System;
using System.IO;
using System.Collections.Generic;

namespace RoadColorChangerContinued
{

    public class RoadColorChanger2 : MonoBehaviour
    {
        public static Texture2D LoadTextureDDS(string texturePath)
        {
            var ddsBytes = File.ReadAllBytes(texturePath);
            var height = BitConverter.ToInt32(ddsBytes, 12);
            var width = BitConverter.ToInt32(ddsBytes, 16);
            var texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < ddsBytes.Length; i++)
            {
                if (i > 127)
                {
                    byteList.Add(ddsBytes[i]);
                }
            }

            texture.LoadRawTextureData(byteList.ToArray());

            texture.Apply();
            texture.anisoLevel = 8;
            return texture;

        }

        public static void ChangeColor(float red, float green, float blue, string roadtype, string dir)
        {
            var collection = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            foreach (var nc in collection)
            {
                foreach (var prefab in nc.m_prefabs)
                {
                    if (prefab.m_class.name.ToLower().Contains(roadtype))
                    {
                        prefab.m_color = new Color(red, green, blue);

                        /* This is special case, since we need to load textures */
                        /* TODO: Shoud be a separate function */
                        if (prefab.m_class.name.Equals("Highway"))
                        {

                            foreach (var segment in prefab.m_segments)
                            {
                                if (!segment.m_material.name.ToLower().Contains("cable"))
                                {
                                    Texture2D tex = new Texture2D(1, 1);
                                    if (prefab.name.Equals("HighwayRamp") || prefab.name.Equals("HighwayRampElevated"))
                                    {
                                        tex = LoadTextureDDS(Path.Combine(dir, "highway_ramp_segment_apr.dds"));
                                    }
                                    else {
                                        tex = LoadTextureDDS(Path.Combine(dir, "highway_segment_apr.dds"));
                                    }
                                    tex.anisoLevel = 8;
                                    segment.m_segmentMaterial.SetTexture("_APRMap", tex);
                                    segment.m_lodMesh = null;
                                }
                            }

                            foreach (var node in prefab.m_nodes)
                            {
                                Texture2D tex = new Texture2D(1, 1);
                                if (prefab.name.Equals("HighwayRamp") || prefab.name.Equals("HighwayRampElevated"))
                                {
                                    tex = LoadTextureDDS(Path.Combine(dir, "highway_ramp_node_apr.dds"));
                                }
                                else {
                                    tex = LoadTextureDDS(Path.Combine(dir, "highway_node_apr.dds"));
                                }
                                tex.anisoLevel = 8;
                                node.m_nodeMaterial.SetTexture("_APRMap", tex);
                                node.m_lodMesh = null;
                            }
                            prefab.RefreshLevelOfDetail();
                        }
                    }
                }
            }

            foreach (var nn in Singleton<NetManager>.instance.m_nodes.m_buffer)
            {
                if (nn.Info.m_class.name.ToLower().Contains(roadtype))
                {
                    nn.Info.m_color = new Color(red, green, blue);
                }
            }

            foreach (var ns in Singleton<NetManager>.instance.m_segments.m_buffer)
            {
                if (ns.Info.m_class.name.ToLower().Contains(roadtype))
                {
                    ns.Info.m_color = new Color(red, green, blue);
                }
            }
        }

    }

}