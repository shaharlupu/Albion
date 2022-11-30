using System;
using Albion.Native;

namespace Albion.Engine
{
    public class ConditionsFactory
    {
        private readonly NativePointersManager _nativePointers;

        public ConditionsFactory(NativePointersManager nativePointers)
        {
            _nativePointers = nativePointers;
        }

        public FWPM_FILTER_CONDITION0 RpcInterface(Guid rpcInterface)
        {
            IntPtr uuidByteArrayIntPtr = _nativePointers.AddDynamic(rpcInterface.ToByteArray());

            FWPM_FILTER_CONDITION0 condition = new FWPM_FILTER_CONDITION0();
            condition.matchType = FWP_MATCH.EQUAL;
            condition.fieldKey = ConditionKeys.FWPM_CONDITION_RPC_IF_UUID;

            FWP_VALUE0 conditionValue = new FWP_VALUE0();
            conditionValue.type = FWP_DATA_TYPE.BYTE_ARRAY16_TYPE;

            conditionValue.value = new FWP_VALUE0.Union();
            conditionValue.value.byteArray16 = uuidByteArrayIntPtr;

            condition.conditionValue = conditionValue;

            return condition;
        }
    }
}
