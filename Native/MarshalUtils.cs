using System;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public static class MarshalUtils
    {
        public static T PtrToStructure<T>(IntPtr ptr)
        {
            return (T) Marshal.PtrToStructure(ptr, typeof(T));
        }

        public static int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        public static T[] PtrToArray<T>(IntPtr ptr, int count)
        {
            int itemSize = SizeOf<T>();

            T[] array = new T[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = PtrToStructure<T>(ptr + i * itemSize);
            }

            return array;
        }
        
        public static byte[] PtrToByteArray(IntPtr ptr, int size)
        {
            byte[] bytes = new byte[size];

            Marshal.Copy(ptr, bytes, 0, size);

            return bytes;
        }
    }
}
