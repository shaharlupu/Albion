using Albion.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albion.Engine
{
    public static class SubLayers
    {
        public static readonly Guid FWPM_SUBLAYER_UNIVERSAL = new Guid("eebecc03-ced4-4380-819a-2734397b2b74");
        public static readonly Guid RPCFW_SUBLAYER = new Guid("b2e6b9ee-67b8-4ced-8061-598837ebfd8e"); //TODO: provider sublayer as sagi's?
        public static readonly Guid FWPM_SUBLAYER_RPC_AUDIT = new Guid("758c84f4-fb48-4de9-9aeb-3ed9551ab1fd");

        public static KeyValuePair<Guid, string>[] All()
        {
            return new KeyValuePair<Guid, string>[]{}; //TODO 
        }
    }
}
