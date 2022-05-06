using System;
using System.Runtime.CompilerServices;

namespace SnippetCommon;

public unsafe readonly struct PinnedArray<T>
    where T : unmanaged
{
    public readonly T[] Array;
    public T* Pointer => (T*)Unsafe.AsPointer(ref Array[0]);

    public bool IsDefault => Array is null;

    public PinnedArray(int length)
        => Array = GC.AllocateArray<T>(length: length, pinned: true);
}
