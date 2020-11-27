// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public unsafe partial struct PxRepXObject
{
    [FieldOffset(0)] public byte* typeName;

    [FieldOffset(8)] public void* serializable;

    [FieldOffset(16)] public ulong id;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxRepXObject@physx@@QEAA@PEBDPEBX_K@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxRepXObject* @this, byte* inTypeName, void* inSerializable, ulong inId);

    public unsafe void Constructor(byte* inTypeName, void* inSerializable = null, ulong inId = 0)
    {
        fixed (PxRepXObject* @this = &this)
        { Constructor_PInvoke(@this, inTypeName, inSerializable, inId); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxRepXObject@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxRepXObject* @this);

    public unsafe bool isValid()
    {
        fixed (PxRepXObject* @this = &this)
        { return isValid_PInvoke(@this); }
    }
}