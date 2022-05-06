using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SnippetCommon;

public unsafe readonly struct PinnedUtf8String
{
    private readonly byte[] StringBytes;

    public byte* Pointer => (byte*)Unsafe.AsPointer(ref StringBytes[0]);

    public bool IsDefault => StringBytes is null;

    public PinnedUtf8String(string value)
    {
        StringBytes = GC.AllocateUninitializedArray<byte>(length: Encoding.UTF8.GetMaxByteCount(value.Length) + 1, pinned: true); // +1 for null terminator
        int bytesWritten = Encoding.UTF8.GetBytes(value, StringBytes);
        StringBytes[bytesWritten] = 0; // Add null terminator
    }

    public static implicit operator byte*(PinnedUtf8String pinnedString) => pinnedString.Pointer;
    public static explicit operator void*(PinnedUtf8String pinnedString) => pinnedString.Pointer;

    public static implicit operator PinnedUtf8String(string value) => new(value);
}
