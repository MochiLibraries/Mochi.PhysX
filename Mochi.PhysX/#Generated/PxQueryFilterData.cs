// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public unsafe partial struct PxQueryFilterData
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper118", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQueryFilterData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQueryFilterData()
        {
            fixed (PxQueryFilterData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper119", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQueryFilterData* @this, PxFilterData* fd, PxQueryFlags* f);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQueryFilterData(PxFilterData* fd, PxQueryFlags* f)
        {
            fixed (PxQueryFilterData* @this = &this)
            { Constructor_PInvoke(@this, fd, f); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper120", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQueryFilterData* @this, PxQueryFlags* f);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQueryFilterData(PxQueryFlags* f)
        {
            fixed (PxQueryFilterData* @this = &this)
            { Constructor_PInvoke(@this, f); }
        }

        [FieldOffset(0)] public PxFilterData data;

        [FieldOffset(16)] public PxQueryFlags flags;
    }
}