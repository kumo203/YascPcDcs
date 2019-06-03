using OpcRcw.Da;
using OpcRcw.Comn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace YascPcDcsControls
{
    public enum BROWSE_TYPE
    {
        // Token: 0x04000007 RID: 7
        ROOT,
        // Token: 0x04000008 RID: 8
        BRANCH,
        // Token: 0x04000009 RID: 9
        LEAF,
        // Token: 0x0400000A RID: 10
        UP
    }

    public enum DEF_OPCDA
    {
        // Token: 0x04000002 RID: 2
        VER_NONE,
        // Token: 0x04000003 RID: 3
        VER_10,
        // Token: 0x04000004 RID: 4
        VER_20,
        // Token: 0x04000005 RID: 5
        VER_30
    }
    public class DxpSimpleClass
    {
        // Token: 0x06000001 RID: 1
        [DllImport("ole32.dll")]
        private static extern void CoCreateInstanceEx(ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, uint dwClsCtx, [In] ref DxpSimpleClass.COSERVERINFO pServerInfo, uint dwCount, [In] [Out] DxpSimpleClass.MULTI_QI[] pResults);

        // Token: 0x06000002 RID: 2
        [DllImport("oleaut32.dll")]
        private static extern void VariantClear(IntPtr pVariant);

        // Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
        public DxpSimpleClass()
        {
            this.m_bConnect = false;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x0000206C File Offset: 0x0000026C
        public bool EnumServerList(string sNodeName, out string[] sServerNameArray)
        {
            bool result;
            try
            {
                int num;
                DxpSimpleClass.SERVERPARAM[] array = this.BrowseServer(sNodeName, out num);
                sServerNameArray = new string[num];
                for (int i = 0; i < num; i++)
                {
                    sServerNameArray[i] = array[i].progID;
                }
                result = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                sServerNameArray = new string[1];
                sServerNameArray[0] = "";
                result = false;
            }
            return result;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
        public bool Connect(string sNodeName, string sServerName)
        {
            if (this.m_OPCServer != null)
            {
                return true;
            }
            IOPCServerList iopcserverList = (IOPCServerList)DxpSimpleClass.CreateInstance(DxpSimpleClass.CLSID_SERVERLIST, sNodeName);
            bool result;
            try
            {
                Guid clsid;
                iopcserverList.CLSIDFromProgID(sServerName, out clsid);
                this.m_OPCServer = (IOPCServer)DxpSimpleClass.CreateInstance(clsid, sNodeName);
                if (this.m_OPCServer != null)
                {
                    ((IOPCCommon)this.m_OPCServer).SetClientName("TestClient");
                    this.m_bConnect = true;
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                result = false;
            }
            finally
            {
                Marshal.ReleaseComObject(iopcserverList);
                iopcserverList = null;
            }
            return result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002170 File Offset: 0x00000370
        public bool Read(string[] sItemIDArray, out object[] oValueArray, out short[] wQualityArray, out OpcRcw.Da.FILETIME[] fTimeArray, out int[] nErrorArray)
        {
            int num = sItemIDArray.Count<string>();
            int[] array = new int[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = 0;
            }
            int[] array2 = new int[num];
            oValueArray = new object[num];
            wQualityArray = new short[num];
            fTimeArray = new OpcRcw.Da.FILETIME[num];
            nErrorArray = new int[num];
            bool result;
            try
            {
                IOPCItemIO iopcitemIO = (IOPCItemIO)this.m_OPCServer;
                IntPtr intPtr;
                IntPtr intPtr2;
                IntPtr intPtr3;
                IntPtr intPtr4;
                iopcitemIO.Read(num, sItemIDArray, array, out intPtr, out intPtr2, out intPtr3, out intPtr4);
                IntPtr intPtr5 = intPtr;
                IntPtr ptr = intPtr3;
                Marshal.Copy(intPtr4, nErrorArray, 0, num);
                Marshal.Copy(intPtr2, wQualityArray, 0, num);
                for (int j = 0; j < num; j++)
                {
                    if (array2[j] == 0)
                    {
                        oValueArray[j] = Marshal.GetObjectForNativeVariant(intPtr5);
                    }
                    else
                    {
                        oValueArray[j] = "Error Value";
                    }
                    fTimeArray[j] = (OpcRcw.Da.FILETIME)Marshal.PtrToStructure(ptr, typeof(OpcRcw.Da.FILETIME));
                    DxpSimpleClass.VariantClear(intPtr5);
                    Marshal.DestroyStructure(ptr, typeof(OpcRcw.Da.FILETIME));
                    intPtr5 = (IntPtr)(intPtr5.ToInt32() + 16);
                    ptr = (IntPtr)(ptr.ToInt32() + Marshal.SizeOf(typeof(OpcRcw.Da.FILETIME)));
                }
                Marshal.FreeCoTaskMem(intPtr);
                Marshal.FreeCoTaskMem(intPtr3);
                Marshal.FreeCoTaskMem(intPtr4);
                Marshal.FreeCoTaskMem(intPtr2);
                result = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                result = false;
            }
            return result;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000022DC File Offset: 0x000004DC
        public bool Write(string[] sItemIDArray, object[] oValArray, out int[] nErrorArray)
        {
            int num = sItemIDArray.Count<string>();
            nErrorArray = new int[num];
            bool result;
            try
            {
                IOPCItemIO iopcitemIO = (IOPCItemIO)this.m_OPCServer;
                OPCITEMVQT[] array = new OPCITEMVQT[num];
                for (int i = 0; i < oValArray.Count<object>(); i++)
                {
                    array[i] = new OPCITEMVQT
                    {
                        vDataValue = oValArray[i]
                    };
                }
                IntPtr intPtr;
                iopcitemIO.WriteVQT(num, sItemIDArray, array, out intPtr);
                Marshal.Copy(intPtr, nErrorArray, 0, num);
                Marshal.FreeCoTaskMem(intPtr);
                result = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                result = false;
            }
            return result;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x0000237C File Offset: 0x0000057C
        public bool Disconnect()
        {
            if (this.m_OPCServer == null)
            {
                return true;
            }
            bool result;
            try
            {
                foreach (DxpSimpleClass.cGroupObject cGroupObject in this.m_dicGroup.Values)
                {
                    if (cGroupObject.m_OPCConnPoint != null)
                    {
                        Marshal.ReleaseComObject(cGroupObject.m_OPCConnPoint);
                        cGroupObject.m_OPCConnPoint = null;
                    }
                    if (cGroupObject.m_iServerGroup != 0)
                    {
                        this.m_OPCServer.RemoveGroup(cGroupObject.m_iServerGroup, 0);
                        cGroupObject.m_iServerGroup = 0;
                    }
                    if (cGroupObject.m_OPCGroup != null)
                    {
                        Marshal.ReleaseComObject(cGroupObject.m_OPCGroup);
                        cGroupObject.m_OPCGroup = null;
                    }
                }
                this.m_dicGroup.Clear();
                Marshal.ReleaseComObject(this.m_OPCServer);
                this.m_OPCServer = null;
                this.m_bConnect = false;
                result = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                this.m_bConnect = false;
                result = false;
            }
            return result;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002470 File Offset: 0x00000670
        private DxpSimpleClass.SERVERPARAM[] BrowseServer(string sNodeName, out int nServerCnt)
        {
            nServerCnt = 0;
            IOPCServerList iopcserverList = (IOPCServerList)DxpSimpleClass.CreateInstance(DxpSimpleClass.CLSID_SERVERLIST, sNodeName);
            DxpSimpleClass.SERVERPARAM[] result;
            try
            {
                Guid clsid_DA_ = DxpSimpleClass.CLSID_DA_30;
                DxpSimpleClass.SERVERPARAM[] serverParam = this.GetServerParam(iopcserverList, clsid_DA_, out nServerCnt);
                result = serverParam;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                result = null;
            }
            finally
            {
                Marshal.ReleaseComObject(iopcserverList);
                iopcserverList = null;
            }
            return result;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000024D4 File Offset: 0x000006D4
        private DxpSimpleClass.SERVERPARAM[] GetServerParam(IOPCServerList svrList, Guid catid, out int nServerCnt)
        {
            IEnumGUID enumGUID = null;
            svrList.EnumClassesOfCategories(1, new Guid[]
            {
                catid
            }, 0, null, out enumGUID);
            ArrayList arrayList = new ArrayList();
            int num = 0;
            Guid[] array = new Guid[100];
            nServerCnt = 0;
            do
            {
                try
                {
                    enumGUID.Next(array.Length, array, out num);
                    for (int i = 0; i < num; i++)
                    {
                        DxpSimpleClass.SERVERPARAM serverparam;
                        serverparam.clsid = array[i];
                        svrList.GetClassDetails(ref serverparam.clsid, out serverparam.progID, out serverparam.description);
                        arrayList.Add(serverparam);
                        nServerCnt++;
                    }
                }
                catch
                {
                    break;
                }
            }
            while (num > 0);
            Marshal.ReleaseComObject(enumGUID);
            enumGUID = null;
            return (DxpSimpleClass.SERVERPARAM[])arrayList.ToArray(typeof(DxpSimpleClass.SERVERPARAM));
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000025B0 File Offset: 0x000007B0
        private static object CreateInstance(Guid clsid, string hostName)
        {
            DxpSimpleClass.COSERVERINFO coserverinfo = default(DxpSimpleClass.COSERVERINFO);
            DxpSimpleClass.MULTI_QI[] array = new DxpSimpleClass.MULTI_QI[1];
            GCHandle gchandle = GCHandle.Alloc(DxpSimpleClass.IID_IUnknown, GCHandleType.Pinned);
            array[0].iid = gchandle.AddrOfPinnedObject();
            array[0].pItf = null;
            array[0].hr = 0u;
            try
            {
                uint clsctx_ALL = DxpSimpleClass.CLSCTX_ALL;
                coserverinfo.pwszName = hostName;
                DxpSimpleClass.CoCreateInstanceEx(ref clsid, null, clsctx_ALL, ref coserverinfo, 1u, array);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                return null;
            }
            gchandle.Free();
            return array[0].pItf;
        }

        // Token: 0x0400000B RID: 11
        private static readonly uint CLSCTX_INPROC_SERVER = 1u;

        // Token: 0x0400000C RID: 12
        private static readonly uint CLSCTX_INPROC_HANDLER = 2u;

        // Token: 0x0400000D RID: 13
        private static readonly uint CLSCTX_LOCAL_SERVER = 4u;

        // Token: 0x0400000E RID: 14
        private static readonly uint CLSCTX_REMOTE_SERVER = 16u;

        // Token: 0x0400000F RID: 15
        private static readonly uint CLSCTX_ALL = DxpSimpleClass.CLSCTX_INPROC_SERVER | DxpSimpleClass.CLSCTX_INPROC_HANDLER | DxpSimpleClass.CLSCTX_LOCAL_SERVER | DxpSimpleClass.CLSCTX_REMOTE_SERVER;

        // Token: 0x04000010 RID: 16
        private static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        // Token: 0x04000011 RID: 17
        private static readonly Guid CLSID_SERVERLIST = new Guid("13486D51-4821-11D2-A494-3CB306C10000");

        // Token: 0x04000012 RID: 18
        private static readonly Guid CLSID_DA_20 = new Guid("63D5F432-CFE4-11d1-B2C8-0060083BA1FB");

        // Token: 0x04000013 RID: 19
        private static readonly Guid CLSID_DA_30 = new Guid("CC603642-66D7-48f1-B69A-B625E73652D7");

        // Token: 0x04000014 RID: 20
        private IOPCServer m_OPCServer;

        // Token: 0x04000015 RID: 21
        private bool m_bConnect;

        // Token: 0x04000016 RID: 22
        private Dictionary<int, DxpSimpleClass.cGroupObject> m_dicGroup = new Dictionary<int, DxpSimpleClass.cGroupObject>();

        // Token: 0x02000005 RID: 5
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct COSERVERINFO
        {
            // Token: 0x04000017 RID: 23
            public uint dwReserved1;

            // Token: 0x04000018 RID: 24
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwszName;

            // Token: 0x04000019 RID: 25
            public IntPtr pAuthInfo;

            // Token: 0x0400001A RID: 26
            public uint dwReserved2;
        }

        // Token: 0x02000006 RID: 6
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MULTI_QI
        {
            // Token: 0x0400001B RID: 27
            public IntPtr iid;

            // Token: 0x0400001C RID: 28
            [MarshalAs(UnmanagedType.IUnknown)]
            public object pItf;

            // Token: 0x0400001D RID: 29
            public uint hr;
        }

        // Token: 0x02000007 RID: 7
        private struct SERVERPARAM
        {
            // Token: 0x0400001E RID: 30
            public Guid clsid;

            // Token: 0x0400001F RID: 31
            public string progID;

            // Token: 0x04000020 RID: 32
            public string description;
        }

        // Token: 0x02000008 RID: 8
        private class cGroupObject
        {
            // Token: 0x0600000D RID: 13 RVA: 0x000026D2 File Offset: 0x000008D2
            public cGroupObject()
            {
                this.m_OPCGroup = null;
                this.m_OPCConnPoint = null;
                this.m_iServerGroup = 0;
                this.m_iClientGroup = 0;
                this.m_iCallBackConnection = 0;
                this.m_bAdvise = false;
            }

            // Token: 0x04000021 RID: 33
            public IOPCGroupStateMgt m_OPCGroup;

            // Token: 0x04000022 RID: 34
            public IConnectionPoint m_OPCConnPoint;

            // Token: 0x04000023 RID: 35
            public int m_iServerGroup;

            // Token: 0x04000024 RID: 36
            public int m_iClientGroup;

            // Token: 0x04000025 RID: 37
            public int m_iCallBackConnection;

            // Token: 0x04000026 RID: 38
            public bool m_bAdvise;
        }
    }
}
