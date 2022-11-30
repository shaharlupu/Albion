using Albion.Native;
using System;
using System.Collections.Generic;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        // public IEnumerable<FWPM_FILTER0> GetProviders()
        // {
        //     IntPtr enumHandle = IntPtr.Zero;
        //     IntPtr entries = IntPtr.Zero;
        //
        //     try
        //     {
        //         FWP_E code = (FWP_E) Methods.FwpmProviderCreateEnumHandle0(engineHandle, IntPtr.Zero, out enumHandle);
        //         if (code != FWP_E.SUCCESS)
        //             throw new NativeException("FwpmProviderCreateEnumHandle0", code);
        //
        //         uint numEntriesReturned;
        //         code = (FWP_E) Methods.FwpmProviderEnum0(engineHandle, enumHandle, uint.MaxValue, out entries, out numEntriesReturned);
        //         if (code != FWP_E.SUCCESS)
        //             throw new NativeException("FwpmProviderEnum0", code);
        //
        //         for (uint i = 0; i < numEntriesReturned; i++)
        //         {
        //             IntPtr ptr = new IntPtr(entries.ToInt64() + i * IntPtr.Size);
        //             IntPtr ptr2 = MarshalUtils.PtrToStructure<IntPtr>(ptr);
        //             FWPM_PROVIDER0 provider = MarshalUtils.PtrToStructure<FWPM_PROVIDER0>(ptr2);
        //
        //             yield return provider;
        //         }
        //     }
        //     finally
        //     {
        //         if (entries != IntPtr.Zero)
        //             Methods.FwpmFreeMemory0(ref entries);
        //
        //         if (enumHandle != IntPtr.Zero)
        //         {
        //             FWP_E code = (FWP_E) Methods.FwpmProviderDestroyEnumHandle0(engineHandle, enumHandle);
        //             if (code != FWP_E.SUCCESS)
        //                 throw new NativeException("FwpmProviderDestroyEnumHandle0", code);
        //         }
        //     }
        // }

        public void AddProviderIfMissing()
        {
            FWPM_PROVIDER0 provider = new FWPM_PROVIDER0();
            provider.providerKey = ProviderKey;
            provider.displayData.name = ProviderName;
            provider.flags = FWPM_PROVIDER_FLAG.PERSISTENT;

            FwpStatus code = (FwpStatus) WfpMethods.FwpmProviderAdd0(_engineHandle, ref provider, IntPtr.Zero);
            if (code != FwpStatus.SUCCESS && code != FwpStatus.ALREADY_EXISTS)
                throw new NativeException("FwpmProviderAdd0", code);
        }

        public void DeleteProvider()
        {
            Guid providerKey = ProviderKey;

            FwpStatus code = (FwpStatus) WfpMethods.FwpmProviderDeleteByKey0(_engineHandle, ref providerKey);
            if (code != FwpStatus.SUCCESS && code != FwpStatus.PROVIDER_NOT_FOUND)
                throw new NativeException("FwpmProviderAdd0", code);
        }
    }
}
