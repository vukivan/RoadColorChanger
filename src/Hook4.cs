using UnityEngine;
using ColossalFramework;

using System.Collections.Generic;
using System.Reflection;

namespace RoadColorChangerContinued
{
    public class Hook4 : MonoBehaviour
    {
        public bool hookEnabled = false;
        private Dictionary<MethodInfo, RedirectCallsState> redirects = new Dictionary<MethodInfo, RedirectCallsState>();
        public static bool arActive = false;

        public bool colorsChanged = false;

        //public void Awake(){
        //arActive = RoadColorLoader.americanRoadsActive ();
        //}





        public void Update()
        {
            if (!hookEnabled)
            {
                EnableHook();
            }

            if (!colorsChanged)
            {
                colorsChanged = true;
            }

        }

        public void EnableHook()
        {
#if false
            arActive = RoadColorLoader2.americanRoadsActive();
            if (!arActive)
            {
                var allFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
                var method = typeof(NetSegment).GetMethods(allFlags).Single(c => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
                redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethod("RenderInstanceSegment", allFlags)));

                method = typeof(NetSegment).GetMethods(allFlags).Single(c => c.Name == "RenderLod");
                redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethod("RenderInstanceSegment", allFlags)));

                method = typeof(NetNode).GetMethods(allFlags).Single(c => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
                redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethods(allFlags).Single(c => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));

                method = typeof(NetNode).GetMethods(allFlags).Single(c => c.Name == "RenderLod");
                redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethods(allFlags).Single(c => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));

            }
