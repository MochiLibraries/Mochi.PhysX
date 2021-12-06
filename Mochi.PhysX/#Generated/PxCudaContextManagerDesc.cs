// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using Mochi.PhysX.Infrastructure;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 80)]
    public unsafe partial struct PxCudaContextManagerDesc
    {
        [FieldOffset(0)] public CUctx_st** ctx;

        [FieldOffset(8)] public void* graphicsDevice;

        [FieldOffset(16)] public byte* appGUID;

        [FieldOffset(24)] public PxCudaInteropMode interopMode;

        [FieldOffset(28)] public ConstantArray_uint32_t_4 memoryBaseSize;

        [FieldOffset(44)] public ConstantArray_uint32_t_4 memoryPageSize;

        [FieldOffset(60)] public ConstantArray_uint32_t_4 maxMemorySize;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper79", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxCudaContextManagerDesc* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxCudaContextManagerDesc()
        {
            fixed (PxCudaContextManagerDesc* @this = &this)
            { Constructor_PInvoke(@this); }
        }
    }
}