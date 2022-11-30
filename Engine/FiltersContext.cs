using System;
using Albion.Native;

namespace Albion.Engine
{
    public class FiltersContext : IDisposable
    {
        private readonly NativePointersManager _nativePointers = new NativePointersManager();
        private readonly ConditionsFactory _conditionsFactory;

        public FiltersContext()
        {
            _conditionsFactory = new ConditionsFactory(_nativePointers);
        }

        public NativePointersManager NativePointers
        {
            get { return _nativePointers; }
        }

        public ConditionsFactory ConditionsFactory
        {
            get { return _conditionsFactory; }
        }

        public void Dispose()
        {
            _nativePointers.Dispose();
        }
    }
}
