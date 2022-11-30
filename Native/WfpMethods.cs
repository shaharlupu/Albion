using System;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public static class WfpMethods
    {
        const string DllName = "FWPUCLNT.DLL";

        [DllImport(DllName)]
        public static extern void FwpmFreeMemory0(
            ref IntPtr p);

        [DllImport(DllName)]
        public static extern uint FwpmEngineOpen0(
            [MarshalAs(UnmanagedType.LPWStr)] string serverName,
            uint authnService,
            IntPtr authIdentity,
            ref FWPM_SESSION0 session,
            out IntPtr engineHandle);

        [DllImport(DllName)]
        public static extern uint FwpmEngineClose0(
            IntPtr engineHandle);

        [DllImport(DllName)]
        public static extern uint FwpmProviderAdd0(
            IntPtr engineHandle,
            ref FWPM_PROVIDER0 provider,
            IntPtr sd);

        // [DllImport(DllName)]
        // public static extern uint FwpmProviderCreateEnumHandle0(
        //     IntPtr engineHandle,
        //     IntPtr enumTemplate,
        //     out IntPtr enumHandle);
        //
        // [DllImport(DllName)]
        // public static extern uint FwpmProviderEnum0(
        //     IntPtr engineHandle,
        //     IntPtr enumHandle,
        //     uint numEntriesRequested,
        //     out IntPtr entries,
        //     out uint numEntriesReturned);

        [DllImport(DllName)]
        public static extern uint FwpmLayerCreateEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumTemplate,
            out IntPtr enumHandle);
        
        [DllImport(DllName)]
        public static extern uint FwpmLayerEnum0(
            IntPtr engineHandle,
            IntPtr enumHandle,
            uint numEntriesRequested,
            out IntPtr entries,
            out uint numEntriesReturned);

        [DllImport(DllName)]
        public static extern uint FwpmProviderDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport(DllName)]
        internal static extern uint FwpmFilterAdd0(
            IntPtr engineHandle,
            ref FWPM_FILTER0 filter,
            IntPtr sd,
            out ulong id);

        [DllImport(DllName)]
        internal static extern uint FwpmFilterDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport(DllName)]
        public static extern uint FwpmFilterCreateEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumTemplate,
            out IntPtr enumHandle);

        [DllImport(DllName)]
        public static extern uint FwpmFilterEnum0(
            IntPtr engineHandle,
            IntPtr enumHandle,
            uint numEntriesRequested,
            out IntPtr entries,
            out uint numEntriesReturned);

        [DllImport(DllName)]
        public static extern uint FwpmFilterDestroyEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumHandle);

        [DllImport(DllName)]
        public static extern uint FwpmSubLayerAdd0(
            IntPtr engineHandle,
            ref FWPM_SUBLAYER0 subLayer,
            IntPtr sd);

        [DllImport(DllName)]
        public static extern uint FwpmSubLayerDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport(DllName)]
        public static extern uint FwpmSubLayerCreateEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumTemplate,
            out IntPtr enumHandle);

        [DllImport(DllName)]
        public static extern uint FwpmSubLayerEnum0(
            IntPtr engineHandle,
            IntPtr enumHandle,
            uint numEntriesRequested,
            out IntPtr entries,
            out uint numEntriesReturned);
        
        [DllImport(DllName)]
        public static extern uint FwpmLayerDestroyEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumHandle);

        [DllImport(DllName)]
        public static extern uint FwpmSubLayerDestroyEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumHandle);
           
        [DllImport(DllName)]
        public static extern uint FwpmLayerGetById0(
            IntPtr engineHandle,
            uint id,
            out IntPtr layer);

        [DllImport(DllName)]
        public static extern uint FwpmFilterGetById0(
            IntPtr engineHandle,
            uint id,
            out IntPtr layer);

    }
}
