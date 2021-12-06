// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe partial struct PxConstraintInvMassScale
    {
        [FieldOffset(0)] public float linear0;

        [FieldOffset(4)] public float angular0;

        [FieldOffset(8)] public float linear1;

        [FieldOffset(12)] public float angular1;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper33", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxConstraintInvMassScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxConstraintInvMassScale()
        {
            fixed (PxConstraintInvMassScale* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper34", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxConstraintInvMassScale* @this, float lin0, float ang0, float lin1, float ang1);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxConstraintInvMassScale(float lin0, float ang0, float lin1, float ang1)
        {
            fixed (PxConstraintInvMassScale* @this = &this)
            { Constructor_PInvoke(@this, lin0, ang0, lin1, ang1); }
        }
    }
}