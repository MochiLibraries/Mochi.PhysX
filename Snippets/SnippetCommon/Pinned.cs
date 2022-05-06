using System;
using System.Runtime.CompilerServices;

namespace SnippetCommon;

public unsafe readonly struct Pinned<T>
    where T : unmanaged
{
    private readonly T[] _Value;

    public ref T Value => ref _Value[0];
    public T* Pointer => (T*)Unsafe.AsPointer(ref Value);

    public bool IsDefault => _Value is null;

    public Pinned(T value)
    {
        _Value = GC.AllocateUninitializedArray<T>(length: 1, pinned: true);
        Value = value;
    }

    public static implicit operator T*(Pinned<T> pinnedT) => pinnedT.Pointer;
    public static explicit operator void*(Pinned<T> pinnedT) => pinnedT.Pointer;

    public static implicit operator Pinned<T>(in T value) => new(value);
}
