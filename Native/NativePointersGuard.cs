using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public class NativePointersManager : IDisposable
    {
        private bool _disposed;
        
        private readonly List<IntPtr> _pointers = new List<IntPtr>();
        private readonly List<GCHandle> _pinnedGcHandles = new List<GCHandle>();

        public IntPtr Add(int size)
        {
            IntPtr ptr = Marshal.AllocHGlobal(size);
            _pointers.Add(ptr);
            return ptr;
        }

        public IntPtr Add<T>(T value)
        {
            IntPtr ptr = Add(MarshalUtils.SizeOf<T>());
            Marshal.StructureToPtr(value, ptr, false);
            return ptr;
        }

        public IntPtr AddDynamic(object obj)
        {
            GCHandle gcHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            IntPtr ptr = gcHandle.AddrOfPinnedObject();

            _pinnedGcHandles.Add(gcHandle);

            return ptr;
        }

        public void Dispose()
        {
            DisposeInner();
            GC.SuppressFinalize(this);
        }

        ~NativePointersManager()
        {
            DisposeInner();
        }

        private void DisposeInner()
        {
            if (_disposed)
            {
                return;
            }
            
            foreach (IntPtr ptr in _pointers)
            {
                Marshal.FreeHGlobal(ptr);
            }

            foreach (GCHandle gcHandle in _pinnedGcHandles)
            {
                gcHandle.Free();
            }

            _disposed = true;
        }
    }
}