#endif
            hookEnabled = true;
        }

        public void DisableHook()
        {
            if (!hookEnabled)
            {
                return;
            }
            foreach (var kvp in redirects)
            {

                RedirectionHelper.RevertRedirect(kvp.Key, kvp.Value);
            }
            redirects.Clear();
            hookEnabled = false;
        }

        private MethodInfo GetMethod(string name, uint argCount)
        {
            MethodInfo[] methods = typeof(NetNode).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MethodInfo m in methods)
            {
                if (m.Name == name && m.GetParameters().Length == argCount)
                    return m;
            }
            return null;
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex)
        {

            MethodInfo refreshJunctionData = GetMethod("RefreshJunctionData", 3);
            object[] p = new object[] { nodeID, info, instanceIndex };
            refreshJunctionData.Invoke(netnode, p);
        }

        private void RefreshBendData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
        {

            MethodInfo refreshBendData = GetMethod("RefreshBendData", 4);
            object[] p = new object[] { nodeID, info, instanceIndex, data };
            refreshBendData.Invoke(netnode, p);
            data = (RenderManager.Instance)p[3];
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, ushort nodeSegment, Vector3 centerPos, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            NetManager instance = Singleton<NetManager>.instance;
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 cornerPos1 = Vector3.zero;
            Vector3 cornerPos2 = Vector3.zero;
            Vector3 cornerDirection1 = Vector3.zero;
            Vector3 cornerDirection2 = Vector3.zero;
            Vector3 cornerPos3 = Vector3.zero;
            Vector3 cornerPos4 = Vector3.zero;
            Vector3 cornerDirection3 = Vector3.zero;
            Vector3 cornerDirection4 = Vector3.zero;
            Vector3 cornerPos5 = Vector3.zero;
            Vector3 cornerPos6 = Vector3.zero;
            Vector3 cornerDirection5 = Vector3.zero;
            Vector3 cornerDirection6 = Vector3.zero;
            NetSegment netSegment1 = instance.m_segments.m_buffer[(int)nodeSegment];
            NetInfo info1 = netSegment1.Info;
            ItemClass connectionClass1 = info1.GetConnectionClass();
            Vector3 vector3_1 = (int)nodeID != (int)netSegment1.m_startNode ? netSegment1.m_endDirection : netSegment1.m_startDirection;
            float num1 = -4f;
            float num2 = -4f;
            ushort segmentID1 = (ushort)0;
            ushort segmentID2 = (ushort)0;
            for (int index = 0; index < 8; ++index)
            {
                ushort segment = netnode.GetSegment(index);
                if ((int)segment != 0 && (int)segment != (int)nodeSegment)
                {
                    ItemClass connectionClass2 = instance.m_segments.m_buffer[(int)segment].Info.GetConnectionClass();
                    if (connectionClass1.m_service == connectionClass2.m_service)
                    {
                        NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segment];
                        Vector3 vector3_2 = (int)nodeID != (int)netSegment2.m_startNode ? netSegment2.m_endDirection : netSegment2.m_startDirection;
                        float num3 = (float)((double)vector3_1.x * (double)vector3_2.x + (double)vector3_1.z * (double)vector3_2.z);
                        if ((double)vector3_2.z * (double)vector3_1.x - (double)vector3_2.x * (double)vector3_1.z < 0.0)
                        {
                            if ((double)num3 > (double)num1)
                            {
                                num1 = num3;
                                segmentID1 = segment;
                            }
                            float num4 = -2f - num3;
                            if ((double)num4 > (double)num2)
                            {
                                num2 = num4;
                                segmentID2 = segment;
                            }
                        }
                        else
                        {
                            if ((double)num3 > (double)num2)
                            {
                                num2 = num3;
                                segmentID2 = segment;
                            }
                            float num4 = -2f - num3;
                            if ((double)num4 > (double)num1)
                            {
                                num1 = num4;
                                segmentID1 = segment;
                            }
                        }
                    }
                }
            }
            bool start1 = (int)netSegment1.m_startNode == (int)nodeID;
            bool smooth;
            netSegment1.CalculateCorner(nodeSegment, true, start1, false, out cornerPos1, out cornerDirection1, out smooth);
            netSegment1.CalculateCorner(nodeSegment, true, start1, true, out cornerPos2, out cornerDirection2, out smooth);
            if ((int)segmentID1 != 0 && (int)segmentID2 != 0)
            {
                float x = (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5);
                float y = 1f;
                if ((int)segmentID1 != 0)
                {
                    NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segmentID1];
                    NetInfo info2 = netSegment2.Info;
                    bool start2 = (int)netSegment2.m_startNode == (int)nodeID;
                    netSegment2.CalculateCorner(segmentID1, true, start2, true, out cornerPos3, out cornerDirection3, out smooth);
                    netSegment2.CalculateCorner(segmentID1, true, start2, false, out cornerPos4, out cornerDirection4, out smooth);
                    float num3 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
                    x = (float)(((double)x + (double)num3) * 0.5);
                    y = (float)(2.0 * (double)info1.m_halfWidth / ((double)info1.m_halfWidth + (double)info2.m_halfWidth));
                }
                float z = (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5);
                float w = 1f;
                if ((int)segmentID2 != 0)
                {
                    NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segmentID2];
                    NetInfo info2 = netSegment2.Info;
                    bool start2 = (int)netSegment2.m_startNode == (int)nodeID;
                    netSegment2.CalculateCorner(segmentID2, true, start2, true, out cornerPos5, out cornerDirection5, out smooth);
                    netSegment2.CalculateCorner(segmentID2, true, start2, false, out cornerPos6, out cornerDirection6, out smooth);
                    float num3 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
                    z = (float)(((double)z + (double)num3) * 0.5);
                    w = (float)(2.0 * (double)info1.m_halfWidth / ((double)info1.m_halfWidth + (double)info2.m_halfWidth));
                }
                Vector3 middlePos1_1;
                Vector3 middlePos2_1;
                NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos3, -cornerDirection3, true, true, out middlePos1_1, out middlePos2_1);
                Vector3 middlePos1_2;
                Vector3 middlePos2_2;
                NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos4, -cornerDirection4, true, true, out middlePos1_2, out middlePos2_2);
                Vector3 middlePos1_3;
                Vector3 middlePos2_3;
                NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos5, -cornerDirection5, true, true, out middlePos1_3, out middlePos2_3);
                Vector3 middlePos1_4;
                Vector3 middlePos2_4;
                NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos6, -cornerDirection6, true, true, out middlePos1_4, out middlePos2_4);
                data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_3, middlePos2_3, cornerPos5, cornerPos1, middlePos1_3, middlePos2_3, cornerPos5, netnode.m_position, vScale);
                data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_4, middlePos2_4, cornerPos6, cornerPos2, middlePos1_4, middlePos2_4, cornerPos6, netnode.m_position, vScale);
                data.m_dataVector0 = new Vector4(0.5f / info1.m_halfWidth, 1f / info1.m_segmentLength, (float)(0.5 - (double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5));
                data.m_dataVector1 = (Vector4)(centerPos - data.m_position);
                data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
                data.m_dataVector2 = new Vector4(x, y, z, w);
                data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            }
            else
            {
                centerPos.x = (float)(((double)cornerPos1.x + (double)cornerPos2.x) * 0.5);
                centerPos.z = (float)(((double)cornerPos1.z + (double)cornerPos2.z) * 0.5);
                Vector3 vector3_2 = cornerPos2;
                Vector3 vector3_3 = cornerPos1;
                Vector3 vector3_4 = cornerDirection2;
                Vector3 vector3_5 = cornerDirection1;
                float num3 = Mathf.Min(info1.m_halfWidth * 1.333333f, 16f);
                Vector3 vector3_6 = cornerPos1 - cornerDirection1 * num3;
                Vector3 vector3_7 = vector3_2 - vector3_4 * num3;
                Vector3 vector3_8 = cornerPos2 - cornerDirection2 * num3;
                Vector3 vector3_9 = vector3_3 - vector3_5 * num3;
                Vector3 vector3_10 = cornerPos1 + cornerDirection1 * num3;
                Vector3 vector3_11 = vector3_2 + vector3_4 * num3;
                Vector3 vector3_12 = cornerPos2 + cornerDirection2 * num3;
                Vector3 vector3_13 = vector3_3 + vector3_5 * num3;
                data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, vector3_6, vector3_7, vector3_2, cornerPos1, vector3_6, vector3_7, vector3_2, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, vector3_12, vector3_13, vector3_3, cornerPos2, vector3_12, vector3_13, vector3_3, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(cornerPos1, vector3_10, vector3_11, vector3_2, cornerPos1, vector3_10, vector3_11, vector3_2, netnode.m_position, vScale);
                data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos2, vector3_8, vector3_9, vector3_3, cornerPos2, vector3_8, vector3_9, vector3_3, netnode.m_position, vScale);
                data.m_dataMatrix0.SetRow(3, data.m_dataMatrix0.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_extraData.m_dataMatrix2.SetRow(3, data.m_extraData.m_dataMatrix2.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_extraData.m_dataMatrix3.SetRow(3, data.m_extraData.m_dataMatrix3.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_dataMatrix1.SetRow(3, data.m_dataMatrix1.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_dataVector0 = new Vector4(0.5f / info1.m_halfWidth, 1f / info1.m_segmentLength, (float)(0.5 - (double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5));
                data.m_dataVector1 = (Vector4)(centerPos - data.m_position);
                data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
                data.m_dataVector2 = new Vector4((float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), 1f, (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), 1f);
                data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            }
            data.m_dataInt0 = segmentIndex;
            data.m_dataColor0 = info1.m_color;
            data.m_dataColor0.a = 0.0f;
            if (info1.m_requireSurfaceMaps)
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector3);
            instanceIndex = (uint)data.m_nextInstance;
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, NetInfo info, ushort nodeSegment, ushort nodeSegment2, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 cornerPos1 = Vector3.zero;
            Vector3 cornerPos2 = Vector3.zero;
            Vector3 cornerPos3 = Vector3.zero;
            Vector3 cornerPos4 = Vector3.zero;
            Vector3 cornerDirection1 = Vector3.zero;
            Vector3 cornerDirection2 = Vector3.zero;
            Vector3 cornerDirection3 = Vector3.zero;
            Vector3 cornerDirection4 = Vector3.zero;
            bool start1 = (int)Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].m_startNode == (int)nodeID;
            bool smooth;
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start1, false, out cornerPos1, out cornerDirection1, out smooth);
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start1, true, out cornerPos2, out cornerDirection2, out smooth);
            bool start2 = (int)Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].m_startNode == (int)nodeID;
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, true, out cornerPos3, out cornerDirection3, out smooth);
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, false, out cornerPos4, out cornerDirection4, out smooth);
            Vector3 middlePos1_1;
            Vector3 middlePos2_1;
            NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos3, -cornerDirection3, true, true, out middlePos1_1, out middlePos2_1);
            Vector3 middlePos1_2;
            Vector3 middlePos2_2;
            NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos4, -cornerDirection4, true, true, out middlePos1_2, out middlePos2_2);
            data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, netnode.m_position, vScale);
            data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
            data.m_dataVector3 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            data.m_dataInt0 = 8 | segmentIndex;
            data.m_dataColor0 = info.m_color;
            data.m_dataColor0.a = 0.0f;
            if (info.m_requireSurfaceMaps)
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
            instanceIndex = (uint)data.m_nextInstance;
        }

        private int CalculateRendererCount(NetNode netnode, NetInfo info)
        {
            if ((netnode.m_flags & NetNode.Flags.Junction) == NetNode.Flags.None)
                return 1;
            int num = 0;
            if (info.m_requireSegmentRenderers)
                num += netnode.CountSegments();
            if (info.m_requireDirectRenderers)
                num += (int)netnode.m_connectCount;
            return num;
        }

        public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, int layerMask)
        {
            NetManager nm = Singleton<NetManager>.instance;
            NetNode nn = nm.m_nodes.m_buffer[(int)nodeID];

            if (nn.m_flags == NetNode.Flags.None)
                return;
            NetInfo info = nn.Info;
            if (!cameraInfo.Intersect(nn.m_bounds))
                return;
            if (nn.m_problems != Notification.Problem.None && (layerMask & 1 << Singleton<NotificationManager>.instance.m_notificationLayer) != 0)
            {
                Vector3 position = nn.m_position;
                position.y += Mathf.Max(5f, info.m_maxHeight);
                Notification.RenderInstance(cameraInfo, nn.m_problems, position, 1f);
            }
            if ((layerMask & info.m_netLayers) == 0 || (nn.m_flags & (NetNode.Flags.End | NetNode.Flags.Bend | NetNode.Flags.Junction)) == NetNode.Flags.None)
                return;
            if ((nn.m_flags & NetNode.Flags.Bend) != NetNode.Flags.None)
            {
                if (info.m_segments == null || info.m_segments.Length == 0)
                    return;
            }
            else if (info.m_nodes == null || info.m_nodes.Length == 0)
                return;
            uint count = (uint)CalculateRendererCount(nn, info);
            RenderManager instance = Singleton<RenderManager>.instance;
            uint instanceIndex;
            if (!instance.RequireInstance(65536U + (uint)nodeID, count, out instanceIndex))
                return;
            int iter = 0;
            while ((int)instanceIndex != (int)ushort.MaxValue)
            {
                RenderInstanceNode(cameraInfo, nodeID, info, iter, nn.m_flags, ref instanceIndex, ref instance.m_instances[instanceIndex]);
                if (++iter > 36)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + System.Environment.StackTrace);
                    break;
                }
            }
        }

        public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, NetInfo info, int iter, NetNode.Flags flags, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            NetManager nm = Singleton<NetManager>.instance;
            NetNode nn = nm.m_nodes.m_buffer[(int)nodeID];

            if (data.m_dirty)
            {
                data.m_dirty = false;
                if (iter == 0)
                {
                    if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
                        RefreshJunctionData(nn, nodeID, info, instanceIndex);
                    else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
                        RefreshBendData(nn, nodeID, info, instanceIndex, ref data);
                    else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
                        RefreshEndData(nn, nodeID, info, instanceIndex, ref data);
                }
            }

            if (data.m_initialized)
            {
                if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
                {
                    if ((data.m_dataInt0 & 8) != 0)
                    {
                        ushort segment = nn.GetSegment(data.m_dataInt0 & 7);
                        if ((int)segment != 0)
                        {
                            NetManager instance = Singleton<NetManager>.instance;
                            info = instance.m_segments.m_buffer[(int)segment].Info;
                            for (int index = 0; index < info.m_nodes.Length; ++index)
                            {
                                NetInfo.Node nodeData = info.m_nodes[index];
                                if (nodeData.CheckFlags(flags) && nodeData.m_directConnect)
                                {

                                    instance.m_materialBlock.Clear();
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                                    instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                                    instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                                    instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                                    if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                                    {
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                        instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                                    }
                                    ++instance.m_drawCallData.m_defaultCalls;
                                    Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                                }
                            }
                        }
                    }
                    else
                    {
                        ushort segment = nn.GetSegment(data.m_dataInt0 & 7);
                        if ((int)segment != 0)
                        {
                            NetManager instance = Singleton<NetManager>.instance;
                            info = instance.m_segments.m_buffer[(int)segment].Info;
                            for (int index = 0; index < info.m_nodes.Length; ++index)
                            {
                                NetInfo.Node nodeData = info.m_nodes[index];
                                if (nodeData.CheckFlags(flags) && !nodeData.m_directConnect)
                                {

                                    instance.m_materialBlock.Clear();
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrixB, data.m_dataMatrix1);
                                    instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                                    instance.m_materialBlock.AddVector(instance.ID_CenterPos, data.m_dataVector1);
                                    instance.m_materialBlock.AddVector(instance.ID_SideScale, data.m_dataVector2);
                                    instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_extraData.m_dataVector4);
                                    instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                                    if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                                    {
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                        instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector3);
                                    }
                                    ++instance.m_drawCallData.m_defaultCalls;
                                    Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                                }
                            }
                        }
                    }
                }
                else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
                {
                    NetManager instance = Singleton<NetManager>.instance;
                    for (int index = 0; index < info.m_nodes.Length; ++index)
                    {
                        NetInfo.Node nodeData = info.m_nodes[index];
                        if (nodeData.CheckFlags(flags) && !nodeData.m_directConnect)
                        {
                            instance.m_materialBlock.Clear();
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrixB, data.m_dataMatrix1);
                            instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                            instance.m_materialBlock.AddVector(instance.ID_CenterPos, data.m_dataVector1);
                            instance.m_materialBlock.AddVector(instance.ID_SideScale, data.m_dataVector2);
                            instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_extraData.m_dataVector4);
                            instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                            if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                            {
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector3);
                            }
                            ++instance.m_drawCallData.m_defaultCalls;
                            Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                        }
                    }
                }
                else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
                {
                    NetManager instance = Singleton<NetManager>.instance;
                    for (int index = 0; index < info.m_segments.Length; ++index)
                    {
                        NetInfo.Segment segmentData = info.m_segments[index];
                        bool turnAround;
                        if (segmentData.CheckFlags(NetSegment.Flags.None, out turnAround))
                        {
                            instance.m_materialBlock.Clear();
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                            instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                            instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                            instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                            if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                            {
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                            }
                            ++instance.m_drawCallData.m_defaultCalls;
                            Graphics.DrawMesh(segmentData.m_segmentMesh, data.m_position, data.m_rotation, segmentData.m_segmentMaterial, segmentData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                        }
                    }
                }
            }
            instanceIndex = (uint)data.m_nextInstance;
        }

        private void RefreshEndData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
        {
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 zero = Vector3.zero;
            Vector3 zero2 = Vector3.zero;
            Vector3 vector = Vector3.zero;
            Vector3 vector2 = Vector3.zero;
            Vector3 zero3 = Vector3.zero;
            Vector3 zero4 = Vector3.zero;
            Vector3 a = Vector3.zero;
            Vector3 a2 = Vector3.zero;
            for (int i = 0; i < 8; i++)
            {
                ushort segment = netnode.GetSegment(i);
                if (segment != 0)
                {
                    NetSegment netSegment = Singleton<NetManager>.instance.m_segments.m_buffer[(int)segment];
                    bool start = netSegment.m_startNode == nodeID;
                    bool flag;
                    netSegment.CalculateCorner(segment, true, start, false, out zero, out zero3, out flag);
                    netSegment.CalculateCorner(segment, true, start, true, out zero2, out zero4, out flag);
                    vector = zero2;
                    vector2 = zero;
                    a = zero4;
                    a2 = zero3;
                }
            }
            float d = Mathf.Min(info.m_halfWidth * 1.33333337f, 16f);
            Vector3 vector3 = zero - zero3 * d;
            Vector3 vector4 = vector - a * d;
            Vector3 vector5 = zero2 - zero4 * d;
            Vector3 vector6 = vector2 - a2 * d;
            Vector3 vector7 = zero + zero3 * d;
            Vector3 vector8 = vector + a * d;
            Vector3 vector9 = zero2 + zero4 * d;
            Vector3 vector10 = vector2 + a2 * d;
            data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(zero, vector3, vector4, vector, zero, vector3, vector4, vector, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(zero2, vector9, vector10, vector2, zero2, vector9, vector10, vector2, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(zero, vector7, vector8, vector, zero, vector7, vector8, vector, netnode.m_position, vScale);
            data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(zero2, vector5, vector6, vector2, zero2, vector5, vector6, vector2, netnode.m_position, vScale);
            data.m_dataMatrix0.SetRow(3, data.m_dataMatrix0.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_extraData.m_dataMatrix2.SetRow(3, data.m_extraData.m_dataMatrix2.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_extraData.m_dataMatrix3.SetRow(3, data.m_extraData.m_dataMatrix3.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_dataMatrix1.SetRow(3, data.m_dataMatrix1.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 0.5f - info.m_pavementWidth / info.m_halfWidth * 0.5f, info.m_pavementWidth / info.m_halfWidth * 0.5f);
            data.m_dataVector1 = new Vector4(0f, (float)netnode.m_heightOffset * 0.015625f, 0f, 0f);
            data.m_dataVector1.w = (data.m_dataMatrix0.m33 + data.m_extraData.m_dataMatrix2.m33 + data.m_extraData.m_dataMatrix3.m33 + data.m_dataMatrix1.m33) * 0.25f;
            data.m_dataVector2 = new Vector4(info.m_pavementWidth / info.m_halfWidth * 0.5f, 1f, info.m_pavementWidth / info.m_halfWidth * 0.5f, 1f);
            data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536u + (uint)nodeID);
            data.m_dataColor0 = info.m_color;
            data.m_dataColor0.a = 0f;
            if (info.m_requireSurfaceMaps)
            {
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector3);
            }
        }



        public void RenderInstanceSegment(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask)
        {
            NetManager nm = Singleton<NetManager>.instance;
            NetSegment ns = nm.m_segments.m_buffer[(int)segmentID];

            if (ns.m_flags == NetSegment.Flags.None)
                return;
            NetInfo info = ns.Info;
            if ((layerMask & info.m_netLayers) == 0 || !cameraInfo.Intersect(ns.m_bounds))
                return;
            RenderManager instance = Singleton<RenderManager>.instance;
            uint instanceIndex;
            if (!instance.RequireInstance(32768U + (uint)segmentID, 1U, out instanceIndex))
                return;

            RenderInstanceSegmentNew(cameraInfo, segmentID, layerMask, info, ref instance.m_instances[instanceIndex]);
        }

        private void RenderInstanceSegmentNew(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask, NetInfo info, ref RenderManager.Instance data)
        {
            NetManager instance = Singleton<NetManager>.instance;
            if (data.m_dirty)
            {
                data.m_dirty = false;
                Vector3 vector3_1 = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].m_position;
                Vector3 vector3_2 = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].m_position;
                data.m_position = (vector3_1 + vector3_2) * 0.5f;
                data.m_rotation = Quaternion.identity;
                data.m_dataColor0 = info.m_color;
                data.m_dataColor0.a = 0.0f;
                data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
                data.m_dataVector3 = RenderManager.GetColorLocation(32768U + (uint)segmentID);
                data.m_dataVector3.w = Singleton<WeatherManager>.instance.GetWindSpeed(data.m_position);
                if (info.m_segments == null || info.m_segments.Length == 0)
                {
                    if (info.m_lanes != null)
                    {
                        bool invert;
                        NetNode.Flags flags1;
                        Color color1;
                        NetNode.Flags flags2;
                        Color color2;
                        if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
                        {
                            invert = true;
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags1, out color1);
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
                        }
                        else
                        {
                            invert = false;
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags1, out color1);
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
                        }
                        float startAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleStart * 0.02454369f;
                        float endAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleEnd * 0.02454369f;

                        int propIndex = 0;
                        uint laneID = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
                        for (int index = 0; index < info.m_lanes.Length && (int)laneID != 0; ++index)
                        {
                            instance.m_lanes.m_buffer[laneID].RefreshInstance(laneID, info.m_lanes[index], startAngle, endAngle, invert, ref data, ref propIndex);
                            laneID = instance.m_lanes.m_buffer[laneID].m_nextLane;
                        }
                    }
                }
                else
                {
                    bool turnAround = false;
                    int index = 0;
                    while (index < info.m_segments.Length && !info.m_segments[index].CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out turnAround))
                        ++index;
                    float vScale = 0.05f;
                    Vector3 cornerPos1;
                    Vector3 cornerDirection1;
                    bool smooth1;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, true, out cornerPos1, out cornerDirection1, out smooth1);
                    Vector3 cornerPos2;
                    Vector3 cornerDirection2;
                    bool smooth2;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, true, out cornerPos2, out cornerDirection2, out smooth2);
                    Vector3 cornerPos3;
                    Vector3 cornerDirection3;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, false, out cornerPos3, out cornerDirection3, out smooth1);
                    Vector3 cornerPos4;
                    Vector3 cornerDirection4;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, false, out cornerPos4, out cornerDirection4, out smooth2);
                    Vector3 middlePos1_1;
                    Vector3 middlePos2_1;
                    NetSegment.CalculateMiddlePoints(cornerPos1, cornerDirection1, cornerPos4, cornerDirection4, smooth1, smooth2, out middlePos1_1, out middlePos2_1);
                    Vector3 middlePos1_2;
                    Vector3 middlePos2_2;
                    NetSegment.CalculateMiddlePoints(cornerPos3, cornerDirection3, cornerPos2, cornerDirection2, smooth1, smooth2, out middlePos1_2, out middlePos2_2);
                    if (turnAround)
                    {
                        data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos2_2, middlePos1_2, cornerPos3, cornerPos4, middlePos2_1, middlePos1_1, cornerPos1, data.m_position, vScale);
                        data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos4, middlePos2_1, middlePos1_1, cornerPos1, cornerPos2, middlePos2_2, middlePos1_2, cornerPos3, data.m_position, vScale);
                    }
                    else
                    {
                        data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos4, cornerPos3, middlePos1_2, middlePos2_2, cornerPos2, data.m_position, vScale);
                        data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos3, middlePos1_2, middlePos2_2, cornerPos2, cornerPos1, middlePos1_1, middlePos2_1, cornerPos4, data.m_position, vScale);
                    }
                }
                if (info.m_requireSurfaceMaps)
                    Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
            }
            if (info.m_segments != null)
            {
                for (int index = 0; index < info.m_segments.Length; ++index)
                {
                    NetInfo.Segment segmentData = info.m_segments[index];
                    bool turnAround;
                    if (segmentData.CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out turnAround))
                    {
                        instance.m_materialBlock.Clear();
                        instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                        instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_dataMatrix1);
                        instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                        instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                        instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                        if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture0 != (UnityEngine.Object)null)
                        {
                            instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                            instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                            instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                        }
                        ++instance.m_drawCallData.m_defaultCalls;
                        Graphics.DrawMesh(segmentData.m_segmentMesh, data.m_position, data.m_rotation, segmentData.m_segmentMaterial, segmentData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                    }
                }
            }
            if (info.m_lanes == null || (layerMask & info.m_treeLayers) == 0 && !cameraInfo.CheckRenderDistance(data.m_position, info.m_maxPropDistance + 128f))
                return;
            bool invert1;
            NetNode.Flags flags3;
            Color color3;
            NetNode.Flags flags4;
            Color color4;
            if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
            {
                invert1 = true;
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags3, out color3);
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags4, out color4);
            }
            else
            {
                invert1 = false;
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags3, out color3);
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags4, out color4);
            }

            float startAngle1 = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleStart * 0.02454369f;
            float endAngle1 = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleEnd * 0.02454369f;

            Vector4 objectIndex = data.m_dataVector3;
            InfoManager.InfoMode currentMode = Singleton<InfoManager>.instance.CurrentMode;
            if (currentMode != InfoManager.InfoMode.None && !info.m_netAI.ColorizeProps(currentMode))
                objectIndex.z = 0.0f;
            int propIndex1 = info.m_segments == null || info.m_segments.Length == 0 ? 0 : -1;
            uint laneID1 = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
            for (int index = 0; index < info.m_lanes.Length && (int)laneID1 != 0; ++index)
            {
                instance.m_lanes.m_buffer[laneID1].RenderInstance(cameraInfo, segmentID, laneID1, info.m_lanes[index], flags3, flags4, color3, color4, startAngle1, endAngle1, invert1, layerMask, objectIndex, objectIndex, ref data, ref propIndex1);
                laneID1 = instance.m_lanes.m_buffer[laneID1].m_nextLane;
            }

        }

    }
}
