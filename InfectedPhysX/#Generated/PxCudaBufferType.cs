// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public unsafe partial struct PxCudaBufferType
{
    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxCudaBufferType@physx@@QEAA@AEBU01@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxCudaBufferType* @this, PxCudaBufferType* t);

    public unsafe void Constructor(PxCudaBufferType* t)
    {
        fixed (PxCudaBufferType* @this = &this)
        { Constructor_PInvoke(@this, t); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxCudaBufferType@physx@@QEAA@W4Enum@PxCudaBufferMemorySpace@1@W42PxCudaBufferFlags@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxCudaBufferType* @this, PxCudaBufferMemorySpace _memSpace, PxCudaBufferFlags _flags);

    public unsafe void Constructor(PxCudaBufferMemorySpace _memSpace, PxCudaBufferFlags _flags)
    {
        fixed (PxCudaBufferType* @this = &this)
        { Constructor_PInvoke(@this, _memSpace, _flags); }
    }

    [FieldOffset(0)] public PxCudaBufferMemorySpace memorySpace;

    [FieldOffset(4)] public PxCudaBufferFlags flags;
}