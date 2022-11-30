using Albion.Native;
using System;

namespace Albion.Engine
{
    public partial class FilteringEngine : IDisposable
    {
        private static readonly NativePointersManager GlobalPointersManager = new NativePointersManager();

        private const string ProviderName = "ZeroNetworks"; //TODO: change to RPCFW + guid the same as Sagi's so enable/disable is smooth?
        private static readonly Guid ProviderKey = new Guid("5c02b4c2-7002-11ed-a1eb-0242ac120002");
        private static readonly IntPtr ProviderKeyPtr = GlobalPointersManager.Add(ProviderKey); //TODO: free
        private readonly IntPtr _engineHandle;

        public FilteringEngine()
        {
            FWPM_SESSION0 session = new FWPM_SESSION0();
            session.sessionKey = Guid.NewGuid();
            session.flags = FWPM_SESSION_FLAG.NONE;
            session.txnWaitTimeoutInMSec = UInt32.MaxValue;

            FwpStatus code = (FwpStatus) WfpMethods.FwpmEngineOpen0(null, (uint)RPC_C_AUTHN.DEFAULT, IntPtr.Zero, ref session, out _engineHandle);
            if (code != FwpStatus.SUCCESS)
                throw new NativeException("FwpmEngineOpen0", code);
        }

        public void Dispose()
        {
            if (_engineHandle != IntPtr.Zero)
            {
                FwpStatus code = (FwpStatus) WfpMethods.FwpmEngineClose0(_engineHandle);
                if (code != FwpStatus.SUCCESS)
                    throw new NativeException("FwpmEngineClose0", code);
            }
        }

        public void Initialize()
        {
            // AddProviderIfMissing(); 
            // AddSubLayers();
        }

        public void Clear()
        {
            ClearFilters();
            // ClearSubLayers();
            // DeleteProvider();
        }
    }
}
