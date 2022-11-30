using Albion.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        public FWPM_FILTER0 GetFilterById(uint layerId)
        {
            var filterPtr = IntPtr.Zero;
            var status = (FwpStatus) WfpMethods.FwpmFilterGetById0(_engineHandle, layerId, out filterPtr);
            if (status != FwpStatus.SUCCESS)
            {
                throw new NativeException(nameof(WfpMethods.FwpmFilterGetById0), status);
            }
        
            //TODO: destroy ptr
        
            return Marshal.PtrToStructure<FWPM_FILTER0>(filterPtr);
        }

        public List<FWPM_FILTER0> GetAllFilters()
        {
            return GetFilters(IntPtr.Zero);
        }

        public List<FWPM_FILTER0> GetFiltersForProvider()
        {
            FWPM_FILTER_ENUM_TEMPLATE0 enumTemplate = new FWPM_FILTER_ENUM_TEMPLATE0();
            enumTemplate.providerKey = ProviderKeyPtr;
            enumTemplate.layerKey = Layers.FWPM_LAYER_RPC_UM;
            enumTemplate.enumType = FWP_FILTER_ENUM_TYPE.OVERLAPPING;
            enumTemplate.flags = FWP_FILTER_ENUM_FLAG.SORTED;
            enumTemplate.actionMask = 0xFFFFFFFF;

            IntPtr enumTemplatePtr = GlobalPointersManager.Add(enumTemplate); //TODO: wrong allocation scope

            return GetFilters(enumTemplatePtr);
        }

        private List<FWPM_FILTER0> GetFilters(IntPtr enumTemplate)
        {
            List<FWPM_FILTER0> filters = new List<FWPM_FILTER0>();
            
            IntPtr enumHandle = IntPtr.Zero;
            IntPtr entries = IntPtr.Zero;

            try
            {
                FwpStatus code = (FwpStatus) WfpMethods.FwpmFilterCreateEnumHandle0(_engineHandle, enumTemplate, out enumHandle);
                if (code != FwpStatus.SUCCESS)
                    throw new NativeException("FwpmFilterCreateEnumHandle0", code);

                uint numEntriesReturned;
                code = (FwpStatus) WfpMethods.FwpmFilterEnum0(_engineHandle, enumHandle, uint.MaxValue, out entries, out numEntriesReturned);
                if (code != FwpStatus.SUCCESS)
                    throw new NativeException("FwpmFilterEnum0", code);

                for (uint i = 0; i < numEntriesReturned; i++)
                {
                    IntPtr ptr = new IntPtr(entries.ToInt64() + i * IntPtr.Size);
                    IntPtr ptr2 = MarshalUtils.PtrToStructure<IntPtr>(ptr);
                    FWPM_FILTER0 filter = MarshalUtils.PtrToStructure<FWPM_FILTER0>(ptr2);

                    filters.Add(filter);
                }
            }
            finally
            {
                // if (entries != IntPtr.Zero)
                //     WfpMethods.FwpmFreeMemory0(ref entries);

                if (enumHandle != IntPtr.Zero)
                {
                    FwpStatus code = (FwpStatus) WfpMethods.FwpmFilterDestroyEnumHandle0(_engineHandle, enumHandle);
                    if (code != FwpStatus.SUCCESS)
                        throw new NativeException("FwpmFilterDestroyEnumHandle0", code);
                }
            }

            return filters;
        }

        public void DeleteFilter(Guid key)
        {
            FwpStatus code = (FwpStatus) WfpMethods.FwpmFilterDeleteByKey0(_engineHandle, ref key);
            if (code != FwpStatus.SUCCESS && code != FwpStatus.FILTER_NOT_FOUND)
                throw new NativeException("FwpmFilterDeleteByKey0", code);
        }

        public void ClearFilters()
        {
            foreach (FWPM_FILTER0 filter in GetFiltersForProvider())
            {
                Console.WriteLine($"Deleting filter {filter.filterKey}");
                DeleteFilter(filter.filterKey);
            }
        }

        public FWPM_FILTER0 AddRpcFilter(FiltersContext context, FWP_ACTION_TYPE actionType, Guid rpcInterface)
        {
            FWPM_FILTER0 filter = new FWPM_FILTER0();

            filter.providerKey = context.NativePointers.Add(ProviderKey);
            // filter.filterKey = Guid.NewGuid();
            filter.layerKey = Layers.FWPM_LAYER_RPC_UM;
            filter.subLayerKey = SubLayers.FWPM_SUBLAYER_RPC_AUDIT;//TODO: RPCFW_SUBLAYER?
            filter.context.rawContext = 1;
            filter.flags = FWPM_FILTER_FLAG.PERSISTENT;
            filter.action.type = actionType;
            filter.action.calloutKey = Guid.Empty;
            filter.weight.type = FWP_DATA_TYPE.UINT64;
            filter.weight.value.uint64 = context.NativePointers.Add((actionType == FWP_ACTION_TYPE.PERMIT) ? 1UL : 0UL);
            filter.displayData.name = "My RPC display data";
            filter.displayData.description = "My RPC description";

            FWPM_FILTER_CONDITION0[] conditions =
            {
                context.ConditionsFactory.RpcInterface(rpcInterface),
                context.ConditionsFactory.RpcInterface(rpcInterface),
            };

            if (conditions.Length > 0)
            {
                int conditionSize = MarshalUtils.SizeOf<FWPM_FILTER_CONDITION0>();
                IntPtr filterConditions = context.NativePointers.Add(conditionSize * conditions.Length);

                for (int i = 0; i < conditions.Length; i++)
                {
                    IntPtr ptr = new IntPtr(filterConditions.ToInt64() + i * conditionSize);
                    Marshal.StructureToPtr(conditions[i], ptr, false);
                }

                filter.numFilterConditions = conditions.Length;
                filter.filterConditions = filterConditions;
            }

            ulong id;
            FwpStatus code = (FwpStatus) WfpMethods.FwpmFilterAdd0(_engineHandle, ref filter, IntPtr.Zero, out id);
            if (code != FwpStatus.SUCCESS)
                throw new NativeException("FwpmFilterAdd0", code);

            return filter;
        }
    }
}
