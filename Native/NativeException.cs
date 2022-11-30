using System;

namespace Albion.Native
{
    public class NativeException : Exception
    {
        public NativeException(string method, FwpStatus code) :
            base(string.Format("Method {0} returned error code {1}", method, code))
        {
        }
    }
}
