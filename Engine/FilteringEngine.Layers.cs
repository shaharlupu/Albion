using Albion.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        public FWPM_LAYER0 GetLayerById(uint layerId)
        {
            IntPtr layerPtr = IntPtr.Zero;

            try
            {
                FwpStatus status = (FwpStatus) WfpMethods.FwpmLayerGetById0(_engineHandle, layerId, out layerPtr);
                if (status != FwpStatus.SUCCESS)
                {
                    throw new NativeException(nameof(WfpMethods.FwpmLayerGetById0), status);
                }

                return Marshal.PtrToStructure<FWPM_LAYER0>(layerPtr);
            }
            finally
            {
                if (layerPtr != IntPtr.Zero)
                {
                    WfpMethods.FwpmFreeMemory0(ref layerPtr);
                }
            }
        }

        public IEnumerable<FWPM_LAYER0> GetLayers()
        {
            var enumHandle = IntPtr.Zero;
            var entries = IntPtr.Zero;

            try
            {
                var code = WfpMethods.FwpmLayerCreateEnumHandle0(_engineHandle, IntPtr.Zero, out enumHandle);
                if (code != 0)
                    throw new NativeException(nameof(WfpMethods.FwpmLayerCreateEnumHandle0), (FwpStatus) code);

                code = WfpMethods.FwpmLayerEnum0(_engineHandle, enumHandle, uint.MaxValue, out entries, out uint numEntriesReturned);
                if (code != 0)
                    throw new NativeException(nameof(WfpMethods.FwpmLayerEnum0), (FwpStatus) code);

                var layerSize = Marshal.SizeOf<FWPM_LAYER0>();
                for (uint i = 0; i < numEntriesReturned; i++)
                {
                    var ptr = new IntPtr(entries.ToInt64() + i * IntPtr.Size);
                    var ptr2 = Marshal.PtrToStructure<IntPtr>(ptr);
                    var layer = Marshal.PtrToStructure<FWPM_LAYER0>(ptr2);

                    yield return layer;
                }
            }
            finally
            {
                if (entries != IntPtr.Zero)
                    WfpMethods.FwpmFreeMemory0(ref entries);

                if (enumHandle != IntPtr.Zero)
                {
                    var code = WfpMethods.FwpmLayerDestroyEnumHandle0(_engineHandle, enumHandle);
                    if (code != 0)
                        throw new NativeException(nameof(WfpMethods.FwpmLayerDestroyEnumHandle0), (FwpStatus) code);
                }
            }
        }
    }
}
